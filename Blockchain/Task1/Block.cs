using System;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain.Task1
{
    public class Block
    {
        public int Index { get; set; }

        public string PreviousHash { get; set; }

        public DateTimeOffset Date { get; set; }

        public string Data { get; set; }

        public string DataSign { get; set; }

        public string Hash { get; set; }

        public string HashSign { get; set; }

        public Block(int index, string data, string previousHash, DateTimeOffset date)
        {
            Index = index;
            Data = data;
            PreviousHash = previousHash;
            Date = date;
        }

        public string SignData(RSAParameters rsaKeyInfo)
        {
            var rsa = RSA.Create(rsaKeyInfo);
            var temp = rsa.SignData(Encoding.ASCII.GetBytes(Data), HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
            DataSign = Convert.ToBase64String(temp);
            return DataSign;
        }

        public string CreateHash()
        {
            var sha = SHA256.Create();
            var toEncrypt = Encoding.ASCII.GetBytes($"{PreviousHash}-{Date}-{Data}-{DataSign}");
            var temp = sha.ComputeHash(toEncrypt);
            Hash = Convert.ToBase64String(temp);
            return Hash;
        }

        public string SignHash(RSAParameters rsaKeyInfo)
        {
            var rsa = RSA.Create(rsaKeyInfo);
            var byteHash = Convert.FromBase64String(Hash);
            var temp = rsa.SignHash(byteHash, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
            HashSign = Convert.ToBase64String(temp);
            return HashSign;
        }

        public bool VerifyData(RSAParameters rsaKeyInfo)
        {
            var rsa = RSA.Create(rsaKeyInfo);
            var byteData = Encoding.ASCII.GetBytes(Data);
            var byteSign = Convert.FromBase64String(DataSign);
            return rsa.VerifyData(byteData, byteSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
        }

        public bool VerifyHash(RSAParameters rsaKeyInfo)
        {
            var rsa = RSA.Create(rsaKeyInfo);
            var byteHash = Convert.FromBase64String(Hash);
            var byteSign = Convert.FromBase64String(HashSign);
            return rsa.VerifyHash(byteHash, byteSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
        }
    }
}
