using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    // Stopgap - pretty much everywhere in the universe we are using "string" as the type
    // for cards identifiers.  That's bad; we should basically be using *anything else* but
    // a hard primitive.  Putting this here in the hope that we can slowly migrate to a more
    // sensible type.  This will probably be in a few more places as well, without the comment.
    using CardIdentifier = String;

    class CardList
    {
        public const CardIdentifier Copper = "Copper";
        public const CardIdentifier Silver = "Silver";
        public const CardIdentifier Gold = "Gold";

        public const CardIdentifier Estate = "Estate";
        public const CardIdentifier Duchy = "Duchy";
        public const CardIdentifier Province = "Province";
        public const CardIdentifier Curse = "Curse";

        // Original Dominion
        public const CardIdentifier Adventurer = "Adventurer";
        public const CardIdentifier Bureaucrat = "Bureaucrat";      // Not implemented
        public const CardIdentifier Cellar = "Cellar";
        public const CardIdentifier Chancellor = "Chancellor";      // Not implemented
        public const CardIdentifier Chapel = "Chapel";
        public const CardIdentifier CouncilRoom = "CouncilRoom";
        public const CardIdentifier Feast = "Feast";
        public const CardIdentifier Festival = "Festival";
        public const CardIdentifier Gardens = "Gardens";            // Not implemented
        public const CardIdentifier Laboratory = "Laboratory";
        public const CardIdentifier Library = "Library";
        public const CardIdentifier Market = "Market";
        public const CardIdentifier Militia = "Militia";
        public const CardIdentifier Mine = "Mine";
        public const CardIdentifier Moat = "Moat";
        public const CardIdentifier Moneylender = "Moneylender";
        public const CardIdentifier Remodel = "Remodel";
        public const CardIdentifier Smithy = "Smithy";
        public const CardIdentifier Spy = "Spy";
        public const CardIdentifier Thief = "Thief";
        public const CardIdentifier ThroneRoom = "ThroneRoom";      // Not implemented
        public const CardIdentifier Village = "Village";
        public const CardIdentifier Witch = "Witch";
        public const CardIdentifier Woodcutter = "Woodcutter";
        public const CardIdentifier Workshop = "Workshop";

        // Intrigue
        public const CardIdentifier Harem = "Harem";


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
