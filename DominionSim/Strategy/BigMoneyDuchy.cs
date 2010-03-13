using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    /// <summary>
    /// Starts off using a Big Money strategy, but will buy Duchies if there are few Provinces remaining
    /// </summary>
    class BigMoneyDuchy : IStrategy
    {
        const int PROVINCE_THRESHOLD = 4;

        public void TurnAction(Player p, Supply s)
        {
            p.PlayActionCard(null);
        }

        public void TurnBuy(Player p, Supply s)
        {
            // Always buy provinces
            if (p.Moneys >= 8)
            {
                p.BuyCard(CardList.Province.Clone());
                return;
            }

            // If there's still a bit of time (more than 4 Provinces) buy Gold
            if (p.Moneys >= 6 && s.Quantity(CardList.Province) > PROVINCE_THRESHOLD)
            {
                p.BuyCard(CardList.Gold.Clone());
                return;
            }

            // If we're close to the end of the game (fewer than 4 Provinces left) buy Duchies
            if (p.Moneys >= 5 && s.Quantity(CardList.Province) <= PROVINCE_THRESHOLD)
            {
                p.BuyCard(CardList.Duchy.Clone());
                return;
            }

            // Else buy silver
            if (p.Moneys >= 3)
            {
                p.BuyCard(CardList.Silver.Clone());
                return;
            }

            p.BuyCard(null);
        }
    }
}
