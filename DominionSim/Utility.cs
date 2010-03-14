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

        public static int CountCardIn(string card, List<string> inThis)
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

        public static List<string> FilterCardListByType(List<string> toFilter, Card.CardType type)
        {
            List<string> list = new List<string>();
            foreach (string name in toFilter)
            {
                Card c = CardList.Cards[name];
                if ((type & c.Type) != 0)
                {
                    list.Add(name);
                }
            }
            return list;
        }
    }
}
