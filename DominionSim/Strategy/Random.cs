using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    using CardIdentifier = String;

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
                IEnumerable<CardIdentifier> actionCards = Utility.FilterCardListByType(p.GetHand(), Card.CardType.Action);

                if (actionCards.Count() == 0)
                {
                    p.PlayActionCard(null);
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

            IEnumerable<CardIdentifier> possibleCards = new List<CardIdentifier>();

            while(possibleCards.Count() == 0)
            {
                possibleCards = s.GetAllCardsAtCost(moneys);
                moneys--;
            }

            int card = mRandomizer.Next(possibleCards.Count());

            p.BuyCard(possibleCards.ElementAt(card));
        }
    }
}
