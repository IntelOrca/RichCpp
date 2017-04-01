using System;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using RichCpp.Extensions;

namespace RichCpp.IntelliSense.BlockContext
{
    internal class BraceBlockTag : BlockTag
    {
        private readonly string _text;

        public BraceBlockTag(SnapshotSpan span, SnapshotSpan statementSpan, IBlockTag parent) :
            base(span, statementSpan, parent, "block", true, true, true, "...", GetCollapsedFormHint(span))
        {
            _text = span.GetText();
        }

        private static object GetCollapsedFormHint(SnapshotSpan span)
        {
            var firstLine = span.Snapshot.GetLineFromPosition(span.Start);
            int firstBraceStart = span.Start - firstLine.Start;

            string spanText = span.GetText();
            string[] spanLines = spanText.Replace("\r\n", "\n")
                                         .Split('\n');

            int trimSize = firstBraceStart;
            for (int i = 1; i < spanLines.Length; i++)
            {
                int indent = spanLines[i].IndexOfFirstNonWhitespace();
                if (indent < trimSize && indent != -1)
                {
                    trimSize = indent;
                }
            }

            if (trimSize > 0)
            {
                for (int i = 1; i < spanLines.Length; i++)
                {
                    string line = spanLines[i];
                    if (line.Length >= trimSize)
                    {
                        spanLines[i] = line.Substring(trimSize);
                    }
                }
            }

            string text = string.Join(Environment.NewLine, spanLines);
            return text;
        }

        public override string ToString()
        {
            return _text;
        }
    }
}
