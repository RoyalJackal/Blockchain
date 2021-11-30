using System;
using CsvHelper.Configuration.Attributes;

namespace Blockchain.Currencies.Entities
{
    public class CurrencyData
    {
        [Name("<DATE>")]
        public int Date { get; set; }

        [Name("<OPEN>")]
        public double Open { get; set; }

        [Name("<CLOSE>")]
        public double Close { get; set; }
    }
}
