using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Org.BouncyCastle.Asn1.Esf;

namespace Blockchain.Requests
{
    public static class Requests
    {
        public static readonly DateTime StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
        public static readonly DateTime EndDate = new DateTime(2021, 2, 1, 0, 0, 0);
        public static readonly DateTime PrevDate = new DateTime(2020, 12, 1, 0, 0, 0);
        public static readonly DateTime NextDate = new DateTime(2021, 3, 1, 0, 0, 0);
        public static readonly DateTime StartDateTotal = new DateTime(2020, 4, 1, 0, 0, 0);
        public static readonly DateTime EndDateTotal = new DateTime(2021, 9, 1, 0, 0, 0);
        public static readonly List<DateTime> Data = GetData();
        
        /// <summary>
        /// Интенсивность обслуживания одним каналом в час
        /// </summary>
        public static readonly double Myu = 1 / (2.3 / 3600);


        private static int _cores = 4;
        /// <summary>
        /// Число потоков
        /// </summary>
        public static int N => _cores * 2;

        /// <summary>
        /// Таймаут обработки
        /// </summary>
        public static readonly double T = 30 / (double)3600;

        /// <summary>
        /// Интенсивность потока
        /// </summary>
        public static readonly double Lambda = Intensity();

        /// <summary>
        /// Интенсивность нагрузки
        /// </summary>    
        public static double Rho => Lambda / Myu;

        public static double A => N / Rho;
        public static double B => Lambda * T;
        public static double C => Math.Pow(Math.E, B * (1 - A));
        public static double BTilda => (C - 1) / (1 - A);
        public static double DTilda => (A - C * (A + B * (A - 1))) / (B * (A - 1) * (A - 1));


        /// <summary>
        /// Доля времени, когда все каналы свободны
        /// </summary>
        public static double P0 => 1 / (Enumerable.Range(0, N).Sum(k => Math.Pow(Rho, k) / Factorial(k)) + Math.Pow(Rho, N) * BTilda / Factorial(N));
        
        /// <summary>
        /// Вероятность отказа
        /// </summary>
        public static double POtk => Math.Pow(Rho, N) * C * P0 / Factorial(N);

        /// <summary>
        /// Среднее время в очереди
        /// </summary>
        public static double W0 => T * Math.Pow(Rho, N) * DTilda * P0 / Factorial(N);

        public static double PenaltyCount => Lambda * 730 * POtk;

        public static double Price(double x, int coreCount)
        {
            _cores = coreCount;
            var result = PenaltyCount * 16 * x + _cores * 2400 * 16;
            _cores = 2;
            return result;
        }

        public static double CalcX(int coreCount)
        {
            _cores = coreCount;
            var penalties1 = PenaltyCount * 16;
            _cores = coreCount + 1;
            var penalties2 = PenaltyCount * 16;
            _cores = 2;
            return 2400 * 16 / (penalties1 - penalties2);

        }

        private static double Factorial(int x)
        {
            var result = 1.0;
            for (int i = 1; i <= x; i++)
            {
                result *= i;
            }

            return result;
        }

        public static List<DateTime> GetData() =>
            File.ReadAllLines("ddd.csv")
                .Select(l => DateTime.Parse(l))
                .ToList();

        public static List<int> PerHour()
        {
            var result = new List<int>();
            var date = StartDate;
            var nextDate = date + TimeSpan.FromHours(1);
            var endDate = date + TimeSpan.FromDays(3);
            while (date < endDate)
            {
                var count = Data.Count(x => x >= date && x < nextDate && x < endDate);
                result.Add(count);
                date = nextDate;
                nextDate += TimeSpan.FromHours(1);
            }

            return result;
        }

        public static List<int> PerDay()
        {
            var result = new List<int>();
            var date = StartDate;
            var nextDate = date + TimeSpan.FromDays(1);
            var endDate = EndDate;
            while (date < endDate)
            {
                var count = Data.Count(x => x >= date && x < nextDate && x < endDate);
                result.Add(count);
                date = nextDate;
                nextDate += TimeSpan.FromDays(1);
            }

            return result.Select(x => x / 24).ToList();
        }

        public static List<int> PerMonday()
        {
            var result = new List<int>();
            var date = PrevDate;
            var nextDate = date + TimeSpan.FromDays(1);
            var endDate = NextDate;
            while (date < endDate)
            {
                if (date.DayOfWeek == DayOfWeek.Monday)
                {
                    var count = Data.Count(x => x >= date && x < nextDate && x < endDate);
                    result.Add(count);
                }
                date = nextDate;
                nextDate += TimeSpan.FromDays(1);
            }

            return result.Select(x => x / 24).ToList();
        }

        public static List<int> PerSunday()
        {
            var result = new List<int>();
            var date = PrevDate;
            var nextDate = date + TimeSpan.FromDays(1);
            var endDate = NextDate;
            while (date < endDate)
            {
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    var count = Data.Count(x => x >= date && x < nextDate && x < endDate);
                    result.Add(count);
                }
                date = nextDate;
                nextDate += TimeSpan.FromDays(1);
            }

            return result.Select(x => x / 24).ToList();
        }

        public static List<int> PerWeek()
        {
            var result = new List<int>();
            var date = PrevDate;
            var nextDate = date + TimeSpan.FromDays(7);
            var endDate = NextDate;
            while (date < endDate)
            {
                var count = Data.Count(x => x >= date && x < nextDate && x < endDate);
                result.Add(count);
                date = nextDate;
                nextDate += TimeSpan.FromDays(7);
            }

            return result.Select(x => x / 168).ToList();
        }

        public static List<int> PerMonth()
        {
            var result = new List<int>();
            var date = StartDateTotal;
            var nextDate = date + TimeSpan.FromDays(DateTime.DaysInMonth(date.Year, date.Month));
            var endDate = EndDateTotal;
            while (date < endDate)
            {
                var count = Data.Count(x => x >= date && x < nextDate && x < endDate);
                result.Add(count);
                date = nextDate;
                nextDate += TimeSpan.FromDays(DateTime.DaysInMonth(nextDate.Year, nextDate.Month));
            }

            return result.Select(x => x / 730).ToList();
        }

        private static double Median(List<int> values) =>
            values.Sum() / (double)values.Count;

        public static (double, double) Variance(List<int> values)
        {
            var median = Median(values);
            return (values.Sum(x => Math.Pow(x - median, 2)) / values.Count, median);
        }

        public static double Intensity()
        {
            var variances = new List<(double, double)>
            {
                Variance(PerHour()),
                Variance(PerDay()),
                Variance(PerMonday()),
                Variance(PerSunday()),
                Variance(PerWeek()),
                Variance(PerMonth())
            };

            var minVariance = variances.Min(x => x.Item1);
            var tuple = variances.First(x => Math.Abs(x.Item1 - minVariance) < 1E-6);
            return (tuple.Item1 + tuple.Item2) / 2;
        }

    }
}
