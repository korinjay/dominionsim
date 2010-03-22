using System.Collections.Generic;
using System.Linq;
using DominionSim.VirtualCards;

namespace DominionSim.Strategy
{
    abstract class BaseStrategy : IStrategy
    {
        #region IStrategy Members

        /// <summary>
        /// Set up this strategy!  Some may want to inspect the initial state of the supply,
        /// register for events from players, or whatever.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        public virtual void Init(PlayerFacade p, Supply s)
        {
            // Eh, do nothing.
        }

        /// <summary>
        /// Base implementation of the TurnAction function
        /// Defaults to playing no action cards.  Many strategies will override this to actually do something.
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="s">Strategy</param>
        public virtual void TurnAction(PlayerFacade p, Supply s)
        {
            p.PlayActionCard(null);
        }

        /// <summary>
        /// Base implementation of the TurnBuy function.
        /// Default to buying no cards.  Probably all strategies will override this to actually do something.
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="s">Strategy</param>
        public virtual void TurnBuy(PlayerFacade p, Supply s)
        {
            p.BuyCard(null);
        }

        /// <summary>
        /// Base implementation of the ChooseCardsToTrash function.
        /// Default to naively choosing the cheapest cards in hand
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="min">Minimum cards to trash</param>
        /// <param name="max">Maximum cards to trash</param>
        /// <returns>Set of cards out of hand to trash</returns>
        public virtual IEnumerable<VirtualCard> ChooseCardsToTrash(PlayerFacade p, int min, int max, Card.CardType type, Supply s)
        {
            return p.GetHand().Where( c => (c.Logic.Type & type) != 0)
                              .OrderBy(c => c.Logic.Cost).Take(min);
        }

        /// <summary>
        /// An Action (perhaps one you played) is asking you to trash some of your opponent's cards!
        /// From the collection provided, choose which to trash
        /// </summary>
        /// <param name="p">The Player who is using this Strategy</param>
        /// <param name="min">Minimum number of cards you must trash</param>
        /// <param name="max">Maximum number of cards you may trash</param>
        /// <param name="opponent">Name of the opponent whose cards you are trashing</param>
        /// <param name="cards">Collection of cards to choose from</param>
        /// <returns>An enumeration of cards from the provided collection to trash</returns>
        public virtual IEnumerable<VirtualCard> ChoosePlayerCardsToTrash(PlayerFacade p, int min, int max, string opponent, IEnumerable<VirtualCard> cards)
        {
            // Choose to trash the maximum we can, the most expensive cards he has
            return cards.OrderByDescending(c => c.Logic.Cost)
                        .Take(max);
        }


        /// <summary>
        /// Base implementation of the ChooseCardsToDiscard function.
        /// Default to naively choosing the most victory-point-heavy cards in hand.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public virtual IEnumerable<VirtualCard> ChooseCardsToDiscard(PlayerFacade p, int min, int max, Card.CardType type, Supply s)
        {
            var orderedCards = p.GetHand().Where( c => (c.Logic.Type & type) != 0 )
                                          .OrderBy( c => (c.Logic.Cost) )
                                          .OrderByDescending(c => (c.Logic.Type & (Card.CardType.Curse | Card.CardType.Victory)) != 0);

            return orderedCards.Take(min);
        }

        /// <summary>
        /// Base implementation of the ChoosePlayerCardsToDiscard function
        /// </summary>
        /// <param name="p"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="opponent"></param>
        /// <param name="cards"></param>
        /// <returns></returns>
        public virtual IEnumerable<VirtualCard> ChoosePlayerCardsToDiscard(PlayerFacade p, int min, int max, string opponent, IEnumerable<VirtualCard> cards)
        {
            // If this our own attack hitting ourselves, mitigate the damage
            if (p.GetName() == opponent)
            {
                var deadCards = cards.Where(c => (c.Logic.Type & (Card.CardType.Curse | Card.CardType.Victory)) != 0);

                deadCards = deadCards.Take(max);

                return deadCards;
            }
            else
            {
                return cards.Where(c => (c.Logic.Type & (Card.CardType.Curse | Card.CardType.Victory)) == 0).Take(max);
            }
        }

