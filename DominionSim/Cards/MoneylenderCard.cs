using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class MoneylenderCard : Card
    {
        public MoneylenderCard() : base( CardList.Moneylender, CardType.Action, 4, 0, 0, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            if (p.Hand.Contains(CardList.Copper))
            {
                p.TrashCard(CardList.Copper);

                p.Moneys += 3;
            }
        }
    }
}
