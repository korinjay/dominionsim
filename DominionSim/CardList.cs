using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    /// <summary>
    /// Class to identify a Card.  Example CardIdentifier: "Estate", "Moat", or "Shanty Town".
    /// This class is part of a stepwise conversion to get type safety around Card Ids, rather
    /// than using "string" everywhere.
    /// 
    /// Actually, I think I like this better than an enum.  This might be enough for me!
    /// </summary>
    class CardIdentifier
    {
        /// <summary>
        /// Internal storage of the name of the Card, eg "Militia"
        /// </summary>
        private String mId;

        /// <summary>
        /// Constructor, must pass a String identifier
        /// </summary>
        /// <param name="id"></param>
        public CardIdentifier(String id)
        {
            mId = id;
        }

        /// <summary>
        /// Implicit cast from a string to a CardIdentifier, 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static implicit operator CardIdentifier(string s)
        {
            return new CardIdentifier(s);
        }

        /// <summary>
        /// Overriding ToString
        /// </summary>
        /// <returns>String for this CardIdentifier</returns>
        public override string ToString()
        {
            return mId;
        }

        /// <summary>
        /// Overriding equality
        /// </summary>
        /// <param name="obj">Comparison</param>
        /// <returns>true if equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is CardIdentifier)
            {
                return (obj as CardIdentifier).mId.Equals(mId);
            }
            return false;
        }

        /// <summary>
        /// OVerriding HashCodes
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return mId.GetHashCode();
        }
    };

    class CardList
    {
        public static readonly CardIdentifier Copper = "Copper";
        public static readonly CardIdentifier Silver = "Silver";
        public static readonly CardIdentifier Gold = "Gold";

        public static readonly CardIdentifier Estate = "Estate";
        public static readonly CardIdentifier Duchy = "Duchy";
        public static readonly CardIdentifier Province = "Province";
        public static readonly CardIdentifier Curse = "Curse";

        // Original Dominion
        public static readonly CardIdentifier Adventurer = "Adventurer";
        public static readonly CardIdentifier Bureaucrat = "Bureaucrat";      // Not implemented
        public static readonly CardIdentifier Cellar = "Cellar";
        public static readonly CardIdentifier Chancellor = "Chancellor";      // Not implemented
        public static readonly CardIdentifier Chapel = "Chapel";
        public static readonly CardIdentifier CouncilRoom = "CouncilRoom";
        public static readonly CardIdentifier Feast = "Feast";
        public static readonly CardIdentifier Festival = "Festival";
        public static readonly CardIdentifier Gardens = "Gardens";            // Not implemented
        public static readonly CardIdentifier Laboratory = "Laboratory";
        public static readonly CardIdentifier Library = "Library";
        public static readonly CardIdentifier Market = "Market";
        public static readonly CardIdentifier Militia = "Militia";
        public static readonly CardIdentifier Mine = "Mine";
        public static readonly CardIdentifier Moat = "Moat";
        public static readonly CardIdentifier Moneylender = "Moneylender";
        public static readonly CardIdentifier Remodel = "Remodel";
        public static readonly CardIdentifier Smithy = "Smithy";
        public static readonly CardIdentifier Spy = "Spy";
        public static readonly CardIdentifier Thief = "Thief";
        public static readonly CardIdentifier ThroneRoom = "ThroneRoom";      // Not implemented
        public static readonly CardIdentifier Village = "Village";
        public static readonly CardIdentifier Witch = "Witch";
        public static readonly CardIdentifier Woodcutter = "Woodcutter";
        public static readonly CardIdentifier Workshop = "Workshop";

        // Intrigue
        public static readonly CardIdentifier Harem = "Harem";


        public static Dictionary<CardIdentifier, Card> Cards;

        public static void SetupCardList()
        {
            Cards = new Dictionary<CardIdentifier,Card>();

            // Use cool C# runtime type info and reflection stuff to find all non-abstract classes in all loaded
            // assemblies that inherit from IStrategy, and dump them into my simple StrategyTypeHolder class that
            // we can throw into a ComboBox
            var inheritType = typeof(Card);

            var cardList = AppDomain.CurrentDomain.GetAssemblies().ToList()   // List of all loaded assemblies (this exe, dlls)
                .SelectMany(assemblies => assemblies.GetTypes())              // Convert that list to a list of all loaded Types from each Assembly
                .Where(type => inheritType.IsAssignableFrom(type) &&          // Only pick out subclasses of Card
                       !type.IsAbstract)                                      // That are not abstract
                .Select(type => Activator.CreateInstance(type) as Card);      // Now return a list of instances of each type

            // For each card we found add it to the big lookup table by name
            foreach (Card c in cardList)
            {
                Cards.Add(c.CardId, c);
            }

        } 
    }
}
