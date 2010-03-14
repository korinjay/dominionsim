using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    class Smithy1 : Smithy
    {
        public Smithy1() : base(1) { }
        public static string GetDisplayName() { return "Smithy - Buy 1"; }
    }
    class Smithy2 : Smithy
    {
        public Smithy2() : base(2) { }
        public static string GetDisplayName() { return "Smithy - Buy 2"; }
    }

    /// <summary>
    /// Purchases Smithies based on deck size
    /// </summary>
    class SmithyDeckRatio : Smithy
    {
        public SmithyDeckRatio() : base(0) { }
        public static string GetDisplayName() { return "Smithy - Buy When Needed"; }

        /// <summary>
        /// Override Smithy purchasing logic.  Attempt to keep the number of Smithies in the deck intelligent
        /// </summary>
        /// <param name="p">Player</param>
        /// <returns>Whether we bought one</returns>
        protected override bool AttemptBuySmithy(Player p)
        {
            if (CanAfford(p, CardList.Smithy))
            {
                int numSmithies = p.CountCardIn(CardList.Smithy, p.Deck);
                int numCards = p.Deck.Count;
                // Attempt to maintain a healthy ratio of 1 per # cards
                if ((numCards / 8) > numSmithies)
                {
                    p.BuyCard(CardList.Smithy);
                    return true;
                }
            }
            return false;
        }
    }

    abstract class Smithy : BaseStrategy
    {

        private int mNumSmithysToBuy = 1;

        public Smithy(int numSmithys)
        {
            mNumSmithysToBuy = numSmithys;
        }

        #region IStrategy Members
        const int PROVINCE_THRESHOLD = 4;

        public override void TurnAction(Player p, Supply s)
        {
            if (p.Hand.Contains(CardList.Smithy))
            {
                p.PlayActionCard(CardList.Smithy);
            }
        }

        public override void TurnBuy(Player p, Supply s)
        {
            // Always buy provinces
            if (p.Moneys >= 8)
            {
                p.BuyCard(CardList.Province);
                return;
            }

            // If there's still a bit of time (more than 4 Provinces) buy Gold
            if (p.Moneys >= 6 && s.Quantity(CardList.Province) > PROVINCE_THRESHOLD)
            {
                p.BuyCard(CardList.Gold);
                return;
            }

            // If we're close to the end of the game (fewer than 4 Provinces left) buy Duchies
            if (p.Moneys >= 5 && s.Quantity(CardList.Province) <= PROVINCE_THRESHOLD)
            {
                p.BuyCard(CardList.Duchy);
                return;
            }

            if (AttemptBuySmithy(p))
            {
                return;
            }

            // Else buy silver
            if (p.Moneys >= 3)
            {
                p.BuyCard(CardList.Silver);
                return;
            }
        }


        /// <summary>
        /// Attempt to purchase a Smithy.
        /// </summary>
        /// <param name="p">Player</param>
        /// <returns>Whether we bought one</returns>
        protected virtual bool AttemptBuySmithy(Player p)
        {
            int numSmithys = p.CountCardIn(CardList.Smithy, p.Deck);
            // If we have 4 and we didn't already buy a smithy, buy one!
            if (CanAfford(p, CardList.Smithy) && numSmithys < mNumSmithysToBuy)
            {
                p.BuyCard(CardList.Smithy);
                return true;
            }
            return false;
        }

        #endregion
    }
}
