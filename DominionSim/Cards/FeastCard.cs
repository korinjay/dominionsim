using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    

    class FeastCard : Card
    {
        public FeastCard() : base(CardList.Feast, CardType.Action, 4, 0, 0, 0, 0, 0)
        {
        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            // First trash ourselves!
            p.TrashCardFromPlay(p.PlayPile.First(CardId));
            
            // Then gain!
            GainACard(p, supply);

            base.ExecuteCard(p, supply);
        }

        private static void GainACard(Player p, Supply supply)
        {
            CardIdentifier gain = p.Strategy.ChooseCardToGainFromSupply(p.GetFacade(), 0, 5, Card.CardType.Any, supply);
            if (null != gain)
            {
                p.GainCardFromSupply(gain);
            }
        }


        /// <summary>
        /// Feast has special logic when executing with Throne Room
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="supply">Supply</param>
        public override void ExecuteCardTwice(Player p, Supply supply)
        {
            // Trash once, gain twice
            p.TrashCardFromPlay(p.PlayPile.First(CardId));

            GainACard(p, supply);
            base.ExecuteCard(p, supply);

            GainACard(p, supply);
            base.ExecuteCard(p, supply);
        }

    }
}
