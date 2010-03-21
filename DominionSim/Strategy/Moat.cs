
namespace DominionSim.Strategy
{
    class Moat1 : BuyOneCard
    {
        public Moat1() : base(CardList.Moat, 1) { }
        public static string GetDisplayName() { return "Moat - Buy 1"; }
    }
    class Moat2 : BuyOneCard
    {
        public Moat2() : base(CardList.Moat, 2) { }
        public static string GetDisplayName() { return "Moat - Buy 2"; }
    }
    class Moat3 : BuyOneCard
    {
        public Moat3() : base(CardList.Moat, 3) { }
        public static string GetDisplayName() { return "Moat - Buy 3"; }
    }
}
