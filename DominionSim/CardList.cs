using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Get at the Card logic assocaited with this instance
        /// </summary>
        public Card Logic
        {
            get { return CardList.GetLogic(this); }
        }

        /// <summary>
        /// Constructor, must pass a String identifier
        /// </summary>
        /// <param name="id"></param>
        public CardIdentifier(String id)
        {
            mId = id;
        }

        /// <summary>
        /// Explicit cast from a string to a CardIdentifier, 
        /// </summary>
        /// <param name="s">a string</param>
        /// <returns>CardIdentifier for that string</returns>
        public static explicit operator CardIdentifier(string s)
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
        public static readonly CardIdentifier Copper = (CardIdentifier)"Copper";
        public static readonly CardIdentifier Silver = (CardIdentifier)"Silver";
        public static readonly CardIdentifier Gold = (CardIdentifier)"Gold";

        public static readonly CardIdentifier Estate = (CardIdentifier)"Estate";
        public static readonly CardIdentifier Duchy = (CardIdentifier)"Duchy";
        public static readonly CardIdentifier Province = (CardIdentifier)"Province";
        public static readonly CardIdentifier Curse = (CardIdentifier)"Curse";

        // Original Dominion
        public static readonly CardIdentifier Adventurer = (CardIdentifier)"Adventurer";
        public static readonly CardIdentifier Bureaucrat = (CardIdentifier)"Bureaucrat";      // Not implemented
        public static readonly CardIdentifier Cellar = (CardIdentifier)"Cellar";
        public static readonly CardIdentifier Chancellor = (CardIdentifier)"Chancellor";
        public static readonly CardIdentifier Chapel = (CardIdentifier)"Chapel";
        public static readonly CardIdentifier CouncilRoom = (CardIdentifier)"Council Room";
        public static readonly CardIdentifier Feast = (CardIdentifier)"Feast";
        public static readonly CardIdentifier Festival = (CardIdentifier)"Festival";
        public static readonly CardIdentifier Gardens = (CardIdentifier)"Gardens";
        public static readonly CardIdentifier Laboratory = (CardIdentifier)"Laboratory";
        public static readonly CardIdentifier Library = (CardIdentifier)"Library";
        public static readonly CardIdentifier Market = (CardIdentifier)"Market";
        public static readonly CardIdentifier Militia = (CardIdentifier)"Militia";
        public static readonly CardIdentifier Mine = (CardIdentifier)"Mine";
        public static readonly CardIdentifier Moat = (CardIdentifier)"Moat";
        public static readonly CardIdentifier Moneylender = (CardIdentifier)"Moneylender";
        public static readonly CardIdentifier Remodel = (CardIdentifier)"Remodel";
        public static readonly CardIdentifier Smithy = (CardIdentifier)"Smithy";
        public static readonly CardIdentifier Spy = (CardIdentifier)"Spy";
        public static readonly CardIdentifier Thief = (CardIdentifier)"Thief";
        public static readonly CardIdentifier ThroneRoom = (CardIdentifier)"Throne Room";
        public static readonly CardIdentifier Village = (CardIdentifier)"Village";
        public static readonly CardIdentifier Witch = (CardIdentifier)"Witch";
        public static readonly CardIdentifier Woodcutter = (CardIdentifier)"Woodcutter";
        public static readonly CardIdentifier Workshop = (CardIdentifier)"Workshop";

        // Intrigue
        public static readonly CardIdentifier SecretChamber = (CardIdentifier)"Secret Chamber";     // NYI
        public static readonly CardIdentifier Baron = (CardIdentifier)"Baron";                      // NYI
        public static readonly CardIdentifier Bridge = (CardIdentifier)"Bridge";                    // NYI
        public static readonly CardIdentifier Conspirator = (CardIdentifier)"Conspirator";          // NYI
        public static readonly CardIdentifier Coppersmith = (CardIdentifier)"Coppersmith";          // NYI
        public static readonly CardIdentifier Courtyard = (CardIdentifier)"Courtyard";              // NYI
        public static readonly CardIdentifier Ironworks = (CardIdentifier)"Ironworks";
        public static readonly CardIdentifier Maskquerade = (CardIdentifier)"Maskquerade";          // NYI
        public static readonly CardIdentifier MiningVillage = (CardIdentifier)"Mining Village";     // NYI
        public static readonly CardIdentifier Minion = (CardIdentifier)"Minion";                    // NYI
        public static readonly CardIdentifier Pawn = (CardIdentifier)"Pawn";                        // NYI
        public static readonly CardIdentifier Saboteur = (CardIdentifier)"Saboteur";                // NYI
        public static readonly CardIdentifier Scout = (CardIdentifier)"Scout";                      // NYI
        public static readonly CardIdentifier ShantyTown = (CardIdentifier)"Shanty Town";
        public static readonly CardIdentifier Steward = (CardIdentifier)"Steward";                  // NYI
        public static readonly CardIdentifier Swindler = (CardIdentifier)"Swindler";                // NYI
        public static readonly CardIdentifier Torturer = (CardIdentifier)"Torturer";                // NYI
        public static readonly CardIdentifier TradingPost = (CardIdentifier)"Trading Post";         // NYI
        public static readonly CardIdentifier Tribute = (CardIdentifier)"Tribute";                  // NYI
        public static readonly CardIdentifier Upgrade = (CardIdentifier)"Upgrade";                  // NYI
        public static readonly CardIdentifier WishingWell = (CardIdentifier)"Wishing Well";         // NYI
        public static readonly CardIdentifier GreatHall = (CardIdentifier)"Great Hall";
        public static readonly CardIdentifier Nobles = (CardIdentifier)"Nobles";                    // NYI
        public static readonly CardIdentifier Harem = (CardIdentifier)"Harem";                      // NYI
        public static readonly CardIdentifier Duke = (CardIdentifier)"Duke";

        // Seaside
        public static readonly CardIdentifier Ambassador = (CardIdentifier)"Ambassador";            // NYI
        public static readonly CardIdentifier Bazaar = (CardIdentifier)"Bazaar";                    // NYI
        public static readonly CardIdentifier Caravan = (CardIdentifier)"Caravan";
        public static readonly CardIdentifier Cutpurse = (CardIdentifier)"Cutpurse";                // NYI
        public static readonly CardIdentifier Embargo = (CardIdentifier)"Embargo";                  // NYI
        public static readonly CardIdentifier Explorer = (CardIdentifier)"Explorer";                // NYI
        public static readonly CardIdentifier FishingVillage = (CardIdentifier)"Fishing Village";
        public static readonly CardIdentifier GhostShip = (CardIdentifier)"Ghost Ship";             // NYI
        public static readonly CardIdentifier Haven = (CardIdentifier)"Haven";                      // NYI
        public static readonly CardIdentifier Island = (CardIdentifier)"Island";                    // NYI
        public static readonly CardIdentifier Lighthouse = (CardIdentifier)"Lighthouse";
        public static readonly CardIdentifier Lookout = (CardIdentifier)"Lookout";                  // NYI
        public static readonly CardIdentifier MerchantShip = (CardIdentifier)"Merchant Ship";
        public static readonly CardIdentifier NativeVillage = (CardIdentifier)"Native Village";     // NYI
        public static readonly CardIdentifier Navigator = (CardIdentifier)"Navigator";              // NYI
        public static readonly CardIdentifier Outpost = (CardIdentifier)"Outpost";                  // NYI
        public static readonly CardIdentifier PearlDiver = (CardIdentifier)"Pearl Diver";           // NYI
        public static readonly CardIdentifier PirateShip = (CardIdentifier)"Pirate Ship";           // NYI
        public static readonly CardIdentifier Salvager = (CardIdentifier)"Salvager";                // NYI
        public static readonly CardIdentifier SeaHag = (CardIdentifier)"Sea Hag";                   // NYI
        public static readonly CardIdentifier Smugglers = (CardIdentifier)"Smugglers";              // NYI
        public static readonly CardIdentifier Tactician = (CardIdentifier)"Tactician";              // NYI, subtly difficult due to an "if"
        public static readonly CardIdentifier TreasureMap = (CardIdentifier)"Treasure Map";         // NYI
        public static readonly CardIdentifier Treasury = (CardIdentifier)"Treasury";                // NYI
        public static readonly CardIdentifier Warehouse = (CardIdentifier)"Warehouse";              // NYI
        public static readonly CardIdentifier Wharf = (CardIdentifier)"Wharf";



        private static Dictionary<CardIdentifier, Card> Cards;

        public static void SetupCardList()
        {
            Cards = new Dictionary<CardIdentifier,Card>();

            // Use cool C# runtime type info and reflection stuff to find all non-abstract classes in all loaded
            // assemblies that inherit from IStrategy, and dump them into my simple StrategyTypeHolder class that
            // we can throw into a ComboBox
            var inheritType = typeof(Card);

            var cardList = AppDomain.CurrentDomain.GetAssemblies().ToList()   // List of all loaded assemblies (this exe, dlls)
                .SelectMany(assembly => assembly.GetTypes())              // Convert that list to a list of all loaded Types from each Assembly
                .Where(type => inheritType.IsAssignableFrom(type) &&          // Only pick out subclasses of Card
                       !type.IsAbstract)                                      // That are not abstract
                .Select(type => Activator.CreateInstance(type) as Card);      // Now return a list of instances of each type

            // For each card we found add it to the big lookup table by name
            foreach (Card c in cardList)
            {
                Cards.Add(c.CardId, c);
            }
        }

        /// <summary>
        /// Simple wrapper for Cards so I can really quickly find all Compiler errors and fix them.
        /// We should go through CardIdentifier to get at the card logic.
        /// </summary>
        /// <param name="cardId">Card identifier</param>
        /// <returns>Actual Card logic associated with that CardIdentifier</returns>
        public static Card GetLogic(CardIdentifier cardId)
        {
            return Cards[cardId];
        }

        /// <summary>
        /// Return a list of all the CardIdentifiers we have registered
        /// </summary>
        /// <returns>A bunch of CardIdentifiers</returns>
        public static IEnumerable<CardIdentifier> GetAllCardIds()
        {
            return Cards.Select(kvp => kvp.Key);
        }
    }
}
