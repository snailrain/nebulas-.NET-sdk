using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Nebulas.Test
{
    [TestClass]
    public class APITest
    {
        [TestMethod]
        public void Call()
        {
            string host = "https://mainnet.nebulas.io";
            //string from, string to, string value, string nonce, int gasPrice, int gasLimit, string function,string args
            string from = "n1TA6on2ikjjUcpwbtjjcsAgHTP7fEZ41Bk";
            string to = "n1o5DKJefXgFNLhUTXRiJFFrTp2npgSuhvW";
            string value = "0";
            string nonce = "0";
            int gasPrice = 1000000;
            int gasLimit = 2000000;
            string function = "getIntegralByPage";
            string args = "[]";
            Nebulas.Neb neb = new Neb(new HttpRequest(host));
            string result = neb.API.CallAsync(from, to, value, nonce, gasPrice, gasLimit, function, args).Result;

            int errorCount = 0;
            try
            {
                var resultJson = JsonConvert.DeserializeObject<dynamic>(result);
            }
            catch (Exception ex)
            {
                errorCount++;
            }
            Assert.IsTrue(errorCount == 0);
        }
    }
}
