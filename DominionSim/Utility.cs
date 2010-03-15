using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    class Utility
    {
        private static Random sRandom;

        public static int RandBetween(int min, int max)
        {
            return sRandom.Next(min, max);
        }

        public static List<T> Shuffle<T>(List<T> deck)
        {
            if (sRandom == null)
            {
                sRandom = new Random();
            }
            List<T> shuffled = new List<T>();

            while (deck.Count > 0)
            {
                int index = sRandom.Next(deck.Count);
                T card = deck[index];
                deck.RemoveAt(index);
                shuffled.Add(card);
            }

            return shuffled;
        }

        public static int CountCardIn(string card, IEnumerable<string> inThis)
        {
            int numCard = 0;
            var g = inThis.GroupBy(name => name);

            foreach (var grp in g)
            {
                if (grp.Key == card)
                {
                    numCard = grp.Count();
                }
            }

            return numCard;
        }

        public static IEnumerable<string> FilterCardListByType(IEnumerable<string> toFilter, Card.CardType type)
        {
            return toFilter.Where(c => (CardList.Cards[c].Type & type) != 0);
        }
    }
}
