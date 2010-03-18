using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    class CardList
    {
        public const string Copper = "Copper";
        public const string Silver = "Silver";
        public const string Gold = "Gold";

        public const string Estate = "Estate";
        public const string Duchy = "Duchy";
        public const string Province = "Province";
        public const string Curse = "Curse";

        public const string Adventurer = "Adventurer";
        public const string Cellar = "Cellar";
        public const string Chapel = "Chapel";
        public const string CouncilRoom = "CouncilRoom";
        public const string Feast = "Feast";
        public const string Festival = "Festival";
        public const string Laboratory = "Laboratory";
        public const string Market = "Market";
        public const string Militia = "Militia";
        public const string Mine = "Mine";
        public const string Moat = "Moat";
        public const string Remodel = "Remodel";
        public const string Smithy = "Smithy";
        public const string Spy = "Spy";
        public const string Thief = "Thief";
        public const string Village = "Village";
        public const string Woodcutter = "Woodcutter";
        public const string Workshop = "Workshop";


        public const string Harem = "Harem";


        public static Dictionary<string, Card> Cards;

        public static void SetupCardList()
        {
            Cards = new Dictionary<string,Card>();

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
                Cards.Add(c.Name, c);
            }

        } 
    }
}
