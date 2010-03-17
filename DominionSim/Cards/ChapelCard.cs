using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class ChapelCard : Card
    {
        /// <summary>
        /// Chapel - Action
        /// "Trash up to 4 cards from your hand"
        /// </summary>
        public ChapelCard() : base(CardList.Chapel, CardType.Action, 2, 0, 0, 0, 0, 0)
        {
        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            var toTrash = p.Strategy.ChooseCardsToTrash(p.GetFacade(), 0, 4, Card.CardType.Any, supply);
            foreach (string card in toTrash)
            {
                p.TrashCard(card);
            }
        }
    }
}
