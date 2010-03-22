using DominionSim.VirtualCards;

namespace DominionSim.Cards
{
    class LibraryCard : Card
    {
        public LibraryCard() : base(CardList.Library, CardType.Action, 5, 0, 0, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            var setAside = new VirtualCardList();
            while (p.Hand.Count < 7)
            {
                var nextCard = p.DrawCard();
                if (nextCard == null)
                {
                    // Deck is out of cards - can't keep drawing.
                    break;
                }

                // If it's an action card, ask the strategy what to do with it
                if ((nextCard.Logic.Type & CardType.Action) != 0)
                {
                    bool setThisAside = p.Strategy.ChooseToSetAsideCard(p.GetFacade(), nextCard);

                    if (setThisAside)
                    {
                        setAside.Add(nextCard);
                    }
                    else
                    {
                        p.AddCardToHand(nextCard);
                    }
                }
                else
                {
                    p.AddCardToHand(nextCard);
                }
            }

            p.Hand.AddRange(setAside);
            foreach (var card in setAside)
            {
                p.DiscardCard(card);
            }
        }
    }
}
