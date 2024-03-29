﻿using System.Linq;
using DominionSim.VirtualCards;

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
                    var twoCards = opponent.DrawCards(2);

                    // Divide them into treasure and non-treasure
                    var treasure = twoCards.Where(c => (c.Logic.Type & CardType.Treasure) != 0);
                    var nonTreasure = twoCards.Where(c => (c.Logic.Type & CardType.Treasure) == 0);

                    // So that it ends up logged as a discard, put the non-treasure in his hand
                    opponent.Hand.AddRange(nonTreasure);
                    foreach (var card in nonTreasure)
                    {
                        // Then tell him to discard the non-treasure
                        opponent.DiscardCard(card);
                    }

                    if (treasure.Count() > 0)
                    {
                        // Choose one treasure card to trash
                        var toTrash = p.Strategy.ChoosePlayerCardsToTrash(p.GetFacade(), 1, 1, opponent.Name, treasure).ElementAt(0);

                        // Remove it from our treasure list
                        treasure = treasure.Where(c => c != toTrash);

                        // Add it to his hand
                        opponent.Hand.Add(toTrash);
                        // Now tell him to trash it
                        opponent.TrashCardFromHand(toTrash);

                        // Choose whether to gain this card
                        var choices = new VirtualCardList();
                        choices.Add(toTrash);
                        var gains = p.Strategy.ChoosePlayerCardsToGain(p.GetFacade(), 0, choices.Count, opponent.Name, choices);

                        // Gain it if needed
                        foreach (var name in gains)
                        {
                            p.GainCard(name);
                        }

                        // Discard any other treasure
                        foreach (var name in treasure)
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
