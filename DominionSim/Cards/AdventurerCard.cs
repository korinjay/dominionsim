using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class AdventurerCard : CardBase
    {
        public AdventurerCard() : base("Adventurer", Card.Adventurer, CardType.Action, 6, 0, 0, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            var treasureCards = new List<Card>();
            var discards = new List<Card>();

            bool outOfCards = false;
            while (treasureCards.Count < 2 && outOfCards == false)
            {
                var nextCards = p.DrawCards(1);

                if (nextCards.Count() > 0)
                {
                    var nextCard = nextCards.ElementAt(0);
                    if ((CardList.Cards[nextCard].Type & CardType.Treasure) != 0)
                    {
                        // Hey, it's treasure!
                        treasureCards.Add(nextCard);
                    }
                    else
                    {
                        discards.Add(nextCard);
                    }
                }
                else
                {
                    outOfCards = true;
                }
            }

            p.AddCardsToHand(treasureCards);

            foreach (var c in discards)
            {
                p.Hand.Add(c);
                p.DiscardCard(c);
            }
        }
    }
}
