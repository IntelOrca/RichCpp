using System.IO;

namespace RichCpp.Extensions
{
    internal static class TextReaderExtensions
    {
        public static bool TryPeek(this TextReader textReader, out char outCh)
        {
            int ch = textReader.Peek();
            if (ch == -1)
            {
                outCh = default(char);
                return false;
            }
            outCh = (char)ch;
            return true;
        }

        public static bool TryRead(this TextReader textReader, out char outCh)
        {
            int ch = textReader.Read();
            if (ch == -1)
            {
                outCh = default(char);
                return false;
            }
            outCh = (char)ch;
            return true;
        }
    }
}
