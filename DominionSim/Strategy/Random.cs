using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DominionSim.VirtualCards;

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
                IEnumerable<CardIdentifier> actionCards = Utility.FilterCardListByType(p.GetHand(), Card.CardType.Action);

                var numCards = actionCards.Count();
                if (numCards == 0)
                {
                    p.PlayActionCard(null);
                }
                else
                {
                    int card = mRandomizer.Next(numCards);
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


        /// <summary>
        /// You have the opportunity to play an Action card twice (i.e. due to Throne Room)
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="supply">The supply</param>
        /// <returns>Card from your hand to play twice</returns>
        public override VirtualCard ChooseCardToPlayTwice(PlayerFacade p, Supply supply)
        {
            var actionCards = Utility.FilterCardListByType(p.GetHand(), Card.CardType.Action);
            var numCards = actionCards.Count();
            if (numCards == 0)
            {
                return null;
            }
            else
            {
                return actionCards.ElementAt(mRandomizer.Next(numCards));
            }
        }
    }
}
