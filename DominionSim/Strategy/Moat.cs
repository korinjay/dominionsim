using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    class Moat1 : BuyOneCard
    {
        public Moat1() : base(Card.Moat, 1) { }
        public static string GetDisplayName() { return "Moat - Buy 1"; }
    }
    class Moat2 : BuyOneCard
    {
        public Moat2() : base(Card.Moat, 2) { }
        public static string GetDisplayName() { return "Moat - Buy 2"; }
    }
    class Moat3 : BuyOneCard
    {
        public Moat3() : base(Card.Moat, 3) { }
        public static string GetDisplayName() { return "Moat - Buy 3"; }
    }
}
