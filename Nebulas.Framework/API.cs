using Nebulas.Schema;
using Nebulas.Schema.Request;
using Nebulas.Schema.Response;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Nebulas
{
    public class API: BaseNeb
    {
        public API(HttpRequest request) : base(request)
        {
            _path = "user";
        }
        
        private void beginSendRequestAsync(string mNasod, string api, string paramsOptions,Action<string> onDownloadProgressEvent)
        {
            string action = _path + api;
            _request.OnDownloadProgressEvent = onDownloadProgressEvent;
            _request.BeginRequestAsync(mNasod, action, paramsOptions);
        }


        /**
         * MNasod get state of Nebulas Network.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc.md#getnebstate}
         *
         * @return [NebStateObject]{@link https://github.com/nebulasio/wiki/blob/master/rpc.md#getnebstate}
         *
         * @example
         * var api = new Neb().api;
         * api.getNebState().then(function(state) {
         * //code
         * });
         */
        public async Task<TRq<TNebStateObject>> GetNebStateAsync()
        {
            return await sendRequestAsync<TNebStateObject>("get", "/nebstate", null, UnderLineSetting);
        }

        /**
         * MNasod get latest irreversible block of Nebulas Network.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc.md#latestirreversibleblock}
         *
         * @return [dataBlockInfo.]{@link https://github.com/nebulasio/wiki/blob/master/rpc.md#latestirreversibleblock}
         *
         * @example
         * var api = new Neb().api;
         * api.latestIrreversibleBlock().then(function(blockData) {
         * //code
         * });
         */
        public async Task<TRq<TDataBlockInfo>> LatestIrreversibleBlockAsync()
        {
            return await sendRequestAsync<TDataBlockInfo>("get", "/lib", null, UnderLineSetting);
        }


        /**
         * MNasod return the state of the account. Balance and nonce.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc.md#getaccountstate}
         *
         * @param {Object} options
         * @param {HexString} options.address
         * @param {String} options.height
         *
         * @return [accaountStateObject]{@link https://github.com/nebulasio/wiki/blob/master/rpc.md#getaccountstate}
         *
         * @example
         * var api = new Neb().api;
         * api.getAccountState({address: "n1QsosVXKxiV3B4iDWNmxfN4VqpHn2TeUcn"}).then(function(state) {
         * //code
         * });
         */
        public async Task<TRq<TAccountStateObject>> GetAccountStateAsync(string address,string height = "0")
        {
            RequestGetAccountState request = new RequestGetAccountState()
            {
                Address = address,
                Height = height
            };

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(request, CamelCaseSetting);
            return await sendRequestAsync<TAccountStateObject>("post", "/accountstate", jsonString, UnderLineSetting);
        }


        /**
         * MNasod wrap smart contract call functionality.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc.md#call}
         *
         * @param {TransactionOptions} options
         *
         * @return [Transcation hash]{@link https://github.com/nebulasio/wiki/blob/master/rpc.md#call}
         *
         * @example
         * var api = new Neb().api;
         * api.call({
         *    chainID: 1,
         *    from: "n1QZMXSZtW7BUerroSms4axNfyBGyFGkrh5",
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000,
         *    contract: {
         *        function: "save",
         *        args: "[0]"
         *    }
         * }).then(function(tx) {
         *     //code
         * });
         */
        public async Task<dynamic> CallAsync(string from, string to, string value, string nonce, int gasPrice, int gasLimit, string function, string args)
        {
            RequestCall callRequest = new RequestCall()
            {
                From = from,
                To = to,
                Value = value,
                Nonce = nonce,
                GasPrice = Convert.ToString(gasPrice),
                GasLimit = Convert.ToString(gasLimit),
                Contract = new RequestContract()
                {
                    Function = function,
                    Args = args
                }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(callRequest, Formatting.Indented, CamelCaseSetting);

            return await sendRequestAsync<dynamic>("post", "/call", json);

        }

        /**
         * MNasod wrap submit the signed transaction.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc.md#sendrawtransaction}
         *
         * @param {Object} options
         * @param {String} options.data
         *
         * @return [Transcation hash]{@link https://github.com/nebulasio/wiki/blob/master/rpc.md#sendrawtransaction}
         *
         * @example
         * var api = new Neb().api;
         * var tx = new Transaction({
         *    chainID: 1,
         *    from: acc1,
         *    to: acc2,
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * });
         * tx.signTransaction();
         * api.sendRawTransaction( {data: tx.toProtoString()} ).then(function(hash) {
         * //code
         * });
         */
        public async Task<dynamic> SendRawTransactionAsync(string data)
        {
            RequestSendRawTransaction request = new RequestSendRawTransaction() {
                Data = data
            };
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(request, Formatting.Indented, CamelCaseSetting);

            return await sendRequestAsync<dynamic>("post", "/rawtransaction", jsonString);

        }


        /**
         * Get block header info by the block hash.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc.md#getblockbyhash}
         *
         * @param {Object} options
         * @param {HexString} options.hash
         * @param {Boolean} options.fullTransaction
         *
         * @return [Block]{@link https://github.com/nebulasio/wiki/blob/master/rpc.md#getblockbyhash}
         *
         * @example
         * var api = new Neb().api;
         * api.getBlockByHash({
         *     hash: "00000658397a90df6459b8e7e63ad3f4ce8f0a40b8803ff2f29c611b2e0190b8",
         *     fullTransaction: true
         * }).then(function(block) {
         * //code
         * });
         */
        public async Task<dynamic> GetBlockByHashAsync(string hash,bool fullTransaction)
        {
            RequestGetBlockByHash request = new RequestGetBlockByHash()
            {
                Hash = hash,
                FullFillTransaction = fullTransaction
            };
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(request, Formatting.Indented, UnderLineSetting);

            return await sendRequestAsync<dynamic>("post", "/getBlockByHash", jsonString);

        }


        /**
         * Get block header info by the block height.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc.md#getblockbyheight}
         *
         * @param {Object} options
         * @param {Number} options.height
         * @param {Boolean} options.fullTransaction
         *
         * @return [Block]{@link https://github.com/nebulasio/wiki/blob/master/rpc.md#getblockbyheight}
         *
         * @example
         * var api = new Neb().api;
         * api.getBlockByHeight({height:2, fullTransaction:true}).then(function(block) {
         * //code
         * });
         */
        public async Task<dynamic> GetBlockByHeightAsync(string height, bool fullTransaction)
        {
            RequestGetBlockByHeight request = new RequestGetBlockByHeight()
            {
                Height = height,
                FullFillTransaction = fullTransaction
            };
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(request, Formatting.Indented, UnderLineSetting);

            return await sendRequestAsync<dynamic>("post", "/getBlockByHeight", jsonString);
        }

        /**
         * Get transactionReceipt info by tansaction hash.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc.md#gettransactionreceipt}
         *
         * @param {Object} options
         * @param {HexString} options.hash
         *
         * @return [TransactionReceipt]{@link https://github.com/nebulasio/wiki/blob/master/rpc.md#gettransactionreceipt}
         *
         * @example
         * var api = new Neb().api;
         * api.getTransactionReceipt({hash: "cc7133643a9ae90ec9fa222871b85349ccb6f04452b835851280285ed72b008c"}).then(function(receipt) {
         * //code
         * });
         */
        public async Task<dynamic> GetTransactionReceiptAsync(string hash)
        {
            RequestGetTransactionReceipt request = new RequestGetTransactionReceipt()
            {
                Hash = hash 
            };
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(request, Formatting.Indented, UnderLineSetting);

            return await sendRequestAsync<dynamic>("post", "/getTransactionReceipt", jsonString);
        }

        
        /**
        * Return the subscribed events of transaction & block.
        * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc.md#subscribe}
        *
        * @param {Object} options
        * @param {Array|String} options.topics
        * @param {Function} options.onDownloadProgress - On progress callback function. Recive chunk.
        *
        * @return [eventData]{@link https://github.com/nebulasio/wiki/blob/master/rpc.md#subscribe}
        *
        * @example
        * var api = new Neb().api;
        * api.subscribe({topics: ["chain.linkBlock", "chain.pendingTransaction"]}).then(function(eventData) {
        * //code
        * });
        */
        //长链接，需保持线程运行
        public void Subscribe(string[] topics,Action<string> callBackEvent)
        {
            RequestSubscribe request = new RequestSubscribe() {
                Topics = topics
            };
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(request, Formatting.Indented, UnderLineSetting);

            beginSendRequestAsync("post", "/subscribe", jsonString, callBackEvent);
        }

        /**
         * Return current gasPrice.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc.md#getgasprice}
         *
         * @return [Gas Price]{@link https://github.com/nebulasio/wiki/blob/master/rpc.md#getgasprice}
         *
         * @example
         * var api = new Neb().api;
         * api.gasPrice().then(function(gasPrice) {
         * //code
         * });
         */
        public async Task<dynamic> GasPriceAsync()
        {
            return await sendRequestAsync<dynamic>("get", "/getGasPrice", null);
        }

        /**
         * Return the estimate gas of transaction.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc.md#estimategas}
         *
         * @param {TransactionOptions} options
         *
         * @return [Gas]{@link https://github.com/nebulasio/wiki/blob/master/rpc.md#estimategas}
         *
         * @example
         * var api = new Neb().api;
         * api.estimateGas({
         *    chainID: 1,
         *    from: "n1QZMXSZtW7BUerroSms4axNfyBGyFGkrh5",
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * }).then(function(gas) {
         * //code
         * });
         */
        public async Task<dynamic> EstimateGasAsync(RequestEstimateGas request)
        {
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(request, Formatting.Indented, CamelCaseSetting);
            return await sendRequestAsync<dynamic>("post", "/estimateGas", jsonString);
        }

        /**
         * Return the events list of transaction.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc.md#geteventsbyhash}
         *
         * @param {Object} options
         * @param {HexString} options.hash
         *
         * @return [Events]{@link https://github.com/nebulasio/wiki/blob/master/rpc.md#geteventsbyhash}
         *
         * @example
         * var api = new Neb().api;
         * api.getEventsByHash({hash: "ec239d532249f84f158ef8ec9262e1d3d439709ebf4dd5f7c1036b26c6fe8073"}).then(function(events) {
         * //code
         * });
         */
        public async Task<dynamic> GetEventsByHashAsync(string hash)
        {
            RequestEventsByHash request = new RequestEventsByHash() {
                Hash = hash
            };
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(request, Formatting.Indented, CamelCaseSetting);
            return await sendRequestAsync<dynamic>("post", "/getEventsByHash", jsonString);
        }


        /**
         * MNasod getter for dpos dynasty.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc.md#getdynasty}
         *
         * @param {Object} options
         * @param {Number} options.height
         *
         * @return [delegatees]{@link https://github.com/nebulasio/wiki/blob/master/rpc.md#getdynasty}
         *
         * @example
         * var api = new Neb().api;
         * api.getDynasty({height: 1}).then(function(delegatees) {
         * //code
         * });
         */
        public async Task<dynamic> GetDynastyAsync(string height)
        {
            RequestGetDynasty request = new RequestGetDynasty()
            {
                Height = height
            };
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(request, Formatting.Indented, CamelCaseSetting);
            return await sendRequestAsync<dynamic>("post", "/dynasty", jsonString);
        }

    }
}