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


    abstract class Chapel : BuyOneCard
    {
        public Chapel(int numChapels) : base(CardList.Chapel, numChapels)
        {
        }

        public override IEnumerable<CardIdentifier> ChooseCardsToTrash(PlayerFacade p, int min, int max, Card.CardType type, Supply s)
        {
            List<CardIdentifier> toTrash = new List<CardIdentifier>();

            int turn = p.GetTurn();

            int numEstates = Utility.CountCardIn(CardList.Estate, p.GetHand());
            int numCopper = Utility.CountCardIn(CardList.Copper, p.GetHand());

            var allTreasure = Utility.FilterCardListByType(p.GetDeck(), Card.CardType.Treasure);
            int totalMoney = 0;
            foreach (CardIdentifier t in allTreasure)
            {
                Card c = CardList.Cards[t];
                totalMoney += c.Moneys;
            }

            // Don't trash so much we drop our deck below 3 treasure
            numCopper = Math.Min(numCopper, totalMoney - 3);

            if (turn < 8)
            {
                // Trash all the estates we can
                for (int i = 0; i < numEstates && toTrash.Count < max; i++)
                {
                    toTrash.Add(CardList.Estate);
                }
            }
            // After that trash all the copper we can
            for (int i = 0; i < numCopper && toTrash.Count < max; i++)
            {
                toTrash.Add(CardList.Copper);
            }

            return toTrash;
        }
    }
}
