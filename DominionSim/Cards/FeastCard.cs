using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class FeastCard : Card
    {
        public FeastCard() : base(CardList.Feast, CardType.Action, 4, 0, 0, 0, 0, 0)
        {
        }

        public override void ExecuteCard(Player p, DominionSim.Strategy.IStrategy s, Supply supply)
        {
            base.ExecuteCard(p, s, supply);

            // First trash ourselves!
            p.TrashCard(Name);

            // Now choose what to gain
            string gain = s.ChooseCardToGain(p.GetFacade(), 0, 5, supply);

            // Now gain it!
            p.GainCard(gain);
        }
    }
}
