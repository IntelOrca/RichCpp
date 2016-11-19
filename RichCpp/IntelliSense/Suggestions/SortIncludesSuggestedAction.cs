using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.OptionsExtensionMethods;

namespace RichCpp.IntelliSense.Suggestions
{
    /// <summary>
    /// Suggested action to sort a block of #include lines.
    /// </summary>
    internal class SortIncludesSuggestedAction : SuggestedActionBase
    {
        private readonly ITextView _textView;
        private readonly ITextBuffer _textBuffer;
        private readonly SnapshotPoint _target;

        public override string DisplayText => "Sort Includes";
        public override ImageMoniker IconMoniker => KnownMonikers.SortAscending;

        public SortIncludesSuggestedAction(ITextView textView, ITextBuffer textBuffer, SnapshotPoint target)
        {
            _textView = textView;
            _textBuffer = textBuffer;
            _target = target;
        }

        public override void Invoke(CancellationToken cancellationToken)
        {
            // Get the range for the current snapshot
            ITextSnapshot snapshot = _textView.TextSnapshot;
            SnapshotPoint target = _target.TranslateTo(snapshot, PointTrackingMode.Positive);
            int startLineIndex = snapshot.GetLineNumberFromPosition(target);
            int endLineIndex = startLineIndex;

            // Find first #include of block
            while (startLineIndex > 0 && LineIsInclude(snapshot, startLineIndex - 1))
            {
                startLineIndex--;
            }

            // Find last #include of block
            while (endLineIndex < snapshot.LineCount - 1 && LineIsInclude(snapshot, endLineIndex + 1))
            {
                endLineIndex++;
            }

            // Get includes
            int numIncludeLines = endLineIndex - startLineIndex + 1;
            var lines = new Tuple<string, string>[numIncludeLines];
            for (int i = startLineIndex; i <= endLineIndex; i++)
            {
                ITextSnapshotLine line = snapshot.GetLineFromLineNumber(i);
                string lineText = line.GetText();
                string includePath = GetIncludePath(snapshot, lineText);
                lines[i - startLineIndex] = new Tuple<string, string>(lineText, includePath);
            }

            // Sort (use LINQ as its stable)
            lines = lines.OrderBy(x => x.Item2.StartsWith("<") ? 0 : 1)
                         .ThenBy(x => x.Item2)
                         .ToArray();

            // Replace the lines with the new sorted lines
            ITextSnapshotLine startLine = snapshot.GetLineFromLineNumber(startLineIndex);
            ITextSnapshotLine endLine = snapshot.GetLineFromLineNumber(endLineIndex);
            string newLineChar = _textView.Options.GetNewLineCharacter();
            using (ITextEdit edit = _textBuffer.CreateEdit(EditOptions.DefaultMinimalChange, null, null))
            {
                var replaceSpan = Span.FromBounds(startLine.Start, endLine.End);
                string replaceText = String.Join(newLineChar, lines.Select(x => x.Item1));
                edit.Replace(replaceSpan, replaceText);
                edit.Apply();
            }
        }

        private static string GetIncludePath(ITextSnapshot snapshot, string lineText)
        {
            string result = null;
            lineText = lineText.Trim();
            if (lineText.StartsWith("#include"))
            {
                result = lineText.Remove(0, 8)
                                 .TrimStart();
            }
            return result;
        }

        private static bool LineIsInclude(ITextSnapshot snapshot, int lineIndex)
        {
            ITextSnapshotLine line = snapshot.GetLineFromLineNumber(lineIndex);
            string lineText = line.GetText().Trim();
            bool result = lineText.StartsWith("#include");
            return result;
        }

        public static Task<bool> CheckAvailabilityAsync(SnapshotSpan range, CancellationToken ct)
        {
            bool result = false;
            ITextSnapshot snapshot = range.Snapshot;
            int startLine = snapshot.GetLineNumberFromPosition(range.Start);
            int endLine = snapshot.GetLineNumberFromPosition(range.End);
            for (int i = startLine; i <= endLine; i++)
            {
                if (LineIsInclude(snapshot, i))
                {
                    result = true;
                    break;
                }
            }
            return Task.FromResult(result);
        }
    }
}
