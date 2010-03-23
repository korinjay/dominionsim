using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DominionSim.VirtualCards;

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

        public Dictionary<CardIdentifier, VirtualCardList> GetCardSupply()
        {
            Dictionary<CardIdentifier, VirtualCardList> newDict = new Dictionary<CardIdentifier, VirtualCardList>();
            foreach (KeyValuePair<CardIdentifier, VirtualCardList> kvp in mSupply.CardSupply)
            {
                VirtualCardList newList = new VirtualCardList();
                newList.AddRange(kvp.Value);

                newDict.Add(kvp.Key, newList);
            }

            return newDict;
        }

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
