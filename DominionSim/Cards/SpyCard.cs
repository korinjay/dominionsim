using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class SpyCard : Card
    {
        public SpyCard() : base( CardList.Spy, Card.ActionAttack, 4, 1, 1, 0, 0, 0)
        {

        }

        protected void ExecuteAttackOnPlayer(Player me, Player them, Supply supply)
        {
            IEnumerable<string> topOfTheirDeck = them.DrawCards(1);

            IEnumerable<string> toDiscard = me.Strategy.ChoosePlayerCardsToDiscard(me.GetFacade(), 0, 1, them.Name, topOfTheirDeck);

            IEnumerable<string> toPutBack = topOfTheirDeck.Except(toDiscard);

            foreach (string card in toDiscard)
            {
                them.Hand.Add(card);
                them.DiscardCard(card);
            }

            foreach (string card in toPutBack)
            {
                them.DrawPile.Insert(0, card);
            }
        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            foreach (Player opponent in p.OtherPlayers)
            {
                if (!HandleAttackReactions(p, opponent, supply))
                {
                    ExecuteAttackOnPlayer(p, opponent, supply);
                }
            }

            // Now... attack ourselves.  No, really.
            ExecuteAttackOnPlayer(p, p, supply);
        }
    }
}
