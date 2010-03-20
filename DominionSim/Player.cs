﻿using System;
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
        public VirtualCardList DrawPile { get; set; }
        public VirtualCardList DiscardPile { get; set; }
        public VirtualCardList DurationCards { get; set; }
        public VirtualCardList Hand { get; set; }
        public VirtualCardList PlayPile { get; set; }

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

        public void MoveCards<T>(IList<T> from, IList<T> to)
        {
            foreach (var item in from)
            {
                to.Add(item);
            }
            from.Clear();
        }

        public void MoveCard<T>(T c, IList<T> from, IList<T> to)
        {
            if (from.Remove(c))
            {
                to.Add(c);
            }
        }

        public void StartNewGame()
        {
            mTurn = 0;
            Deck = new VirtualCardList();
            DrawPile = new VirtualCardList();
            DiscardPile = new VirtualCardList();
            DurationCards = new VirtualCardList();
            Hand = new VirtualCardList();
            PlayPile = new VirtualCardList();
            OtherPlayers = new List<Player>();

            for (int i = 0; i < 7; i++)
            {
                Deck.Add(new VirtualCard(CardList.Copper));
                DrawPile.Add(new VirtualCard(CardList.Copper));
            }

            for (int i = 0; i < 3; i++)
            {
                Deck.Add(new VirtualCard(CardList.Estate));
                DrawPile.Add(new VirtualCard(CardList.Estate));
            }

            DrawPile = Utility.Shuffle(DrawPile);

            Cleanup();
        }

 
        public void TakeTurn(int turn, Supply supply)
        {
            Log("== "+Name+" taking Turn #"+turn+" ==");
            PrintDeckStats();            
            Log("  Hand: "+StringFromCardList(Hand));

            mSupply = supply;

            Log("  Choosing an action...");
            Strategy.TurnAction(mFacade, supply);

            // Cash in Treasure
            foreach (var card in Hand)
            {
                Card c = card.Logic;
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

        public VirtualCard DrawCard()
        {
            return DrawCards(1).ElementAt(0);
        }

        public IEnumerable<VirtualCard> DrawCards(int num)
        {
            Log("  Drawing " + num + " cards.");
            var drawnCards = new VirtualCardList();

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
                    var draw = DrawPile[0];
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
        /// <param name="cards"Cards to add></param>
        public void AddCardsToHand(VirtualCardList cards)
        {
            foreach (var card in cards)
            {
                AddCardToHand(card);
            }
        }

        /// <summary>
        /// Add a single card to the Hand in a trackable, logged way
        /// </summary>
        /// <param name="card">Card to add</param>
        public void AddCardToHand(VirtualCard card)
        {
            Hand.Add(card);
            Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, card, Stats.PlayerAction.AddToHand));
        }

        public void PlayActionCard(VirtualCard card)
        {
            if (card == null)
            {
                Log("    Playing no actions!");
                Actions--;
                return;
            }
            else if (Hand.Contains(card))
            {
                Card c = card.Logic;
                Log("    Playing a " + card + "!");

                Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, card, Stats.PlayerAction.Play));

                Actions--;
                MoveCard(card, Hand, PlayPile);
                c.ExecuteCard(this, mSupply);
            }
            else
            {
                throw new InvalidOperationException("Card " + card + " not in hand.");
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

        /// <summary>
        /// Trash a particular card Id from the hand
        /// </summary>
        /// <param name="cardId">Card Id to trash</param>
        public void TrashCardFromHand(CardIdentifier cardId)
        {
            TrashCardFromHand(Hand.First(cardId));
        }

        /// <summary>
        /// Trash a parcticular card from the hand
        /// </summary>
        /// <param name="card">Card to trash</param>
        public void TrashCardFromHand(VirtualCard card)
        {
            if (Hand.Contains(card))
            {
                Log("    Trashing "+card+"!");
                Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, card, Stats.PlayerAction.Trash));

                Hand.Remove(card);
                Deck.Remove(card);
            }
            else
            {
                throw new Exception("Told to trash " + card + " but I'm not holding that!");
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
                Deck.Remove(Deck.First(vi => vi.CardId == s));  // TODO Should not be First, should be the actual instance
            }
            else
            {
                throw new Exception("Told to trash " + s + " but it's not in play!");
            }
        }

        /// <summary>
        /// Discard the given card.  Must be in the Hand
        /// </summary>
        /// <param name="s">Card to discard</param>
        public void DiscardCard(VirtualCard card)
        {
            if (Hand.Contains(card))
            {
                Log("    Discarding " + card + "!");
                Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, card, Stats.PlayerAction.Discard));

                Hand.Remove(card);
                DiscardPile.Add(card);
            }
            else
            {
                throw new InvalidOperationException("Told to discard " + card + " but I'm not holding that!");
            }
        }


        /// <summary>
        /// Gain a card and put it in the given destination
        /// </summary>
        /// <param name="s"></param>
        /// <param name="destination"></param>
        public void GainCard(VirtualCard card, VirtualCardList destination)
        {
            Log("  Gained a " + card.ToString());
            Stats.Tracker.Instance.LogAction(this, new Stats.PlayerAction(mTurn, s, Stats.PlayerAction.Gain));

            destination.Add(card);
            Deck.Add(card);
        }

        /// <summary>
        /// Gain a card.  Goes to the default location
        /// </summary>
        /// <param name="card">Card to gain</param>
        public void GainCard(VirtualCard card)
        {
            GainCard(card, DiscardPile);
        }

        /// <summary>
        /// Get a Card with the iven Identifier and put it in the given destination
        /// </summary>
        /// <param name="cardId">Card's identifier</param>
        /// <param name="destination">Where to put it</param>
        public void GainCardFromSupply(CardIdentifier cardId, VirtualCardList destination)
        {
            var card = mSupply.GainCard(s);
            if (null != card)
            {
                GainCard(card, destination);
            }
        }

        public void GainCardFromSupply(CardIdentifier s)
        {
            GainCardFromSupply(s, DiscardPile);
        }

        public void AttackedBy(string player, VirtualCard card, IEnumerable<VirtualCard> reactedWith)
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
                Card c = card.Logic;
                vps += c.VictoryPoints;
            }

            return vps;
        }




        public String StringFromCardList(VirtualCardList list)
        {
            // Loop through the list and concatenate everything together like:
            //     "listItem1 listItem2 listItem3"
            return list.Aggregate("", (s, c) => s + c + " ");
        }

        public String StatStringFromList(VirtualCardList list)
        {
            var counts = new Dictionary<CardIdentifier, int>();
            foreach (var card in list)
            {
                if (counts.ContainsKey(card.CardId))
                {
                    counts[card.CardId]++;
                }
                else
                {
                    counts[card.CardId] = 1;
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

            Log(StatStringFromList(Deck.GetCardIds()));
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
