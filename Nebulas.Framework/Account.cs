using Nebulas.Signer;
using Nebulas.Hex.HexConvertors.Extensions;
using System;
using Nebulas.Util;
using Nebulas.Tool;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Cryptography.ECDSA;
using Norgerman.Cryptography.Scrypt;

namespace Nebulas
{
    public class Account
    {
        private HttpRequest _request { get; set; }
        private string _path { get; set; }
        private byte[] _privateKey { get; set; }
        private byte[] _address { get; set; }
        private byte[] _publicKey { get; set; }

        const uint ADDRESS_LENGTH = 26;
        const uint ADDRESS_PREFIX = 25;

        public const int KEYVERSION3 = 3;
        public const int KEYCURRENTVERSION = 4;
        public enum AddressType
        {
            NormalType = 87,
            ContractType = 88
        }

      
        public Account()
        {

        }

        public Account(string privateKey)
        {
            //_path = path;
            SetPrivateKey(privateKey);
        }
        public Account(byte[] privateKey)
        {
            //长度32
            _privateKey = privateKey;
        }

        /**
         * Private Key setter.
         *
         * @param {Hash} priv - Account private key.
         *
         * @example account.setPrivateKey("ac3773e06ae74c0fa566b0e421d4e391333f31aef90b383f0c0e83e4873609d6");
         */
        public void SetPrivateKey(string privateKey)
        {
            _privateKey = privateKey.HexToByteArray();
        }

        public void SetPrivateKey(byte[] privateKey)
        {
            _privateKey = privateKey;
        }


        /**
         * Public Key getter.
         *
         * @return {Buffer} Account public key.
         *
         * @example var publicKey = account.getPublicKey();
         * //<Buffer c0 96 aa 4e 66 c7 4a 9a c7 18 31 f1 24 72 2a c1 3e b5 df 7f 97 1b 13 1d 46 a2 8a e6 81 c6 1d 96 f7 07 d0 aa e9 a7 67 436b 68 af a8 f0 96 65 17 24 29 ... >
         */
        public byte[] GetPrivateKey()
        {
            return _privateKey;
        }

        public string GetPrivateKeyString()
        {
            return _privateKey.ToHex();
        }



        /**
         * Accaunt address getter.
         *
         * @return {Buffer} Account address.
         *
         * @example var publicKey = account.getAddress();
         * //<Buffer 7f 87 83 58 46 96 12 7d 1a c0 57 1a 42 87 c6 25 36 08 ff 32 61 36 51 7c>
         */
        public byte[] GetAddress()
        {
            if (_address == null)
            {
                string address = NasECKey.GetPublicAddress(_privateKey.ToHex());
                
                _address = address.HexToByteArray();
            }
            return _address;
        }


        /**
         * Get account address in hex string format.
         *
         * @return {HexString} Account address in String format.
         *
         * @example var publicKey = account.getAddressString();
         * //"802d529bf55d6693b3ac72c59b4a7d159da53cae5a7bf99c"
         */
        public string GetAddressString()
        {
            return Base58.Encode(GetAddress());
        }


        public byte[] GetPublicKey()
        {
            if (_publicKey == null)
            {
                NasECKey eckey = new NasECKey(_privateKey.ToHex());
                _publicKey = eckey.GetPubKeyNoPrefix();
            }
            return _publicKey;
        }



        public string GetPublicKeyString()
        {
            return GetPublicKey().ToHex();
        }

        /**
            * Account factory mNasod.
            * Create random account.
            * @static
            *
            * @return {Account} Instance of Account constructor.
            *
            * @example var account = Account.NewAccount();
            */
        public static Account NewAccount()
        {
            return new Account(ByteUtil.Random(32));
        }

