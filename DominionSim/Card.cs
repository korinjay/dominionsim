using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    class Card
    {
        [FlagsAttribute] 
        public enum CardType
        {
            Treasure = 0x01,
            Action = 0x02,
            Victory = 0x04,
            Duration = 0x08,
            Attack = 0x10,
            Reaction = 0x20
        }

        public const CardType TreasureAction = CardType.Treasure | CardType.Action;
        public const CardType TreasureVictory = CardType.Treasure | CardType.Victory;
        public const CardType VictoryAction = CardType.Victory | CardType.Action;

        public string Name { get; set; }
        public CardType Type { get; set; }
        public int Cost { get; set; }
        public int Buys { get; set; }
        public int Actions { get; set; }
        public int Draws { get; set; }
        public int Moneys { get; set; }
        public int VictoryPoints { get; set; }

        public Card(string name, CardType type, int cost, int draws, int actions, int moneys, int buys, int vps)
        {
            Name = name;
            Type = type;
            Cost = cost;
            Draws = draws;
            Actions = actions;
            Moneys = moneys;
            Buys = buys;
            VictoryPoints = vps;
        }

        public void ExecuteCard(Player p)
        {
            p.DrawCards(Draws);
            p.Actions += Actions;
            p.Buys += Buys;
            p.Moneys += Moneys;
        }
    }
}
