using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.Cards
{
    class ThroneRoomCard : Card
    {
        public ThroneRoomCard()
            : base(CardList.ThroneRoom, CardType.Action, 4, 0, 0, 0, 0, 0)
        {
        }
         
        /// <summary>
        /// Execute this card action.  The Throne Room allows you to choose a card to play twice
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="supply">Supply</param>
        public override void ExecuteCard(Player p, Supply supply)
        {
            base.ExecuteCard(p, supply);

            var playThisTwice = p.Strategy.ChooseCardToPlayTwice(p.GetFacade(), supply);
            if (null != playThisTwice)
            {
                if (!p.Hand.Contains(playThisTwice))
                {
                    throw new Exception("Player " + p.Name + "'s Strategy lied about a card in his hand he wanted to Throne Room!");
                }
                var Logic = playThisTwice.Logic;
                if ((Logic.Type & CardType.Action) == 0)
                {
                    throw new Exception("Player " + p.Name + "'s Strategy provided a non-action card to Throne Room!");
                }

                p.Log("      Throne Room playing " + playThisTwice + " twice!");

                // The card only goes from the Hand to the PlayPile the first time.  This also costs 0 actions
                p.MoveCard(playThisTwice, p.Hand, p.PlayPile);
                playThisTwice.Logic.ExecuteCardTwice(p, supply);
            }
        }
    }
}
