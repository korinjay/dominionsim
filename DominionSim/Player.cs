using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DominionSim.VirtualCards;

namespace DominionSim
{
    class Player
    {
        public string Name { get; set; }

        public List<Player> OtherPlayers { get; set; }

        public Strategy.IStrategy Strategy { get; set; }
        public VirtualCardList Deck { get; set; }
        public List<CardIdentifier> DrawPile { get; set; }
        public List<CardIdentifier> DiscardPile { get; set; }
        public List<CardIdentifier> DurationCards { get; set; }
        public List<CardIdentifier> Hand { get; set; }
        public List<CardIdentifier> PlayPile { get; set; }

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
            Deck = new VirtualCardList();
            DrawPile = new List<CardIdentifier>();
            DiscardPile = new List<CardIdentifier>();
            DurationCards = new List<CardIdentifier>();
            Hand = new List<CardIdentifier>();
            PlayPile = new List<CardIdentifier>();
            OtherPlayers = new List<Player>();

            for (int i = 0; i < 7; i++)
            {
                Deck.Add(new VirtualCard(CardList.Copper));
                DrawPile.Add(CardList.Copper);
            }

            for (int i = 0; i < 3; i++)
            {
                Deck.Add(new VirtualCard(CardList.Estate));
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
            foreach (CardIdentifier name in Hand)
            {
                Card c = name.Logic;
                if ( (c.Type & Card.CardType.Treasure) != 0)
                {
                    Moneys += c.Moneys;
                }
            }
            Log("  I have "+Moneys+" moneys and " + Buys + " buys.");

            Log("  Choosing Buys...");
            Strategy.TurnBuy(mFacade, supply);

            Cleanup();
            mTurn++;
        }

        public CardIdentifier DrawCard()
        {
            return DrawCards(1).ElementAt(0);
        }

        public IEnumerable<CardIdentifier> DrawCards(int num)
        {
            Log("  Drawing " + num + " cards.");
            List<CardIdentifier> drawnCards = new List<CardIdentifier>();

            string draws = "Drew ";
            for (int i = 0; i < num; i++ )
            {
                if (DrawPile.Count == 0)
                {
                    Log("  Had to shuffle!");
                    MoveCards(DiscardPile, DrawPile);
                    DrawPile = Utility.Shuffle(DrawPile);
                }

                if (DrawPile.Count > 0)
                {
                    CardIdentifier draw = DrawPile[0];
                    DrawPile.RemoveAt(0);
                    drawnCards.Add(draw);
                    draws += draw;
                }
                else
                {
                    draws += "<nothing>";
                }

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
        public void AddCardsToHand(IEnumerable<CardIdentifier> cards)
        {
            Hand.AddRange(cards);

            foreach (CardIdentifier c in cards)
            {
                Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, c, Stats.PlayerAction.AddToHand));
            }
        }

        public void AddCardToHand(CardIdentifier card)
        {
            List<CardIdentifier> list = new List<CardIdentifier>();
            list.Add(card);
            AddCardsToHand(list);
        }

        public void PlayActionCard(CardIdentifier cardId)
        {
            if (cardId == null)
            {
                Log("    Playing no actions!");
                Actions--;
                return;
            }
            else if(Hand.Contains(cardId))
            {
                Card c = cardId.Logic;
                Log("    Playing a " + cardId + "!");

                Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, cardId, Stats.PlayerAction.Play));

                Actions--;
                MoveCard(cardId, Hand, PlayPile);
                c.ExecuteCard(this, mSupply);
            }
            else
            {
                throw new Exception("Card " + cardId + " not in hand.");
            }
        }

        public void BuyCard(CardIdentifier name)
        {
            if (name == null)
            {
                Log("    Buying nothing!");

                Buys--;
                return;
            }

            Card c = name.Logic;

            Log("    Buying a "+name);
            if (Moneys >= c.Cost && Buys > 0)
            {
                var card = mSupply.GainCard(name);
                if (null != card)
                {
                    DiscardPile.Add(card.CardId);
                    Deck.Add(card);
                    Moneys -= c.Cost;
                    Buys--;
                    Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, name, Stats.PlayerAction.Buy));
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

        public void TrashCardFromHand(CardIdentifier s)
        {
            if (Hand.Contains(s))
            {
                Log("    Trashing "+s+"!");
                Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, s, Stats.PlayerAction.Trash));


                Hand.Remove(s);
                Deck.Remove(Deck.First(vi => vi.CardId == s));  // TODO Should not be First, should be the actual instance
            }
            else
            {
                throw new Exception("Told to trash " + s + " but I'm not holding that!");
            }
        }

        public void TrashCardFromPlay(CardIdentifier s)
        {
            if (PlayPile.Contains(s))
            {
                // Some cards trash when you play them (like Feast)
                Log("    Trashing " + s + "!");
                Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, s, Stats.PlayerAction.Trash));

                PlayPile.Remove(s);
                Deck.Remove(s);
            }
            else
            {
                throw new Exception("Told to trash " + s + " but it's not in play!");
            }
        }

        public void DiscardCard(CardIdentifier s)
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


        public void GainCard(CardIdentifier s, List<CardIdentifier> destination)
        {
            Log("  Gained a " + s);
            Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, s, Stats.PlayerAction.Gain));

            destination.Add(s);
            Deck.Add(s);
        }

        public void GainCard(CardIdentifier s)
        {
            GainCard(s, DiscardPile);
        }

        public void GainCardFromSupply(CardIdentifier s, List<CardIdentifier> destination)
        {
            var card = mSupply.GainCard(s);
            if (null != card)
            {
                GainCard(s, destination);
            }
        }

        public void GainCardFromSupply(CardIdentifier s)
        {
            GainCardFromSupply(s, DiscardPile);
        }

        public void AttackedBy(string player, CardIdentifier card, IEnumerable<CardIdentifier> reactedWith)
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
            foreach (CardIdentifier name in Deck)
            {
                Card c = name.Logic;
                vps += c.VictoryPoints;
            }

            return vps;
        }




        public String StringFromList(IEnumerable<CardIdentifier> list)
        {
            // Loop through the list and concatenate everything together like:
            //     "listItem1 listItem2 listItem3"
            return list.Aggregate("", (s, c) => s + c + " ");
        }

        public String StatStringFromList(IEnumerable<CardIdentifier> list)
        {
            Dictionary<CardIdentifier, int> counts = new Dictionary<CardIdentifier, int>();
            foreach (CardIdentifier card in list)
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
            foreach (KeyValuePair<CardIdentifier, int> kvp in counts)
            {
                str += kvp.Key + ":"+kvp.Value + " ";
            }
            return str;
        }

        public void Print(List<CardIdentifier> list)
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
