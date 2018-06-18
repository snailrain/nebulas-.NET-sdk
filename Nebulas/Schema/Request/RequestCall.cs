using System;
using System.Collections.Generic;
using System.Text;

namespace Nebulas.Schema
{
    public class RequestCall
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Value { get; set; }
        public string Nonce { get; set; }
        public string GasPrice { get; set; }
        public string GasLimit { get; set; }
        public RequestContract Contract { get; set; }

    }

    public class RequestContract
    {
        public string Function { get; set; }
        public string Args { get; set; }
    }
}
