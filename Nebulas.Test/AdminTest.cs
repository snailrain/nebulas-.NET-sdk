using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebulas.Test
{
    [TestClass]
    public class AdminTest
    {
        string _host = "https://testnet.nebulas.io";
        [TestMethod]
        public void SendTrancastionTest()
        {
            HttpRequest request = new HttpRequest(_host);
            Admin admin = new Admin(request);
       
        }


    }
}
