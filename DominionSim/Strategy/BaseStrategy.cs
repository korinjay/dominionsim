using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    abstract class BaseStrategy : IStrategy
    {
        #region IStrategy Members

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
        public virtual IEnumerable<string> ChooseCardsToTrash(PlayerFacade p, int min, int max, Card.CardType type, Supply s)
        {
            return p.GetHand().Where( c => (CardList.Cards[c].Type & type) != 0)
                              .OrderBy(c => CardList.Cards[c].Cost).Take(min);
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
        public virtual IEnumerable<string> ChoosePlayerCardsToTrash(PlayerFacade p, int min, int max, string opponent, IEnumerable<string> cards)
        {
            // Choose to trash the maximum we can, the most expensive cards he has
            return cards.OrderByDescending(c => CardList.Cards[c].Cost)
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
        public virtual IEnumerable<string> ChooseCardsToDiscard(PlayerFacade p, int min, int max, Card.CardType type, Supply s)
        {
            var orderedCards = p.GetHand().Where( c => (CardList.Cards[c].Type & type) != 0 )
                                          .OrderBy( c => (CardList.Cards[c].Cost) )
                                          .OrderByDescending(c => (CardList.Cards[c].Type & (Card.CardType.Curse | Card.CardType.Victory)) != 0);

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
        public virtual IEnumerable<string> ChoosePlayerCardsToDiscard(PlayerFacade p, int min, int max, string opponent, IEnumerable<string> cards)
        {
            // If this our own attack hitting ourselves, mitigate the damage
            if (p.GetName() == opponent)
            {
                var deadCards = cards.Where(c => (CardList.Cards[c].Type & (Card.CardType.Curse | Card.CardType.Victory)) != 0);

                deadCards = deadCards.Take(max);

                return deadCards;
            }
            else
            {
                return cards.Where(c => (CardList.Cards[c].Type & (Card.CardType.Curse | Card.CardType.Victory)) == 0).Take(max);
            }
        }

        /// <summary>
        /// An Action (perhaps one you played) is forcing you to gain a card.
        /// Default to randomly choosing among the most expensive available cards
        /// </summary>
        /// <param name="minCost">Minimum cost of the card</param>
        /// <param name="maxCost">Maximum cost of the card</param>
        /// <returns>The kind of card you wish to gain</returns>
        public string ChooseCardToGainFromSupply(PlayerFacade p, int minCost, int maxCost, Card.CardType type, Supply s)
        {
            return s.CardSupply                                             // From the supply, find
                    .Where((k) => (CardList.Cards[k.Key].Type & type) != 0) // cards of the correct type
                    .Where((k) => CardList.Cards[k.Key].Cost <= maxCost)    // that are less than the max cost
                    .OrderByDescending((k) => CardList.Cards[k.Key].Cost)   // in order from most expensive to least
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
        public virtual IEnumerable<string> ChoosePlayerCardsToGain(PlayerFacade p, int min, int max, string opponent, IEnumerable<string> cards)
        {
            // Always gain any non-copper treasure, nothing else
            return cards.Where(c => c == CardList.Silver || c == CardList.Gold).Take(max);
        }


        /// <summary>
        /// Return whether, given the current hand and amount of Money, the Strategy can buy the given card name
        /// </summary>
        /// <param name="cardName"></param>
        /// <returns></returns>
        protected bool CanAfford(PlayerFacade p, string cardName)
        {
            return (p.GetMoneys() >= CardList.Cards[cardName].Cost);
        }

        /// <summary>
        /// Someone attacked you, and now you need to react
        /// </summary>
        /// <param name="victimPlayerFacade">The Player who is getting hit</param>
        /// <param name="supply">The supply</param>
        /// <param name="attackerName">Name of the attacking player</param>
        /// <param name="cardName">Name of the attacking card</param>
        /// <returns>Return the list of cards you wish to react with</returns>
        public IEnumerable<string> ChooseReactionsToAttack(PlayerFacade victimPlayerFacade, Supply supply, string attackerName, string cardName)
        {
            // Naive implementation - just react with everything
            return victimPlayerFacade.GetHand().Where(c => ((CardList.Cards[c].Type & Card.CardType.Reaction) != 0));
        }

        #endregion
    }
}
