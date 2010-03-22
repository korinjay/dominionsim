using System;
using System.Linq;
using DominionSim.VirtualCards;

namespace DominionSim
{

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
        public const CardType ActionVictory = CardType.Action | CardType.Victory;
        public const CardType TreasureAction = CardType.Treasure | CardType.Action;
        public const CardType TreasureVictory = CardType.Treasure | CardType.Victory;
        public const CardType VictoryAction = CardType.Victory | CardType.Action;
        public const CardType ReactionAction = CardType.Reaction | CardType.Action;
        public const CardType ActionDuration = CardType.Action | CardType.Duration;

        /// <summary>
        /// The Name of this card.
        /// Currently just returns the CardId
        /// </summary>
        public String Name { get { return CardId.ToString(); } }

        /// <summary>
        /// Identifier for the kind of card this is
        /// </summary>
        public CardIdentifier CardId { get; private set; }

        public CardType Type { get; private set; }
        public int Cost { get; private set; }
        public int Buys { get; private set; }
        public int Actions { get; private set; }
        public int Draws { get; private set; }
        public int Moneys { get; private set; }

        public int DurationBuys { get; private set; }
        public int DurationActions { get; private set; }
        public int DurationDraws { get; private set; }
        public int DurationMoneys { get; private set; }

        // If this card is just simply worth N VPs, this property is used
        private int mSimpleVPs;

        public Card(CardIdentifier cardId, CardType type, int cost, int draws, int actions, int moneys, int buys, int vps)
        {
            CardId = cardId;
            Type = type;
            Cost = cost;
            Draws = draws;
            Actions = actions;
            Moneys = moneys;
            Buys = buys;
            mSimpleVPs = vps;

            DurationDraws = 0;
            DurationActions = 0;
            DurationMoneys = 0;
            DurationBuys = 0;
        }

        public Card(CardIdentifier cardId, CardType type, int cost, int draws, int actions, int moneys, int buys, int vps, int durationDraws, int durationActions, int durationMoneys, int durationBuys)
            : this(cardId, type, cost, draws, actions, moneys, buys, vps)
        {
            DurationDraws = durationDraws;
            DurationActions = durationActions;
            DurationMoneys = durationMoneys;
            DurationBuys = durationBuys;
        }

        public virtual int GetNumVictoryPoints(VirtualCardList deck)
        {
            return mSimpleVPs;
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
        /// Execute this card when it is in the Duration pile
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="supply">Supply</param>
        public virtual void ExecuteDuration(Player p, Supply supply)
        {
            if (DurationDraws > 0)
            {
                p.AddCardsToHand(p.DrawCards(DurationDraws));
            }
            p.Actions += DurationActions;
            p.Buys += DurationBuys;
            p.Moneys += DurationMoneys;
        }

        /// <summary>
        /// When playing the Throne Room card, another card can be executed twice.  While usually
        /// this simply means executing... twice.  But in some cases there might be some weirdness
        /// going on, especially because we don't *actually* have Card objects anywhere to track
        /// the current state of cards (notable example: you can't trash a Card twice).
        /// 
        /// Note also: We could have made this ExecuteCard(numTimes), but this is so rare (1 card of
        /// 76 has this capability) that I don't want to generalize.
        /// </summary>
        /// <param name="p">Player</param>
        /// <param name="supply">Supply</param>
        public virtual void ExecuteCardTwice(Player p, Supply supply)
        {
            ExecuteCard(p, supply);
            ExecuteCard(p, supply);
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
        /// Perform a reaction when in the Duration pile
        /// </summary>
        /// <param name="attacker">One performing the attack</param>
        /// <param name="victim">One getting hit by the attack</param>
        /// <param name="supply">The supply</param>
        /// <returns>True if the attack was successfully prevented from affecting the victim</returns>
        public virtual bool ExecuteDurationReaction(Player attacker, Player victim, Supply supply)
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

            // Get every card he reacted with.  Find only the ones of type Reaction.
            // Attempt to react to them all, and figure out if any blocked the attack.
            var blockedAttack = attackReactionCards.Where(c => (c.Logic.Type & CardType.Reaction) != 0)
                                                   .Aggregate(false, (blocked, cardId) => blocked || cardId.Logic.ExecuteReaction(attacker, victim, supply));

            // Also check against all the Duration cards.
            blockedAttack = blockedAttack || victim.DurationPile.Aggregate(false, (blocked, cardId) => blocked || cardId.Logic.ExecuteDurationReaction(attacker, victim, supply));

            return blockedAttack;
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
    class GreatHallCard : Card { public GreatHallCard() : base(CardList.GreatHall,  ActionVictory,          3, 1, 1, 0, 0, 1) {} }
    class HaremCard     : Card { public HaremCard()     : base(CardList.Harem,      TreasureVictory,        6, 0, 0, 2, 0, 2) {} }
    #endregion

    #region Seaside
    class FishingVillageCard : Card { public FishingVillageCard() : base(CardList.FishingVillage, ActionDuration, 3, 0, 2, 1, 0, 0, 0, 1, 1, 0) {} }
    class MerchantShipCard   : Card { public MerchantShipCard()   : base(CardList.MerchantShip,   ActionDuration, 5, 0, 0, 2, 0, 0, 0, 0, 2, 0) {} }
    class CaravanCard        : Card { public CaravanCard()        : base(CardList.Caravan,        ActionDuration, 4, 1, 1, 0, 0, 0, 1, 0, 0, 0) {} }
    class WharfCard          : Card { public WharfCard()          : base(CardList.Wharf,          ActionDuration, 5, 2, 0, 0, 1, 0, 2, 0, 0, 1) {} }
    #endregion

    #endregion
}
