using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Blockchain.Currencies.Entities;
using CsvHelper;

namespace Blockchain.Currencies
{
    public static class DataConverter
    {
        //HUF EUR USD NOK
        public static string EURPath = @".\currencies\EURRUB_210801_211123.csv";
        public static string USDPath = @".\currencies\USDRUB_210801_211123.csv";
        public static string HUFPath = @".\currencies\USDHUF_210801_211123.csv";
        public static string NOKPath = @".\currencies\USDNOK_210801_211123.csv";

        public static List<CurrencyData> FetchData(string filePath)
        {
            List<CurrencyData> result;
            using (TextReader reader = new StreamReader(filePath))
            {
                using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csvReader.GetRecords<CurrencyData>();
                    result = records.ToList();
                }
            }

            return result;
        }

        public static List<CurrencyData> Convert(List<CurrencyData> from, List<CurrencyData> to)
        {
            var result = new List<CurrencyData>();
            if (from.Count != to.Count)
                throw new Exception("Couldn't convert data with different sizes");

            for (int i = 0; i < from.Count; i++)
            {
                result.Add(new CurrencyData
                    {
                        Date = from[i].Date,
                        Open = to[i].Open / from[i].Open,
                        Close = to[i].Close / from[i].Close
                    });
            }

            return result;
        }
    }
}
