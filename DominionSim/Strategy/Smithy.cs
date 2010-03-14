using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    class Smithy : IStrategy
    {
        private int mNumSmithysToBuy = 1;

        public Smithy(int numSmithys)
        {
            mNumSmithysToBuy = numSmithys;
        }

        #region IStrategy Members
        const int PROVINCE_THRESHOLD = 4;

        public void TurnAction(Player p, Supply s)
        {
            if (p.Hand.Contains(CardList.Smithy))
            {
                p.PlayActionCard(CardList.Smithy);
            }
        }

        public void TurnBuy(Player p, Supply s)
        {
            // Always buy provinces
            if (p.Moneys >= 8)
            {
                p.BuyCard(CardList.Province);
                return;
            }

            // If there's still a bit of time (more than 4 Provinces) buy Gold
            if (p.Moneys >= 6 && s.Quantity(CardList.Province) > PROVINCE_THRESHOLD)
            {
                p.BuyCard(CardList.Gold);
                return;
            }

            // If we're close to the end of the game (fewer than 4 Provinces left) buy Duchies
            if (p.Moneys >= 5 && s.Quantity(CardList.Province) <= PROVINCE_THRESHOLD)
            {
                p.BuyCard(CardList.Duchy);
                return;
            }

            int numSmithys = 0;
            var g = p.Deck.GroupBy(name => name);
            foreach (var grp in g)
            {
                if (grp.Key == CardList.Smithy)
                {
                    numSmithys = grp.Count();
                }
            }

            // If we have 4 and we didn't already buy a smithy, buy one!
            if (p.Moneys >= 4 && numSmithys < mNumSmithysToBuy)
            {
                p.BuyCard(CardList.Smithy);
                return;
            }

            // Else buy silver
            if (p.Moneys >= 3)
            {
                p.BuyCard(CardList.Silver);
                return;
            }
        }

        #endregion
    }
}
