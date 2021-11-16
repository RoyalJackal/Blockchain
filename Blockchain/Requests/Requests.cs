using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Blockchain.Requests
{
    public static class Requests
    {
        public static readonly DateTime StartDate = new DateTime(2021, 1, 1, 0, 0, 0);
        public static readonly DateTime EndDate = new DateTime(2021, 2, 1, 0, 0, 0);
        public static readonly DateTime PrevDate = new DateTime(2020, 12, 1, 0, 0, 0);
        public static readonly DateTime NextDate = new DateTime(2021, 3, 1, 0, 0, 0);
        public static readonly DateTime StartDateTotal = new DateTime(2020, 4, 1, 0, 0, 0);
        public static readonly DateTime EndDateTotal = new DateTime(2021, 11, 1, 0, 0, 0);
        public static readonly List<DateTime> Data = GetData();

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

            return result;
        }

        public static List<int> PerMonday()
        {
            var result = new List<int>();
            var date = PrevDate;
            var nextDate = date + TimeSpan.FromDays(7);
            var endDate = NextDate;
            while (date < endDate)
            {
                var count = Data.Count(x => x >= date && x < nextDate && x < endDate && date.DayOfWeek == DayOfWeek.Monday);
                result.Add(count);
                date = nextDate;
                nextDate += TimeSpan.FromDays(7);
            }

            return result;
        }

        public static List<int> PerSunday()
        {
            var result = new List<int>();
            var date = PrevDate;
            var nextDate = date + TimeSpan.FromDays(7);
            var endDate = NextDate;
            while (date < endDate)
            {
                var count = Data.Count(x => x >= date && x < nextDate && x < endDate && date.DayOfWeek == DayOfWeek.Sunday);
                result.Add(count);
                date = nextDate;
                nextDate += TimeSpan.FromDays(7);
            }

            return result;
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

            return result;
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

            return result;
        }
    }
}
