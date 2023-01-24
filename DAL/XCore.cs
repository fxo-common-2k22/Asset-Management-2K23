using System;

namespace FAPP.DAL
{
    public static class XCore
    {
        public const string PASS_PHRASE = "YeMpf696K67HmEcj";
        public const string INIT_VECTOR = "u58d7U8G9RPn9eMV";
        public const int KEY_SIZE = 256;

        public static int ToInt32(this string val)
        {
            return Int32.Parse(val);
        }

        public static int ToInt32(this int? val)
        {
            return Convert.ToInt32(val);
        }

        public static long ToInt64(this long? val)
        {
            return Convert.ToInt64(val);
        }
    }

}