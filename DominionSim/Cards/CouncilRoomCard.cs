using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class CouncilRoomCard : CardBase
    {
        public CouncilRoomCard() : base("Council Room", Card.CouncilRoom, CardType.Action, 5, 4, 0, 0, 1, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            foreach (Player opponent in p.OtherPlayers)
            {
                opponent.AddCardsToHand(opponent.DrawCards(1));
            }
        }
    }
}
