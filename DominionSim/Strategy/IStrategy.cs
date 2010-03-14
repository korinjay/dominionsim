using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    interface IStrategy
    {
        /// <summary>
        /// Take the "action" phase of this Player's turn.  Play any and all Action cards you wish to play.
        /// </summary>
        /// <param name="p">The Player who is using this Strategy</param>
        /// <param name="s">The Supply for this game</param>
        void TurnAction(PlayerFacade p, Supply s);

        /// <summary>
        /// Take the "buy" phase of this Player's turn.  Buy as many cards as you wish to buy.
        /// </summary>
        /// <param name="p">The Player who is using this Strategy</param>
        /// <param name="s">The Supply for this game</param>
        void TurnBuy(PlayerFacade p, Supply s);

        /// <summary>
        /// An Action (perhaps one you played) is forcing you to trash some cards!
        /// Choose which cards from your hand you would like to trash.
        /// </summary>
        /// <param name="p">The Player who is using this Strategy</param>
        /// <param name="min">Minimum number of cards you must trash</param>
        /// <param name="max">Maximum number of cards you may trash</param>
        /// <returns></returns>
        List<string> ChooseCardsToTrash(PlayerFacade p, int min, int max);
    }
}
