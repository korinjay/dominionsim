using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    /// <summary>
    /// Starts off using a Big Money strategy, but will buy Duchies if there are few Provinces remaining
    /// </summary>
    class BigMoneyDuchy : BaseStrategy
    {
        protected const int PROVINCE_THRESHOLD = 4;

        public override void TurnBuy(PlayerFacade p, Supply s)
        {
            // Always buy provinces
            if (p.GetMoneys() >= 8)
            {
                p.BuyCard(Card.Province);
                return;
            }

            // If there's still a bit of time (more than 4 Provinces) buy Gold
            if (p.GetMoneys() >= 6 && s.Quantity(Card.Province) > PROVINCE_THRESHOLD)
            {
                p.BuyCard(Card.Gold);
                return;
            }

            // If we're close to the end of the game (fewer than 4 Provinces left) buy Duchies
            if (p.GetMoneys() >= 5 && s.Quantity(Card.Province) <= PROVINCE_THRESHOLD)
            {
                p.BuyCard(Card.Duchy);
                return;
            }

            // Else buy silver
            if (p.GetMoneys() >= 3)
            {
                p.BuyCard(Card.Silver);
                return;
            }

            p.BuyCard(Card.None);
        }
    }
}
