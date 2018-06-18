using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nebulas.Hex.HexConvertors.Extensions;

namespace Nebulas.Test
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void GetPrivateKeyTest()
        {
            var a = new Account("ab14bca2fd7703b76972a696a6df4ebeb45f20d01086d695b46b6120adbae4d9");
            byte[] result = a.GetPrivateKey();
            Assert.IsTrue(result.Length == 32);
        }

        [TestMethod]
        public void GetPublicKeyStringTest()
        {
            var a = new Account("ab14bca2fd7703b76972a696a6df4ebeb45f20d01086d695b46b6120adbae4d9");
            string result = a.GetPublicKeyString();
            Assert.AreEqual(result, "2c898cbae86245c2e4dd10f83d060b8d97eca3c4467bd4f0f4260c16c69912476d18e479adeafbedfdb9900554b9a2fb3806e95bd1d881b12a096bd15ce4e109");
        }


        [TestMethod]
        public void GetPublicKeyTest()
        {
            var a = new Account("ab14bca2fd7703b76972a696a6df4ebeb45f20d01086d695b46b6120adbae4d9");
            byte[] result = a.GetPublicKey();
            byte[] assert = new byte[64] { 44, 137, 140, 186, 232, 98, 69, 194, 228, 221, 16, 248, 61, 6, 11, 141, 151, 236, 163, 196, 70, 123, 212, 240, 244, 38, 12, 22, 198, 153, 18, 71, 109, 24, 228, 121, 173, 234, 251, 237, 253, 185, 144, 5, 84, 185, 162, 251, 56, 6, 233, 91, 209, 216, 129, 177, 42, 9, 107, 209, 92, 228, 225, 9 };

            Assert.AreEqual(result.ToHex(), assert.ToHex());
        }

        [TestMethod]
        public void GetAddressStringTest()
        {
            var a = new Account("ab14bca2fd7703b76972a696a6df4ebeb45f20d01086d695b46b6120adbae4d9");
            string result = a.GetAddressString();

            Assert.AreEqual(result, "n1TA6on2ikjjUcpwbtjjcsAgHTP7fEZ41Bk");
        }
        [TestMethod]
        public void GetAddressTest()
        {
            var a = new Account("ab14bca2fd7703b76972a696a6df4ebeb45f20d01086d695b46b6120adbae4d9");
            byte[] result = a.GetAddress();
            Assert.AreEqual(Cryptography.ECDSA.Base58.Encode(result), "n1TA6on2ikjjUcpwbtjjcsAgHTP7fEZ41Bk");
        }


        [TestMethod]
        public void NewAccountTest()
        {
            var account = Account.NewAccount();
            string _host = "https://testnet.nebulas.io";
            string from = account.GetAddressString();


            ////string from, string to, string value, string nonce, int gasPrice, int gasLimit, string function,string args
            string to = "n22TK8fuUGbMGFKZTWus7a2R3a24uhr1NaQ";
            string value = "0";
            string nonce = "0";
            int gasPrice = 1000000;
            int gasLimit = 2000000;
            string function = "getIntegralByPage";
            string args = "[]";
            Nebulas.Neb neb = new Neb(new HttpRequest(_host));
            dynamic result = neb.API.CallAsync(from, to, value, nonce, gasPrice, gasLimit, function, args).Result;

            Assert.IsNotNull(result.result.result.Value);

            Assert.IsNotNull(account.GetAddressString());
            Assert.IsNotNull(account.GetPrivateKey());
            Assert.IsNotNull(account.GetPrivateKeyString());
            Assert.IsNotNull(account.GetPublicKeyString());
        }

        [TestMethod]
        public void IsValidAddressTest()
        {
            Assert.IsTrue(Account.IsValidAddress("n1TA6on2ikjjUcpwbtjjcsAgHTP7fEZ41Bk"));
            Assert.IsTrue(Account.IsValidAddress("n22TK8fuUGbMGFKZTWus7a2R3a24uhr1NaQ"));

            Assert.IsTrue(Account.IsValidAddress("n1TA6on2ikjjUcpwbtjjcsAgHTP7fEZ41Bk", Account.AddressType.NormalType));
            Assert.IsTrue(Account.IsValidAddress("n22TK8fuUGbMGFKZTWus7a2R3a24uhr1NaQ", Account.AddressType.ContractType));
        }

        [TestMethod]
        public void FromKeyTest()
        {
            //{"version":4,"id":"0e07b66d-7bfd-490e-bd05-b59659827968","address":"n1Gb7sxoj9s6hGbcwKs5mHWqVaBt1xsdgYN","crypto":{"ciphertext":"4b73b7a18c5c510d558d77c6d2d106cb012c6ce4bd9cf5897a205622a5f47769","cipherparams":{"iv":"225bdebc974f5ec6b905c0db3cbbb292"},"cipher":"aes-128-ctr","kdf":"scrypt","kdfparams":{"dklen":32,"salt":"60f656342434d53b891b8236af857f0590e88383103f9b90e0d7a292a7c6b987","n":4096,"r":8,"p":1},"mac":"5d873fb1b76cb2b8106526ba57a58757e1214f2becc3e61a87a42b5f6d0b2d3f","machash":"sha3256"}}
            var a = new Account("16f8c87bc189f437993f8674ea8440daa288c4251111f6c6329c77a97aea5a30");
            a.FromKey(new Key() {
                address = "n1Gb7sxoj9s6hGbcwKs5mHWqVaBt1xsdgYN",
                crypto = new KeyCrypto {
                    
                }
            },"123456789");
        }

        [TestMethod]
        public void ToKeyTest()
        {
            //{"version":4,"id":"0e07b66d-7bfd-490e-bd05-b59659827968","address":"n1Gb7sxoj9s6hGbcwKs5mHWqVaBt1xsdgYN","crypto":{"ciphertext":"4b73b7a18c5c510d558d77c6d2d106cb012c6ce4bd9cf5897a205622a5f47769","cipherparams":{"iv":"225bdebc974f5ec6b905c0db3cbbb292"},"cipher":"aes-128-ctr","kdf":"scrypt","kdfparams":{"dklen":32,"salt":"60f656342434d53b891b8236af857f0590e88383103f9b90e0d7a292a7c6b987","n":4096,"r":8,"p":1},"mac":"5d873fb1b76cb2b8106526ba57a58757e1214f2becc3e61a87a42b5f6d0b2d3f","machash":"sha3256"}}
            var a = new Account(new byte[] { 212, 241, 141, 120, 38, 21, 69, 90, 87, 65, 146, 68, 4, 78, 151, 212, 57, 86, 141, 132, 219, 243, 122, 239, 45, 133, 47, 253, 247, 59, 89, 80 });
            var key = a.ToKey("123456789", new KeyOptions() {
               salt = new byte[] { 108, 240, 186, 81, 62, 252, 26, 167, 159, 190, 71, 140, 74, 201, 131, 157, 127, 54, 150, 85, 135, 137, 233, 76, 201, 203, 11, 200, 76, 110, 146, 91 },
               iv = new byte[] { 174, 176, 29, 188, 185, 223, 149, 83, 130, 30, 94, 120, 100, 252, 2, 217 }
            });
            Assert.AreEqual(key.crypto.mac ,"bb8f620e94ca663bc6555b89a9fcaa981405f46acf4d11c8f5c71529727c9848");
        }

    }
}
