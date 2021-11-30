using System;
using System.Collections.Generic;
using System.Linq;
using Blockchain.Currencies.Entities;
using Org.BouncyCastle.Crypto.Generators;

namespace Blockchain.Currencies
{
    public static class Currencies
    {
        public static List<Portion> MakePortions(List<CurrencyData> currency1, List<CurrencyData> currency2,
            List<CurrencyData> currency3, List<CurrencyData> currency4, int portionSize)
        {
            if (currency1.Count != currency2.Count || currency1.Count != currency3.Count ||
                currency1.Count != currency4.Count)
                throw new Exception("Different currency data packs has different sizes.");

            if (currency1.Count % portionSize != 0)
                throw new Exception("Data pack cannot be equally partitioned with current portion size.");

            var portions = new List<Portion>();
            for (int i = 0; i < currency1.Count; i+=portionSize)
            {
                portions.Add(new Portion(new List<CurrencyData>[]
                {
                    currency1.GetRange(i, portionSize),
                    currency2.GetRange(i, portionSize),
                    currency3.GetRange(i, portionSize),
                    currency4.GetRange(i, portionSize),
                }));
            }

            return portions;
        }

        public static (double, double)[,] MakeMatrix(List<Portion> portions)
        {
            var matrix = new (double, double)[4, 4];

            for (var i = 0; i < Constants.Strategies.Count; i++)
            {
                var j = 0;
                foreach (var type in Enum.GetValues<EnvironmentType>())
                {
                    var sum = portions.Where(x => x.EnvironmentTypes[i] == type).Sum(x => x.Profit[i]);
                    var count = portions.Count(x => x.EnvironmentTypes[i] == type);
                    matrix[i, j] = (count == 0 ? 0 : sum / count, count / (double)portions.Count);
                    j++;
                }
            }

            return matrix;
        }

        public static void PrintMatrix((double, double)[,] matrix)
        {
            for (int i = 0; i < 4; i++)
            {
                Console.Write($"s{i + 1}: | ");
                for (int j = 0; j < 4; j++)
                {
                    Console.Write($"( {matrix[i, j].Item1:F3} , {matrix[i, j].Item2:F2} ) | ");
                }
                Console.WriteLine();
            }
        }

        public static string GetMostValuableS((double, double)[,] matrix, out double value)
        {
            var values = new List<double>();
            for (var i = 0; i < 4; i++)
            {
                values.Add(0);
                for (var j = 0; j < 4; j++)
                {
                    values[i] += matrix[i, j].Item1 * matrix[i, j].Item2;
                }
            }

            value = values.Max();
            var result = values.IndexOf(value);
            return result switch
            {
                0 => "s1",
                1 => "s1",
                2 => "s1",
                3 => "s1",
                _ => throw new Exception("Error on finding most valuable S.")
            };
        }
    }
}
