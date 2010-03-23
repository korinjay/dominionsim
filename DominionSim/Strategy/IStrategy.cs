using System.Collections.Generic;
using DominionSim.VirtualCards;

namespace DominionSim.Strategy
{
    interface IStrategy
    {
        /// <summary>
        /// Set up this strategy!  Some may want to inspect the initial state of the supply,
        /// register for events from players, or whatever.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="s"></param>
        void Init(PlayerFacade p, SupplyFacade s);

        /// <summary>
        /// Take the "action" phase of this Player's turn.  Play any and all Action cards you wish to play.
        /// </summary>
        /// <param name="p">The Player who is using this Strategy</param>
        /// <param name="s">The Supply for this game</param>
        void TurnAction(PlayerFacade p, SupplyFacade s);

        /// <summary>
        /// Take the "buy" phase of this Player's turn.  Buy as many cards as you wish to buy.
        /// </summary>
        /// <param name="p">The Player who is using this Strategy</param>
        /// <param name="s">The Supply for this game</param>
        void TurnBuy(PlayerFacade p, SupplyFacade s);

        /// <summary>
        /// An Action (perhaps one you played) is forcing you to trash some cards!
        /// Choose which cards from your hand you would like to trash.
        /// </summary>
        /// <param name="p">The Player who is using this Strategy</param>
        /// <param name="min">Minimum number of cards you must trash</param>
        /// <param name="max">Maximum number of cards you may trash</param>
        /// <returns>An enumeration of cards in hand to trash</returns>
        IEnumerable<VirtualCard> ChooseCardsToTrash(PlayerFacade p, int min, int max, Card.CardType type, SupplyFacade s);

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
        IEnumerable<VirtualCard> ChoosePlayerCardsToTrash(PlayerFacade p, int min, int max, string victim, IEnumerable<VirtualCard> cards);

        /// <summary>
        /// An Action (perhaps one you played) is forcing you to discard some cards!
        /// Choose which cards from your hand you would like to discard.
        /// </summary>
        /// <param name="p">Player who is using this strategy</param>
        /// <param name="min">Minimum number of cards you must discard</param>
        /// <param name="max">Maximum number of cards you may discard</param>
        /// <param name="s">An enumeration of cards in hand to discard</param>
        /// <returns></returns>
        IEnumerable<VirtualCard> ChooseCardsToDiscard(PlayerFacade p, int min, int max, Card.CardType type, SupplyFacade s);

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
        IEnumerable<VirtualCard> ChoosePlayerCardsToDiscard(PlayerFacade p, int min, int max, string victim, IEnumerable<VirtualCard> cards);

        /// <summary>
        /// An Action (perhaps one you played) is forcing you to gain a card.
        /// </summary>
        /// <param name="p">The Player who is using this Strategy</param>
        /// <param name="minCost">Minimum cost of the card</param>
        /// <param name="maxCost">Maximum cost of the card</param>
        /// <returns>Return the kind of card you wish to gain</returns>
        CardIdentifier ChooseCardToGainFromSupply(PlayerFacade p, int minCost, int maxCost, Card.CardType type, SupplyFacade s);

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
        IEnumerable<VirtualCard> ChoosePlayerCardsToGain(PlayerFacade p, int min, int max, string victim, IEnumerable<VirtualCard> cards);

        /// <summary>
        /// Someone attacked you, and now you need to react
        /// </summary>
        /// <param name="victimPlayerFacade">The Player who is getting hit</param>
        /// <param name="supply">The supply</param>
        /// <param name="attackerName">Name of the attacking player</param>
        /// <param name="cardId">Name of the attacking card</param>
        /// <returns>Return the list of cards you wish to react with</returns>
        IEnumerable<VirtualCard> ChooseReactionsToAttack(PlayerFacade victimPlayerFacade, SupplyFacade supply, string attackerName, CardIdentifier cardId);

        /// <summary>
        /// You have the opportunity to either draw a card or set it aside (i.e. from Library)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="card"></param>
        /// <returns>TRUE to set the card aside, FALSE to draw it</returns>
        bool ChooseToSetAsideCard(PlayerFacade p, VirtualCard card);

        /// <summary>
        /// You have the opportunity to play an Action card twice (i.e. due to Throne Room)
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="supply">The supply</param>
        /// <returns>Card from your hand to play twice</returns>
        VirtualCard ChooseCardToPlayTwice(PlayerFacade p, SupplyFacade s);

        /// <summary>
        /// You have to Reveal a Card - which would you like?
        /// </summary>
        /// <param name="p">Your player</param>
        /// <param name="supply">Current supply</param>
        /// <param name="cardType">Type of card you need to reveal</param>
        /// <param name="becauseOf">Why you are revealing it</param>
        /// <returns>The card</returns>
        VirtualCard ChooseCardToReveal(PlayerFacade p, SupplyFacade supply, Card.CardType cardType, CardIdentifier becauseOf);

        /// <summary>
        /// You have the opportunity to put your deck in your discard pile if you wish
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="s">The supply</param>
        /// <returns>TRUE to put your deck into discard, forcing a reshuffle</returns>
        bool ChooseToDiscardDrawPile(PlayerFacade p, SupplyFacade s);
    }
}
