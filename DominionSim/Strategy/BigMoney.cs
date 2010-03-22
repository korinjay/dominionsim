
namespace DominionSim.Strategy
{
    class BigMoney : BaseStrategy
    {

        public override void TurnBuy(PlayerFacade p, Supply s)
        {
            if (p.GetMoneys() >= 8)
            {
                p.BuyCard(CardList.Province);
            }
            else if (p.GetMoneys() >= 6)
            {
                p.BuyCard(CardList.Gold);
            }
            else if (p.GetMoneys() >= 3)
            {
                p.BuyCard(CardList.Silver);
            }
            else
            {
                p.BuyCard(null);
            }
        }
    }
}
