using System;
using Microsoft.VisualStudio.Text;

namespace RichCpp.Extensions
{
    internal static class TextSnapshotLine
    {
        public static SnapshotSpan GetExtentTrimmed(this ITextSnapshotLine line)
        {
            string text = line.GetText();
            string trimStart = text.TrimStart();
            string trimEnd = text.TrimEnd();
            int startIndex = text.Length - trimStart.Length;
            int length = Math.Max(0, trimEnd.Length - startIndex);
            return new SnapshotSpan(line.Snapshot, line.Start + startIndex, length);
        }
    }
}
