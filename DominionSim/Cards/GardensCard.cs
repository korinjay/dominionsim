using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class GardensCard : Card
    {
        public GardensCard() : base(CardList.Gardens, CardType.Victory, 4, 0, 0, 0, 0, 0)
        {

        }

        public override int GetNumVictoryPoints(DominionSim.VirtualCards.VirtualCardList deck)
        {
            return deck.Count / 10;
        }
    }
}
