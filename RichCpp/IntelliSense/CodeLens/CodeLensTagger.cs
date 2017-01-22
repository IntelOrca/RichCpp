using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.CodeSense;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using RichCpp.Extensions;

namespace RichCpp.IntelliSense.CodeLens
{
    internal class CodeLensTagger : ITagger<ICodeLensTag>
    {
        private readonly string _filePath;
        private readonly ITextBuffer _buffer;

#pragma warning disable 67
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore 67

        public CodeLensTagger(string filePath, ITextBuffer buffer)
        {
            _filePath = filePath;
            _buffer = buffer;
        }

        public IEnumerable<ITagSpan<ICodeLensTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            var tags = new List<ITagSpan<ICodeLensTag>>();
            var snapshot = _buffer.CurrentSnapshot;
            for (int i = 0; i < snapshot.LineCount; i++)
            {
                ITextSnapshotLine line = snapshot.GetLineFromLineNumber(i);
                if (!spans.Contains(line.Extent))
                {
                    continue;
                }

                SnapshotSpan lineTrimmedExtent = line.GetExtentTrimmed();
                ICodeLensDescriptor descriptor = GetDescriptorForLine(line, lineTrimmedExtent);
                if (descriptor != null)
                {
                    var tag = new CodeLensTag(descriptor);
                    var tagSpan = new TagSpan<ICodeLensTag>(lineTrimmedExtent, tag);
                    tags.Add(tagSpan);
                }
            }
            return tags;
        }

        private ICodeLensDescriptor GetDescriptorForLine(ITextSnapshotLine line, SnapshotSpan tExtent)
        {
            tExtent = line.GetExtentTrimmed();
            string lineText = tExtent.GetText();
            if (lineText.Contains("#include"))
            {
                return new TestCodeLensDescriptor(lineText);
            }
            else
            {
                if (IsLineFunction(lineText))
                {
                    return new DocumentDescriptor(_filePath, default(Guid));
                }
            }
            return null;
        }

        private bool IsLineFunction(string lineText)
        {
            int commentIndex = lineText.IndexOf("//");
            if (commentIndex != -1)
            {
                lineText = lineText.Remove(commentIndex);
            }

            if (!lineText.Contains("(") ||
                !lineText.Contains(")"))
            {
                return false;
            }

            string[] tokens = lineText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                switch (token.ToLower()) {
                case "if":
                case "while":
                case "do":
                case "for":
                    return false;
                }
            }

            return true;
        }
    }
}
