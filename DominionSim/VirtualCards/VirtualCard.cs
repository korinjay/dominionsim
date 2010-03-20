using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.VirtualCards
{
    /// <summary>
    /// Class to create actual Instances of Cards in our various Lists (Supplies, Hands, etc.).
    /// Currently, all our Card logic are in Card classes, each of which is a singletone.
    /// VirtualCards are simply barebones class instance that wrap the singletone Card class (accessible
    /// via the Logic property).  This makes it so I can, bit by bit, move toward having Cards
    /// as actual honest-to-goodness OO classes rather than (basically) bags of global functions.
    /// </summary>
    class VirtualCard
    {
        /// <summary>
        /// Card identifier this wraps
        /// </summary>
        public CardIdentifier CardId { get; private set; }

        /// <summary>
        /// Logic associated with this Card
        /// </summary>
        public Card Logic
        {
            get { return CardId.Logic; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cardId">Identifier of the card this will talk to</param>
        public VirtualCard(CardIdentifier cardId)
        {
            CardId = cardId;
        }

        /// <summary>
        /// Override ToString
        /// </summary>
        /// <returns>Name of the card</returns>
        public override string ToString()
        {
            return CardId.ToString();
        }
    }
}
