using System;
using System.Collections.Generic;
using System.Text;

namespace Nebulas.Schema
{
    public class RequestEstimateGas
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Value { get; set; }
        public string Nonce { get; set; }
        public string GasPrice { get; set; }
        public string GasLimit { get; set; }
        public string Contract { get; set; }
        public byte[] Binary { get; set; } = { };
    }
}
