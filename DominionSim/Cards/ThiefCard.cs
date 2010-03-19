using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class ThiefCard : CardBase
    {
        public ThiefCard() : base("Thief", Card.Thief, ActionAttack, 4, 0, 0, 0, 0, 0)
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
                    var twoCards = opponent.DrawCards(2);

                    // Divide them into treasure and non-tsreasure
                    var treasure = new List<Card>( twoCards.Where(c => (CardList.Cards[c].Type & CardType.Treasure) != 0) );
                    var nonTreasure = new List<Card>( twoCards.Where(c => (CardList.Cards[c].Type & CardType.Treasure) == 0) );

                    // So that it ends up logged as a discard, put the non-treasure in his hand
                    opponent.Hand.AddRange(nonTreasure);
                    foreach (var c in nonTreasure)
                    {
                        // Then tell him to discard the non-treasure
                        opponent.DiscardCard(c);
                    }

                    if (treasure.Count() > 0)
                    {
                        // Choose one treasure card to trash
                        var toTrash = p.Strategy.ChooseOpponentCardsToTrash(p.GetFacade(), 1, 1, opponent.Name, treasure).ElementAt(0);

                        // Remove it from our treasure list
                        treasure.Remove(toTrash);

                        // Add it to his hand
                        opponent.Hand.Add(toTrash);
                        // Now tell him to trash it
                        opponent.TrashCard(toTrash);

                        // Choose whether to gain this card
                        var choices = new List<Card>();
                        choices.Add(toTrash);
                        var gains = p.Strategy.ChooseOpponentCardsToGain(p.GetFacade(), 0, choices.Count, opponent.Name, choices);

                        // Gain it if needed
                        foreach (var c in gains)
                        {
                            p.GainCard(c);
                        }

                        // Discard any other treasure
                        foreach (var c in treasure)
                        {
                            opponent.Hand.Add(c);
                            opponent.DiscardCard(c);
                        }

                    }
                }
            }
        }
    }
}
