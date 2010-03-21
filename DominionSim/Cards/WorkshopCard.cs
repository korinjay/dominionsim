
namespace DominionSim.Cards
{
    class WorkshopCard : Card
    {
        /// <summary>
        /// Workshop - Action
        /// "Gain a card costing up to (4)"
        /// </summary>
        public WorkshopCard() : base(CardList.Workshop, CardType.Action, 3, 0, 0, 0, 0, 0) {}

        /// <summary>
        /// Override of Execute to tell the Strategy to please gain a card
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="s">Strategy</param>
        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            var card = p.Strategy.ChooseCardToGainFromSupply(p.GetFacade(), 0, 4, Card.CardType.Any, supply);

            p.GainCardFromSupply(card);
        }
    }
}
