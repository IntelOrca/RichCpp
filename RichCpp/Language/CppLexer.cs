using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RichCpp.Extensions;

namespace RichCpp.Language
{
    internal class CppLexer
    {
        private static readonly string[] Keywords = new string[]
        {
            "char", "class", "const", "constexpr", "int", "long", "signed", "short", "struct",
            "template", "typedef", "typename", "unsigned", "void", "volatile"
        };

        public CppToken[] GetTokens(string content)
        {
            var tokens = new List<CppToken>();
            var sr = new StringReader(content);
            char c;
            while (sr.TryPeek(out c))
            {
                CppToken tok;
                if (Char.IsWhiteSpace(c))
                {
                    tok = new CppToken(ReadWhitespace(sr), CppTokenType.Whitespace);
                }
                else if (Char.IsLetter(c))
                {
                    string text = ReadAlphaNumeric(sr);
                    CppTokenType type = CppTokenType.General;
                    if (IsKeyword(text))
                    {
                        type = CppTokenType.Keyword;
                    }
                    tok = new CppToken(text, type);
                }
                else if (Char.IsDigit(c))
                {
                    tok = new CppToken(ReadNumber(sr), CppTokenType.Number);
                }
                else
                {
                    tok = new CppToken(c.ToString(), CppTokenType.General);
                    sr.Read();
                }
                tokens.Add(tok);
            }
            return tokens.ToArray();
        }

        private static bool IsKeyword(string token)
        {
            return Keywords.Contains(token);
        }

        private static string ReadWhitespace(StringReader sr)
        {
            var sb = new StringBuilder();
            char c;
            while (sr.TryPeek(out c) && Char.IsWhiteSpace(c))
            {
                sr.Read();
                sb.Append(c);
            }
            return sb.ToString();
        }

        private static string ReadAlphaNumeric(StringReader sr)
        {
            var sb = new StringBuilder();
            char c;
            while (sr.TryPeek(out c) && Char.IsLetterOrDigit(c))
            {
                sr.Read();
                sb.Append(c);
            }
            return sb.ToString();
        }

        private static string ReadNumber(StringReader sr)
        {
            var sb = new StringBuilder();
            char c;
            while (sr.TryPeek(out c) && Char.IsDigit(c))
            {
                sr.Read();
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
