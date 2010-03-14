﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    class Chapel : BigMoneyDuchy
    {
        private int mNumChapels = 0;

        public Chapel(int numChapels)
        {
            mNumChapels = numChapels;
        }

        public override void TurnAction(Player p, Supply s)
        {
            if (p.Hand.Contains(CardList.Chapel))
            {
                p.PlayActionCard(CardList.Chapel);
            }
        }

        public override void TurnBuy(Player p, Supply s)
        {
            // Grab our chapels as early as possible
            int chapelCount = p.CountCardIn(CardList.Chapel, p.Deck);
            if (chapelCount < mNumChapels)
            {
                if (p.Moneys < 4)
                {
                    p.BuyCard(CardList.Chapel);
                    return;
                }
            }

            // Otherwise play like BMD
            base.TurnBuy(p, s);
        }

        public override List<string> ChooseCardsToTrash(Player p, int min, int max)
        {
            List<string> toTrash = new List<string>();

            int numEstates = p.CountCardIn(CardList.Estate, p.Hand);
            int numCopper = p.CountCardIn(CardList.Copper, p.Hand);

            List<string> allTreasure = p.GetCardsOfType(Card.CardType.Treasure);
            int totalMoney = 0;
            foreach (string t in allTreasure)
            {
                Card c = CardList.Cards[t];
                totalMoney += c.Moneys;
            }

            // Don't trash so much we drop our deck below 3 treasure
            numCopper = Math.Min(numCopper, totalMoney - 3);

            // Trash all the estates we can
            for (int i = 0; i < numEstates && toTrash.Count < max; i++)
            {
                toTrash.Add(CardList.Estate);
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