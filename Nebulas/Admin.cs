using Nebulas.Schema.Request;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Nebulas
{
    public class Admin : BaseNeb
    {
        public Admin(HttpRequest request) : base(request)
        {
            _path = "admin";
        }

        /**
         * Method get info about nodes in Nebulas Network.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#nodeinfo}
         *
         * @return [nodeInfoObject]{@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#nodeinfo}
         *
         * @example
         * var admin = new Neb().admin;
         * admin.nodeInfo().then(function(info) {
         * //code
         * });
         */
        public async Task<dynamic> NodeInfoAsync()
        {
            return await sendRequestAsync<dynamic>("get", "/nodeinfo", null);
        }

        /**
         * Method get list of available addresses.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#accounts}
         *
         * @return [accountsList]{@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#accounts}
         *
         * @example
         * var admin = new Neb().admin;
         * admin.accounts().then(function(accounts) {
         * //code
         * });
         */
        public async Task<dynamic> AccountsAsync()
        {
            return await sendRequestAsync<dynamic>("get", "/accounts", null);
        }



        /**
        * Method create a new account in Nebulas network with provided passphrase.
        * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#newaccount}
        *
        * @param {Object} options
        * @param {Password} options.passphrase
        *
        * @return [address]{@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#newaccount}
        *
        * @example
        * var admin = new Neb().admin;
        * admin.newAccount({passphrase: "passphrase"}).then(function(address) {
        * //code
        * });
        */
        public async Task<dynamic> NewAccountAsync(string passphrase)
        {
            var paras = new{
                passphrase
            };
            return await sendRequestAsync<dynamic>("post", "/account/new", paras);
        }

        /**
         * Method lock account.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#lockaccount}
         *
         * @param {Object} options
         * @param {HexString} options.address
         *
         * @return [isLocked]{@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#lockaccount}
         *
         * @example
         * var admin = new Neb().admin;
         * admin.lockAccount({address: "n1cYKNHTeVW9v1NQRWuhZZn9ETbqAYozckh"}).then(function(isLocked) {
         * //code
         * });
         */
        public async Task<dynamic> LockAccountAsync(string address)
        {
            var paras = new
            {
                address
            };
            return await sendRequestAsync<dynamic>("post", "/account/lock", paras);
        }

        /**
         * Method lock account.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#lockaccount}
         *
         * @param {Object} options
         * @param {HexString} options.address
         *
         * @return [isLocked]{@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#lockaccount}
         *
         * @example
         * var admin = new Neb().admin;
         * admin.lockAccount({address: "n1cYKNHTeVW9v1NQRWuhZZn9ETbqAYozckh"}).then(function(isLocked) {
         * //code
         * });
         */
        public async Task<dynamic> UnlockAccountAsync(string address,string passphrase,int duration)
        {
            var paras = new
            {
                address,
                passphrase,
                duration
            };
            return await sendRequestAsync<dynamic>("post", "/account/unlock", paras);
        }


        /**
         * Method wrap transaction sending functionality.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#sendtransaction}
         *
         * @param {TransactionOptions} options
         *
         * @return [Transcation hash and contract address]{@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#sendtransaction}
         *
         * @example
         * var admin = new Neb().admin;
         * admin.sendTransaction({
         *    from: "n1QZMXSZtW7BUerroSms4axNfyBGyFGkrh5",
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * }).then(function(tx) {
         * //code
         * });
         */
        public async Task<dynamic> SendTransactionAsync(RequestSendTransaction request)
        {
            string paras = JsonConvert.SerializeObject(request, Formatting.Indented, CamelCaseSetting);
            return await sendRequestAsync<dynamic>("post", "/account/unlock", paras);
        }

        /**
         * Method sign hash.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#signhash}
         *
         * @param {Object} options
         * @param {HexString} options.address
         * @param {Base64} options.hash of hash bytes with base64 encode.
         * @param {UInt32} options.alg
         *
         * @return [data]{@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#signhash}
         *
         * @example
         * var admin = new Neb().admin;
         * admin.SignHash({
         *     address: "n1cYKNHTeVW9v1NQRWuhZZn9ETbqAYozckh",
         *     hash: "OGQ5NjllZWY2ZWNhZDNjMjlhM2E2MjkyODBlNjg2Y2YwYzNmNWQ1YTg2YWZmM2NhMTIwMjBjOTIzYWRjNmM5Mg==",
         *     alg: 1
         * }).then(function(data) {
         * //code
         * });
         */
        public async Task<dynamic> SignHashAsync(RequestSignHash request)
        {
            string paras = JsonConvert.SerializeObject(request, Formatting.Indented, CamelCaseSetting);
            return await sendRequestAsync<dynamic>("post", "/sign/hash", paras);
        }


        /**
         * Method sign transaction with passphrase.
         * The transaction's from addrees must be unlock before sign call.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#signtransactionwithpassphrase}
         *
         * @param {TransactionOptions} options
         * @param {Password} options.passphrase
         *
         * @return [Transcation hash and contract address]{@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#signtransactionwithpassphrase}
         *
         * @example
         * var admin = new Neb().admin;
         * admin.signTransactionWithPassphrase({
         *    from: "n1QZMXSZtW7BUerroSms4axNfyBGyFGkrh5",
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000,
         *    passphrase: "passphrase"
         * }).then(function(tx) {
         * //code
         * });
         */
        public async Task<dynamic> SignTransactionWithPassphraseAsync(RequestSignTransactionWithPassphrase request)
        {
            string paras = JsonConvert.SerializeObject(request, Formatting.Indented, CamelCaseSetting);
            return await sendRequestAsync<dynamic>("post", "/sign", paras);
        }


        /**
         * Method send transaction with passphrase.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#sendtransactionwithpassphrase}
         *
         * @param {TransactionOptions} options
         * @param {Password} options.passphrase
         *
         * @return [data]{@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#sendtransactionwithpassphrase}
         *
         * @example
         * var admin = new Neb().admin;
         * admin.sendTransactionWithPassphrase({
         *    from: "n1QZMXSZtW7BUerroSms4axNfyBGyFGkrh5",
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000,
         *    passphrase: "passphrase"
         * }).then(function(tx) {
         * //code
         * });
         */
        public async Task<dynamic> SendTransactionWithPassphraseAsync(RequestSendTransactionWithPassphrase request)
        {
            string paras = JsonConvert.SerializeObject(request, Formatting.Indented, CamelCaseSetting);
            return await sendRequestAsync<dynamic>("post", "/transactionWithPassphrase", paras);
        }

        /**
         * Method start listen provided port.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#startpprof}
         *
         * @param {Object} options
         * @param {String} options.listen - Listen port.
         *
         * @return [isListenStrted]{@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#startpprof}
         *
         * @example
         * var admin = new Neb().admin;
         * admin.startPprof({listen: '8080'}).then(function(isListenStrted) {
         * //code
         * });
         */
        public async Task<dynamic> StartPprofAsync(string listen)
        {
            var paras = new {
                listen
            };
            return await sendRequestAsync<dynamic>("post", "/pprof", paras);
        }

        /**
         * Method get config of node in Nebulas Network.
         * @see {@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#getConfig}
         *
         * @return [config]{@link https://github.com/nebulasio/wiki/blob/master/rpc_admin.md#getConfig}
         *
         * @example
         * var admin = new Neb().admin;
         * admin.getConfig().then(function(info) {
         * //code
         * });
         */
        public async Task<dynamic> GetConfigAsync(string listen)
        {
            return await sendRequestAsync<dynamic>("get", "/getConfig", null);
        }
    }
}