        /// <summary>
        /// An Action (perhaps one you played) is forcing you to gain a card.
        /// Default to randomly choosing among the most expensive available cards
        /// </summary>
        /// <param name="minCost">Minimum cost of the card</param>
        /// <param name="maxCost">Maximum cost of the card</param>
        /// <returns>The kind of card you wish to gain</returns>
        public CardIdentifier ChooseCardToGainFromSupply(PlayerFacade p, int minCost, int maxCost, Card.CardType type, Supply s)
        {
            return s.CardSupply                                             // From the supply, find
                    .Where((k) => (k.Key.Logic.Type & type) != 0) // cards of the correct type
                    .Where((k) => k.Key.Logic.Cost <= maxCost)    // that are less than the max cost
                    .Where((k) => s.CardSupply[k.Key].Count > 0)            // there are actually cards left
                    .OrderByDescending((k) => k.Key.Logic.Cost)   // in order from most expensive to least
                    .Select((k) => k.Key)                                   // return just their names
                    .ElementAt(0);                                          // and pick the first one
        }

        /// <summary>
        /// An Action (perhaps one you played) is allowing you to
        /// gain a card from your opponent.
        /// </summary>
        /// <param name="p">The Player who is using this Strategy</param>
        /// <param name="min">Minimum number of cards you must gain from the collection</param>
        /// <param name="max">Maximum number of cards you may gain from the collection</param>
        /// <param name="opponent">Name of the opponent you're gaining cards from</param>
        /// <param name="cards">Collection of cards to gain from</param>
        /// <returns>Return the kind of card you wish to gain</returns>
        public virtual IEnumerable<VirtualCard> ChoosePlayerCardsToGain(PlayerFacade p, int min, int max, string opponent, IEnumerable<VirtualCard> cards)
        {
            // Always gain any non-copper treasure, nothing else
            return cards.Where(c => c.CardId == CardList.Silver || c.CardId == CardList.Gold).Take(max);
        }


        /// <summary>
        /// Return whether, given the current hand and amount of Money, the Strategy can buy the given card name
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        protected bool CanAfford(PlayerFacade p, CardIdentifier cardId)
        {
            return (p.GetMoneys() >= cardId.Logic.Cost);
        }

        /// <summary>
        /// Someone attacked you, and now you need to react
        /// </summary>
        /// <param name="victimPlayerFacade">The Player who is getting hit</param>
        /// <param name="supply">The supply</param>
        /// <param name="attackerName">Name of the attacking player</param>
        /// <param name="cardId">Name of the attacking card</param>
        /// <returns>Return the list of cards you wish to react with</returns>
        public IEnumerable<VirtualCard> ChooseReactionsToAttack(PlayerFacade victimPlayerFacade, Supply supply, string attackerName, CardIdentifier cardId)
        {
            // Naive implementation - just react with everything
            return victimPlayerFacade.GetHand().Where(c => ((c.Logic.Type & Card.CardType.Reaction) != 0));
        }


        /// <summary>
        /// You have the opportunity to either draw a card or set it aside (i.e. from Library)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="card"></param>
        /// <returns>TRUE to set the card aside, FALSE to draw it</returns>
        public bool ChooseToSetAsideCard(PlayerFacade p, VirtualCard card)
        {
            // Base strategy naively hopes to get money and sets aside everything else
            if ((card.Logic.Type & Card.CardType.Treasure) != 0)
            {
                // This is some kind of treasure!
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// You have the opportunity to play an Action card twice (i.e. due to Throne Room)
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="supply">The supply</param>
        /// <returns>Card from your hand to play twice</returns>
        public virtual VirtualCard ChooseCardToPlayTwice(PlayerFacade p, Supply supply)
        {
            return null;
        }


        /// <summary>
        /// You have to Reveal a Card - which would you like?
        /// </summary>
        /// <param name="p">Your player</param>
        /// <param name="supply">Current supply</param>
        /// <param name="cardType">Type of card you need to reveal</param>
        /// <param name="becauseOf">Why you are revealing it</param>
        /// <returns>The card</returns>
        public virtual VirtualCard ChooseCardToReveal(PlayerFacade p, Supply supply, Card.CardType cardType, CardIdentifier becauseOf)
        {
            return p.GetHand().First(vi => (vi.Logic.Type & cardType) != 0);
        }

        /// <summary>
        /// You have the opportunity to put your deck in your discard pile if you wish
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="s">The supply</param>
        /// <returns>TRUE to put your deck into discard, forcing a reshuffle</returns>
        public virtual bool ChooseToDiscardDrawPile(PlayerFacade p, Supply supply)
        {
            return false;
        }

        #endregion
    }
}
