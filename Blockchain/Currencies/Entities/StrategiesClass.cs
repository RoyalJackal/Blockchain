using System.Collections.Generic;

namespace Blockchain.Currencies.Entities
{
    public class StrategiesClass
    {
        public StrategiesClass(Strategy strat1, Strategy strat2, Strategy strat3, Strategy strat4)
        {
            _values = new List<Strategy>
            {
                strat1,
                strat2,
                strat3,
                strat4
            };
        }

        private readonly List<Strategy> _values;

        public int Count => _values.Count;

        public int CoeffCount => _values[0].Count;

        public Strategy this[int index] => _values[index];
    }

    public class Strategy
    {
        public Strategy(double coeff1, double coeff2, double coeff3, double coeff4)
        {
            _values = new List<double>
            {
                coeff1,
                coeff2,
                coeff3,
                coeff4
            };
        }

        private readonly List<double> _values;

        public int Count => _values.Count;

        public double this[int index] => _values[index];
    }
}
