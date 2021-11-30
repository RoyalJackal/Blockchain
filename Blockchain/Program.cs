using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using Blockchain.Currencies;
using Blockchain.Task1;
using Blockchain.Task2;
using static Blockchain.Requests.Requests;

namespace Blockchain
{
    class Program
    {
        private static void Main(string[] args)
        {
            Task4();
        }

        private static void Task1()
        {
            var blockchain = new Task1.Blockchain();
            var previousHash = "";
            for (var i = 0; i < 5; i++)
            {
                var date = DateTimeOffset.Now;
                var data = $"Block {i}, date of creation: {date.ToString(CultureInfo.InvariantCulture)}";
                var block = new Block(i, data, previousHash, date);
                var dataSign = block.SignData(blockchain.PrivateKey);
                var hash = block.CreateHash();
                var hashSign = block.SignHash(blockchain.PrivateKey);
                blockchain.Add(block);
                previousHash = hash;
            }

            blockchain.Print();
            Console.WriteLine(blockchain.Verify());
        }

        private static void Task2()
        {
            var client = new ArbiterClient();
            var blockchain = new Blockchain2();
            var previousHash = "";
            for (var i = 0; i < 5; i++)
            {
                var date = DateTimeOffset.Now;
                var data = $"Block {i}, date of creation: {date.ToString(CultureInfo.InvariantCulture)}";
                var block = new Block2(i, data, previousHash, client);
                var hash = block.CreateHash();
                var hashSign = block.SignHash();
                blockchain.Add(block);
                previousHash = hash;
            }

            blockchain.Print();
            blockchain.Verify();
        }

        private static void Task3()
        {
            Console.WriteLine($"Интенсивность потока заявок = {Lambda} заявок/час");
            Console.WriteLine($"Процент необработанных заявок: {POtk * 100}%");
            Console.WriteLine($"Среднее время в очереди: {W0} часов");
            Console.WriteLine($"Средняя сумма от штафов в месяц: {PenaltyCount} * x рублей");
            var x = CalcX(20);
            Console.WriteLine($"X для двадцати ядер: {x} рублей");

            var minPrice = Price(x * 3, 1);
            var minCores = 1;
            for (int i = 2; i <= 100; i++)
            {
                var curPrice = Price(x * 3, i);
                if (curPrice < minPrice)
                {
                    minPrice = curPrice;
                    minCores = i;
                }
            }

            Console.WriteLine($"Оптимальное количество ядер для штрафа размером x*3: {minCores}");
            Console.ReadKey();
        }

        private static void Task4()
        {
            //HUF EUR USD NOK
            var usdhuf = DataConverter.FetchData(DataConverter.HUFPath);
            var eurrub = DataConverter.FetchData(DataConverter.EURPath);
            var usdrub = DataConverter.FetchData(DataConverter.USDPath);
            var usdnok = DataConverter.FetchData(DataConverter.NOKPath);

            var hufrub = DataConverter.Convert(usdhuf, usdrub);
            var nokrub = DataConverter.Convert(usdnok, usdrub);

            var portions = Currencies.Currencies.MakePortions(hufrub, eurrub, usdrub, nokrub, Constants.PortionSize);

            foreach (var portion in portions)
                portion.Print();

            Console.WriteLine();
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine();

            var matrix = Currencies.Currencies.MakeMatrix(portions);

            Currencies.Currencies.PrintMatrix(matrix);

            var s = Currencies.Currencies.GetMostValuableS(matrix, out var value);

            Console.WriteLine($"Самая прибильная выборка - {s}, со средней прибылью = {value}.");
        }
    }
}
