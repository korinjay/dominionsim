using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class WitchCard : Card
    {
        public WitchCard() : base( CardList.Witch, Card.ActionAttack, 5, 2, 0, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            foreach (Player opponent in p.OtherPlayers)
            {
                opponent.GainCardFromSupply(CardList.Curse);
            }
        }
    }
}
