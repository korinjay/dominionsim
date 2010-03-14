using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    class Utility
    {
        private static Random sRandom;

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
    }
}
