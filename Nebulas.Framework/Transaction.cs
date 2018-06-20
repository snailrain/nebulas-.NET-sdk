using Google.Protobuf;
using Nebulas.Util;
using System;
using System.Numerics;
using System.Text;
using Nebulas.Hex.HexConvertors.Extensions;
using Nebulas.Tool;
using Cryptography.ECDSA;

namespace Nebulas
{
    public class Transaction
    {
        private uint _chainId { get; set; }
        private Account _fromAccount { get; set; }
        private Account _toAccount { get; set; }
        private ulong _value { get; set; }
        private ulong _nonce { get; set; }
        private BigInteger _gasPrice { get; set; }
        private BigInteger _gasLimit { get; set; }

        #region 交易数据和type
        private TxPayload _payLoadType { get; set; }
        private byte[] _payLoadData { get; set; }
        #endregion
        #region tran各种类别参数
        private string _function { get; set; }
        private string _args { get; set; }
        
        private byte[] _binary { get; set; }

        private SourceType _sourceType { get; set; }
        private string _source { get; set; }
        #endregion

        private long _timestamp { get; set; }

        const int SECP256K1 = 1;
        public byte[] Hash { get; set; }
        public uint Alg { get; set; }
        public byte[] Sign { get; set; }

        private string _signErrorMessage = "";

        #region 构造函数
        public Transaction(uint chainId, Account fromAccount, string to, ulong value, ulong nonce,
            ulong gasPrice, ulong gasLimit, string function,string args)
        {
            initParas(chainId, fromAccount, to, value, nonce, gasPrice, gasLimit);
            _payLoadType = TxPayload.Call;
            _function = function;
            _args = args;
            var payload = new
            {
                Function = function,
                Args = args
            };
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            _payLoadData = Encoding.Default.GetBytes(data);
        }

        public Transaction(uint chainId, Account fromAccount, string to, ulong value, ulong nonce,
    ulong gasPrice, ulong gasLimit, byte[] binary)
        {
            initParas(chainId, fromAccount, to, value, nonce, gasPrice, gasLimit);
            _payLoadType = TxPayload.Binary;
            _binary = binary;
            var payload = new
            {
                Data = binary
            };
            _payLoadData = Encoding.Default.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(payload));
        }

