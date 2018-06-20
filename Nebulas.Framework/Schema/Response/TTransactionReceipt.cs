using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebulas.Schema.Response
{
    public class TTransactionReceipt
    {
        public string Hash { get; set; }
        //有的地方叫chain_id，有的地方叫chainId，你这样我很为难啊
        [JsonProperty(PropertyName = "chainId")]
        public int ChainId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Value { get; set; }
        public string Nonce { get; set; }
        public long Timestamp { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public string GasPrice { get; set; }
        public string GasLimit { get; set; }
        public string ContractAddress { get; set; }
        public int Status { get; set; }
        public string ExecuteError { get; set; }
        public string ExecuteResult { get; set; }
    }
}
