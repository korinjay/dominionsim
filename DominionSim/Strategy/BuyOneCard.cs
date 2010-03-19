using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    abstract class BuyOneCard : BigMoneyDuchy
    {
        /// <summary>
        /// The card we are attempting to buy
        /// </summary>
        protected Card mCardToBuy;

        /// <summary>
        /// The number of that card we want to have in the deck
        /// </summary>
        private int mNumCardsToHave;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="card">Card to buy</param>
        public BuyOneCard(Card card) : this(card, 1)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="card">Card to buy</param>
        /// <param name="numCardsOfThatType">Num to buy</param>
        public BuyOneCard(Card card, int numCardsOfThatType)
        {
            mCardToBuy = card;
            mNumCardsToHave = numCardsOfThatType;
        }


        /// <summary>
        /// Take an action - attempt to play the card in hand if we have it
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="s">Supply</param>
        public override void TurnAction(PlayerFacade p, Supply s)
        {
            if (mCardToBuy != Card.None)
            {
                if ((CardList.Cards[mCardToBuy].Type & CardType.Action) != 0)
                {
                    while (p.GetActions() > 0 && p.GetHand().Contains(mCardToBuy))
                    {
                        p.PlayActionCard(mCardToBuy);
                    }
                }
            }
        }

        /// <summary>
        /// Attempt to buy
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="s">Supply</param>
        public override void TurnBuy(PlayerFacade p, Supply s)
        {
            // Always buy provinces
            if (p.GetMoneys() >= 8)
            {
                p.BuyCard(Card.Province);
                return;
            }

            // If there's still a bit of time (more than 4 Provinces) buy Gold
            if (p.GetMoneys() >= 6 && s.Quantity(Card.Province) > PROVINCE_THRESHOLD)
            {
                p.BuyCard(Card.Gold);
                return;
            }

            // If we're close to the end of the game (fewer than 4 Provinces left) buy Duchies
            if (p.GetMoneys() >= 5 && s.Quantity(Card.Province) <= PROVINCE_THRESHOLD)
            {
                p.BuyCard(Card.Duchy);
                return;
            }

            if (AttemptBuyCard(p))
            {
                return;
            }

            // Else buy silver
            if (p.GetMoneys() >= 3)
            {
                p.BuyCard(Card.Silver);
                return;
            }
        }

        /// <summary>
        /// Attempt to purchase the Card we are supposed to purchase.
        /// </summary>
        /// <param name="p">Player</param>
        /// <returns>Whether we bought one</returns>
        protected virtual bool AttemptBuyCard(PlayerFacade p)
        {
            if (CanAfford(p, mCardToBuy))
            {
                int numOwned = Utility.CountCardIn(mCardToBuy, p.GetDeck());
                if (numOwned < mNumCardsToHave)
                {
                    p.BuyCard(mCardToBuy);
                    return true;
                }
            }
            return false;
        }
    }
}
