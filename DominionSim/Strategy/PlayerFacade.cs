using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    /// <summary>
    /// Wrapper class exposing only certain functionality of a Player to a Strategy
    /// This is the only object that Strategies can manipulate in order to make stuff happen in the game.
    /// </summary>
    class PlayerFacade
    {
        /// <summary>
        /// The Player this instance is wrapping
        /// </summary>
        Player mPlayer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p">Player to wrap</param>
        public PlayerFacade(Player p)
        {
            mPlayer = p;
        }

        /// <summary>
        /// Utility function to make a shallow copy of a list of cards
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        private List<string> CopyList(List<string> original)
        {
            List<string> copy = new List<string>();
            for (int i = 0; i < original.Count; i++)
            {
                copy.Add(original[i]);
            }
            return copy;
        }

        /// <summary>
        /// Return a copy of the Player's Hand, so that there's no chance the actual Hand can be manipulated
        /// </summary>
        /// <returns></returns>
        public List<string> GetHand()
        {
            return CopyList(mPlayer.Hand);
        }

        /// <summary>
        /// Return a copy of the Player's Deck, so that there's no chance the actual Deck can by manipulated
        /// </summary>
        /// <returns></returns>
        public List<string> GetDeck()
        {
            return CopyList(mPlayer.Deck);
        }

        #region Passthrough Functions


        public int GetMoneys()
        {
            return mPlayer.Moneys;
        }

        public int GetActions()
        {
            return mPlayer.Actions;
        }

        public int GetBuys()
        {
            return mPlayer.Buys;
        }

        public void PlayActionCard(string name)
        {
            mPlayer.PlayActionCard(name);
        }

        public void BuyCard(string name)
        {
            mPlayer.BuyCard(name);
        }

        #endregion
    }
}
