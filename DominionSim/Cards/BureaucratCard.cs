using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class BureaucratCard : Card
    {
        public BureaucratCard() : base(CardList.Bureaucrat, ActionAttack, 4, 0, 0, 0, 0, 0)
        {
        }

        /// <summary>
        /// "Gain a Silver card, put it on top of your deck.
        /// Each other player reveals a Victory card from his hand and puts it on his deck
        /// (or reveals a hand with no Victory cards)."
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="supply">Supply</param>
        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            // Silver goes on top of your deck
            var silverCard = supply.TakeCard(CardList.Silver);
            if (null != silverCard)
            {
                p.DrawPile.Insert(0, silverCard);
                p.Deck.Add(silverCard);
                p.Log("    Bureaucrat gained " + p.Name + " a silver to the top of the DrawPile.");
                Stats.Tracker.Instance.LogAction(p, new DominionSim.Stats.PlayerAction(p.GetTurn(), CardList.Silver, Stats.PlayerAction.Gain));
            }

            // Attack everyone.  If their reaction doesn't block it, perform the whole "Reveal Victory card" stuff.
            foreach (Player opponent in p.OtherPlayers)
            {
                if (!HandleAttackReactions(p, opponent, supply))
                {
                    if (opponent.Hand.Where(vc => (vc.Logic.Type & CardType.Victory) != 0).Count() > 0)
                    {
                        var card = opponent.Strategy.ChooseCardToReveal(opponent.GetFacade(), supply, CardType.Victory, CardList.Bureaucrat);
                        if (card == null || (card.Logic.Type & CardType.Victory) == 0 || !opponent.Hand.Contains(card))
                        {
                            throw new Exception("Strategy for " + opponent.Name + " did not reveal a Victory Card to put back on his Deck.");
                        }
                        
                        // TODO Player reveals this

                        opponent.Hand.Remove(card);
                        opponent.DrawPile.Insert(0, card);
                        opponent.Log("  " + opponent.Name + " revealed a " + card + " and it went on top of his DrawPile.");
                        Stats.Tracker.Instance.LogAction(opponent, new DominionSim.Stats.PlayerAction(opponent.GetTurn(), card.CardId, Stats.PlayerAction.Revealed));
                    }
                    else
                    {
                        // TODO Player reveals his Deck
                    }
                }
            }
        }
    }
}
