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
            Reaction = 0x20,
            Curse = 0x40,
        }

        public const CardType ActionAttack = CardType.Action | CardType.Attack;
        public const CardType TreasureAction = CardType.Treasure | CardType.Action;
        public const CardType TreasureVictory = CardType.Treasure | CardType.Victory;
        public const CardType VictoryAction = CardType.Victory | CardType.Action;
        public const CardType ReactionAction = CardType.Reaction | CardType.Action;

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

        public virtual void ExecuteCard(Player p, Strategy.IStrategy s, Supply supply)
        {
            p.DrawCards(Draws);
            p.Actions += Actions;
            p.Buys += Buys;
            p.Moneys += Moneys;
        }
    }

    #region No-frills cards
    class CopperCard    : Card { public CopperCard()    : base(CardList.Copper,     Card.CardType.Treasure, 0, 0, 0, 1, 0, 0) {} }
    class SilverCard    : Card { public SilverCard()    : base(CardList.Silver,     Card.CardType.Treasure, 3, 0, 0, 2, 0, 0) {} }
    class GoldCard      : Card { public GoldCard()      : base(CardList.Gold,       Card.CardType.Treasure, 6, 0, 0, 3, 0, 0) {} }
    class EstateCard    : Card { public EstateCard()    : base(CardList.Estate,     Card.CardType.Victory,  2, 0, 0, 0, 0, 1) {} }
    class DuchyCard     : Card { public DuchyCard()     : base(CardList.Duchy,      Card.CardType.Victory,  5, 0, 0, 0, 0, 3) {} }
    class ProvinceCard  : Card { public ProvinceCard()  : base(CardList.Province,   Card.CardType.Victory,  8, 0, 0, 0, 0, 6) {} }
    class CurseCard     : Card { public CurseCard()     : base(CardList.Curse,      Card.CardType.Curse,    0, 0, 0, 0, 0, -1) {} }

    #region Dominion
    class SmithyCard    : Card { public SmithyCard()    : base(CardList.Smithy,     Card.CardType.Action,   4, 3, 0, 0, 0, 0) {} }
    #endregion

    #region Intrigue
    class HaremCard     : Card { public HaremCard()     : base(CardList.Harem,      TreasureVictory,        6, 0, 0, 2, 0, 2) {} }
    #endregion

    #region Seaside
    #endregion

    #endregion
}
