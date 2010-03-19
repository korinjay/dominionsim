using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{

    /// <summary>
    /// Buys and plays cards completely at random
    /// </summary>
    class Random : BaseStrategy
    {
        System.Random mRandomizer;

        public Random()
        {
            mRandomizer = new System.Random();
        }
        
        public override void TurnAction(PlayerFacade p, Supply s)
        {
            while (p.GetActions() > 0)
            {
                var actionCards = Utility.FilterCardListByType(p.GetHand(), CardType.Action);

                if (actionCards.Count() == 0)
                {
                    p.PlayActionCard(Card.None);
                }
                else
                {
                    int card = mRandomizer.Next(actionCards.Count());

                    p.PlayActionCard(actionCards.ElementAt(card));
                }
            }

        }

        public override void TurnBuy(PlayerFacade p, Supply s)
        {
            int moneys = p.GetMoneys();

            IEnumerable<Card> possibleCards;

            do
            {
                possibleCards = s.GetAllCardsAtCost(moneys);
                moneys--;
            }
            while (possibleCards.Count() == 0);

            int card = mRandomizer.Next(possibleCards.Count());

            p.BuyCard(possibleCards.ElementAt(card));
        }
    }
}
