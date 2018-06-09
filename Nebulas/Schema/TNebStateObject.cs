using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebulas.Schema
{
    public class TNebStateObject 
    {
        /// <summary>
        /// chain_id Block chain id
        /// </summary>
        public string ChainId { get; set; }
        /// <summary>
        /// tail Current neb tail hash
        /// </summary>
        /// 
        public string Tail { get; set; }

        /// <summary>
        /// lib Current neb lib hash
        /// </summary>
        public string Lib { get; set; }

        public string Height { get; set; }

        public string ProtocolVersion { get; set; }
        public bool Synchronized { get; set; }
        public string Version { get; set; }
    }
}
