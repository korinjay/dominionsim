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
        /// <returns>An enumeration of cards in hand to trash</returns>
        IEnumerable<string> ChooseCardsToTrash(PlayerFacade p, int min, int max, Card.CardType type, Supply s);

        /// <summary>
        /// An Action (perhaps one you played) is forcing you to discard some cards!
        /// Choose which cards from your hand you would like to discard.
        /// </summary>
        /// <param name="p">Player who is using this strategy</param>
        /// <param name="min">Minimum number of cards you must discard</param>
        /// <param name="max">Maximum number of cards you may discard</param>
        /// <param name="s">An enumeration of cards in hand to discard</param>
        /// <returns></returns>
        IEnumerable<string> ChooseCardsToDiscard(PlayerFacade p, int min, int max, Card.CardType type, Supply s);

        /// <summary>
        /// An Action (perhaps one you played) is forcing you to gain a card.
        /// </summary>
        /// <param name="p">The Player who is using this Strategy</param>
        /// <param name="minCost">Minimum cost of the card</param>
        /// <param name="maxCost">Maximum cost of the card</param>
        /// <returns>Return the kind of card you wish to gain</returns>
        string ChooseCardToGain(PlayerFacade p, int minCost, int maxCost, Card.CardType type, Supply s);
    }
}
