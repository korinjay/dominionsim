using System;
using System.Linq;

namespace DominionSim.Cards
{
    class RemodelCard : Card
    {
        public RemodelCard() : base(CardList.Remodel, CardType.Action, 4, 0, 0, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            if (p.Hand.Count > 0)
            {
                var toTrash = p.Strategy.ChooseCardsToTrash(p.GetFacade(), 1, 1, CardType.Any, supply.GetFacade());

                if (toTrash.Count() == 1)
                {
                    var card = toTrash.ElementAt(0);
                    p.TrashCardFromHand(card);

                    var toGain = p.Strategy.ChooseCardToGainFromSupply(p.GetFacade(), 0, card.Logic.Cost + 2, CardType.Any, supply.GetFacade());
                    p.GainCardFromSupply(toGain);
                }
                else
                {
                    throw new Exception("Strategy did not trash a card for Remodel!");
                }
            }
        }
    }
}
