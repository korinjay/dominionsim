using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Stats
{
    using CardIdentifier = String;

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

        public PlayerAction(int turn, CardIdentifier card, string action)
        {
            Turn = turn;
            Card = card;
            Action = action;
        }
    }
}
