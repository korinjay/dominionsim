using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class ShantyTownCard : Card
    {
        public ShantyTownCard() : base(CardList.ShantyTown, CardType.Action, 3, 0, 2, 0, 0, 0)
        {}

        /// <summary>
        /// "+2 Actions
        /// 
        /// Reveal your hand.
        /// If you have no Action cards in hand,
        /// +2 Cards"
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="supply">Supply</param>
        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            // TODO Player reveals his hand

            // No actions, gain 2.
            if (p.Hand.Where(vc => (vc.Logic.Type & CardType.Action) != 0).Count() == 0)
            {
                p.AddCardsToHand(p.DrawCards(2));
            }
        }
    }
}
