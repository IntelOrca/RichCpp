using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace RichCpp.IntelliSense.CodeLens
{
    internal class CodeLensTagger : ITagger<ICodeLensTag>
    {
        private readonly ITextBuffer _buffer;

#pragma warning disable 67
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore 67

        public CodeLensTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
        }

        public IEnumerable<ITagSpan<ICodeLensTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            var tags = new List<ITagSpan<ICodeLensTag>>();
            var snapshot = _buffer.CurrentSnapshot;
            for (int i = 0; i < snapshot.LineCount; i++)
            {
                var line = snapshot.GetLineFromLineNumber(i);
                string lineText = line.GetText();
                if (lineText.Contains("#include"))
                {
                    int startCol = lineText.Length - lineText.TrimStart().Length;
                    int endCol = lineText.Length;
                    int spanLen = endCol - startCol;
                    var span = new SnapshotSpan(snapshot, line.Start + startCol, spanLen);

                    var tagDesc = new TestCodeLensDescriptor(lineText);
                    var tag = new CodeLensTag(tagDesc);
                    var tagSpan = new TagSpan<ICodeLensTag>(span, tag);

                    tags.Add(tagSpan);
                }
            }
            return tags;
        }
    }
}
