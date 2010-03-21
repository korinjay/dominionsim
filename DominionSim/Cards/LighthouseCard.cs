using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class LighthouseCard : Card
    {
        public LighthouseCard() : base(CardList.Lighthouse, ActionDuration, 5, 2, 0, 0, 1, 0, 2, 0, 0, 1)
        {}

        /// <summary>
        /// When this card is in play, attacks do not affect you
        /// </summary>
        /// <param name="attacker">Attacker</param>
        /// <param name="victim">Victim of the attack</param>
        /// <param name="supply">Supply</param>
        /// <returns>True if the attack was blocked</returns>
        public override bool ExecuteDurationReaction(Player attacker, Player victim, Supply supply)
        {
            return true;
        }
    }
}
