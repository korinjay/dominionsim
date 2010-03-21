using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    using Matchup = List<Player>;

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

        public int GetTimesHighest()
        {
            return Wins + Ties;
        }

        public float GetTimesHighestPercent()
        {
            if (Games > 0)
            {
                return 1.0f * GetTimesHighest() / Games;
            }
            else
            {
                return 0;
            }
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
            return RunSwiss(4);
        }

        public int RunSwiss(int numPlayers)
        {
            Random rand = new Random();
            mStats = new Dictionary<string, PlayerStats>();

            Dictionary<string, Dictionary<string, int>> versus = new Dictionary<string, Dictionary<string, int>>();

            // Randomly fill games with N players
            List<Player> players = new List<Player>();
            players.AddRange(mStrategies.Select((t) => new Player(t.Name, Activator.CreateInstance(t) as Strategy.IStrategy)));

            foreach (Player p in players)
            {
                mStats[p.Name] = new PlayerStats();
                versus[p.Name] = new Dictionary<string, int>();
            }

            List<Matchup> games = new List<Matchup>();

            List<Player> temp = new List<Player>();
            temp.AddRange(players);

            Console.WriteLine("=== Round 1 ===");

            // While there are players left to be assigned...
            while (temp.Count >= numPlayers)
            {
                Matchup m = new Matchup();

                // While this matchup is not yet full
                while (m.Count < numPlayers)
                {
                    int randIndex = rand.Next(temp.Count);
                    Player p = temp[randIndex];
                    temp.RemoveAt(randIndex);
                    m.Add(p);
                }

                Console.WriteLine("  Game "+(games.Count+1)+" : "+m.Aggregate("", (s, p) => s + p.Name + " "));

                foreach (Player p in m)
                {
                    Dictionary<string, int> opponents = versus[p.Name];

                    foreach (Player opp in m.Where(dude => dude != p))
                    {
                        if (!opponents.ContainsKey(opp.Name))
                        {
                            opponents[opp.Name] = 1;
                        }
                        else
                        {
                            opponents[opp.Name] += 1;
                        }
                    }
                }

                // Add this matchup to the list of games
                games.Add(m);
            }

            // The number of rounds should be rougly log2(numPlayers)
            //int NumRounds = (int)(Math.Ceiling(Math.Log((double)players.Count)));
            int NumRounds = players.Count;
            int NumGames = 400;

            // For each round
            for (int round = 0; round < NumRounds; round++)
            {
                // For each game
                foreach (Matchup m in games)
                {
                    mSim = new Simulator();

                    foreach (Player p in m)
                    {
                        mSim.Players.Add(p);
                    }

                    // Run the game
                    mSim.PlayNGames(NumGames, false);
                    
                    // Award points to the players based on their finish
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

                games = new List<Matchup>();

                // Fill games based on points
                temp = new List<Player>();
                temp.AddRange(players);
                var ordered = temp.OrderBy((p) => (mStats[p.Name].GetTimesHighestPercent()));

                Console.WriteLine(String.Format("  {0,15:s}  {1,6} {2,6} {3,6} {4,10:#.##}", "Strategy", "Wins", "Ties", "Games", "Highest%"));
                Console.WriteLine(String.Format("  {0,15:s}  {1,6} {2,6} {3,6} {4,10:#.##}", "--------", "----", "----", "-----", "--------"));
                foreach (Player p in ordered)
                {
                    PlayerStats stats = mStats[p.Name];
                    string line = String.Format("  {0,15:s}: {1,6} {2,6} {3,6} {4,10:#.##}%", p.Name, stats.Wins, stats.Ties, stats.Games, 100.0f * stats.GetTimesHighestPercent());

                    Console.WriteLine(line);
                }

                Console.WriteLine("=== Round " + (round + 2) + " ===");

                // Randomly assign byes to players
                while (ordered.Count() % numPlayers != 0)
                {
                    var mostActive = ordered.Where(p => mStats[p.Name].Games == ordered.Max(p2 => mStats[p2.Name].Games));
                    int index = rand.Next(mostActive.Count());
                    Player bye = mostActive.ElementAt(index);

                    ordered = ordered.Where(p => p != bye).OrderBy(p => mStats[p.Name].GetTimesHighestPercent());
                }

                // While there are players left to be assigned...
                while (ordered.Count() >= numPlayers)
                {
                    Matchup m = new Matchup();

                    // While this matchup is not yet full
                    // Add players from lowest score to highest
                    // Thus the highest players will get a bye
                    while (m.Count < numPlayers)
                    {
                        Player player = ordered.ElementAt(0);
                        ordered = ordered.Skip(1).OrderBy((p) => (mStats[p.Name].GetTimesHighestPercent()));
                        m.Add(player);
                    }

                    Console.WriteLine("  Game " + (games.Count + 1) + " : " + m.Aggregate("", (s, p) => s + p.Name + " "));

                    foreach (Player p in m)
                    {
                        Dictionary<string, int> opponents = versus[p.Name];

                        foreach (Player opp in m.Where(dude => dude != p))
                        {
                            if (!opponents.ContainsKey(opp.Name))
                            {
                                opponents[opp.Name] = 1;
                            }
                            else
                            {
                                opponents[opp.Name] += 1;
                            }
                        }
                    }

                    // Add this matchup to the list of games
                    games.Add(m);
                }
            }

            Console.WriteLine("=== Distinct Opponents ===");
            foreach (KeyValuePair<string, Dictionary<string, int>> kvp in versus)
            {
                Dictionary<string, int> tallies = kvp.Value;

                Console.WriteLine(kvp.Key+": "+tallies.Count());
            }
            Console.WriteLine("=== Who Played Who ===");
            foreach (KeyValuePair<string, Dictionary<string, int>> kvp in versus)
            {
                Dictionary<string, int> tallies = kvp.Value;

                string line = tallies.OrderByDescending(pair => pair.Value)
                                     .Aggregate(kvp.Key+": ", (s, pair) => s + pair.Key + "(" + pair.Value + ") ");

                Console.WriteLine(line);
            }

            return 0;
        }

        public int Run2Player()
        {
            Build2PlayerMatchups();

            mStats = new Dictionary<string, PlayerStats>();

            foreach (Matchup m in mSchedule)
            {
                mSim = new Simulator();

                foreach (Player p in m)
                {
                    mSim.Players.Add(p);
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

        public void Build2PlayerMatchups()
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

                        m.Add(new Player(strat.Name, Activator.CreateInstance(strat) as Strategy.IStrategy));
                        m.Add(new Player(vsStrat.Name, Activator.CreateInstance(vsStrat) as Strategy.IStrategy));

                        mSchedule.Add(m);
                    }
                }
            }
        }
    }
}
