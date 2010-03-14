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

        public const string Smithy = "Smithy";

        public static Dictionary<string, Card> Cards;

        public static void SetupCardList()
        {
            Cards = new Dictionary<string,Card>();

            // Treasure
            Cards.Add(Copper, new Card(Copper, Card.CardType.Treasure, 0, 0, 0, 1, 0, 0));
            Cards.Add(Silver, new Card(Silver, Card.CardType.Treasure, 3, 0, 0, 2, 0, 0));
            Cards.Add(Gold, new Card(Gold, Card.CardType.Treasure, 6, 0, 0, 3, 0, 0));

            
            // VPs
            Cards.Add(Estate, new Card(Estate, Card.CardType.Victory, 2, 0, 0, 0, 0, 1));
            Cards.Add(Duchy,  new Card(Duchy, Card.CardType.Victory, 5, 0, 0, 0, 0, 3));
            
            Cards.Add(Province, new Card(Province, Card.CardType.Victory, 8, 0, 0, 0, 0, 6));
            Cards.Add(Curse, new Card(Curse, Card.CardType.Victory, 0, 0, 0, 0, 0, -1));

            // Original Dominion
            Cards.Add(Smithy, new Card(Smithy, Card.CardType.Action, 4, 3, 0, 0, 0, 0));
        } 
    }
}
