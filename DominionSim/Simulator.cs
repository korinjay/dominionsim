using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    class GameStats
    {
        public IEnumerable<Player> Winners;
        public int WinnerScore;
        public Dictionary<string, int> VictoryPoints = new Dictionary<string, int>();
    }

    class Simulator
    {
        public List<Player> Players { get; set; }
        public Supply Supply { get; set; }
        public Dictionary<string, int> Wins {get ; set;}
        public Dictionary<string, int> Ties {get; set;}


        public Simulator()
        {
            CardList.SetupCardList();
            Players = new List<Player>();
            Supply = new Supply();
            Wins = new Dictionary<string, int>();
            Ties = new Dictionary<string, int>();
        }

        public void PlayNGames(int n, bool verbose)
        {
            for(int i=0; i < n; i++)
            {
                GameStats results = PlayOneGame(verbose);

                var listToAddTo = (results.Winners.Count() > 1 ? Wins : Ties);
                foreach (var winner in results.Winners)
                {
                    if (listToAddTo.ContainsKey(winner.Name))
                    {
                        listToAddTo[winner.Name]++;
                    }
                    else
                    {
                        listToAddTo[winner.Name] = 1;
                    }
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

            int highScore = 0;
            foreach (var player in Players)
            {
                int vps = player.GetNumVictoryPoints();
                stats.VictoryPoints.Add(player.Name, vps);
                if (vps > highScore)
                {
                    highScore = vps;
                }
            }
            stats.Winners = Players.Where(p => stats.VictoryPoints[p.Name] == highScore);
            stats.WinnerScore = highScore;
            

            if (verbose)
            {
                Console.WriteLine("Game ended after "+turns+" turns.  "+stats.Winners+" won with "+stats.WinnerScore+" points!");
            }

            for(int i=0; i < Players.Count; i++)
            {
                Player player = Players[i];
                int vps = player.GetNumVictoryPoints();

                List<string> vpCards = Utility.FilterCardListByType(player.Deck, Card.CardType.Victory);
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
