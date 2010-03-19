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
            p.TrashCard(CardId);

            // Now choose what to gain
            CardIdentifier gain = p.Strategy.ChooseCardToGainFromSupply(p.GetFacade(), 0, 5, Card.CardType.Any, supply);

            // Now gain it!
            p.GainCardFromSupply(gain);
        }

    }
}
