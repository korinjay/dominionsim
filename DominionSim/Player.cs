using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DominionSim
{
    class Player
    {
        public string Name { get; set; }

        public Strategy.IStrategy Strategy { get; set; }
        public List<string> Deck { get; set; }
        public List<string> DrawPile { get; set; }
        public List<string> DiscardPile { get; set; }
        public List<string> Hand { get; set; }
        public List<string> PlayPile { get; set; }

        public int Buys { get; set; }
        public int Actions { get; set; }
        public int Moneys { get; set; }

        public bool Verbose { get; set; }

        private int mTurn = 0;
        private Dictionary<int, string> mPurchases;
        private Supply mSupply;

        private Strategy.PlayerFacade mFacade;

        public Player(string name)
        {
            mFacade = new Strategy.PlayerFacade(this);
            Name = name;

            StartNewGame();
        }

        public void Cleanup()
        {
            Buys = 1;
            Actions = 1;
            Moneys = 0;

            MoveCards(PlayPile, DiscardPile);
            MoveCards(Hand, DiscardPile);
            DrawNewHand();
        }

        private void DrawNewHand()
        {
            Log("  Drawing a new hand...");
            DrawCards(5);
        }

        public void MoveCards<T>(List<T> from, List<T> to)
        {
            to.AddRange(from);
            from.Clear();
        }

        public void MoveCard<T>(T c, List<T> from, List<T> to)
        {
            from.Remove(c);
            to.Add(c);
        }

        public void StartNewGame()
        {
            mPurchases = new Dictionary<int, string>();
            mTurn = 0;
            Deck = new List<string>();
            DrawPile = new List<string>();
            DiscardPile = new List<string>();
            Hand = new List<string>();
            PlayPile = new List<string>();

            for (int i = 0; i < 7; i++)
            {
                Deck.Add(CardList.Copper);
                DrawPile.Add(CardList.Copper);
            }

            for (int i = 0; i < 3; i++)
            {
                Deck.Add(CardList.Estate);
                DrawPile.Add(CardList.Estate);
            }

            DrawPile = Utility.Shuffle(DrawPile);

            Cleanup();
        }

 
        public void TakeTurn(int turn, Supply supply)
        {
            Log("== "+Name+" taking Turn #"+turn+" ==");
            PrintDeckStats();            
            Log("  Hand: "+StringFromList(Hand));

            mSupply = supply;

            Log("  Choosing an action...");
            Strategy.TurnAction(mFacade, supply);

            // Cash in Treasure
            foreach (string name in Hand)
            {
                Card c = CardList.Cards[name];
                if ( (c.Type & Card.CardType.Treasure) != 0)
                {
                    Moneys += c.Moneys;
                }
            }
            Log("  I have "+Moneys+" moneys");

            Log("  Choosing Buys...");
            Strategy.TurnBuy(mFacade, supply);

            Cleanup();
            mTurn++;
        }

        public void DrawCards(int num)
        {
            Log("  Drawing " + num + " cards.");
            string draws = "Drew ";
            for (int i = 0; i < num; i++ )
            {
                if (DrawPile.Count == 0)
                {
                    Log("  Had to shuffle!");
                    MoveCards(DiscardPile, DrawPile);
                    DrawPile = Utility.Shuffle(DrawPile);
                }

                string draw;
                if (DrawPile.Count > 0)
                {
                    draw = DrawPile[0];
                    DrawPile.RemoveAt(0);
                    Hand.Add(draw);
                }
                else
                {
                    draw = "<nothing>";
                }

                draws += draw;
                if (i != num - 1)
                {
                    draws += ", ";
                }
            }
            Log("    " + draws);
        }

        public void PlayActionCard(string name)
        {
            if (name == null)
            {
                Log("    Playing no actions!");
                Actions--;
                return;
            }
            else if(Hand.Contains(name))
            {
                Card c = CardList.Cards[name];
                Log("    Playing a " + name + "!");

                Actions--;
                MoveCard(name, Hand, PlayPile);
                c.ExecuteCard(this, Strategy);
            }
        }

        public void BuyCard(string name)
        {
            if (name == null)
            {
                Log("    Buying nothing!");
                mPurchases.Add(mTurn, "<nothing>");
                Buys--;
                return;
            }

            Card c = CardList.Cards[name];

            Log("    Buying a "+name);
            if (Moneys >= c.Cost && Buys > 0)
            {
                if (mSupply.GainCard(name))
                {
                    DiscardPile.Add(name);
                    Deck.Add(name);
                    Moneys -= c.Cost;
                    Buys--;
                    mPurchases.Add(mTurn, name);
                }
                else
                {
                    Log("    Failed!  None left!");
                }
            }
            else
            {
                if (Buys == 0)
                {
                    throw new Exception("Attempted a buy with none remaining!");
                }
                else
                {
                    throw new Exception("Tried to buy a card I couldn't afford!");
                }
            }
        }

        public void TrashCard(string s)
        {
            if (Hand.Contains(s))
            {
                Log("    Trashing "+s+"!");

                Hand.Remove(s);
                Deck.Remove(s);
            }
            else
            {
                throw new Exception("Told to trash " + s + " but I'm not holding that!");
            }
        }

        public Strategy.PlayerFacade GetFacade()
        {
            return mFacade;
        }

 

        public int GetNumVictoryPoints()
        {
            int vps = 0;
            foreach (string name in Deck)
            {
                Card c = CardList.Cards[name];
                vps += c.VictoryPoints;
            }

            return vps;
        }

        public String PurchaseString()
        {
            // Loop through all purchases and output a string in the format:
            //    "1: Silver 2: Silver 3: Gold"
            return mPurchases.Aggregate("", (s, kvp) => s + kvp.Key + ":" + kvp.Value + " ");
        }

        public String StringFromList(IEnumerable<string> list)
        {
            // Loop through the list and concatenate everything together like:
            //     "listItem1 listItem2 listItem3"
            return list.Aggregate("", (s, c) => s + c + " ");
        }

        public String StatStringFromList(IEnumerable<string> list)
        {
            Dictionary<string, int> counts = new Dictionary<string, int>();
            foreach (string card in list)
            {
                if (counts.ContainsKey(card))
                {
                    counts[card]++;
                }
                else
                {
                    counts[card] = 1;
                }
            }

            string str = "";
            foreach (KeyValuePair<string, int> kvp in counts)
            {
                str += kvp.Key + ":"+kvp.Value + " ";
            }
            return str;
        }

        public void Print(List<string> list)
        {
            Log(StringFromList(list));
        }


        public void PrintDeckStats()
        {
            Log("== Deck for " + Name + " ==");

            Log(StatStringFromList(Deck));
        }

        public void Log(string str)
        {
            if (Verbose)
            {
                Console.WriteLine(str);
            }
        }
    }


}
