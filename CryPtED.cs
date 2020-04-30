using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegistrationTool
{
    public class CryptED
    {
        private readonly int C_1 = 42594;
        private readonly int C_2 = 14712;

        private string ByteToHex(byte b)
        {
            string ret = Convert.ToString(b, 16);
            while (ret.Length < 2) ret = "0" + ret;
            ret = ret.ToUpper();
            return ret;
        }

        private byte IntToByte(int src)
        {
            while ((src > 255) || (src < 0)) src = src & 0xff;
            return Convert.ToByte(src);
        }

        private byte StrToByte(string s)
        {
            char c = Convert.ToChar(s);
            int i = Convert.ToInt32(c);
            byte b = IntToByte(i);
            return b;
        }

        private byte StrToByte(char c)
        {
            byte b = Convert.ToByte(c);
            return b;
        }

        private string StringToBCD(string StrIn)
        {
            string ret = "";
            int Len = StrIn.Length;
            for (int I = 0; I < Len; I++)
            {
                string s = StrIn.Substring(I, 1);
                ret = ret + ByteToHex(StrToByte(s));
            }
            return ret;
        }

        private string Encrypt(string StrIn, int Key)
        {
            string ret = "";
            int Len = StrIn.Length;
            for (int I = 0; I < Len; I++)
            {
                string s = StrIn.Substring(I, 1);
                byte b = StrToByte(s);
                b = (byte)(b ^ (Key >> 8));
                s = Convert.ToChar(b).ToString();
                ret = ret + s;
                Key = (b + Key) * C_1 + C_2;
            }
            return ret;
        }

        private string StringEncryptToBCD(string StrIn, int Key)
        {
            return StringToBCD(Encrypt(StrIn, Key));
        }

        public string StrEncrypt(string src, string Key, int Len)
        {
            if (Len > 0) while (src.Length < Len) src = "0" + src;
            if (src == "") return "";
            int key = 0;
            for (int i = 0; i < Key.Length; i++) key = key + StrToByte(Key[i]);
            return StringEncryptToBCD(src, key);
        }

        public string StrEncrypt(string src, int Key)
        {
            if (src == "") return "";
            return StringEncryptToBCD(src, Key);
        }

        private int CharToInt(string c)
        {
            c = c.ToUpper();
            if ((c == "0") || (c == "1") || (c == "2") || (c == "3") || (c == "4") || (c == "5") ||
              (c == "6") || (c == "7") || (c == "8") || (c == "9"))
                return Convert.ToByte(Convert.ToChar(c)) - 48;
            else if ((c == "A") || (c == "B") || (c == "C") || (c == "D") || (c == "E") || (c == "F"))
                return Convert.ToByte(Convert.ToChar(c)) - 55;
            else
                return 0;
        }

        private string BCDToString(string StrIn)
        {
            string ret = "";
            int Len = StrIn.Length / 2;
            for (int I = 0; I < Len; I++)
            {
                string s = StrIn.Substring(I * 2, 1);
                int i1 = CharToInt(s) * 16;
                s = StrIn.Substring(I * 2 + 1, 1);
                int i2 = CharToInt(s);
                byte b = (byte)(i1 + i2);
                s = Convert.ToChar(b).ToString();
                ret = ret + s;
            }
            return ret;
        }

        private string Decrypt(string StrIn, int Key)
        {
            string ret = "";
            int Len = StrIn.Length;
            for (int I = 0; I < Len; I++)
            {
                string s = StrIn.Substring(I, 1);
                byte b = StrToByte(s);
                byte b1 = (byte)(b ^ (Key >> 8));
                s = Convert.ToChar(b1).ToString();
                ret = ret + s;
                Key = (b + Key) * C_1 + C_2;
            }
            return ret;
        }

        private string StringDecryptFromBCD(string StrIn, int Key)
        {
            return Decrypt(BCDToString(StrIn), Key);
        }

        public string StrDecrypt(string src, string Key)
        {
            int key = 0;
            for (int i = 0; i < Key.Length; i++) key = key + StrToByte(Key[i]);
            return StringDecryptFromBCD(src, key);
        }

        public string StrDecrypt(string src, int Key)
        {
            return StringDecryptFromBCD(src, Key);
        }
    }
}
