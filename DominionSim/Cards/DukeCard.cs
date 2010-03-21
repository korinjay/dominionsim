using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class DukeCard : Card
    {
        public DukeCard() : base(CardList.Duke, CardType.Victory, 5, 0, 0, 0, 0, 0) {}

        /// <summary>
        /// Override the number of Victory points
        /// "Worth 1 VP per Duchy you have"
        /// </summary>
        /// <param name="deck">The deck</param>
        /// <returns># Victory Points</returns>
        public override int GetNumVictoryPoints(DominionSim.VirtualCards.VirtualCardList deck)
        {
            return deck.Where(vc => vc.CardId == CardList.Duchy).Count();
        }
    }
}
