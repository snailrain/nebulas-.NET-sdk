using System;
using System.Collections.Generic;
using System.Text;

namespace Nebulas.Schema.Request
{
    public class RequestGetBlockByHeight
    {
        public string Height { get; set; }
        public bool FullFillTransaction { get; set; }
    }
}