        public Transaction(uint chainId, Account fromAccount, string to, ulong value, ulong nonce,
ulong gasPrice, ulong gasLimit, SourceType sourceType, string source, string args)
        {
            initParas(chainId, fromAccount, to, value, nonce, gasPrice, gasLimit);
            _payLoadType = TxPayload.Deploy;
            _sourceType = sourceType;
            _source = source;
            _args = args;
            var payload = new
            {
                SourceType = sourceType,
                Source = source,
                Args = args
            };
            _payLoadData = Encoding.Default.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(payload));
        }

        private void initParas(uint chainId, Account fromAccount, string to, ulong value, ulong nonce,
    BigInteger gasPrice, ulong gasLimit)
        {
            _chainId = chainId;
            _fromAccount = fromAccount;
            _toAccount = Account.FromAddress(to);
            _value = value;
            _nonce = nonce;
            _gasPrice = gasPrice;
            _gasLimit = gasLimit;
            _timestamp = GetUnixTimestamp(DateTime.Now);
            //_timestamp = 1529286663;

            if (_gasPrice == 0)
            {
                _gasPrice = 1000000;
            }
            if (_gasLimit == 0)
            {
                _gasLimit = 20000;
            }
            _signErrorMessage = "You should sign transaction before this operation.";
        }
        #endregion


        /**
         * Convert transaction to hash by SHA3-256 algorithm.
         *
         * @return {Hash} hash of Transaction.
         *
         * @example
         * var acc = Account.NewAccount();
         *
         * var tx = new Transaction({
         *    chainID: 1,
         *    from: acc,
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * });
         * var txHash = tx.hashTransaction();
         * //Uint8Array(32) [211, 213, 102, 103, 23, 231, 246, 141, 20, 202, 210, 25, 92, 142, 162, 242, 232, 95, 44, 239, 45, 57, 241, 61, 34, 2, 213, 160, 17, 207, 75, 40]
         */
        public byte[] HashTransaction()
        {
            var data = new Corepb.Data
            {
                Payload = ByteString.CopyFrom(_payLoadData),
                Type = Enum.GetName(typeof(TxPayload), _payLoadType).ToLower()
            };

            var dataBuffer = data.ToByteArray();

            var hash = Sha3Util.Get256Hash(ByteUtil.Merge(
                _fromAccount.GetAddress(),
                _toAccount.GetAddress(),
                CryptoUtils.PadToBigEndian(_value.ToString("x"), 128),
                CryptoUtils.PadToBigEndian(_nonce.ToString("x"), 64),
                CryptoUtils.PadToBigEndian(_timestamp.ToString("x"), 64),
                dataBuffer,
                CryptoUtils.PadToBigEndian(_chainId.ToString("x"), 32),
                CryptoUtils.PadToBigEndian(_gasPrice.ToString("x"), 128),
                CryptoUtils.PadToBigEndian(_gasLimit.ToString("x"), 128))
                );
            return hash;
        }

        /**
         * Sign transaction with the specified algorithm.
         *
         * @example
         * var acc = Account.NewAccount();
         *
         * var tx = new Transaction({
         *    chainID: 1,
         *    from: acc,
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * });
         * tx.signTransaction();
         */
        public void SignTransaction()
        {
            if (_fromAccount.GetPrivateKey() != null)
            {
                Hash = HashTransaction();
                Alg = SECP256K1;
                Sign = sign(Hash, _fromAccount.GetPrivateKey());
            }
            else
            {
                throw new Exception("transaction from address's private key is invalid");
            }
        }

        /**
         * Conver transaction data to plain JavaScript object.
         *
         * @return {Object} Plain JavaScript object with Transaction fields.
         * @example
         * var acc = Account.NewAccount();
         * var tx = new Transaction({
         *    chainID: 1,
         *    from: acc,
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * });
         * txData = tx.toPlainObject();
         * // {chainID: 1001, from: "n1USdDKeZXQYubA44W2ZVUdW1cjiJuqswxp", to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17", value: 1000000000000000000, nonce: 1, …}
         */
        public dynamic ToPlainObject()
        {
            return new 
            {
                chainID = _chainId,
                from = _fromAccount.GetAddressString(),
                to = _toAccount.GetAddressString(),
                value = _value,
                nonce = _nonce,
                gasPrice = _gasPrice,
                gasLimit = _gasLimit,
                contract = new {
                    sourceType = Enum.GetName(typeof(SourceType), _sourceType).ToLower(),
                    function = _function,
                    args = _args,
                    source = _source,
                    binary = _binary
                }
            };
        }


        /**
        * Convert transaction to JSON string.
        * </br><b>Note:</b> Transaction should be [sign]{@link Transaction#signTransaction} before converting.
        *
        * @return {String} JSON stringify of transaction data.
        * @example
        * var acc = Account.NewAccount();
        *
        * var tx = new Transaction({
        *    chainID: 1,
        *    from: acc,
        *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
        *    value: 10,
        *    nonce: 12,
        *    gasPrice: 1000000,
        *    gasLimit: 2000000
        * });
        * tx.signTransaction();
        * var txHash = tx.toString();
        * // "{"chainID":1001,"from":"n1QZMXSZtW7BUerroSms4axNfyBGyFGkrh5","to":"n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17","value":"1000000000000000000","nonce":1,"timestamp":1521905294,"data":{"payloadType":"binary","payload":null},"gasPrice":"1000000","gasLimit":"20000","hash":"f52668b853dd476fd309f21b22ade6bb468262f55402965c3460175b10cb2f20","alg":1,"sign":"cf30d5f61e67bbeb73bb9724ba5ba3744dcbc995521c62f9b5f43efabd9b82f10aaadf19a9cdb05f039d8bf074849ef4b508905bcdea76ae57e464e79c958fa900"}"
        */
        public override string ToString()
        {
            if (Sign == null)
            {
                throw new Exception("You should sign transaction before this operation.");
            }
            var payload = _payLoadData;
            var tx = new 
            {
                chainID = _chainId,
                from = _fromAccount.GetAddressString(),
                to = _toAccount.GetAddressString(),
                value = _value,
                nonce = _nonce,
                gasPrice = _gasPrice,
                gasLimit = _gasLimit,
                timestamp = _timestamp,
                data = new { type = Enum.GetName(typeof(TxPayload), _payLoadType).ToLower(), payload = payload },
                hash = Hash,
                alg = Alg,
                sign = Sign

            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(tx);
        }

        /**
         * Convert transaction to Protobuf format.
         * </br><b>Note:</b> Transaction should be [sign]{@link Transaction#signTransaction} before converting.
         *
         * @return {Buffer} Transaction data in Protobuf format
         *
         * @example
         * var acc = Account.NewAccount();
         *
         * var tx = new Transaction({
         *    chainID: 1,
         *    from: acc,
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * });
         * tx.signTransaction();
         * var txHash = tx.toProto();
         * // Uint8Array(127)
         */
        public byte[] ToProto()
        {
            if (Sign == null)
            {
                throw new Exception(_signErrorMessage);
            }
            var data = new Corepb.Data
            {
                Payload = ByteString.CopyFrom(_payLoadData),
                Type = Enum.GetName(typeof(TxPayload), _payLoadType).ToLower()
            };

            var txData = new Corepb.Transaction
            {
                Hash = ByteString.CopyFrom(Hash),
                From = ByteString.CopyFrom(_fromAccount.GetAddress()),
                To = ByteString.CopyFrom(_toAccount.GetAddress()),
                Value = ByteString.CopyFrom(CryptoUtils.PadToBigEndian(_value.ToString("x"), 128)),
                Nonce = _nonce,
                Timestamp = _timestamp,
                Data = data,
                ChainId = _chainId,
                GasPrice = ByteString.CopyFrom(CryptoUtils.PadToBigEndian(_gasPrice.ToString("x"), 128)),
                GasLimit = ByteString.CopyFrom(CryptoUtils.PadToBigEndian(_gasLimit.ToString("x"), 128)),
                Alg = Alg,
                Sign = ByteString.CopyFrom(Sign)
            };

            var txBuffer = txData.ToByteArray();
            return txBuffer;
        }


        /**
        * Convert transaction to Protobuf hash string.
        * </br><b>Note:</b> Transaction should be [sign]{@link Transaction#signTransaction} before converting.
        *
        * @return {Base64} Transaction string.
        *
        * @example
        * var acc = Account.NewAccount();
        *
        * var tx = new Transaction({
        *    chainID: 1,
        *    from: acc,
        *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
        *    value: 10,
        *    nonce: 12,
        *    gasPrice: 1000000,
        *    gasLimit: 2000000
        * });
        * tx.signTransaction();
        * var txHash = tx.toProtoString();
        * // "EhjZTY/gKLhWVVMZ+xoY9GiHOHJcxhc4uxkaGNlNj+AouFZVUxn7Ghj0aIc4clzGFzi7GSIQAAAAAAAAAAAN4Lazp2QAACgBMPCz6tUFOggKBmJpbmFyeUDpB0oQAAAAAAAAAAAAAAAAAA9CQFIQAAAAAAAAAAAAAAAAAABOIA=="
        */
        public string toProtoString()
        {
            var txBuffer = ToProto();
            return Convert.ToBase64String(txBuffer, 0, txBuffer.Length);
        }

        private byte[] sign(byte[] msgHash, byte[] privateKey)
        {
            var recovery = 0;

            var sig = Secp256K1Manager.SignCompact(msgHash, privateKey, out recovery);
            var _recBuf = recovery.ToString("x").HexToByteArray();
            var ret = ByteUtil.Merge(sig,_recBuf);

            return ret;
        }

        /**
        * Restore Transaction from Protobuf format.
        * @property {Buffer|String} data - Buffer or stringify Buffer.
        *
        * @return {Transaction} Restored transaction.
        *
        * @example
        * var acc = Account.NewAccount();
        *
        * var tx = new Transaction({
        *    chainID: 1,
        *    from: acc,
        *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
        *    value: 10,
        *    nonce: 12,
        *    gasPrice: 1000000,
        *    gasLimit: 2000000
        * });
        * var tx = tx.fromProto("EhjZTY/gKLhWVVMZ+xoY9GiHOHJcxhc4uxkaGNlNj+AouFZVUxn7Ghj0aIc4clzGFzi7GSIQAAAAAAAAAAAN4Lazp2QAACgBMPCz6tUFOggKBmJpbmFyeUDpB0oQAAAAAAAAAAAAAAAAAA9CQFIQAAAAAAAAAAAAAAAAAABOIA==");
        */
        public Transaction FromProto(string data)
        {
            var txBuffer = Convert.FromBase64String(data);
            return FromProto(txBuffer);
        }

        public Transaction FromProto(byte[] data)
        {

            var txBuffer = data;

            var txProto = Corepb.Transaction.Parser.ParseFrom(txBuffer);

            Hash = txProto.Hash.ToByteArray();
            _fromAccount = Account.FromAddress(txProto.From.ToString());
            _toAccount = Account.FromAddress(txProto.To.ToString());

            _value = ulong.Parse(txProto.Value.ToString());

            _nonce = ulong.Parse(txProto.Nonce.ToString());
            _timestamp = long.Parse(txProto.Timestamp.ToString());

            _payLoadData = txProto.Data.Payload.ToByteArray();
            _payLoadType = (TxPayload)Enum.Parse(typeof(TxPayload), txProto.Data.Type);
            if (_payLoadData.Length == 0)
            {
                _payLoadData = null;
            }
            _chainId = txProto.ChainId;
            _gasPrice = BigInteger.Parse(txProto.GasPrice.ToString());
            _gasLimit = BigInteger.Parse(txProto.GasLimit.ToString());
            Alg = uint.Parse(txProto.Alg.ToString());
            Sign = txProto.Sign.ToByteArray();

            return this;
        }


        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long GetUnixTimestamp(DateTime time)
        {
            return (long)((time - new DateTime(1970, 1, 1)).TotalMilliseconds / 1000);
        }


    }

    public enum TxPayload {
        Binary,
        Deploy,
        Call
    }

    public enum SourceType
    {
        JS,
        TS
    }
}
