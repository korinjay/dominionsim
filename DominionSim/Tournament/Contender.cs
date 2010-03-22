using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Tournament
{
    /// <summary>
    /// Simple class to hold an instance of a Strategy and its Name so it can take place in a Tournement.
    /// Used to fill a Roster in a Tournament class.
    /// </summary>
    class Contender
    {
        /// <summary>
        /// Name of this Strategy
        /// </summary>
        public string Name;

        /// <summary>
        /// Instance of this Strategy
        /// </summary>
        public Strategy.IStrategy Strategy;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">What to call the contender</param>
        /// <param name="strat">Strategy instance</param>
        public Contender(string name, Strategy.IStrategy strat)
        {
            Name = name;
            Strategy = strat;
        }
    }
}
