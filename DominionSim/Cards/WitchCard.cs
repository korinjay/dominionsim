using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class WitchCard : CardBase
    {
        public WitchCard() : base("Witch", Card.Witch, ActionAttack, 5, 2, 0, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            foreach (Player opponent in p.OtherPlayers)
            {
                opponent.GainCardFromSupply(Card.Curse);
            }
        }
    }
}
