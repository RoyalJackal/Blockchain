using System;
using System.Collections.Generic;
using System.Linq;
using static Blockchain.Currencies.Constants;

namespace Blockchain.Currencies.Entities
{
    public class Portion
    {
        public List<CurrencyData>[] Values { get; set; }

        public double[] Variance { get; set; } = new double[Strategies.Count];

        public double[] Profit { get; set; } = new double[Strategies.Count];

        public int RiskStrategy { get; set; }

        public EnvironmentType[] EnvironmentTypes = new EnvironmentType[Strategies.Count];


        public Portion(List<CurrencyData>[] values)
        {
            Values = values;
            CalculateProfit();
            CalculateVariance();
            CalculateRiskStrategyIndex();
            CalculateEnvironmentTypes();
        }


        private void CalculateProfit()
        {
            for (var i = 0; i < Strategies.Count; i++)
            {
                Profit[i] = Budget * (
                    Strategies[i][0] *
                    (Values[0].Last().Close / Values[0][0].Open - 1) +

                    Strategies[i][1] *
                    (Values[1].Last().Close / Values[1][0].Open - 1) +

                    Strategies[i][2] *
                    (Values[2].Last().Close / Values[2][0].Open - 1) +

                    Strategies[i][3] *
                    (Values[3].Last().Close / Values[3][0].Open - 1)
                );
            }
        }

        private void CalculateVariance()
        {
            for (int i = 0; i < Strategies.Count; i++)
            {
                var median = 0.0;
                for (int j = 0; j < Strategies.CoeffCount; j++)
                    median += Values[j].Sum(x => x.Close * Strategies[i][j]);
                median /= Values[0].Count;

                var variance = 0.0;
                for (int j = 0; j < Strategies.CoeffCount; j++)
                    variance += Math.Pow(Values[j].Sum(x => x.Close * Strategies[i][j]) - median, 2);
                Variance[i] = variance / Values[0].Count;
            }
        }

        private void CalculateRiskStrategyIndex()
        {
            RiskStrategy = Variance.ToList().IndexOf(Variance.Max());
        }

        private void CalculateEnvironmentTypes()
        {
            for (int i = 0; i < Strategies.Count; i++)
            {
                if (Profit[i] > A && i == RiskStrategy)
                    EnvironmentTypes[i] = EnvironmentType.P1;
                if (Profit[i] <= A && i == RiskStrategy)
                    EnvironmentTypes[i] = EnvironmentType.P2;
                if (Profit[i] > A && i != RiskStrategy)
                    EnvironmentTypes[i] = EnvironmentType.P3;
                if (Profit[i] <= A && i != RiskStrategy)
                    EnvironmentTypes[i] = EnvironmentType.P4;
            }
        }

        public void Print()
        {
            Console.WriteLine($"Выборка от {Values[0][0].Date} до {Values[0].Last().Date}");
            for (int i = 0; i < Strategies.Count; i++)
            {
                Console.WriteLine($"profit: {Profit[i]}; variance: {Variance[i]}; environment: {EnvironmentTypes[i].ToString()}");
            }
        }
    }
}
