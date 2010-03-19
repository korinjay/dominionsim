using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    class Supply
    {
        public enum GameState
        {
            Playing,
            EndViaProvinces,
            EndViaSupply
        }

        public Dictionary<string, int> CardSupply { get; set; }

        private int mNumPlayers;

        public Supply()
        {
            CardSupply = new Dictionary<string, int>();
        }

        private void SetupTreasureAndVictory(int numPlayers)
        {
            mNumPlayers = numPlayers;

            CardSupply.Clear();

            CardSupply.Add(CardList.Copper, 100);
            CardSupply.Add(CardList.Silver, 100);
            CardSupply.Add(CardList.Gold, 100);

            CardSupply.Add(CardList.Estate, 12);
            CardSupply.Add(CardList.Duchy, 12);
            CardSupply.Add(CardList.Province, 12 + ((mNumPlayers - 4) * 3));
        }

        /// <summary>
        /// Set up the Supply to have every card available just for testing purposes
        /// </summary>
        /// <param name="numPlayers"></param>
        public void SetupForTesting(int numPlayers)
        {
            SetupTreasureAndVictory(numPlayers);

            var cardsToAdd = CardList.Cards.Where((kvp) => !CardSupply.ContainsKey(kvp.Key))
                                           .Select((kvp) => kvp.Key);

            foreach (string name in cardsToAdd)
            {
                Card.CardType type = CardList.Cards[name].Type;

                if ((type & Card.CardType.Victory) != 0)
                {
                    // Add 12 copies of Victory cards
                    CardSupply.Add(name, 12);
                }
                else
                {
                    // Add 10 copies of all other cards
                    CardSupply.Add(name, 10);
                }
            }
        }

        public void SetupForStartingSet(int numPlayers)
        {
            SetupTreasureAndVictory(numPlayers);

            // STARTING SET
            CardSupply.Add(CardList.Cellar, 10);
            CardSupply.Add(CardList.Moat, 10);
            CardSupply.Add(CardList.Village, 10);
            CardSupply.Add(CardList.Militia, 10);
            CardSupply.Add(CardList.Workshop, 10);
            CardSupply.Add(CardList.Woodcutter, 10);
            CardSupply.Add(CardList.Smithy, 10);
            CardSupply.Add(CardList.Remodel, 10);
            CardSupply.Add(CardList.Mine, 10);
            CardSupply.Add(CardList.Market, 10);
            // END STARTING SET
        }

        /// <summary>
        /// Gain a card from the supply
        /// </summary>
        /// <param name="c">Card you would like to gain</param>
        /// <returns>TRUE if there were enough left for you to gain one</returns>
        public bool GainCard(string cardName)
        {
            if (!CardSupply.ContainsKey(cardName))
            {
                throw new Exception("Card "+cardName+" is not in the supply for this game!");
            }

            if (CardSupply[cardName] > 0)
            {
                CardSupply[cardName]--;
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Quantity(string cardName)
        {
            return CardSupply[cardName];
        }

        public IEnumerable<string> GetAllCardsAtCost(int cost)
        {
            return GetAllCardsInCostRange(cost, cost);
        }

        public IEnumerable<string> GetAllCardsInCostRange(int min, int max)
        {
            return CardSupply.Where( (kvp) => CardList.Cards[kvp.Key].Cost >= min && CardList.Cards[kvp.Key].Cost <= max)
                             .OrderByDescending( (kvp) => CardList.Cards[kvp.Key].Cost)
                             .Select((kvp) => kvp.Key);
        }

        public GameState GetGameState()
        {
            if(CardSupply[CardList.Province] == 0)
            {
                return GameState.EndViaProvinces;
            }
            else
            {
                int numEmpty = CardSupply.Where(k => k.Value == 0).Count();

                int numToEnd = 3;
                if(mNumPlayers > 4)
                {
                    numToEnd = 4;
                }
                if(numEmpty >= numToEnd)
                {
                    return GameState.EndViaSupply;
                }
            }

            return GameState.Playing;
        }

        public string GetGameStateString()
        {
            GameState state = GetGameState();

            if (state == GameState.Playing)
            {
                return "Playing";
            }
            else if (state == GameState.EndViaProvinces)
            {
                return "Ran out of Provinces";
            }
            else if (state == GameState.EndViaSupply)
            {
                var emptyPiles = CardSupply.Where(k => k.Value == 0);

                return "Ran out of "+emptyPiles.Aggregate("", (a, k) => a + k.Key + " ");
            }
            else
            {
                return "Unknown state!";
            }
        }
    }
}
