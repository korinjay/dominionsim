using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    class CardList
    {
        public static Card Copper = new Card("Copper", Card.CardType.Treasure, 0, 0, 0, 1, 0, 0);
        public static Card Silver = new Card("Silver", Card.CardType.Treasure, 3, 0, 0, 2, 0, 0);
        public static Card Gold = new Card("Gold", Card.CardType.Treasure, 6, 0, 0, 3, 0, 0);

        public static Card Estate = new Card("Estate", Card.CardType.Victory, 2, 0, 0, 0, 0, 1);
        public static Card Duchy = new Card("Duchy", Card.CardType.Victory, 5, 0, 0, 0, 0, 3);
        public static Card Province = new Card("Province", Card.CardType.Victory, 8, 0, 0, 0, 0, 6);
    }
}
