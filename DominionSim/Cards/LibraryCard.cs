﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    using CardIdentifier = String;

    class LibraryCard : Card
    {
        public LibraryCard() : base(CardList.Library, CardType.Action, 5, 0, 0, 0, 0, 0)
        {

        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            List<CardIdentifier> setAside = new List<CardIdentifier>();
            while (p.Hand.Count < 7)
            {
                CardIdentifier nextCard = p.DrawCard();

                // If it's an action card, ask the strategy what to do with it
                if ((CardList.Cards[nextCard].Type & CardType.Action) != 0)
                {
                    bool setThisAside = p.Strategy.ChooseToSetAsideCard(p.GetFacade(), nextCard);

                    if (setThisAside)
                    {
                        setAside.Add(nextCard);
                    }
                    else
                    {
                        p.AddCardToHand(nextCard);
                    }
                }
                else
                {
                    p.AddCardToHand(nextCard);
                }
            }

            p.Hand.AddRange(setAside);
            foreach (CardIdentifier card in setAside)
            {
                p.DiscardCard(card);
            }
        }
    }
}
