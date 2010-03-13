using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    class Supply
    {
        public Dictionary<string, int> Counts { get; set; }

        private int mNumPlayers;

        public Supply()
        {
            Counts = new Dictionary<string, int>();
        }

        public void SetupForNewGame(int numPlayers)
        {
            mNumPlayers = numPlayers;

            Counts.Clear();

            Counts.Add("Copper", 100);
            Counts.Add("Silver", 100);
            Counts.Add("Gold", 100);

            Counts.Add("Estate", 12);
            Counts.Add("Duchy", 12);
            Counts.Add("Province", 12 + ((mNumPlayers - 4) * 3));
        }

        /// <summary>
        /// Gain a card from the supply
        /// </summary>
        /// <param name="c">Card you would like to gain</param>
        /// <returns>TRUE if there were enough left for you to gain one</returns>
        public bool GainCard(Card c)
        {
            if (Counts[c.Name] > 0)
            {
                Counts[c.Name]--;
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Quantity(Card c)
        {
            return Counts[c.Name];
        }

        public bool IsGameOver()
        {
            if(Counts["Province"] == 0)
            {
                return true;
            }
            else
            {
                int numEmpty = 0;
                foreach(KeyValuePair<string, int> kvp in Counts)
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
