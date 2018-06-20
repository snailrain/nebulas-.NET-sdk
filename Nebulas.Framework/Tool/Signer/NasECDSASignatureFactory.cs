﻿using System;
using Org.BouncyCastle.Math;

namespace Nebulas.Signer
{
    public class NasECDSASignatureFactory
    {
        public static NasECDSASignature FromComponents(byte[] r, byte[] s)
        {
            return new NasECDSASignature(new BigInteger(1, r), new BigInteger(1, s));
        }

        public static NasECDSASignature FromComponents(byte[] r, byte[] s, byte v)
        {
            var signature = FromComponents(r, s);
            signature.V = new[] {v};
            return signature;
        }

        public static NasECDSASignature FromComponents(byte[] r, byte[] s, byte[] v)
        {
            var signature = FromComponents(r, s);
            signature.V = v;
            return signature;
        }

        public static NasECDSASignature FromComponents(byte[] rs)
        {
            var r = new byte[32];
            var s = new byte[32];
            Array.Copy(rs, 0, r, 0, 32);
            Array.Copy(rs, 32, s, 0, 32);
            var signature = FromComponents(r, s);
            return signature;
        }
    }
}