using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class ThiefCard : Card
    {
        public ThiefCard() : base(CardList.Thief, Card.ActionAttack, 4, 0, 0, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            foreach (Player opponent in p.OtherPlayers)
            {
                if (!HandleAttackReactions(p, opponent, supply))
                {
                    // Draw two cards from this player
                    IEnumerable<string> twoCards = opponent.DrawCards(2);

                    // Divide them into treasure and non-treasure
                    List<string> treasure = new List<string>( twoCards.Where(c => (CardList.Cards[c].Type & CardType.Treasure) != 0) );
                    List<string> nonTreasure = new List<string>( twoCards.Where(c => (CardList.Cards[c].Type & CardType.Treasure) == 0) );

                    // So that it ends up logged as a discard, put the non-treasure in his hand
                    opponent.Hand.AddRange(nonTreasure);
                    foreach (string name in nonTreasure)
                    {
                        // Then tell him to discard the non-treasure
                        opponent.DiscardCard(name);
                    }

                    if (treasure.Count() > 0)
                    {
                        // Choose one treasure card to trash
                        string toTrash = p.Strategy.ChooseOpponentCardsToTrash(p.GetFacade(), 1, 1, opponent.Name, treasure).ElementAt(0);

                        // Remove it from our treasure list
                        treasure.Remove(toTrash);

                        // Add it to his hand
                        opponent.Hand.Add(toTrash);
                        // Now tell him to trash it
                        opponent.TrashCard(toTrash);

                        // Choose whether to gain this card
                        List<string> choices = new List<string>();
                        choices.Add(toTrash);
                        IEnumerable<string> gains = p.Strategy.ChooseOpponentCardsToGain(p.GetFacade(), 0, choices.Count, opponent.Name, choices);

                        // Gain it if needed
                        foreach (string name in gains)
                        {
                            p.GainCard(name);
                        }

                        // Discard any other treasure
                        foreach (string name in treasure)
                        {
                            opponent.Hand.Add(name);
                            opponent.DiscardCard(name);
                        }

                    }
                }
            }
        }
    }
}
