﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class MineCard : Card
    {
        public MineCard() : base(CardList.Mine, CardType.Action, 5, 0, 0, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            if (p.Hand.Select((c) => CardList.Cards[c].Type)       // Get our hand as Types
                     .Where((t) => (t & CardType.Treasure) != 0)   // Filter only Treasure
                     .Count() > 0)                                  // And see if we ended up with any
            {
                string trashing = p.Strategy.ChooseCardsToTrash(p.GetFacade(), 1, 1, CardType.Treasure, supply).ElementAt(0);
                Card trashCard = CardList.Cards[trashing];

                p.TrashCard(trashing);

                string gaining = p.Strategy.ChooseCardToGain(p.GetFacade(), 0, trashCard.Cost + 3, CardType.Treasure, supply);

                p.GainCard(gaining);
            }
            else
            {
                // Dummy playing this card had no treasure!
            }
        }
    }
}