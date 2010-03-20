using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    

    class AdventurerCard : Card
    {
        public AdventurerCard() : base( CardList.Adventurer, CardType.Action, 6, 0, 0, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            List<CardIdentifier> treasureCards = new List<CardIdentifier>();
            List<CardIdentifier> discards = new List<CardIdentifier>();

            bool outOfCards = false;
            while (treasureCards.Count < 2 && outOfCards == false)
            {
                IEnumerable<CardIdentifier> nextCards = p.DrawCards(1);

                if (nextCards.Count() > 0)
                {
                    CardIdentifier nextCard = nextCards.ElementAt(0);
                    if ((nextCard.Logic.Type & CardType.Treasure) != 0)
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

            foreach (CardIdentifier c in discards)
            {
                p.Hand.Add(c);
                p.DiscardCard(c);
            }
        }
    }
}
