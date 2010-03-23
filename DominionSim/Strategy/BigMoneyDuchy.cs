
namespace DominionSim.Strategy
{
    /// <summary>
    /// Starts off using a Big Money strategy, but will buy Duchies if there are few Provinces remaining
    /// </summary>
    class BigMoneyDuchy : BaseStrategy
    {
        protected const int PROVINCE_THRESHOLD = 4;

        public override void TurnBuy(PlayerFacade p, SupplyFacade s)
        {
            // Always buy provinces
            if (p.GetMoneys() >= 8)
            {
                p.BuyCard(CardList.Province);
                return;
            }

            // If there's still a bit of time (more than 4 Provinces) buy Gold
            if (p.GetMoneys() >= 6 && s.Quantity(CardList.Province) > PROVINCE_THRESHOLD)
            {
                p.BuyCard(CardList.Gold);
                return;
            }

            // If we're close to the end of the game (fewer than 4 Provinces left) buy Duchies
            if (p.GetMoneys() >= 5 && s.Quantity(CardList.Province) <= PROVINCE_THRESHOLD)
            {
                p.BuyCard(CardList.Duchy);
                return;
            }

            // Else buy silver
            if (p.GetMoneys() >= 3)
            {
                p.BuyCard(CardList.Silver);
                return;
            }

            p.BuyCard(null);
        }
    }
}
