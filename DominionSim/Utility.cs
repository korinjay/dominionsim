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

        public static int CountCardIn(CardIdentifier card, IEnumerable<CardIdentifier> inThis)
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

        public static IEnumerable<VirtualCard> FilterCardsByType(IEnumerable<VirtualCard> toFilter, Card.CardType type)
        {
            return toFilter.Where(c => (c.Logic.Type & type) != 0);
        }

        public static IEnumerable<CardIdentifier> FilterCardListByType(IEnumerable<CardIdentifier> toFilter, Card.CardType type)
        {
            return toFilter.Where(c => (c.Logic.Type & type) != 0);
        }
    }
}
