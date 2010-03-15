using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class WorkshopCard : Card
    {
        /// <summary>
        /// Constructor for the Workshop
        /// </summary>
        public WorkshopCard() : base(CardList.Militia, CardType.Action, 3, 0, 0, 0, 0, 0) {}

        /// <summary>
        /// Override of Execute to tell the Strategy to please gain a card
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="s">Strategy</param>
        public override void ExecuteCard(Player p, Strategy.IStrategy s)
        {
            base.ExecuteCard(p, s);

            s.ChooseCardToGain(p.GetFacade(), 0, 4);
        }
    }
}
