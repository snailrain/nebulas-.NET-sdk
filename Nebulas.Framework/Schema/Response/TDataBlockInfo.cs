using System;
using System.Collections.Generic;
using System.Text;

namespace Nebulas.Schema.Response
{
    public class TDataBlockInfo
    {
        public string Hash { get; set; }
        public string ParentHash { get; set; }
        public int Height { get; set; }
        public string Nonce { get; set; }
        public string Coinbase { get; set; }
        public long Timestamp { get; set; }
        public int ChainId { get; set; }
        public string StateRoot { get; set; }
        public string TxsRoot{ get; set; }
        public string EventsRoot { get; set; }
        public TConsensusRoot ConsensusRoot { get; set; }
        public string Miner { get; set; }
        public bool IsFinality { get; set; }
        public TTransactionReceipt[] Transactions { get; set; }
    }

    public class TConsensusRoot
    {
        public long Timestamp { get; set; }
        public string Proposer { get; set; }
        public string DynastyRoot { get; set; }
    }
}
