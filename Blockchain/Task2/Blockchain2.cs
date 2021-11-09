using System;
using System.Collections.Generic;

namespace Blockchain.Task2
{
    public class Blockchain2
    {
        public List<Block2> Chain { get; set; } = new List<Block2>();

        public void Add(Block2 block) =>
            Chain.Add(block);

        public void Print()
        {
            foreach (var block in Chain)
            {
                Console.WriteLine($"Index: {block.Index};\n" +
                                  $"Data: {block.Data};\n" +
                                  $"Date: {block.Date};\n" +
                                  $"PrevHash: {block.PreviousHash};\n" +
                                  $"Hash: {block.Hash};\n" +
                                  $"HashSign: {block.HashSign}\n");
            }
        }

        public List<(bool, bool, bool)> Verify()
        {
            var result = new List<(bool, bool, bool)>();
            for (var i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];

                var f1 = currentBlock.VerifyWithArbiter();
                var f2 = currentBlock.VerifyWithKey();
                var f3 = currentBlock.PreviousHash == previousBlock.Hash;

                result.Add((f1, f2, f3));
            }

            foreach (var (item1, item2, item3) in result)
            {
                Console.WriteLine($"({item1}, {item2}, {item3})");
            }

            return result;
        }
    }
}
