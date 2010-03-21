using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DominionSim.VirtualCards;

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

        public static IEnumerable<VirtualCard> FilterCardsByType(IEnumerable<VirtualCard> toFilter, Card.CardType type)
        {
            return toFilter.Where(c => (c.Logic.Type & type) != 0);
        }
    }
}
