using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
	class CellarCard : Card
	{
        public CellarCard() : base(CardList.Cellar, CardType.Action, 2, 0, 1, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            IEnumerable<string> cards = p.Strategy.ChooseCardsToDiscard(p.GetFacade(), 0, 4, Card.CardType.Any, supply);

            foreach (string card in cards)
            {
                p.DiscardCard(card);
                p.DrawCards(1);
            }
        }
	}
}
