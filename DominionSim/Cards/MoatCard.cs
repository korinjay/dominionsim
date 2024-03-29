﻿
namespace DominionSim.Cards
{
    class MoatCard : Card
    {
        public MoatCard() : base(CardList.Moat, ReactionAction, 2, 2, 0, 0, 0, 0)
        {
        }

        /// <summary>
        /// Perform a reaction
        /// </summary>
        /// <param name="attacker">One performing the attack</param>
        /// <param name="victim">One getting hit by the attack</param>
        /// <param name="supply">The supply</param>
        /// <returns>True if the attack was successfully prevented from affecting the victim</returns>
        public override bool ExecuteReaction(Player attacker, Player victim, Supply supply)
        {
            return true;
        }
    }
}
