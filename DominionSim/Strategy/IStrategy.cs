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
        /// An Action (perhaps one you played) is asking you to trash some of your victim's cards!
        /// From the collection provided, choose which to trash
        /// </summary>
        /// <param name="p">The Player who is using this Strategy</param>
        /// <param name="min">Minimum number of cards you must trash</param>
        /// <param name="max">Maximum number of cards you may trash</param>
        /// <param name="victim">Name of the victim whose cards you are trashing</param>
        /// <param name="cards">Collection of cards to choose from</param>
        /// <returns>An enumeration of cards from the provided collection to trash</returns>
        IEnumerable<string> ChoosePlayerCardsToTrash(PlayerFacade p, int min, int max, string victim, IEnumerable<string> cards);

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
        /// An Action (perhaps one you played) is letting you make an victim discard some cards!
        /// Choose which ones from the collection you would like them to discard.
        /// </summary>
        /// <param name="p">Player who is using this strategy</param>
        /// <param name="min">Minimum number of cards you must make them discard</param>
        /// <param name="max">Maximum number of cards you may make them discard</param>
        /// <param name="victim">Name of the victim you're screwing</param>
        /// <param name="cards">Collection of cards to choose from</param>
        /// <returns>Collection of cards to discard</returns>
        IEnumerable<string> ChoosePlayerCardsToDiscard(PlayerFacade p, int min, int max, string victim, IEnumerable<string> cards);

        /// <summary>
        /// An Action (perhaps one you played) is forcing you to gain a card.
        /// </summary>
        /// <param name="p">The Player who is using this Strategy</param>
        /// <param name="minCost">Minimum cost of the card</param>
        /// <param name="maxCost">Maximum cost of the card</param>
        /// <returns>Return the kind of card you wish to gain</returns>
        string ChooseCardToGainFromSupply(PlayerFacade p, int minCost, int maxCost, Card.CardType type, Supply s);

        /// <summary>
        /// An Action (perhaps one you played) is allowing you to
        /// gain a card from your victim.
        /// </summary>
        /// <param name="p">The Player who is using this Strategy</param>
        /// <param name="min">Minimum number of cards you must gain from the collection</param>
        /// <param name="max">Maximum number of cards you may gain from the collection</param>
        /// <param name="victim">Name of the victim you're gaining cards from</param>
        /// <param name="cards">Collection of cards to gain from</param>
        /// <returns>All cards you wish to gain from the collection</returns>
        IEnumerable<string> ChoosePlayerCardsToGain(PlayerFacade p, int min, int max, string victim, IEnumerable<string> cards);


        /// <summary>
        /// Someone attacked you, and now you need to react
        /// </summary>
        /// <param name="victimPlayerFacade">The Player who is getting hit</param>
        /// <param name="supply">The supply</param>
        /// <param name="attackerName">Name of the attacking player</param>
        /// <param name="cardName">Name of the attacking card</param>
        /// <returns>Return the list of cards you wish to react with</returns>
        IEnumerable<string> ChooseReactionsToAttack(PlayerFacade victimPlayerFacade, Supply supply, string attackerName, string cardName);
    }
}
