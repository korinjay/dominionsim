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

        public IStrategy Strategy { get; set; }
        public List<Card> Deck { get; set; }
        public List<Card> DrawPile { get; set; }
        public List<Card> DiscardPile { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> PlayPile { get; set; }

        public int Buys { get; set; }
        public int Actions { get; set; }
        public int Moneys { get; set; }

        public bool Verbose { get; set; }

        private int mTurn = 0;
        private Dictionary<int, string> mPurchases;
        private Supply mSupply;

        public Player(string name)
        {
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
            while (Hand.Count < 5)
            {
                if (DrawPile.Count == 0)
                {
                    Log("  Had to shuffle!");
                    MoveCards(DiscardPile, DrawPile);
                    DrawPile = Utility.Shuffle(DrawPile);
                }

                Card draw = DrawPile[0];
                DrawPile.RemoveAt(0);

                Hand.Add(draw);
            }
        }

        private void MoveCards(List<Card> from, List<Card> to)
        {
            to.AddRange(from);
            from.Clear();
        }

        private void MoveCard(Card c, List<Card> from, List<Card> to)
        {
            from.Remove(c);
            to.Add(c);
        }

        public void StartNewGame()
        {
            mPurchases = new Dictionary<int, string>();
            mTurn = 0;
            Deck = new List<Card>();
            DrawPile = new List<Card>();
            DiscardPile = new List<Card>();
            Hand = new List<Card>();
            PlayPile = new List<Card>();

            for (int i = 0; i < 7; i++)
            {
                Deck.Add(CardList.Copper.Clone());
                DrawPile.Add(CardList.Copper.Clone());
            }

            for (int i = 0; i < 3; i++)
            {
                Deck.Add(CardList.Estate.Clone());
                DrawPile.Add(CardList.Estate.Clone());
            }

            DrawPile = Utility.Shuffle(DrawPile);

            Cleanup();
        }

 
        public void TakeTurn(Supply supply)
        {
            Log("== "+Name+" taking a turn ==");
            PrintDeckStats();            
            Log("  Hand: "+StringFromList(Hand));

            mSupply = supply;

            Log("  Choosing an action...");
            Strategy.TurnAction(this, supply);

            // Cash in Treasure
            foreach (Card c in Hand)
            {
                if ( (c.Type & Card.CardType.Treasure) != 0)
                {
                    Moneys += c.Moneys;
                }
            }
            Log("  I have "+Moneys+" moneys");

            Log("  Choosing Buys...");
            Strategy.TurnBuy(this, supply);

            Cleanup();
            mTurn++;
        }

        public void PlayActionCard(Card c)
        {

            if (c == null)
            {
                Log("    Playing no actions!");
                Actions--;
                return;
            }
            else
            {

            }
        }

        public void BuyCard(Card c)
        {
            if (c == null)
            {
                Log("    Buying nothing!");
                mPurchases.Add(mTurn, "<nothing>");
                Buys--;
                return;
            }

            Log("    Buying a "+c.Name);
            if (Moneys >= c.Cost && Buys > 0)
            {
                if (mSupply.GainCard(c))
                {
                    DiscardPile.Add(c.Clone());
                    Deck.Add(c.Clone());
                    Moneys -= c.Cost;
                    Buys--;
                    mPurchases.Add(mTurn, c.Name);
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
                    Log("    Failed!  Out of Buys!");
                }
                else
                {
                    Log("    Failed!  Not enough Moneys!");
                }
            }
        }

        public List<Card> GetCardsOfType(Card.CardType type)
        {
            List<Card> list = new List<Card>();
            foreach (Card c in Deck)
            {
                if ((type & c.Type) != 0)
                {
                    list.Add(c);
                }
            }
            return list;
        }

        public int GetNumVictoryPoints()
        {
            int vps = 0;
            foreach (Card c in Deck)
            {
                vps += c.VictoryPoints;
            }

            return vps;
        }

        public String PurchaseString()
        {
            string str = "";
            foreach (KeyValuePair<int, string> kvp in mPurchases)
            {
                str += kvp.Key + ":" + kvp.Value + " ";
            }
            return str;
        }

        public String StringFromList(List<Card> list)
        {
            string str = "";
            foreach (Card card in list)
            {
                str += (card.Name + " ");
            }
            return str;
        }

        public String StatStringFromList(List<Card> list)
        {
            Dictionary<string, int> counts = new Dictionary<string, int>();
            foreach (Card card in list)
            {
                if (counts.ContainsKey(card.Name))
                {
                    counts[card.Name]++;
                }
                else
                {
                    counts[card.Name] = 1;
                }
            }

            string str = "";
            foreach (KeyValuePair<string, int> kvp in counts)
            {
                str += kvp.Key + ":"+kvp.Value + " ";
            }
            return str;
        }

        public void Print(List<Card> list)
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
