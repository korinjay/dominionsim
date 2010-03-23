using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class ChancellorCard : Card
    {
        public ChancellorCard() : base(CardList.Chancellor, CardType.Action, 3, 0, 0, 2, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            if(p.Strategy.ChooseToDiscardDrawPile(p.GetFacade(), supply.GetFacade()))
            {
                // Okay, move his draw pile into the discard.
                p.MoveCards(p.DrawPile, p.DiscardPile);
            }
        }
    }
}
