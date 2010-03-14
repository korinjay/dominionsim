using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    abstract class BaseStrategy : IStrategy
    {
        #region IStrategy Members

        public virtual void TurnAction(PlayerFacade p, Supply s)
        {
            /// Default to playing no action cards.  Many strategies will override this to actually do something.
            p.PlayActionCard(null);
        }

        public virtual void TurnBuy(PlayerFacade p, Supply s)
        {
            /// Default to buying no cards.  Probably all strategies will override this to actually do something.
            p.BuyCard(null);
        }

        public virtual List<string> ChooseCardsToTrash(PlayerFacade p, int min, int max)
        {
            List<string> trashed = new List<string>();

            // Horribly naive - just trash the first MIN cards
            for (int i = 0; i < min; i++)
            {
                trashed.Add(p.GetHand()[i]);
            }

            return trashed;
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
