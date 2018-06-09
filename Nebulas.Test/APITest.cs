using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nebulas.Schema;
using Newtonsoft.Json;

namespace Nebulas.Test
{
    [TestClass]
    public class APITest
    {
        string _host = "https://mainnet.nebulas.io";
        string from = "n1TbtKS8uTkN36DApfj7jPGYD53KjAN3HC3";
        [TestMethod]
        public void Call()
        {

            //string from, string to, string value, string nonce, int gasPrice, int gasLimit, string function,string args
            string to = "n1o5DKJefXgFNLhUTXRiJFFrTp2npgSuhvW";
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
            RequestEstimateGas request = new RequestEstimateGas() {
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
    }
}
