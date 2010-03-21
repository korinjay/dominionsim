
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

            var cards = p.Strategy.ChooseCardsToDiscard(p.GetFacade(), 0, 4, Card.CardType.Any, supply);

            foreach (var card in cards)
            {
                p.DiscardCard(card);
                p.AddCardsToHand(p.DrawCards(1));
            }
        }
    }
}
