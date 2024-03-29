﻿using System.Linq;

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

            if (p.Hand.Select((c) => c.Logic.Type)       // Get our hand as Types
                     .Where((t) => (t & CardType.Treasure) != 0)   // Filter only Treasure
                     .Count() > 0)                                  // And see if we ended up with any
            {
                var trashing = p.Strategy.ChooseCardsToTrash(p.GetFacade(), 1, 1, CardType.Treasure, supply.GetFacade()).ElementAt(0);
                Card trashCard = trashing.Logic;

                p.TrashCardFromHand(trashing);

                CardIdentifier gaining = p.Strategy.ChooseCardToGainFromSupply(p.GetFacade(), 0, trashCard.Cost + 3, CardType.Treasure, supply.GetFacade());

                p.GainCardFromSupply(gaining, p.Hand);
            }
            else
            {
                // Dummy playing this card had no treasure!
            }
        }
    }
}
