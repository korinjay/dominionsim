using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class IronworksCard : Card
    {
        public IronworksCard() : base(CardList.Ironworks, CardType.Action, 4, 0, 0, 0, 0, 0) {}

        /// <summary>
        /// "Gain a card costing up to 4.
        /// 
        /// If it is an...
        ///   Action card, +1 Action
        ///   Treasure card, +1 Coin
        ///   Victory card, +1 Card
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="supply">Supply</param>
        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            var gaining = p.Strategy.ChooseCardToGainFromSupply(p.GetFacade(), 0, 4, CardType.Any, supply);
            p.GainCardFromSupply(gaining);

            // Note that all 3 of these things could potentially happy if you get a card that is
            // an Action, Treasure, and Victory (not sure if any exist).
            var cardType = gaining.Logic.Type;
            if ((cardType & CardType.Action) != 0)
            {
                p.Actions += 1;
            }
            if ((cardType & CardType.Treasure) != 0)
            {
                p.Moneys += 1;
            }
            if ((cardType & CardType.Victory) != 0)
            {
                p.AddCardsToHand(p.DrawCards(1));
            }
        }
    }
}
