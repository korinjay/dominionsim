using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DominionSim.VirtualCards;

namespace DominionSim.Strategy
{
    

    class Chapel1 : Chapel
    {
        public Chapel1() : base(1) {}
        public static string GetDisplayName() { return "Chapel - Buy 1"; }
    }
    class Chapel2 : Chapel
    {
        public Chapel2() : base(2) {}
        public static string GetDisplayName() { return "Chapel - Buy 2"; }
    }


    abstract class Chapel : BuyOneCard
    {
        public Chapel(int numChapels) : base(CardList.Chapel, numChapels)
        {
        }

        public override IEnumerable<VirtualCard> ChooseCardsToTrash(PlayerFacade p, int min, int max, Card.CardType type, Supply s)
        {
            var toTrash = new VirtualCardList();

            int turn = p.GetTurn();

            // Could up all the money so we make sure we don't go too low
            var allTreasure = Utility.FilterCardsByType(p.GetDeck(), Card.CardType.Treasure);
            int totalMoney = 0;
            foreach (var t in allTreasure)
            {
                Card c = t.Logic;
                totalMoney += c.Moneys;
            }

            if (turn < 8)
            {
                // Trash all the estates we can
                var estateEnumer = p.GetHand().Where(vi => vi.CardId == CardList.Estate).GetEnumerator();
                while (estateEnumer.MoveNext() && toTrash.Count < max)
                {
                    toTrash.Add(estateEnumer.Current);
                }
            }
            // After that trash all the copper we can.  Make sure to keep at least money in the deck
            var copperEnumer = p.GetHand().Where(vi => vi.CardId == CardList.Copper).GetEnumerator();
            int numCopperTrashed = 0;
            while (copperEnumer.MoveNext() && toTrash.Count < max && (totalMoney - numCopperTrashed) > 3)
            {
                numCopperTrashed++;
                toTrash.Add(copperEnumer.Current);
            }

            return toTrash;
        }
    }
}
