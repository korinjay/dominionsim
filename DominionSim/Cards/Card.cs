using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    using CardIdentifier = String;


    abstract class Card
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

        public const CardType ActionAttack = CardType.Action | CardType.Attack;
        public const CardType TreasureAction = CardType.Treasure | CardType.Action;
        public const CardType TreasureVictory = CardType.Treasure | CardType.Victory;
        public const CardType VictoryAction = CardType.Victory | CardType.Action;
        public const CardType ReactionAction = CardType.Reaction | CardType.Action;

        /// <summary>
        /// The Name of this card.
        /// Currently just returns the CardId
        /// </summary>
        public String Name { get { return CardId; } }

        /// <summary>
        /// Identifier for the kind of card this is
        /// </summary>
        public CardIdentifier CardId { get; set; }

        public CardType Type { get; set; }
        public int Cost { get; set; }
        public int Buys { get; set; }
        public int Actions { get; set; }
        public int Draws { get; set; }
        public int Moneys { get; set; }
        public int VictoryPoints { get; set; }

        public Card(CardIdentifier cardId, CardType type, int cost, int draws, int actions, int moneys, int buys, int vps)
        {
            CardId = cardId;
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
            var attackReactionCards = victim.Strategy.ChooseReactionsToAttack(victim.GetFacade(), supply, attacker.Name, CardId);
            victim.AttackedBy(attacker.Name, CardId, attackReactionCards);

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
                                      .Aggregate(false, (blocked, cardId) => blocked || CardList.Cards[cardId].ExecuteReaction(attacker, victim, supply));
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
    class VillageCard   : Card { public VillageCard()   : base(CardList.Village,    Card.CardType.Action,   3, 1, 2, 0, 0, 0) {} }
    class LaboratoryCard : Card { public LaboratoryCard() : base(CardList.Laboratory, Card.CardType.Action, 5, 2, 1, 0, 0, 0) {} }
    class FestivalCard  : Card { public FestivalCard()  : base(CardList.Festival,   Card.CardType.Action,   5, 0, 2, 2, 1, 0) {} }
    class MarketCard    : Card { public MarketCard()    : base(CardList.Market,     Card.CardType.Action,   5, 1, 1, 1, 1, 0) {} }
    class WoodcutterCard : Card { public WoodcutterCard() : base(CardList.Woodcutter, CardType.Action,      3, 0, 0, 2, 1, 0) {} }
    #endregion

    #region Intrigue
    class HaremCard     : Card { public HaremCard()     : base(CardList.Harem,      TreasureVictory,        6, 0, 0, 2, 0, 2) {} }
    #endregion

    #region Seaside
    #endregion

    #endregion
}
