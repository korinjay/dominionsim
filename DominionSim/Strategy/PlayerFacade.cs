using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DominionSim.VirtualCards;

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
        /// Return a copy of the Player's Hand, so that there's no chance the actual Hand can be manipulated
        /// </summary>
        /// <returns>Cards in hand</returns>
        public VirtualCardList GetHand()
        {
            return mPlayer.Hand.AsReadOnly();
        }

        /// <summary>
        /// Return a copy of the Player's Deck, so that there's no chance the actual Deck can by manipulated
        /// </summary>
        /// <returns>Cards in deck</returns>
        public VirtualCardList GetDeck()
        {
            return mPlayer.Deck.AsReadOnly();
        }

        #region Passthrough Functions

        public string GetName()
        {
            return mPlayer.Name;
        }

        public int GetTurn()
        {
            return mPlayer.GetTurn();
        }

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

        public void PlayActionCard(VirtualCard card)
        {
            mPlayer.PlayActionCard(card);
        }

        public void BuyCard(CardIdentifier name)
        {
            mPlayer.BuyCard(name);
        }

        public void Log(string msg)
        {
            mPlayer.Log(msg);
        }

        #endregion
    }
}
