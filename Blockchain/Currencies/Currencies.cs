using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
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

        public static string Bayes((double, double)[,] matrix, List<int> scores, out List<int> results)
        {
            results = scores;
            var values = new List<double>();
            for (var i = 0; i < 4; i++)
            {
                values.Add(0);
                for (var j = 0; j < 4; j++)
                {
                    values[i] += matrix[i, j].Item1 * matrix[i, j].Item2;
                }
            }

            var value = values.Max();
            var result = values.IndexOf(value);
            results[result]++;
            return result switch
            {
                0 => "s1",
                1 => "s1",
                2 => "s1",
                3 => "s1",
                _ => throw new Exception("Error on finding most valuable S.")
            };
        }

        public static string Wald((double, double)[,] matrix, List<int> scores, out List<int> results)
        {
            results = scores;
            var mins = new List<double>();
            for (int i = 0; i < matrix.GetLength(0); i++)
                mins.Add(new[] {matrix[i, 0], matrix[i, 1], matrix[i, 2], matrix[i, 3]}.Min(x => x.Item1));

            var maxMinIndex = mins.IndexOf(mins.Max());
            results[maxMinIndex]++;
            return $"s{maxMinIndex + 1}";
        }

        public static string Optimistic((double, double)[,] matrix, List<int> scores, out List<int> results)
        {
            results = scores;
            var maxs = new List<double>();
            for (int i = 0; i < matrix.GetLength(0); i++)
                maxs.Add(new[] {matrix[i, 0], matrix[i, 1], matrix[i, 2], matrix[i, 3]}.Max(x => x.Item1));

            var maxMaxIndex = maxs.IndexOf(maxs.Max());
            results[maxMaxIndex]++;
            return $"s{maxMaxIndex + 1}";
        }

        public static string Savage((double, double)[,] matrix, List<int> scores, out List<int> results)
        {
            results = scores;
            var maxs = new List<double>();
            for (int i = 0; i < matrix.GetLength(1); i++)
                maxs.Add(new[] {matrix[0, i], matrix[1, i], matrix[2, i], matrix[3, i]}.Max(x => x.Item1));

            var regretMatrix = new double[4, 4];
            for (var i = 0; i < regretMatrix.GetLength(0); i++)
                for (var j = 0; j < regretMatrix.GetLength(1); j++)
                    regretMatrix[i, j] = maxs[j] - matrix[i, j].Item1;

                var maxRegrets = new List<double>();
            for (int i = 0; i < matrix.GetLength(0); i++)
                maxRegrets.Add(new[] {regretMatrix[i, 0], regretMatrix[i, 1], regretMatrix[i, 2], regretMatrix[i, 3]}.Min());

            var minMaxRegretIndex = maxRegrets.IndexOf(maxRegrets.Min());
            results[minMaxRegretIndex]++;
            return $"s{minMaxRegretIndex + 1}";
        }

        public static string Hurwitz((double, double)[,] matrix, double coeff, List<int> scores, out List<int> results)
        {
            results = scores;
            var maxs = new List<double>();
            for (int i = 0; i < matrix.GetLength(0); i++)
                maxs.Add(new[] {matrix[i, 0], matrix[i, 1], matrix[i, 2], matrix[i, 3]}.Max(x => x.Item1) * coeff);

            var mins = new List<double>();
            for (int i = 0; i < matrix.GetLength(0); i++)
                mins.Add(new[] {matrix[i, 0], matrix[i, 1], matrix[i, 2], matrix[i, 3]}.Min(x => x.Item1) * (1 - coeff));

            var sums = new List<double>();
            for (int i = 0; i < matrix.GetLength(0); i++)
                sums.Add(mins[i] + maxs[i]);

            var maxSumIndex = sums.IndexOf(sums.Min());
            results[maxSumIndex]++;
            return $"s{maxSumIndex + 1}";
        }


        public static string GetUrl((double, double)[,] matrix, string cur1, string cur2, string cur3, string cur4,
            string name, string s)
        {
            var data = "{" +
                       $"\"name\": \"{name}\"," +
                       $" \"currency1\": \"{cur1}\"," +
                       $" \"currency2\": \"{cur2}\"," +
                       $" \"currency3\": \"{cur3}\"," +
                       $" \"currency4\": \"{cur4}\"," +
                       $" \"strategy\": \"{s}\"," +

                       $" \"x11\": \"{matrix[0, 0].Item1}\"," +
                       $" \"x12\": \"{matrix[0, 1].Item1}\"," +
                       $" \"x13\": \"{matrix[0, 2].Item1}\"," +
                       $" \"x14\": \"{matrix[0, 3].Item1}\"," +
                       $" \"x21\": \"{matrix[1, 0].Item1}\"," +
                       $" \"x22\": \"{matrix[1, 1].Item1}\"," +
                       $" \"x23\": \"{matrix[1, 2].Item1}\"," +
                       $" \"x24\": \"{matrix[1, 3].Item1}\"," +
                       $" \"x31\": \"{matrix[2, 0].Item1}\"," +
                       $" \"x32\": \"{matrix[2, 1].Item1}\"," +
                       $" \"x33\": \"{matrix[2, 2].Item1}\"," +
                       $" \"x34\": \"{matrix[2, 3].Item1}\"," +
                       $" \"x41\": \"{matrix[3, 0].Item1}\"," +
                       $" \"x42\": \"{matrix[3, 1].Item1}\"," +
                       $" \"x43\": \"{matrix[3, 2].Item1}\"," +
                       $" \"x44\": \"{matrix[3, 3].Item1}\"," +

                       $" \"p11\": \"{matrix[0, 0].Item2}\"," +
                       $" \"p12\": \"{matrix[0, 1].Item2}\"," +
                       $" \"p13\": \"{matrix[0, 2].Item2}\"," +
                       $" \"p14\": \"{matrix[0, 3].Item2}\"," +
                       $" \"p21\": \"{matrix[1, 0].Item2}\"," +
                       $" \"p22\": \"{matrix[1, 1].Item2}\"," +
                       $" \"p23\": \"{matrix[1, 2].Item2}\"," +
                       $" \"p24\": \"{matrix[1, 3].Item2}\"," +
                       $" \"p31\": \"{matrix[2, 0].Item2}\"," +
                       $" \"p32\": \"{matrix[2, 1].Item2}\"," +
                       $" \"p33\": \"{matrix[2, 2].Item2}\"," +
                       $" \"p34\": \"{matrix[2, 3].Item2}\"," +
                       $" \"p41\": \"{matrix[3, 0].Item2}\"," +
                       $" \"p42\": \"{matrix[3, 1].Item2}\"," +
                       $" \"p43\": \"{matrix[3, 2].Item2}\"," +
                       $" \"p44\": \"{matrix[3, 3].Item2}\"" +
                       "}";
            var zalupa = UrlEncoder.Create();
            var result = @"http://188.93.211.195/currency/add?value=" + zalupa.Encode(data);
            return result;
        }
    }
}
