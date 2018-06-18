using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebulas.Test
{
    [TestClass]
    public class TransactionTest
    {
        string _host = "https://testnet.nebulas.io";
        string to = "n22TK8fuUGbMGFKZTWus7a2R3a24uhr1NaQ";
        string value = "0";

        int gasPrice = 1000000;
        int gasLimit = 200000;


        [TestMethod]
        public void SignTransaction()
        {
            HttpRequest request = new HttpRequest(_host);
            API api = new API(request);
            var accountState = api.GetAccountStateAsync("n1TA6on2ikjjUcpwbtjjcsAgHTP7fEZ41Bk").Result;
            int nonce = ++accountState.Result.Nonce;
            //string from = "n1TA6on2ikjjUcpwbtjjcsAgHTP7fEZ41Bk";
            string function = "addIntegral";
            string args = "[\"哈哈哈测试一下\",\"40000\"]";
            var fromAccount = new Account("ab14bca2fd7703b76972a696a6df4ebeb45f20d01086d695b46b6120adbae4d9");
            var tx = new Transaction(1001, fromAccount, to, ulong.Parse(value), ulong.Parse(nonce.ToString()), (ulong)gasPrice, (ulong)gasLimit, function, args);
            tx.SignTransaction();

            var result = api.SendRawTransactionAsync(tx.toProtoString()).Result;

            Assert.IsNotNull(result.Result.txhash.Value);
        }
    }
}
