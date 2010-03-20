using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    using Matchup = List<Type>;

    class PlayerStats
    {
        public int Wins { get; set; }
        public int Ties { get; set; }
        public int Games { get; set; }

        public PlayerStats()
        {
            Wins = 0;
            Ties = 0;
            Games = 0;
        }
    }

    class Tourney
    {
        private int mMinPlayers = 2;
        private int mMaxPlayers = 6;

        IEnumerable<Type> mStrategies;

        Simulator mSim;

        List<Matchup> mSchedule;
        Dictionary<string, PlayerStats> mStats;

        public Tourney(IEnumerable<Type> strategies)
            : this(2, 6, strategies)
        {

        }

        public Tourney(int minPlayers, int maxPlayers, IEnumerable<Type> strategies)
        {
            mMinPlayers = minPlayers;
            mMaxPlayers = maxPlayers;

            mStrategies = strategies.Distinct();

            mSchedule = new List<Matchup>();
        }

        public int Run()
        {
            BuildMatchups();

            mStats = new Dictionary<string, PlayerStats>();

            foreach (Matchup m in mSchedule)
            {
                mSim = new Simulator();

                foreach (Type t in m)
                {
                    Player newPlayer = new Player(t.Name);
                    newPlayer.Strategy = Activator.CreateInstance(t) as Strategy.IStrategy;
                    newPlayer.Verbose = false;
                    mSim.Players.Add(newPlayer);
                }

                const int NumGames = 500;
                mSim.PlayNGames(NumGames, false);

                // Sort out the players so the most wins go on top
                var sortedPlayers = mSim.Players.OrderByDescending(p => mSim.Wins[p] + mSim.Ties[p]);
                foreach (var player in sortedPlayers)
                {
                    string playerName = player.Name;
                    int numWins = mSim.Wins.ContainsKey(player) ? mSim.Wins[player] : 0;
                    int numTies = mSim.Ties.ContainsKey(player) ? mSim.Ties[player] : 0;

                    if (!mStats.ContainsKey(player.Name))
                    {
                        mStats[player.Name] = new PlayerStats();
                    }

                    mStats[player.Name].Wins += numWins;
                    mStats[player.Name].Ties += numTies;
                    mStats[player.Name].Games += NumGames;
                }
            }

            return mSchedule.Count;
        }

        public Dictionary<string, PlayerStats> GetStats()
        {
            return mStats;
        }

        public void BuildMatchups()
        {
            for (int i = 0; i < mStrategies.Count(); i++)
            {
                var strat = mStrategies.ElementAt(i);

                for (int j = i; j < mStrategies.Count(); j++)
                {
                    var vsStrat = mStrategies.ElementAt(j);

                    if (vsStrat != strat)
                    {
                        Matchup m = new Matchup();
                        m.Add(strat);
                        m.Add(vsStrat);

                        mSchedule.Add(m);
                    }
                }
            }
        }
    }
}
