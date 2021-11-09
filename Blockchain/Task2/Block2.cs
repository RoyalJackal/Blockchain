using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.OpenSsl;

namespace Blockchain.Task2
{
    public class Block2
    {
        public int Index { get; set; }

        public string PreviousHash { get; set; }

        public DateTimeOffset Date { get; set; }

        public string Data { get; set; }

        public string Hash { get; set; }

        public string HashSign { get; set; }

        private readonly ArbiterClient _client;

        public Block2(int index, string data, string previousHash, ArbiterClient client)
        {
            Index = index;
            Data = data;
            PreviousHash = previousHash;
            _client = client;
        }

        public string CreateHash()
        {
            var sha = SHA256.Create();
            var toEncrypt = Encoding.UTF8.GetBytes($"{PreviousHash}-{Data}");
            var temp = sha.ComputeHash(toEncrypt);
            Hash = Convert.ToHexString(temp).ToLower();
            return Hash;
        }

        public string SignHash()
        {
            var response = _client.GetTimeStamp(Hash);
            if (response.Status != 0)
                throw new Exception(response.StatusString);

            Date = DateTimeOffset.Parse(response.TimeStampToken.Ts);
            HashSign = response.TimeStampToken.Signature;
            return HashSign;
        }

        public bool VerifyWithArbiter() =>
            _client.VerifySignature(Date, Hash, HashSign);

        public bool VerifyWithKey()
        {
            var dateString = ArbiterClient.DateToString(Date);
            var byteDate = Encoding.UTF8.GetBytes(dateString);

            var publicKey = _client.GetPublicKey();
            var rsa = RSA.Create(512);
            rsa.ImportFromPem(publicKey);
            var byteHash = Convert.FromHexString(Hash);
            var byteSign = Convert.FromHexString(HashSign);
            var dateHash = byteDate.Concat(byteHash).ToArray();

            var sha = SHA256.Create();
            var shaHash = sha.ComputeHash(dateHash);

            return rsa.VerifyHash(shaHash, byteSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);

            /*var pr = new PemReader(new StringReader(publicKey));
            var pk = (AsymmetricKeyParameter)pr.ReadObject();

            var pssSigner = new PssSigner(new RsaEngine(), new Sha256Digest());
            pssSigner.Init(false, pk);
            pssSigner.BlockUpdate(dateHash, 0, dateHash.Length);
            var valid = pssSigner.VerifySignature(byteSign);
            return valid;*/


        }
    }
}
