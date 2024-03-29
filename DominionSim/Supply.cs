﻿using System;
using System.Collections.Generic;
using System.Linq;
using DominionSim.VirtualCards;

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

        private Strategy.SupplyFacade mFacade;

        public Dictionary<CardIdentifier, VirtualCardList> CardSupply { get; set; }

        private int mNumPlayers;

        public Supply()
        {
            CardSupply = new Dictionary<CardIdentifier, VirtualCardList>();
            mFacade = new Strategy.SupplyFacade(this);
        }

        public Strategy.SupplyFacade GetFacade()
        {
            return mFacade;
        }

        private void SetupTreasureAndVictory(int numPlayers)
        {
            mNumPlayers = numPlayers;

            CardSupply.Clear();

            AddToSupply(CardList.Copper, 100);
            AddToSupply(CardList.Silver, 100);
            AddToSupply(CardList.Gold, 100);

            if (mNumPlayers == 2)
            {
                AddToSupply(CardList.Estate, 8);
                AddToSupply(CardList.Duchy, 8);
                AddToSupply(CardList.Province, 8);
            }
            else if(mNumPlayers <= 4)
            {
                AddToSupply(CardList.Estate, 12);
                AddToSupply(CardList.Duchy, 12);
                AddToSupply(CardList.Province, 12);
            }
            else
            {
                AddToSupply(CardList.Estate, 12);
                AddToSupply(CardList.Duchy, 12);
                AddToSupply(CardList.Province, (12 + ((mNumPlayers - 4) * 3)));
            }
        }

        /// <summary>
        /// Set up the Supply to have every card available just for testing purposes
        /// </summary>
        /// <param name="numPlayers"></param>
        public void SetupForTesting(int numPlayers)
        {
            SetupTreasureAndVictory(numPlayers);

            // Find all the Card classes that exist but we haven't put in the supply yet

            var cardsToAdd = CardList.GetAllCardIds().Where(cardId => !CardSupply.ContainsKey(cardId));

            foreach (CardIdentifier cardId in cardsToAdd)
            {
                Card.CardType type = cardId.Logic.Type;

                if ((type & Card.CardType.Victory) != 0)
                {
                    // Add 12 copies of Victory cards
                    AddToSupply(cardId, 12);
                }
                else
                {
                    // Add 10 copies of all other cards
                   AddToSupply(cardId, 10);
                }
            }
        }

        /// <summary>
        /// Add the specified number of cards to the Supply
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="count"></param>
        private void AddToSupply(CardIdentifier cardId, int count)
        {
            CardSupply.Add(cardId, new VirtualCardList(cardId, count));
        }


        public void SetupForStartingSet(int numPlayers)
        {
            SetupTreasureAndVictory(numPlayers);

            // STARTING SET
            AddToSupply(CardList.Cellar, 10);
            AddToSupply(CardList.Moat, 10);
            AddToSupply(CardList.Village, 10);
            AddToSupply(CardList.Militia, 10);
            AddToSupply(CardList.Workshop, 10);
            AddToSupply(CardList.Woodcutter, 10);
            AddToSupply(CardList.Smithy, 10);
            AddToSupply(CardList.Remodel, 10);
            AddToSupply(CardList.Mine, 10);
            AddToSupply(CardList.Market, 10);
            // END STARTING SET
        }

        /// <summary>
        /// Take a card from the supply
        /// </summary>
        /// <param name="c">Card you would like to take</param>
        /// <returns>The card you are taking, or null if there isn't one</returns>
        public VirtualCard TakeCard(CardIdentifier cardId)
        {
            if (!CardSupply.ContainsKey(cardId))
            {
                throw new Exception("Card "+cardId+" is not in the supply for this game!");
            }

            if (CardSupply[cardId].Count > 0)
            {
                var first = CardSupply[cardId].First();
                CardSupply[cardId].Remove(first);
                return first;
            }
            else
            {
                return null;
            }
        }

        public int Quantity(CardIdentifier cardId)
        {
            return CardSupply[cardId].Count;
        }

        public IEnumerable<CardIdentifier> GetAllCardsAtCost(int cost)
        {
            return GetAllCardsInCostRange(cost, cost);
        }

        public IEnumerable<CardIdentifier> GetAllCardsInCostRange(int min, int max)
        {
            return CardSupply.Where( (kvp) => kvp.Key.Logic.Cost >= min && kvp.Key.Logic.Cost <= max)
                             .OrderByDescending( (kvp) => kvp.Key.Logic.Cost)
                             .Select((kvp) => kvp.Key);
        }

        public GameState GetGameState()
        {
            if(CardSupply[CardList.Province].Count == 0)
            {
                return GameState.EndViaProvinces;
            }
            else
            {
                int numEmpty = CardSupply.Where(k => k.Value.Count == 0).Count();

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
                var emptyPiles = CardSupply.Where(k => k.Value.Count == 0);

                return "Ran out of "+emptyPiles.Aggregate("", (a, k) => a + k.Key + " ");
            }
            else
            {
                return "Unknown state!";
            }
        }
    }
}
