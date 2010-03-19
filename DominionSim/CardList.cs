using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    enum Card
    {
        None,
        // Common
        Copper,
        Silver,
        Gold,
        Estate,
        Duchy,
        Province,
        Curse,
        // Original Dominion
        Adventurer,
        Bureaucrat,
        Cellar,
        Chancellor,
        Chapel,
        CouncilRoom,
        Feast,
        Festival,
        Gardens,   
        Laboratory,
        Library,   
        Market,
        Militia,
        Mine,
        Moat,
        Moneylender,
        Remodel,
        Smithy,
        Spy,        
        Thief,
        ThroneRoom, 
        Village,
        Witch,
        Woodcutter,
        Workshop,
        // Intrigue
        Harem,
    };

    class CardList
    {
        public static Dictionary<Card, CardBase> Cards;

        public static void SetupCardList()
        {
            Cards = new Dictionary<Card, CardBase>();

            // Use cool C# runtime type info and reflection stuff to find all non-abstract classes in all loaded
            // assemblies that inherit from IStrategy, and dump them into my simple StrategyTypeHolder class that
            // we can throw into a ComboBox
            var inheritType = typeof(CardBase);

            var cardList = AppDomain.CurrentDomain.GetAssemblies().ToList()   // List of all loaded assemblies (this exe, dlls)
                .SelectMany(assembly => assembly.GetTypes())              // Convert that list to a list of all loaded Types from each Assembly
                .Where(type => inheritType.IsAssignableFrom(type) &&          // Only pick out subclasses of Card
                       !type.IsAbstract)                                      // That are not abstract
                .Select(type => Activator.CreateInstance(type) as CardBase);  // Now return a list of instances of each type

            // For each card we found add it to the big lookup table by name
            foreach (var c in cardList)
            {
                Cards.Add(c.Card, c);
            }

        } 
    }
}
