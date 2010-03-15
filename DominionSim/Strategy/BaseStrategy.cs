﻿using System;
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
        public virtual IEnumerable<string> ChooseCardsToTrash(PlayerFacade p, int min, int max, Supply s)
        {
            return p.GetHand().OrderBy(c => CardList.Cards[c].Cost).Take(min);
        }

        /// <summary>
        /// An Action (perhaps one you played) is forcing you to gain a card.
        /// Default to randomly choosing among the most expensive available cards
        /// </summary>
        /// <param name="minCost">Minimum cost of the card</param>
        /// <param name="maxCost">Maximum cost of the card</param>
        /// <returns>The kind of card you wish to gain</returns>
        public string ChooseCardToGain(PlayerFacade p, int minCost, int maxCost, Supply s)
        {
            IEnumerable<string> bestCards = s.CardSupply.Where((k) => CardList.Cards[k.Key].Cost == maxCost)
                                                        .Select((k) => k.Key);

            return bestCards.ElementAt(Utility.RandBetween(0, bestCards.Count()));
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

        #endregion
    }
}
