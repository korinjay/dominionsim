using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class MilitiaCard : Card
    {
        public MilitiaCard() : base(CardList.Militia, Card.ActionAttack, 4, 0, 0, 2, 0, 0)
        {
        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            foreach(Player opponent in p.OtherPlayers)
            {
                int numToDiscard = opponent.Hand.Count - 3;

                var discards = opponent.Strategy.ChooseCardsToDiscard(opponent.GetFacade(), numToDiscard, numToDiscard, supply);

                foreach (string card in discards)
                {
                    opponent.DiscardCard(card);
                }
            }
        }
    }
}
