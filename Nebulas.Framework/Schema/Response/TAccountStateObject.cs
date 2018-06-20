using System;
using System.Collections.Generic;
using System.Text;

namespace Nebulas.Schema.Response
{
    public class TAccountStateObject
    {
        public string Balance { get; set; }
        public int Nonce { get; set; }
        public int Type { get; set; }
    }
}
