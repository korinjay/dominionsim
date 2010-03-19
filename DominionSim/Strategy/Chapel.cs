using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    class Chapel1 : Chapel
    {
        public Chapel1() : base(1) {}
        public static string GetDisplayName() { return "Chapel - Buy 1"; }
    }
    class Chapel2 : Chapel
    {
        public Chapel2() : base(2) {}
        public static string GetDisplayName() { return "Chapel - Buy 2"; }
    }


    abstract class Chapel : BigMoneyDuchy
    {
        private int mNumChapels = 0;

        public Chapel(int numChapels)
        {
            mNumChapels = numChapels;
        }

        public override void TurnAction(PlayerFacade p, Supply s)
        {
            if (p.GetHand().Contains(Card.Chapel))
            {
                p.PlayActionCard(Card.Chapel);
            }
        }

        public override void TurnBuy(PlayerFacade p, Supply s)
        {
            // Grab our chapels as early as possible
            int chapelCount = Utility.CountCardIn(Card.Chapel, p.GetDeck());
            if (chapelCount < mNumChapels)
            {
                if (p.GetMoneys() < 4 && CanAfford(p, Card.Chapel))
                {
                    p.BuyCard(Card.Chapel);
                    return;
                }
            }

            // Otherwise play like BMD
            base.TurnBuy(p, s);
        }

        public override IEnumerable<Card> ChooseCardsToTrash(PlayerFacade p, int min, int max, CardType type, Supply s)
        {
            var toTrash = new List<Card>();

            int turn = p.GetTurn();

            int numEstates = Utility.CountCardIn(Card.Estate, p.GetHand());
            int numCopper = Utility.CountCardIn(Card.Copper, p.GetHand());

            var allTreasure = Utility.FilterCardListByType(p.GetDeck(), CardType.Treasure);
            int totalMoney = 0;
            foreach (var t in allTreasure)
            {
                CardBase c = CardList.Cards[t];
                totalMoney += c.Moneys;
            }

            // Don't trash so much we drop our deck below 3 treasure
            numCopper = Math.Min(numCopper, totalMoney - 3);

            if (turn < 8)
            {
                // Trash all the estates we can
                for (int i = 0; i < numEstates && toTrash.Count < max; i++)
                {
                    toTrash.Add(Card.Estate);
                }
            }
            // After that trash all the copper we can
            for (int i = 0; i < numCopper && toTrash.Count < max; i++)
            {
                toTrash.Add(Card.Copper);
            }

            return toTrash;
        }
    }
}
