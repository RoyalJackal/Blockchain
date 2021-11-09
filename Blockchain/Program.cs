using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using Blockchain.Task1;
using Blockchain.Task2;

namespace Blockchain
{
    class Program
    {
        private static void Main(string[] args)
        {
            Task2();
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
    }
}
