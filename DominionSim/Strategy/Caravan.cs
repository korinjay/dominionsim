using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    class Caravan3 : BuyOneCard
    {
        public Caravan3() : base(CardList.Caravan, 3) { }
        public static string GetDisplayName() { return "Caravan - Buy 3"; }
    }

    class Caravan4 : BuyOneCard
    {
        public Caravan4() : base(CardList.Caravan, 4) { }
        public static string GetDisplayName() { return "Caravan - Buy 4"; }
    }
}
