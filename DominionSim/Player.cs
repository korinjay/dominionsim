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

        public List<Player> OtherPlayers { get; set; }

        public Strategy.IStrategy Strategy { get; set; }
        public List<Card> Deck { get; set; }
        public List<Card> DrawPile { get; set; }
        public List<Card> DiscardPile { get; set; }
        public List<Card> DurationCards { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> PlayPile { get; set; }

        public int Buys { get; set; }
        public int Actions { get; set; }
        public int Moneys { get; set; }

        public bool Verbose { get; set; }

        private int mTurn = 0;
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
            Hand.AddRange(DrawCards(5));
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
            mTurn = 0;
            Deck = new List<Card>();
            DrawPile = new List<Card>();
            DiscardPile = new List<Card>();
            DurationCards = new List<Card>();
            Hand = new List<Card>();
            PlayPile = new List<Card>();
            OtherPlayers = new List<Player>();

            for (int i = 0; i < 7; i++)
            {
                Deck.Add(Card.Copper);
                DrawPile.Add(Card.Copper);
            }

            for (int i = 0; i < 3; i++)
            {
                Deck.Add(Card.Estate);
                DrawPile.Add(Card.Estate);
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
            foreach (var card in Hand)
            {
                var c = CardList.Cards[card];
                if ( (c.Type & CardType.Treasure) != 0)
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

        public IEnumerable<Card> DrawCards(int num)
        {
            Log("  Drawing " + num + " cards.");
            var drawnCards = new List<Card>();

            string draws = "Drew ";
            for (int i = 0; i < num; i++ )
            {
                if (DrawPile.Count == 0)
                {
                    Log("  Had to shuffle!");
                    MoveCards(DiscardPile, DrawPile);
                    DrawPile = Utility.Shuffle(DrawPile);
                }

                Card draw;
                if (DrawPile.Count > 0)
                {
                    draw = DrawPile[0];
                    DrawPile.RemoveAt(0);
                    drawnCards.Add(draw);
                }
                else
                {
                    draw = Card.None;
                }

                draws += draw.ToString();
                if (i != num - 1)
                {
                    draws += ", ";
                }
            }
            Log("    " + draws);

            return drawnCards;
        }

        /// <summary>
        /// Add a collection of cards to a player's hand in a trackable, logged way.
        /// This should be used when a card makes a player draw.
        /// If you don't want the stats to record "Drew X", then just manipulate Hand directly.
        /// </summary>
        /// <param name="cards"></param>
        public void AddCardsToHand(IEnumerable<Card> cards)
        {
            Hand.AddRange(cards);

            foreach (var c in cards)
            {
                Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, c, Stats.PlayerAction.AddToHand));
            }
        }

        public void PlayActionCard(Card card)
        {
            if (card == Card.None)
            {
                Log("    Playing no actions!");
                Actions--;
                return;
            }
            else if (Hand.Contains(card))
            {
                var c = CardList.Cards[card];
                Log("    Playing a " + card.ToString() + "!");

                Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, card, Stats.PlayerAction.Play));

                Actions--;
                MoveCard(card, Hand, PlayPile);
                c.ExecuteCard(this, mSupply);
            }
            else
            {
                throw new Exception("Card " + card.ToString() + " not in hand.");
            }
        }

        public void BuyCard(Card card)
        {
            if (card == Card.None)
            {
                Log("    Buying nothing!");

                Buys--;
                return;
            }

            var c = CardList.Cards[card];

            Log("    Buying a "+card.ToString());
            if (Moneys >= c.Cost && Buys > 0)
            {
                if (mSupply.GainCard(card))
                {
                    DiscardPile.Add(card);
                    Deck.Add(card);
                    Moneys -= c.Cost;
                    Buys--;
                    Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, card, Stats.PlayerAction.Buy));
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

        public void TrashCard(Card s)
        {
            if (Hand.Contains(s))
            {
                Log("    Trashing "+s+"!");
                Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, s, Stats.PlayerAction.Trash));


                Hand.Remove(s);
                Deck.Remove(s);
            }
            else if (PlayPile.Contains(s))
            {
                // Some cards trash when you play them (like Feast)
                Log("    Trashing " + s + "!");
                Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, s, Stats.PlayerAction.Trash));

                PlayPile.Remove(s);
                Deck.Remove(s);
            }
            else
            {
                throw new Exception("Told to trash " + s + " but I'm not holding that!");
            }
        }


        public void DiscardCard(Card s)
        {
            if (Hand.Contains(s))
            {
                Log("    Discarding " + s + "!");
                Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, s, Stats.PlayerAction.Discard));

                Hand.Remove(s);
                DiscardPile.Add(s);
            }
            else
            {
                throw new Exception("Told to discard " + s + " but I'm not holding that!");
            }
        }


        public void GainCard(Card s, List<Card> destination)
        {
            Log("  Gained a " + s);
            Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, s, Stats.PlayerAction.Gain));

            destination.Add(s);
            Deck.Add(s);
        }

        public void GainCard(Card s)
        {
            GainCard(s, DiscardPile);
        }

        public void GainCardFromSupply(Card s, List<Card> destination)
        {
            if (mSupply.GainCard(s))
            {
                GainCard(s, destination);
            }
        }

        public void GainCardFromSupply(Card s)
        {
            GainCardFromSupply(s, DiscardPile);
        }

        public void AttackedBy(string player, Card card, IEnumerable<Card> reactedWith)
        {
            Log(this.Name + " was attacked by a " + card + " from " + player + (reactedWith.Count() > 0 ? ", reacted with: (" + reactedWith.Aggregate("", (s, c) => s + c + " " + ")") : "") + "!");
            Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, card, Stats.PlayerAction.AttackedBy));
        }


        public Strategy.PlayerFacade GetFacade()
        {
            return mFacade;
        }

        public int GetTurn()
        {
            return mTurn;
        }

        public int GetNumVictoryPoints()
        {
            int vps = 0;
            foreach (var card in Deck)
            {
                CardBase c = CardList.Cards[card];
                vps += c.VictoryPoints;
            }

            return vps;
        }




        public String StringFromList(IEnumerable<string> list)
        {
            // Loop through the list and concatenate everything together like:
            //     "listItem1 listItem2 listItem3"
            return list.Aggregate("", (s, c) => s + c + " ");
        }

        public String StringFromList(IEnumerable<Card> list)
        {
            return StringFromList(list.Select(c => c.ToString()));
        }

        public String StatStringFromList(IEnumerable<Card> list)
        {
            var counts = new Dictionary<Card, int>();
            foreach (var card in list)
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
            foreach (KeyValuePair<Card, int> kvp in counts)
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
