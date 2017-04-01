using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace RichCpp.IntelliSense.BlockContext
{
    internal class BraceBlockContextSource : IBlockContextSource
    {
        private readonly ITextBuffer _textBuffer;

        public BraceBlockContextSource(ITextBuffer textBuffer)
        {
            _textBuffer = textBuffer;
        }

        public void Dispose()
        {
        }

        public Task<IBlockContext> GetBlockContextAsync(IBlockTag blockTag, ITextView view, CancellationToken token)
        {
            var contentLines = new List<string>();
            IBlockTag tag = blockTag;
            do
            {
                string indent = new string(' ', tag.Level * 4);
                string line = indent + GetStatement(tag);
                contentLines.Add(line);
            }
            while ((tag = tag.Parent) != null);
            contentLines.Reverse();

            string content = string.Join(Environment.NewLine, contentLines);
            var context = new BraceBlockContext(view, blockTag, content);
            return Task.FromResult<IBlockContext>(context);
        }

        private static string GetStatement(IBlockTag blockTag)
        {
            string statement = string.Empty;
            var snapshot = blockTag.Span.Snapshot;
            var braceLine = snapshot.GetLineFromPosition(blockTag.Span.Start);
            int braceOffset = blockTag.Span.Start - braceLine.Start;
            string textBeforeBrace = braceLine.GetText().Substring(0, braceOffset).Trim();
            if (textBeforeBrace.Length > 0)
            {
                statement = textBeforeBrace;
            }
            else if (braceLine.LineNumber > 0)
            {
                var lastLine = snapshot.GetLineFromLineNumber(braceLine.LineNumber - 1);
                statement = lastLine.GetText().Trim();
            }
            return statement;
        }
    }
}
