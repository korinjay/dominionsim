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
        public const string Chapel = "Chapel";
        public const string Workshop = "Workshop";
        public const string Feast = "Feast";
        public const string Cellar = "Cellar";

        public const string Militia = "Militia";
        public const string Moat = "Moat";

        public const string Harem = "Harem";


        public static Dictionary<string, Card> Cards;

        public static void SetupCardList()
        {
            Cards = new Dictionary<string,Card>();

            // Treasure
            Cards.Add(Copper, new CopperCard());
            Cards.Add(Silver, new SilverCard());
            Cards.Add(Gold, new GoldCard());

            // VPs
            Cards.Add(Estate, new EstateCard());
            Cards.Add(Duchy,  new DuchyCard());
            
            Cards.Add(Province, new ProvinceCard());
            Cards.Add(Curse, new CurseCard());

            // Original Dominion
            Cards.Add(Smithy, new SmithyCard());
            Cards.Add(Chapel, new Cards.ChapelCard());
            Cards.Add(Workshop, new Cards.WorkshopCard());
            Cards.Add(Feast, new Cards.FeastCard());
            Cards.Add(Cellar, new Cards.CellarCard());

            // Workin' on it - Colin
            //Cards.Add(Militia, new Cards.MilitiaCard());
            //Cards.Add(Moat, new Cards.MoatCard());
        } 
    }
}
