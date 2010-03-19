using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    class Militia1 : BuyOneCard
    {
        public Militia1() : base(Card.Militia, 1) { }
        public static string GetDisplayName() { return "Militia - Buy 1"; }
    }
    class Militia2 : BuyOneCard
    {
        public Militia2() : base(Card.Militia, 2) { }
        public static string GetDisplayName() { return "Militia - Buy 2"; }
    }
}
