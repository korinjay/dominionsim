using DominionSim.VirtualCards;

namespace DominionSim.Stats
{
    class PlayerAction
    {
        public const string Buy = "Buy";
        public const string AddToHand = "Drew";
        public const string Gain = "Gain";
        public const string Trash = "Trash";
        public const string Play = "Play";
        public const string Discard = "Discard";
        public const string AttackedBy = "Attacked by";

        public int Turn;
        public string Action;
        public CardIdentifier Card;

        public PlayerAction(int turn, VirtualCard card, string action) : this(turn, card.CardId, action)
        {
        }

        public PlayerAction(int turn, CardIdentifier cardId, string action)
        {
            Turn = turn;
            Card = cardId;
            Action = action;
        }
    }
}