        /**
         * Address validation method.
         *
         * @static
         * @param {String/Hash} addr - Account address.
         * @param {Number} type - NormalType / ContractType
         *
         * @return {Boolean} Is address has correct format.
         *
         * @example
         * if ( Account.isValidAddress("n1QZMXSZtW7BUerroSms4axNfyBGyFGkrh5") ) {
         *     // some code
         * };
         */
        public static bool IsValidAddress(string address,AddressType? type = null)
        {
            byte[] addBytes;
            try {
                addBytes = Base58.Decode(address);

                if (addBytes.Length != ADDRESS_LENGTH)
                {
                    return false;
                }
                if (Convert.ToUInt32(addBytes[0]) != ADDRESS_PREFIX)
                {
                    return false;
                }
                uint typeUint;
                if (type.HasValue)
                {
                    typeUint = Convert.ToUInt32(type.Value);
                    if (typeUint != Convert.ToUInt32(addBytes[1]))
                    {
                        return false;
                    }
                }
                else
                {
                    typeUint = Convert.ToUInt32(addBytes[1]);
                }

                if (typeUint != (uint)AddressType.ContractType && typeUint != (uint)AddressType.NormalType)
                {
                    return false;
                }
                var content = addBytes.Slice(0, 22);
                var checksum = addBytes.Slice(addBytes.Length - 4);
                return ByteUtil.Compare(Sha3Util.Get256Hash(content).Slice(0, 4), checksum);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /**
         * Restore account from address.
         * Receive addr or Account instance.
         * If addr is Account instance return new Account instance with same PrivateKey.
         *
         * @static
         * @param {(Hash|Object)} - Client address or Account instance.
         *
         * @return {Account} Instance of Account restored from address.
         *
         * @example var account = Account.fromAddress("n1QZMXSZtW7BUerroSms4axNfyBGyFGkrh5");
         */
        public static Account FromAddress(string address)
        {
            if (!IsValidAddress(address))
            {
                throw new Exception("地址不合法");
            }
            var account = new Account();
            account._address = Base58.Decode(address);
            return account;
        }

        /**
        * Generate key by passphrase and options.
        *
        * @param {Password} password - Provided password.
        * @param {KeyOptions} opts - Key options.
        *
        * @return {Key} Key Object.
        *
        * @example var key = account.toKey("passphrase");
        */
        public Key ToKey(string password, KeyOptions opts = null)
        {
            byte[] derivedKey;
            if (opts.kdf == KDFEnum.Pbkdf2)
            {
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                crypto.GetBytes(opts.salt);
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, opts.salt, opts.c);
                derivedKey = pbkdf2.GetBytes(opts.dklen);
            }
            else if (opts.kdf == KDFEnum.Scrypt)
            {
                //derivedKey = Replicon.Cryptography.SCrypt.SCrypt.DeriveKey(Encoding.UTF8.GetBytes(password), opts.salt, (ulong)opts.n, (uint)opts.r, (uint)opts.p, (uint)opts.dklen);

                derivedKey = ScryptUtil.Scrypt(Encoding.UTF8.GetBytes(password), opts.salt, opts.n, opts.r, opts.p, opts.dklen);
            }
            else
            {
                throw new Exception("Unsupported kdf");
            }


            var iv = opts.iv.Slice(0, opts.iv.Length);
            Aes128CTR aes = new Aes128CTR(iv);

            byte[] key = derivedKey.Slice(0, 16);
            byte[] src = GetPrivateKey();
            byte[] dest;

            using (ICryptoTransform encrypt = aes.CreateEncryptor(key, null))
            {
                dest = encrypt.TransformFinalBlock(src, 0, src.Length);
                //encrypt.TransformBlock(src, 0, src.Length, dest, 0);
            }


            byte[] ciphertext = ByteUtil.Merge(dest, new byte[] { });


            var algoStr = opts.cipher;
            var algoBuf = Encoding.UTF8.GetBytes(algoStr);

            var data = ByteUtil.Merge(derivedKey.Slice(16, 32), ciphertext, opts.iv, algoBuf);

            bool bl = data.Compare(new byte[] { 29, 52, 224, 140, 175, 174, 254, 246, 150, 111, 54, 131, 20, 66, 32, 30, 74, 150, 12, 191, 5, 61, 192, 196, 41, 236, 65, 21, 61, 184, 251, 34, 181, 156, 116, 62, 192, 63, 123, 193, 144, 210, 110, 229, 144, 235, 148, 57, 174, 176, 29, 188, 185, 223, 149, 83, 130, 30, 94, 120, 100, 252, 2, 217, 97, 101, 115, 45, 49, 50, 56, 45, 99, 116, 114 });
            byte[] mac = Sha3Util.Get256Hash(data);

            return new Key
            {
                version = KEYCURRENTVERSION,
                id = Guid.NewGuid().ToString(),
                address = GetAddressString(),
                crypto = new KeyCrypto
                {
                    ciphertext = ciphertext.ToHex(),
                    cipherparams = new CipherParams() {
                        iv = opts.iv.ToHex()
                    },
                    cipher = opts.cipher,
                    kdf = Enum.GetName(typeof(KDFEnum), opts.kdf).ToLower(),
                    kdfparams = new KDFParams {
                        c = opts.c,
                        salt = opts.salt.ToHex(),
                        dklen = opts.dklen,
                        n = opts.n,
                        p = opts.p,
                        prf = (opts.kdf == KDFEnum.Pbkdf2) ? "hmac-sha256" : "",
                        r = opts.r
                    },
                    mac = mac.ToHex(),
                    machash = "sha3256"
                }
            };
        }


        public string ToKeyString(string password, KeyOptions opts)
        {
            return JsonConvert.SerializeObject(ToKey(password, opts));
        }


        /**
         * Restore account from key and passphrase.
         *
         * @param {Key} input - Key Object.
         * @param {Password} password - Provided password.
         * @param {Boolean} nonStrict - Strict сase sensitivity flag.
         *
         * @return {@link Account} - Instance of Account restored from key and passphrase.
         */
        public Account FromKey(Key input, string password)
        {
            var json = input;
            if (json.version != KEYVERSION3 && json.version != KEYCURRENTVERSION)
            {
                throw new Exception("Not supported wallet version");
            }
            byte[] derivedKey = null;
            KDFParams kdfparams = new KDFParams();
            if (json.crypto.kdf == "scrypt")
            {
                kdfparams = json.crypto.kdfparams;

                //derivedKey = Replicon.Cryptography.SCrypt.SCrypt.DeriveKey(Encoding.UTF8.GetBytes(password), kdfparams.salt.HexToByteArray(), (ulong)kdfparams.n, (uint)kdfparams.r, (uint)kdfparams.p, (uint)kdfparams.dklen);
                derivedKey = ScryptUtil.Scrypt(Encoding.UTF8.GetBytes(password), kdfparams.salt.HexToByteArray(), kdfparams.n, kdfparams.r, kdfparams.p, kdfparams.dklen);
            }
            else if (json.crypto.kdf == "pbkdf2")
            {
                kdfparams = json.crypto.kdfparams;
                if (kdfparams.prf != "hmac-sha256")
                {
                    throw new Exception("Unsupported parameters to PBKDF2");
                }

                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                crypto.GetBytes(kdfparams.salt.HexToByteArray());
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), kdfparams.salt.HexToByteArray(), kdfparams.c);
                derivedKey = pbkdf2.GetBytes(kdfparams.dklen);
            }
            else
            {
                throw new Exception("Unsupported key derivation scheme");
            }
            var ciphertext = json.crypto.ciphertext.HexToByteArray();
            byte[] mac;

