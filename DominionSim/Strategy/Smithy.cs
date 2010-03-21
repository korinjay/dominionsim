using System.Linq;

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
        /// <param name="s">Supply</param>
        /// <returns>Whether we bought one</returns>
        protected override bool AttemptBuyCard(PlayerFacade p, Supply s)
        {
            if (AbleToBuy(p, s, mCardToBuy))
            {
                int numSmithies = p.GetDeck().Where(vi => vi.CardId == mCardToBuy).Count();
                int numCards = p.GetDeck().Count();
                // Attempt to maintain a healthy ratio of 1 per # cards
                if ((numCards / 8) > numSmithies)
                {
                    p.BuyCard(mCardToBuy);
                    return true;
                }
            }
            return false;
        }
    }

    abstract class Smithy : BuyOneCard
    {
        public Smithy(int numSmithys) : base(CardList.Smithy, numSmithys)
        {
        }
    }
}
