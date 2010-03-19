using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class ChapelCard : CardBase
    {
        /// <summary>
        /// Chapel - Action
        /// "Trash up to 4 cards from your hand"
        /// </summary>
        public ChapelCard() : base("Chapel", Card.Chapel, CardType.Action, 2, 0, 0, 0, 0, 0)
        {
        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            var toTrash = p.Strategy.ChooseCardsToTrash(p.GetFacade(), 0, 4, CardType.Any, supply);
            foreach (var card in toTrash)
            {
                p.TrashCard(card);
            }
        }
    }
}
