using Blockchain.Currencies.Entities;

namespace Blockchain.Currencies
{
    public static class Constants
    {
        public const int Budget = 1000000;
        public const int PortionSize = 5;
        public const int A = 3000;
        public static readonly StrategiesClass Strategies = new StrategiesClass(
            new Strategy(0.1, 0.15, 0.15, 0.6),
            new Strategy(0.3, 0.3, 0.3, 0.1),
            new Strategy(0.6, 0.1, 0.1, 0.2),
            new Strategy(0.35, 0.35, 0.15, 0.15));
    }
}
