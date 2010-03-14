using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    class BigMoney : IStrategy
    {

        public void TurnAction(Player p, Supply s)
        {
            p.PlayActionCard(null);
        }

        public void TurnBuy(Player p, Supply s)
        {
            if (p.Moneys >= 8)
            {
                p.BuyCard(CardList.Province);
            }
            else if (p.Moneys >= 6)
            {
                p.BuyCard(CardList.Gold);
            }
            else if (p.Moneys >= 3)
            {
                p.BuyCard(CardList.Silver);
            }
            else
            {
                p.BuyCard(null);
            }
        }
    }
}
