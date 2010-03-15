using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    class Supply
    {
        public Dictionary<string, int> CardSupply { get; set; }

        private int mNumPlayers;

        public Supply()
        {
            CardSupply = new Dictionary<string, int>();
        }

        public void SetupForNewGame(int numPlayers)
        {
            mNumPlayers = numPlayers;

            CardSupply.Clear();

            CardSupply.Add(CardList.Copper, 100);
            CardSupply.Add(CardList.Silver, 100);
            CardSupply.Add(CardList.Gold, 100);

            CardSupply.Add(CardList.Estate, 12);
            CardSupply.Add(CardList.Duchy, 12);
            CardSupply.Add(CardList.Province, 12 + ((mNumPlayers - 4) * 3));

            CardSupply.Add(CardList.Smithy, 10);
            CardSupply.Add(CardList.Chapel, 10);
            CardSupply.Add(CardList.Workshop, 10);
            CardSupply.Add(CardList.Feast, 10);
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

        public bool IsGameOver()
        {
            if(CardSupply[CardList.Province] == 0)
            {
                return true;
            }
            else
            {
                int numEmpty = 0;
                foreach(KeyValuePair<string, int> kvp in CardSupply)
                {
                    if(kvp.Value == 0)
                    {
                        numEmpty++;
                    }
                }

                int numToEnd = 3;
                if(mNumPlayers > 4)
                {
                    numToEnd = 4;
                }
                if(numEmpty >= numToEnd)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
