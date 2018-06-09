using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Nebulas.Schema
{
    public class RequestGetAccountState
    {
        public string Address { get; set; }

        public string Height { get; set; } = "0";
    }
}