            if (json.version == KEYCURRENTVERSION)
            {
                var _derviedKey = new byte[32];
                Array.Copy(derivedKey, 16, _derviedKey, 0, 32);
                var iv = json.crypto.cipherparams.iv.HexToByteArray();
                var cipher = json.crypto.cipher.HexToByteArray();
                var _mac = new byte[_derviedKey.Length + ciphertext.Length + iv.Length + cipher.Length];
                Array.Copy(_derviedKey, 0, _mac, 0, _derviedKey.Length);
                Array.Copy(ciphertext, 0, _mac, _derviedKey.Length, ciphertext.Length);
                Array.Copy(iv, 0, _mac, _derviedKey.Length + ciphertext.Length, iv.Length);
                Array.Copy(cipher, 0, _mac, _derviedKey.Length + ciphertext.Length + iv.Length, cipher.Length);
                mac = Sha3Util.Get256Hash(_mac);
            }
            else
            {
                // KeyVersion3
                var _mac = new byte[derivedKey.Slice(16, 32).Length + ciphertext.Length];
                mac = Sha3Util.Get256Hash(_mac);
            }

            if (mac.ToHex() != json.crypto.mac)
            {
                throw new Exception("Key derivation failed - possibly wrong passphrase");
            }

            Aes128CTR aes = new Aes128CTR(json.crypto.cipherparams.iv.HexToByteArray());

            byte[] src = ciphertext;
            byte[] dest = new byte[0];

            using (ICryptoTransform decrypt = aes.CreateDecryptor(derivedKey.Slice(0, 16), null))
            {
                dest = decrypt.TransformFinalBlock(src, 0, src.Length);
            }

            var _seed = new byte[src.Length + dest.Length];
            var seed = new byte[32];
            CryptoUtils.Zeros(seed);
            Array.Copy(_seed, 0, seed, 32 - _seed.Length, _seed.Length);
            SetPrivateKey(seed);
            return this;
        }
    }




    public class KeyOptions
    {
        public byte[] salt { get; set; } = ByteUtil.Random(32);
        public byte[] iv { get; set; } = ByteUtil.Random(16);
        public KDFEnum kdf { get; set; } = KDFEnum.Scrypt;
        public int dklen { get; set; } = 32;
        public int c { get; set; } = 262144;
        public int n { get; set; } = 4096;
        public int r { get; set; } = 8;
        public int p { get; set; } = 1;
        public string cipher { get; set; } = "aes-128-ctr";
        public byte[] uuid { get; set; } = ByteUtil.Random(16);
    }


    public enum KDFEnum
    {
        Scrypt,
        Pbkdf2
    }

    public class Key
    {
        public int version { get; set; }
        public string id { get; set; }
        public string address { get; set; }
        public KeyCrypto crypto { get; set; }
    }
    public class CipherParams
    {
        public string iv { get; set; }
    }
    public class KDFParams
    {
        public int dklen { get; set; }
        public string salt { get; set; }
        public int c { get; set; }
        public string prf { get; set; }
        public int n { get; set; }
        public int r { get; set; }
        public int p { get; set; }
    }
    public class KeyCrypto
    {
        public string ciphertext { get; set; }
        public CipherParams cipherparams { get; set; }
        public string cipher { get; set; }
        public string kdf { get; set; }
        public KDFParams kdfparams { get; set; }
        public string mac { get; set; }
        public string machash { get; set; }
    }



}