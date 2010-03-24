using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Tournament
{
    using Matchup = List<Player>;
    using System.Diagnostics;

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
        IEnumerable<Contender> mContenders;

        Simulator mSim;

        Dictionary<string, PlayerStats> mStats;

        Contender mFeatured = null;

        /// <summary>
        /// Create a Tournament with a list of Contenders and the # of players
        /// </summary>
        /// <param name="contenders">Instances of Contenders</param>
        public Tourney(IEnumerable<Contender> contenders)
        {
            // Create ContenderHolders from the passed-in Strategy Types
            mContenders = contenders;
        }

        public Dictionary<string, PlayerStats> GetStats()
        {
            return mStats;
        }

        public void Feature(Contender c)
        {
            mFeatured = c;
        }

        public int Run(int minPlayers, int maxPlayers, int gamesPerMatch)
        {
            return RunAllMatchups(minPlayers, maxPlayers, gamesPerMatch);
        }

        private int RunAllMatchups(int min, int max, int gamesPerMatch)
        {
            List<Player> players = new List<Player>();
            players.AddRange(mContenders.Select(sh => new Player(sh.Name, sh.Strategy)));

            int totalGames = 0;
            if (mFeatured != null)
            {
                Console.WriteLine("*** FEATURING "+mFeatured.Name+" ***");
            }

            for (int gameSize = min; gameSize <= max; gameSize++)
            {
                // Reset stats per bracket
                mStats = new Dictionary<string, PlayerStats>();
                foreach (Player p in players)
                {
                    mStats[p.Name] = new PlayerStats();
                }

                List<Matchup> games = CreateAllMatchups(players, gameSize);
                totalGames += games.Count;

                Console.WriteLine("====== "+gameSize+" PLAYER BRACKET =======");
                Console.WriteLine("        ("+games.Count+" matchups)");
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int currMatchup = 0;
                // For each game
                foreach (Matchup m in games)
                {
                    mSim = new Simulator();

                    foreach (Player p in m)
                    {
                        mSim.Players.Add(p);
                    }

                    if (mFeatured != null && m.Select(p => p.Name).Contains(mFeatured.Name))
                    {
                        Console.WriteLine("  Matchup " + (currMatchup + 1) + " : " + m.Aggregate("", (s, p) => s + p.Name + " "));
                    }
                    // Run the game
                    mSim.PlayNGames(gamesPerMatch, false);

                    var orderedStats = mSim.Players.OrderByDescending(p => mSim.Wins[p] + mSim.Ties[p] );
                    // Record ending stats for each player
                    foreach (var player in orderedStats)
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
                        mStats[player.Name].Games += gamesPerMatch;

                        if (mFeatured != null && m.Select(p => p.Name).Contains(mFeatured.Name))
                        {
                            string line = String.Format("  {0,15:s}: {1,6} {2,6} {3,6} {4,9:#.##}%", playerName, numWins, numTies, gamesPerMatch, 100.0f * (numWins +numTies) / gamesPerMatch);
                            Console.WriteLine(line);
                        }

                    }

                    currMatchup++;
                }
                sw.Stop();

                var ordered = players.OrderByDescending((p) => (mStats[p.Name].GetTimesHighestPercent()));

                Console.WriteLine();
                Console.WriteLine(String.Format("  {0,15:s}  {1,6} {2,6} {3,6} {4,10:#.##}", "Strategy", "Wins", "Ties", "Games", "Highest%"));
                Console.WriteLine(String.Format("  {0,15:s}  {1,6} {2,6} {3,6} {4,10:#.##}", "--------", "----", "----", "-----", "--------"));
                foreach (Player p in ordered)
                {
                    PlayerStats stats = mStats[p.Name];
                    string line = String.Format("  {0,15:s}: {1,6} {2,6} {3,6} {4,9:#.##}%", p.Name, stats.Wins, stats.Ties, stats.Games, 100.0f * stats.GetTimesHighestPercent());

                    Console.WriteLine(line);
                }
                Console.WriteLine("Elapsed time = {0}", sw.Elapsed.ToString());
            }

            return totalGames;
        }

        /// <summary>
        /// Create every permutation of the given contestants with the given number of contestants in each match
        /// </summary>
        /// <param name="contestants">List of contestants</param>
        /// <param name="matchupSize">How many contestants you want in a given match</param>
        /// <returns>List of lists of contestants.  Each inner list will have matchupSize contenders in it.  Each inner list is unique</returns>
        private List<Matchup> CreateAllMatchups(List<Player> contestants, int matchupSize)
        {
            return CreateAllMatchupsRecursive(contestants, matchupSize, -1);
        }

        /// <summary>
        /// Helper for CreateAllMatchups.  Don't call directly
        /// </summary>
        private List<Matchup> CreateAllMatchupsRecursive(List<Player> contestants, int numRemainingToChoose, int lastIndexChosen)
        {
            List<Matchup> ret = new List<Matchup>();

            if (numRemainingToChoose == 0)
            {
                // Base case: add just 1 empty list
                ret.Add(new Matchup());
            }
            else
            {
                // Future cases: tack on each contender to each list returned from the recursion
                for (int i = lastIndexChosen + 1; i < contestants.Count() - numRemainingToChoose + 1; ++i)
                {
                    var inner = CreateAllMatchupsRecursive(contestants, numRemainingToChoose - 1, i);
                    foreach (var listOfMatchups in inner)
                    {
                        listOfMatchups.Add(contestants[i]);
                        ret.Add(listOfMatchups);
                    }
                }
            }
            return ret;
        }

    }
}
