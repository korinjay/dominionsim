using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    class Caravan : BuyOneCard
    {
        public Caravan() : base(CardList.Caravan, 10)
        {
        }
    }
}
