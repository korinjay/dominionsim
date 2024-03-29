﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Strategy
{
    delegate void OpponentCardEventHandler(OpponentFacade sender, CardIdentifier card);

    class OpponentFacade
    {
        public event OpponentCardEventHandler Gained;
        public event OpponentCardEventHandler Bought;
        public event OpponentCardEventHandler Discarded;
        public event OpponentCardEventHandler Played;
        public event OpponentCardEventHandler Revealed;
        public event OpponentCardEventHandler Trashed;

        private Player mPlayer;

        public OpponentFacade(Player p)
        {
            mPlayer = p;
            mPlayer.Gained += new DominionSim.PlayerCardActionEventHandler(ForwardGained);
            mPlayer.Bought += new DominionSim.PlayerCardActionEventHandler(ForwardBought);
            mPlayer.Discarded += new DominionSim.PlayerCardActionEventHandler(ForwardDiscarded);
            mPlayer.Played += new DominionSim.PlayerCardActionEventHandler(ForwardPlayed);
            mPlayer.Revealed += new DominionSim.PlayerCardActionEventHandler(ForwardRevealed);
            mPlayer.Trashed += new DominionSim.PlayerCardActionEventHandler(ForwardTrashed);
        }

        public string GetName()
        {
            return mPlayer.Name;
        }

        #region Event Forwarding
        void ForwardPlayed(Player sender, CardIdentifier card)
        {
            if (Played != null)
            {
                Played(this, card);
            }
        }

        void ForwardRevealed(Player sender, CardIdentifier card)
        {
            if (Revealed != null)
            {
                Revealed(this, card);
            }
        }

        void ForwardTrashed(Player sender, CardIdentifier card)
        {
            if (Trashed != null)
            {
                Trashed(this, card);
            }
        }

        void ForwardDiscarded(Player sender, CardIdentifier card)
        {
            if (Discarded != null)
            {
                Discarded(this, card);
            }
        }

        void ForwardBought(Player sender, CardIdentifier card)
        {
            if (Bought != null)
            {
                Bought(this, card);
            }
        }

        private void ForwardGained(Player sender, CardIdentifier card)
        {
            if (Gained != null)
            {
                Gained(this, card);
            }
        }

        #endregion Event Forwarding

    }
}
