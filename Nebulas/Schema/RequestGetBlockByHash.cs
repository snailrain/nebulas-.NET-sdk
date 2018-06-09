using System;
using System.Collections.Generic;
using System.Text;

namespace Nebulas.Schema
{
    public class RequestGetBlockByHash
    {
        public string Hash { get; set; }
        public bool FullFillTransaction { get; set; }
    }
}
