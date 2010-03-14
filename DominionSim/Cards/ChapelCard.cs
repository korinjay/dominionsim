using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class ChapelCard : Card
    {
        public ChapelCard() : base(CardList.Chapel, CardType.Action, 2, 0, 0, 0, 0, 0)
        {
        }

        public override void ExecuteCard(Player p, Strategy.IStrategy s)
        {
            List<string> toTrash = s.ChooseCardsToTrash(p.GetFacade(), 0, 4);

            foreach (string card in toTrash)
            {
                p.TrashCard(card);
            }
        }
    }
}
