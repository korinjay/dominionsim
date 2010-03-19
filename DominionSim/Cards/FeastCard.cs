using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class FeastCard : CardBase
    {
        public FeastCard() : base("Feast", Card.Feast, CardType.Action, 4, 0, 0, 0, 0, 0)
        {
        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            // First trash ourselves!
            p.TrashCard(Card);

            // Now choose what to gain
            var gain = p.Strategy.ChooseCardToGainFromSupply(p.GetFacade(), 0, 5, CardType.Any, supply);

            // Now gain it!
            p.GainCardFromSupply(gain);
        }

    }
}
