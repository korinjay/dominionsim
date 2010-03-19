using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DominionSim
{
    using CardIdentifier = String;

    class GameStats
    {
        public IEnumerable<Player> Winners;
        public int WinnerScore;
        public Dictionary<Player, int> VictoryPoints = new Dictionary<Player, int>();
    }

    class Simulator
    {
        public List<Player> Players { get; set; }
        public Supply Supply { get; set; }
        public Dictionary<Player, int> Wins {get ; set;}
        public Dictionary<Player, int> Ties { get; set; }

        public int CurrentGame { get; set; }

        public Simulator()
        {
            CardList.SetupCardList();
            Players = new List<Player>();
            Supply = new Supply();
            Wins = new Dictionary<Player, int>();
            Ties = new Dictionary<Player, int>();
        }

        public void PlayNGames(int n, bool verbose)
        {
            foreach (Player player in Players)
            {
                Wins[player] = 0;
                Ties[player] = 0;
            }

            for(int i=0; i < n; i++)
            {
                CurrentGame = n;
                GameStats results = PlayOneGame(verbose);

                var listToAddTo = (results.Winners.Count() > 1 ? Ties : Wins);
                foreach (var winner in results.Winners)
                {
                    if (listToAddTo.ContainsKey(winner))
                    {
                        listToAddTo[winner]++;
                    }
                    else
                    {
                        listToAddTo[winner] = 1;
                    }
                }

                Thread.Sleep(0);
            }
        }

 
        public GameStats PlayOneGame(bool verbose)
        {
            Supply.SetupForTesting(Players.Count);

            Players = Utility.Shuffle(Players);

            foreach (Player player in Players)
            {
                player.StartNewGame();

                player.OtherPlayers.AddRange(Players.Where(p => p != player));

            }

            int turns = 0;
            while(Supply.GetGameState() == Supply.GameState.Playing)
            {
                foreach (Player player in Players)
                {
                    player.TakeTurn(turns, Supply);
                    if (Supply.GetGameState() != Supply.GameState.Playing)
                    {
                        break;
                    }
                }
                turns++;
            }

            Supply.GameState state = Supply.GetGameState();
            GameStats stats = new GameStats();

            int highScore = 0;
            foreach (var player in Players)
            {
                int vps = player.GetNumVictoryPoints();
                stats.VictoryPoints.Add(player, vps);
                if (vps > highScore)
                {
                    highScore = vps;
                }
            }
            stats.Winners = Players.Where(p => stats.VictoryPoints[p] == highScore);
            stats.WinnerScore = highScore;
            

            if (verbose)
            {
                Console.WriteLine("Game ended after "+turns+" turns: "+Supply.GetGameStateString());
                Console.WriteLine(stats.Winners.Aggregate("", (s, p) => (s + "'"+ p.Name + "' "))+"won with "+stats.WinnerScore+" points!");
            }

            for(int i=0; i < Players.Count; i++)
            {
                Player player = Players[i];
                int vps = player.GetNumVictoryPoints();

                IEnumerable<CardIdentifier> vpCards = Utility.FilterCardListByType(player.Deck, Card.CardType.Victory);
                if (verbose)
                {
                    Console.WriteLine(player.Name + ": " + vps + " ( " + player.StatStringFromList(vpCards) + ")");
                    Console.WriteLine("  Deck: ( " + player.StatStringFromList(player.Deck) + ")");
                    Console.WriteLine("  Purchases: ( " + Stats.Tracker.Instance.PurchaseString(player) + ")");
                    Console.WriteLine(player.Name+" Activity:");
                    Console.WriteLine(Stats.Tracker.Instance.ActivityString(player));
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
