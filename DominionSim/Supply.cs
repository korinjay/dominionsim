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

        public Dictionary<Card, int> CardSupply { get; set; }

        private int mNumPlayers;

        public Supply()
        {
            CardSupply = new Dictionary<Card, int>();
        }

        private void SetupTreasureAndVictory(int numPlayers)
        {
            mNumPlayers = numPlayers;

            CardSupply.Clear();

            CardSupply.Add(Card.Copper, 100);
            CardSupply.Add(Card.Silver, 100);
            CardSupply.Add(Card.Gold, 100);

            CardSupply.Add(Card.Estate, 12);
            CardSupply.Add(Card.Duchy, 12);
            CardSupply.Add(Card.Province, 12 + ((mNumPlayers - 4) * 3));
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

            foreach (var c in cardsToAdd)
            {
                CardType type = CardList.Cards[c].Type;

                if ((type & CardType.Victory) != 0)
                {
                    // Add 12 copies of Victory cards
                    CardSupply.Add(c, 12);
                }
                else
                {
                    // Add 10 copies of all other cards
                    CardSupply.Add(c, 10);
                }
            }
        }

        public void SetupForStartingSet(int numPlayers)
        {
            SetupTreasureAndVictory(numPlayers);

            // STARTING SET
            CardSupply.Add(Card.Cellar, 10);
            CardSupply.Add(Card.Moat, 10);
            CardSupply.Add(Card.Village, 10);
            CardSupply.Add(Card.Militia, 10);
            CardSupply.Add(Card.Workshop, 10);
            CardSupply.Add(Card.Woodcutter, 10);
            CardSupply.Add(Card.Smithy, 10);
            CardSupply.Add(Card.Remodel, 10);
            CardSupply.Add(Card.Mine, 10);
            CardSupply.Add(Card.Market, 10);
            // END STARTING SET
        }

        /// <summary>
        /// Gain a card from the supply
        /// </summary>
        /// <param name="c">Card you would like to gain</param>
        /// <returns>TRUE if there were enough left for you to gain one</returns>
        public bool GainCard(Card card)
        {
            if (!CardSupply.ContainsKey(card))
            {
                throw new Exception("Card "+card.ToString()+" is not in the supply for this game!");
            }

            if (CardSupply[card] > 0)
            {
                CardSupply[card]--;
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Quantity(Card card)
        {
            return CardSupply[card];
        }

        public IEnumerable<Card> GetAllCardsAtCost(int cost)
        {
            return GetAllCardsInCostRange(cost, cost);
        }

        public IEnumerable<Card> GetAllCardsInCostRange(int min, int max)
        {
            return CardSupply.Where( (kvp) => CardList.Cards[kvp.Key].Cost >= min && CardList.Cards[kvp.Key].Cost <= max)
                             .OrderByDescending( (kvp) => CardList.Cards[kvp.Key].Cost)
                             .Select((kvp) => kvp.Key);
        }

        public GameState GetGameState()
        {
            if(CardSupply[Card.Province] == 0)
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
