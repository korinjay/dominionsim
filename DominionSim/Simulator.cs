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
        public Dictionary<string, int> Wins {get ; set;} 


        public Simulator()
        {
            CardList.SetupCardList();
            Players = new List<Player>();
            Supply = new Supply();
            Wins = new Dictionary<string, int>();
        }

        public void PlayNGames(int n, bool verbose)
        {
            for(int i=0; i < n; i++)
            {
                GameStats results = PlayOneGame(verbose);

                if (Wins.ContainsKey(results.Winner))
                {
                    Wins[results.Winner]++;
                }
                else
                {
                    Wins[results.Winner] = 1;
                }
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
                    player.TakeTurn(turns, Supply);
                    if (Supply.IsGameOver())
                    {
                        break;
                    }
                }
                turns++;
            }

            GameStats stats = new GameStats();
            stats.VictoryPoints = new Dictionary<string, int>();

            for (int i = 0; i < Players.Count; i++)
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
            }

            if (verbose)
            {
                Console.WriteLine("Game ended after "+turns+" turns.  "+stats.Winner+" won with "+stats.WinnerScore+" points!");
            }

            for(int i=0; i < Players.Count; i++)
            {
                Player player = Players[i];
                int vps = player.GetNumVictoryPoints();

                List<string> vpCards = player.GetCardsOfType(Card.CardType.Victory);
                if (verbose)
                {
                    Console.WriteLine(player.Name + ": " + vps + " ( " + player.StatStringFromList(vpCards) + ")");
                    Console.WriteLine("  Deck: ( " + player.StatStringFromList(player.Deck) + ")");
                    Console.WriteLine("  Purchases: ( " + player.PurchaseString() + ")");
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
