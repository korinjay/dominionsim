using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class WorkshopCard : Card
    {
        /// <summary>
        /// Workshop - Action
        /// "Gain a card costing up to (4)"
        /// </summary>
        public WorkshopCard() : base(CardList.Workshop, CardType.Action, 3, 0, 0, 0, 0, 0) {}

        /// <summary>
        /// Override of Execute to tell the Strategy to please gain a card
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="s">Strategy</param>
        public override void ExecuteCard(Player p, Strategy.IStrategy s, Supply supply)
        {
            base.ExecuteCard(p, s, supply);

            string card = s.ChooseCardToGain(p.GetFacade(), 0, 4, supply);

            p.GainCard(card);
        }
    }
}
