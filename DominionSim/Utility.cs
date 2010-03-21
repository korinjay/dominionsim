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

        /// <summary>
        /// Shuffle an arbitrary List and return another List of the items shuffled
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="list">List to shuffle</param>
        /// <returns>Shuffled version of the list</returns>
        public static List<T> Shuffle<T>(List<T> list)
        {
            if (sRandom == null)
            {
                sRandom = new Random();
            }
            List<T> shuffled = new List<T>();

            while (list.Count > 0)
            {
                int index = sRandom.Next(list.Count);
                T item = list[index];
                list.RemoveAt(index);
                shuffled.Add(item);
            }

            return shuffled;
        }

        public static IEnumerable<VirtualCard> FilterCardsByType(IEnumerable<VirtualCard> toFilter, Card.CardType type)
        {
            return toFilter.Where(c => (c.Logic.Type & type) != 0);
        }
    }
}
