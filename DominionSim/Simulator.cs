using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    struct GameStats
    {
        public string Winner;
        public int WinnerScore;
        public Dictionary<string, int> VictoryPoints;
    }

    class Simulator
    {
        public List<Player> Players { get; set; }
        public Supply Supply { get; set; }

        public Simulator()
        {
            Players = new List<Player>();
            Supply = new Supply();
        }

        public void CreatePlayers(int num, bool verbose)
        {
            Player newPlayer = new Player("Big Money 1");
            newPlayer.Strategy = new Strategy.BigMoney();
            newPlayer.Verbose = false;
            Players.Add(newPlayer);
            
            newPlayer = new Player("Big Money 2");
            newPlayer.Strategy = new Strategy.BigMoney();
            newPlayer.Verbose = false;
            Players.Add(newPlayer);
            
            newPlayer = new Player("Big Money 3");
            newPlayer.Strategy = new Strategy.BigMoney();
            newPlayer.Verbose = false;
            Players.Add(newPlayer);

            newPlayer = new Player("Big Money Duchy");
            newPlayer.Strategy = new Strategy.BigMoneyDuchy();
            newPlayer.Verbose = verbose;
            Players.Add(newPlayer);
        }

        public void PlayNGames(int n, bool verbose)
        {
            Dictionary<string, int> wins = new Dictionary<string, int>();

            for(int i=0; i < n; i++)
            {
                GameStats results = PlayOneGame(verbose);

                if (wins.ContainsKey(results.Winner))
                {
                    wins[results.Winner]++;
                }
                else
                {
                    wins[results.Winner] = 1;
                }
            }

            int numTies = wins.ContainsKey("Tie") ? wins["Tie"] : 0;
            Console.WriteLine(n + " games playes, "+ (n - numTies) + " games had an outright winner.");

            for (int i = 0; i < Players.Count; i++ )
            {
                string playerName = Players[i].Name;
                int numWins = wins.ContainsKey(playerName) ? wins[playerName] : 0;

                float percent = 100.0f * numWins / (n - numTies);
                Console.WriteLine(playerName + " : " + numWins + " / " + (n - numTies) + " = " + percent + "%");
            }
        }

 
        public GameStats PlayOneGame(bool verbose)
        {
            Supply.SetupForNewGame(Players.Count);

            Players = Utility.Shuffle(Players);

            foreach (Player player in Players)
            {
                player.StartNewGame();
            }

            int turns = 0;
            while(!Supply.IsGameOver())
            {
                foreach (Player player in Players)
                {
                    player.TakeTurn(Supply);
                    if (Supply.IsGameOver())
                    {
                        break;
                    }
                }
                turns++;
            }

            GameStats stats = new GameStats();
            stats.VictoryPoints = new Dictionary<string, int>();

            if (verbose)
            {
                Console.WriteLine("Game ended after "+turns+" turns.");
            }

            for(int i=0; i < Players.Count; i++)
            {
                Player player = Players[i];
                int vps = player.GetNumVictoryPoints();
                stats.VictoryPoints.Add(player.Name, vps);

                if (vps > stats.WinnerScore)
                {
                    stats.Winner = player.Name;
                    stats.WinnerScore = vps;
                }
                else if (vps == stats.WinnerScore)
                {
                    stats.Winner = "Tie";
                }

                List<Card> vpCards = player.GetCardsOfType(Card.CardType.Victory);
                if (verbose)
                {
                    Console.WriteLine(player.Name + ": " + vps + " ( " + player.StatStringFromList(vpCards) + ") ( " + player.PurchaseString() + ")");
                }
            }

            if (verbose)
            {
                Console.WriteLine();
            }
            return stats;
        }
    }
}
