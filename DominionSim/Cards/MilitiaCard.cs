﻿using System;
using System.Linq;

namespace DominionSim.Cards
{

    class MilitiaCard : Card
    {
        public MilitiaCard() : base(CardList.Militia, Card.ActionAttack, 4, 0, 0, 2, 0, 0)
        {
        }

        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            foreach(Player opponent in p.OtherPlayers)
            {
                if (opponent == p)
                {
                    throw new Exception("How did I get in my own 'other players' list?");
                }

                int numToDiscard = opponent.Hand.Count - 3;

                if (!HandleAttackReactions(p, opponent, supply))
                {
                    var discards = opponent.Strategy.ChooseCardsToDiscard(opponent.GetFacade(), numToDiscard, numToDiscard, CardType.Any, supply.GetFacade());

                    if (discards.Count() < numToDiscard)
                    {
                        throw new Exception("Player " + opponent.Name + " failed to discard the required number of cards!");
                    }

                    foreach (var card in discards)
                    {
                        opponent.DiscardCard(card);
                    }
                }
            }
        }
    }
}
