using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DominionSim.VirtualCards;

namespace DominionSim.Cards
{
    

    class LibraryCard : Card
    {
        public LibraryCard() : base(CardList.Library, CardType.Action, 5, 0, 0, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            var setAside = new VirtualCardList();
            while (p.Hand.Count < 7)
            {
                var nextCard = p.DrawCard();

                // If it's an action card, ask the strategy what to do with it
                if ((nextCard.Logic.Type & CardType.Action) != 0)
                {
                    bool setThisAside = p.Strategy.ChooseToSetAsideCard(p.GetFacade(), nextCard);

                    if (setThisAside)
                    {
                        setAside.Add(nextCard);
                    }
                    else
                    {
                        p.AddCardToHand(nextCard);
                    }
                }
                else
                {
                    p.AddCardToHand(nextCard);
                }
            }

            p.Hand.AddRange(setAside);
            foreach (CardIdentifier card in setAside)
            {
                p.DiscardCard(card);
            }
        }
    }
}
