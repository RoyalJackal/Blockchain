using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Blockchain.Task1
{
    public class Blockchain
    {
        public List<Block> Chain { get; set; } = new List<Block>();

        public void Add(Block block) =>
            Chain.Add(block);

        public RSAParameters PrivateKey { get; set; }

        public RSAParameters PublicKey { get; set; }

        public Blockchain()
        {
            var rsa = RSA.Create();
            PrivateKey = rsa.ExportParameters(true);
            PublicKey = rsa.ExportParameters(false);
        }

        public void Print()
        {
            foreach (var block in Chain)
            {
                Console.WriteLine($"Index: {block.Index};\n" +
                                  $"Data: {block.Data};\n" +
                                  $"Date: {block.Date};\n" +
                                  $"PrevHash: {block.PreviousHash};\n" +
                                  $"DataSign: {block.DataSign};\n" +
                                  $"Hash: {block.Hash};\n" +
                                  $"HashSign: {block.HashSign}\n");
            }
        }

        public bool Verify()
        {
            for (var i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];

                if (!currentBlock.VerifyData(PublicKey))
                    return false;

                if (!currentBlock.VerifyHash(PublicKey))
                    return false;

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
