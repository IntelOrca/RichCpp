using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using RichCpp.Language;

namespace RichCpp.IntelliSense.QuickInfo
{
    internal class ColourisedQuickInfoContent : TextBlock
    {
        public ColourisedQuickInfoContent(IEnumerable<CppToken> tokens)
        {
            foreach (CppToken tok in tokens)
            {
                var run = new Run(tok.Text);
                Color colour = GetTokenTypeColour(tok.Type);
                run.Foreground = new SolidColorBrush(colour);
                Inlines.Add(run);
            }
        }

        private Color GetTokenTypeColour(CppTokenType tokenType)
        {
            // TODO try to get colours from IClassificationFormatMap

            switch (tokenType) {
            case CppTokenType.Keyword:
                return Colors.Blue;
            default:
                return Colors.Black;
            }
        }
    }
}
