using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Stats
{
    class Tracker
    {
        /// Singleton pattern!
        static Tracker sInstance = null;
        public static Tracker Instance
        {
            get
            {
                if (sInstance == null)
                {
                    sInstance = new Tracker();
                }
                return sInstance;
            }
        }


        public Dictionary<Player, List<PlayerAction>> mPlayerActions;

        public Tracker()
        {
            mPlayerActions = new Dictionary<Player, List<PlayerAction>>();
        }

        public void Clear()
        {
            mPlayerActions.Clear();
        }
       
        public void LogAction(Player p, PlayerAction action)
        {
            if (!mPlayerActions.ContainsKey(p))
            {
                mPlayerActions[p] = new List<PlayerAction>();
            }
            mPlayerActions[p].Add(action);
        }

        public string ActivityString(Player p)
        {
            string str = "";

            var grouped = mPlayerActions[p]
                .GroupBy((a) => a.Turn)
                .OrderBy((a) => a.Key);

            foreach (var group in grouped)
            {
                str += "[Turn " + group.Key + "] ";
                foreach (var action in group)
                {
                    str += action.Action + " " + action.Card + "  ";
                }
                str += "\n";
            }

            return str;
        }

        public string PurchaseString(Player p)
        {
            // Loop through all purchases and output a string in the format:
            //    "1:Silver 2:Silver 3:Gold"
            return mPlayerActions[p].Where((a) => (a.Action == Stats.PlayerAction.Buy)).Aggregate("", (s, a) => s + a.Turn + ":" + a.Card + " ");
        }

        public string GetAllPlayCountsString()
        {
            var all = mPlayerActions.SelectMany(kvp => kvp.Value);

            var grouped = GetActionsByCard(all, PlayerAction.Play);

            string str = "";

            int total = grouped.Aggregate(0, (i, g) => i + g.Count());
            str += grouped.Count() + " different cards played a total of "+total+" times." + Environment.NewLine;

            foreach (var group in grouped)
            {
                int num = group.Count();
                float percent = 100.0f * num / total;
                str += group.Key + ": " + num + " (" + percent + "%)" + Environment.NewLine;
            }

            return str;            
        }

        private IEnumerable<IGrouping<Card, PlayerAction>> GetActionsByCard(IEnumerable<PlayerAction> actions, string whichKind)
        {
            var grouped = actions.Where(act => act.Action == whichKind)
                                 .GroupBy(act => act.Card)
                                 .OrderByDescending(group => group.Count() );
            return grouped;
        }

        public string GetPlayCountString(Player p)
        {
            string str = "";
            var grouped = GetActionsByCard(mPlayerActions[p], PlayerAction.Play);

            foreach (var group in grouped)
            {
                str += group.Key + ": " + group.Count() + Environment.NewLine;
            }

            return str;
        }
    }
}
