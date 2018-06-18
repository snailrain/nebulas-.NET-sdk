using System;
using System.Collections.Generic;
using System.Text;

namespace Nebulas.Schema.Request
{
    public class RequestSignHash
    {
        public string Address { get; set; }
        public string Hash { get; set; }
        public int Alg { get; set; }
    }
}
