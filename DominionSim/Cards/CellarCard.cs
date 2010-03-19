using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
	class CellarCard : CardBase
	{
        public CellarCard() : base("Cellar", Card.Cellar, CardType.Action, 2, 0, 1, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            var cards = p.Strategy.ChooseCardsToDiscard(p.GetFacade(), 0, 4, CardType.Any, supply);

            foreach (var card in cards)
            {
                p.DiscardCard(card);
                p.AddCardsToHand(p.DrawCards(1));
            }
        }
	}
}
