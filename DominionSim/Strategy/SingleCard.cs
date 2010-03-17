using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    class SingleCard : BigMoneyDuchy
    {
        private int mNumDesiredCard = 0;
        private string mCard = "";

        /// <summary>
        /// We should change this strategy so it can take params for which card and how many, 
        /// but right now our reflection stuff doesn't work that way
        /// </summary>
        public SingleCard()
        {
            mNumDesiredCard = 4;
            mCard = CardList.Remodel;
        }

        public override void TurnAction(PlayerFacade p, Supply s)
        {
            if (p.GetHand().Contains(mCard))
            {
                p.PlayActionCard(mCard);
            }
        }

        public override void TurnBuy(PlayerFacade p, Supply s)
        {
            // Grab our cards as early as possible
            int cardCount = Utility.CountCardIn(mCard, p.GetDeck());
            if (cardCount < mNumDesiredCard)
            {
                if (CanAfford(p, mCard))
                {
                    p.BuyCard(mCard);
                    return;
                }
            }

            // Otherwise play like BMD
            base.TurnBuy(p, s);
        }
        
    }
}
