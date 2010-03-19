using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    using CardIdentifier = String;

	class CellarCard : Card
	{
        public CellarCard() : base(CardList.Cellar, CardType.Action, 2, 0, 1, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            IEnumerable<CardIdentifier> cards = p.Strategy.ChooseCardsToDiscard(p.GetFacade(), 0, 4, Card.CardType.Any, supply);

            foreach (CardIdentifier card in cards)
            {
                p.DiscardCard(card);
                p.AddCardsToHand(p.DrawCards(1));
            }
        }
	}
}
