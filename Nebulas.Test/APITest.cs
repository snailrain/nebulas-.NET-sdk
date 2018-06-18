using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nebulas.Schema;
using Nebulas.Schema.Request;
using Newtonsoft.Json;

namespace Nebulas.Test
{
    [TestClass]
    public class APITest
    {
        string _host = "https://testnet.nebulas.io";
        string from = "n1TA6on2ikjjUcpwbtjjcsAgHTP7fEZ41Bk";
        [TestMethod]
        public void Call()
        {

            //string from, string to, string value, string nonce, int gasPrice, int gasLimit, string function,string args
            string to = "n22TK8fuUGbMGFKZTWus7a2R3a24uhr1NaQ";
            string value = "0";
            string nonce = "0";
            int gasPrice = 1000000;
            int gasLimit = 2000000;
            string function = "getIntegralByPage";
            string args = "[]";
            Nebulas.Neb neb = new Neb(new HttpRequest(_host));
            dynamic result = neb.API.CallAsync(from, to, value, nonce, gasPrice, gasLimit, function, args).Result;

        }


        [TestMethod]
        public void GetNebState()
        {
            Nebulas.Neb neb = new Neb(new HttpRequest(_host));
            var result = neb.API.GetNebStateAsync().Result;
            Assert.IsNotNull(result.Result.ChainId);
        }

        //LatestIrreversibleBlock
        [TestMethod]
        public void LatestIrreversibleBlock()
        {
            Nebulas.Neb neb = new Neb(new HttpRequest(_host));
            var result = neb.API.LatestIrreversibleBlockAsync().Result;
            Assert.IsNotNull(result.Result.ChainId);
        }

        [TestMethod]
        public void GetAccountState()
        {
            Nebulas.Neb neb = new Neb(new HttpRequest(_host));
            var result = neb.API.GetAccountStateAsync(from).Result;
            Assert.IsNotNull(result.Result.Balance);
        }

        [TestMethod]
        public void Subscribe()
        {
            Nebulas.Neb neb = new Neb(new HttpRequest(_host));
            string temp = "";
            neb.API.Subscribe(new string[] { "chain.linkBlock", "chain.pendingTransaction", "chain.transactionResult" }, (s) =>
            {
                temp = s;
            });
            while (string.IsNullOrEmpty(temp))
            {
                Thread.Sleep(100);
            }
        }


        [TestMethod]
        public void EstimateGasAsync()
        {
            RequestEstimateGas request = new RequestEstimateGas()
            {
                From = from,
                To = "n1o5DKJefXgFNLhUTXRiJFFrTp2npgSuhvW",
                Value = "0",
                Nonce = "0",
                GasPrice = "1000000",
                GasLimit = "2000000"
            };
            Nebulas.Neb neb = new Neb(new HttpRequest(_host));
            var result = neb.API.EstimateGasAsync(request).Result;
            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void GetTransactionReceiptAsync()
        {
            Nebulas.Neb neb = new Neb(new HttpRequest(_host));
            var result = neb.API.GetTransactionReceiptAsync("ef2308cb962188481f879c159117fbd08bb7e3e1cf56f0163ab21c7ec31930cc").Result;
            Assert.IsNotNull(result.Result);
        }
    }
}
