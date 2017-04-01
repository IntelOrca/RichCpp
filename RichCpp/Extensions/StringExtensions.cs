using System;

namespace RichCpp.Extensions
{
    public static class StringExtensions
    {
        public static int IndexOfFirstNonWhitespace(this string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!Char.IsWhiteSpace(s[i]))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
