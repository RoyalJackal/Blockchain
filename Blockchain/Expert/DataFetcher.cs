using System;
using System.Collections.Generic;
using System.Linq;
using Blockchain.Expert.Enums;

namespace Blockchain.Expert
{
    public static class DataFetcher
    {
        public static List<T> Fetch<T>() where T : struct, Enum
        {
            var stings = Enum
                .GetNames<T>()
                .Select(x => x.ToLowerInvariant())
                .ToList();

            Console.WriteLine($"Write down {typeof(T).Name}s from this list separated by comma:");
            Console.WriteLine(string.Join(", ", stings));
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                return new List<T>();

            var cutInput = RemoveWhiteSpaces(input).Split(',').Distinct().ToList();
            var result = new List<T>();
            foreach (var item in cutInput)
            {
                var isEnum = Enum.TryParse(typeof(T), item, true, out var enumValue);
                if (!isEnum)
                    Console.WriteLine($"Couldn't parse: {item}");
                else
                    result.Add((T)enumValue);
            }

            return result;
        }

        private static string RemoveWhiteSpaces(string str) =>
            string.Concat(str.Where(c => !char.IsWhiteSpace(c)));
    }
}
