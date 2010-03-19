using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
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
        Any = 0xFF,
    }

    abstract class CardBase
    {
        public const CardType ActionAttack = CardType.Action | CardType.Attack;
        public const CardType TreasureAction = CardType.Treasure | CardType.Action;
        public const CardType TreasureVictory = CardType.Treasure | CardType.Victory;
        public const CardType VictoryAction = CardType.Victory | CardType.Action;
        public const CardType ReactionAction = CardType.Reaction | CardType.Action;

        public string Name { get; set; }
        public Card Card { get; set; }
        public CardType Type { get; set; }
        public int Cost { get; set; }
        public int Buys { get; set; }
        public int Actions { get; set; }
        public int Draws { get; set; }
        public int Moneys { get; set; }
        public int VictoryPoints { get; set; }

        public CardBase(string name, Card card, CardType type, int cost, int draws, int actions, int moneys, int buys, int vps)
        {
            Name = name;
            Card = card;
            Type = type;
            Cost = cost;
            Draws = draws;
            Actions = actions;
            Moneys = moneys;
            Buys = buys;
            VictoryPoints = vps;
        }

        public virtual void ExecuteCard(Player p, Supply supply)
        {
            if (Draws > 0)
            {
                p.AddCardsToHand(p.DrawCards(Draws));
            }
            p.Actions += Actions;
            p.Buys += Buys;
            p.Moneys += Moneys;
        }

        /// <summary>
        /// Perform a reaction
        /// </summary>
        /// <param name="attacker">One performing the attack</param>
        /// <param name="victim">One getting hit by the attack</param>
        /// <param name="supply">The supply</param>
        /// <returns>True if the attack was successfully prevented from affecting the victim</returns>
        public virtual bool ExecuteReaction(Player attacker, Player victim, Supply supply)
        {
            return false;
        }

        /// <summary>
        /// Perform this card as an attack against the given opponent
        /// </summary>
        /// <param name="attacker">Player attacking</param>
        /// <param name="victim">One being attacked</param>
        /// <param name="supply">The supply</param>
        /// <returns>True if the victim blocked the attack</returns>
        protected bool HandleAttackReactions(Player attacker, Player victim, Supply supply)
        {
            // Tell the victim's strategy to attack, and then tell the victim's player what happened so
            // it can appropriately 
            var attackReactionCards = victim.Strategy.ChooseReactionsToAttack(victim.GetFacade(), supply, attacker.Name, Card);
            victim.AttackedBy(attacker.Name, Card, attackReactionCards);

            // Double check that the Strategy isn't lying.
            // I don't have an easy way to double check that he didn't list the same card a lot.
            if (victim.Hand.Intersect(attackReactionCards).Count() != attackReactionCards.Distinct().Count())
            {
                throw new Exception("Victim " + victim.Name + "'s Strategy lied about the Reaction cards in his hand!");
            }

            // Get every card he reacted with combined with his duration cards.  Find only the ones of type Reaction.
            // Attempt to react to them all, and return true if any of their ExecuteReaction functions returned true.
            return attackReactionCards.Union(victim.DurationCards)
                                      .Where(c => (CardList.Cards[c].Type & CardType.Reaction) != 0)
                                      .Aggregate(false, (blocked, card) => blocked || CardList.Cards[card].ExecuteReaction(attacker, victim, supply));
        }
    }

    #region No-frills cards
    class CopperCard    : CardBase { public CopperCard()    : base("Copper", Card.Copper,     CardType.Treasure, 0, 0, 0, 1, 0, 0) {} }
    class SilverCard    : CardBase { public SilverCard()    : base("Silver", Card.Silver,     CardType.Treasure, 3, 0, 0, 2, 0, 0) {} }
    class GoldCard      : CardBase { public GoldCard()      : base("Gold", Card.Gold,       CardType.Treasure, 6, 0, 0, 3, 0, 0) {} }
    class EstateCard    : CardBase { public EstateCard()    : base("Estate", Card.Estate,     CardType.Victory,  2, 0, 0, 0, 0, 1) {} }
    class DuchyCard     : CardBase { public DuchyCard()     : base("Duchy", Card.Duchy,      CardType.Victory,  5, 0, 0, 0, 0, 3) {} }
    class ProvinceCard  : CardBase { public ProvinceCard()  : base("Province", Card.Province,   CardType.Victory,  8, 0, 0, 0, 0, 6) {} }
    class CurseCard     : CardBase { public CurseCard()     : base("Curse", Card.Curse,      CardType.Curse,    0, 0, 0, 0, 0, -1) {} }

    #region Dominion
    class SmithyCard    : CardBase { public SmithyCard()    : base("Smithy", Card.Smithy,     CardType.Action,   4, 3, 0, 0, 0, 0) {} }
    class VillageCard   : CardBase { public VillageCard()   : base("Village", Card.Village,    CardType.Action,   3, 1, 2, 0, 0, 0) {} }
    class LaboratoryCard : CardBase { public LaboratoryCard() : base("Laboratory", Card.Laboratory, CardType.Action, 5, 2, 1, 0, 0, 0) {} }
    class FestivalCard  : CardBase { public FestivalCard()  : base("Festival", Card.Festival,   CardType.Action,   5, 0, 2, 2, 1, 0) {} }
    class MarketCard    : CardBase { public MarketCard()    : base("Market", Card.Market,     CardType.Action,   5, 1, 1, 1, 1, 0) {} }
    class WoodcutterCard : CardBase { public WoodcutterCard() : base("Woodcutter", Card.Woodcutter, CardType.Action,      3, 0, 0, 2, 1, 0) {} }
    #endregion

    #region Intrigue
    class HaremCard     : CardBase { public HaremCard()     : base("Harem", Card.Harem,      TreasureVictory,        6, 0, 0, 2, 0, 2) {} }
    #endregion

    #region Seaside
    #endregion

    #endregion
}
