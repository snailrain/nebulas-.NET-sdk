using System;
using System.Collections.Generic;
using System.Text;

namespace Nebulas.Schema.Request
{
    public class RequestSignTransactionWithPassphrase 
    {
        public RequestTx Tx { get; set; }
        public string Passphrase { get; set; }
    }
}
