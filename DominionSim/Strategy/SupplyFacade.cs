using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    class SupplyFacade
    {
        Supply mSupply;

        public SupplyFacade(Supply supply)
        {
            mSupply = supply;
        }

        #region Passthru functions

        public int Quantity(CardIdentifier cardId)
        {
            return mSupply.Quantity(cardId);
        }

        public IEnumerable<CardIdentifier> GetAllCardsAtCost(int cost)
        {
            return mSupply.GetAllCardsInCostRange(cost, cost);
        }

        public IEnumerable<CardIdentifier> GetAllCardsInCostRange(int min, int max)
        {
            return mSupply.GetAllCardsInCostRange(min, max);
        }

        #endregion

    }
}
