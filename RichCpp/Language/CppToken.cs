namespace RichCpp.Language
{
    internal struct CppToken
    {
        public string Text { get; }
        public CppTokenType Type { get; }

        public CppToken(string text, CppTokenType type)
        {
            Text = text;
            Type = type;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
