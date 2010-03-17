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

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            // First trash ourselves!
            p.TrashCard(Name);

            // Now choose what to gain
            string gain = p.Strategy.ChooseCardToGain(p.GetFacade(), 0, 5, Card.CardType.Any, supply);

            // Now gain it!
            p.GainCard(gain);
        }

    }
}
