using System.Linq;

namespace DominionSim.Strategy
{
    abstract class BuyOneCard : BigMoneyDuchy
    {
        /// <summary>
        /// The card we are attempting to buy
        /// </summary>
        protected CardIdentifier mCardToBuy;

        /// <summary>
        /// The number of that card we want to have in the deck
        /// </summary>
        private int mNumCardsToHave;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cardId">Card to buy</param>
        public BuyOneCard(CardIdentifier cardId) : this(cardId, 1)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cardId">Card to buy</param>
        /// <param name="numCardsOfThatType">Num to buy</param>
        public BuyOneCard(CardIdentifier cardId, int numCardsOfThatType)
        {
            mCardToBuy = cardId;
            mNumCardsToHave = numCardsOfThatType;
        }


        /// <summary>
        /// Take an action - attempt to play the card in hand if we have it
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="s">Supply</param>
        public override void TurnAction(PlayerFacade p, Supply s)
        {
            if (mCardToBuy != null)
            {
                if ((mCardToBuy.Logic.Type & Card.CardType.Action) != 0)
                {
                    while (p.GetActions() > 0 && p.GetHand().Contains(mCardToBuy))
                    {
                        p.PlayActionCard(p.GetHand().First(mCardToBuy));
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

            if (AttemptBuyCard(p))
            {
                return;
            }

            // Else buy silver
            if (p.GetMoneys() >= 3)
            {
                p.BuyCard(CardList.Silver);
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
                int numOwned = p.GetDeck().Where(vi => vi.CardId == mCardToBuy).Count();
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
