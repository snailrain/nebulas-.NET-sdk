using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Nebulas.Hex.HexConvertors.Extensions;

namespace Nebulas.Tool
{
    public class CryptoUtils
    {
        public static string IntToHex(int i)
        {
            var hex = i.ToString("x");
            return "0x" + PadToEven(hex);
        }
        public static string IntToHex(BigInteger i)
        {
            var hex = i.ToString("x");
            return "0x" + PadToEven(hex);
        }

        public static byte[] intToBuffer(int i)
        {
            var hex = IntToHex(i);
            return hex.HexToByteArray();
        }
        public static byte[] intToBuffer(BigInteger i)
        {
            var hex = IntToHex(i);
            return hex.HexToByteArray();
        }

        public static string PadToEven(string value)
        {
            if (value.Length % 2 != 0)
            {
                return "0" + value;
            }
            return value;
        }


        public static byte[] PadToBigEndian(string value, int digit)
        {
            var _value = value.HexToByteArray();
            return PadToBigEndian(_value, digit);
        }
        public static byte[] PadToBigEndian(byte[] value, int digit)
        {

            var buff = new byte[digit / 8];
            for (var i = 0; i < value.Length; i++)
            {
                var start = buff.Length - value.Length + i;
                if (start >= 0)
                {
                    buff[start] = value[i];
                }
            }
            return buff;
        }

        public static byte[] Zeros(byte[] bytes)
        {
            var ret = bytes;
            Array.Clear(ret, 0, ret.Length);
            return ret;
        }
    }
}
