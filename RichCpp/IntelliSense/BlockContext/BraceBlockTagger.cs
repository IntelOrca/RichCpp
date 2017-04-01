using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace RichCpp.IntelliSense.BlockContext
{
    internal class BraceBlockTagger : ITagger<IBlockTag>
    {
        private readonly ITextBuffer _textBuffer;

        private readonly object _cacheSync = new object();
        private ITagSpan<IBlockTag>[] _cachedTags;
        private int _cacheSnapshotVersion;

#pragma warning disable 67
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore 67

        public BraceBlockTagger(ITextBuffer textBuffer)
        {
            _textBuffer = textBuffer;
        }

        public IEnumerable<ITagSpan<IBlockTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            var snapshot = spans[0].Snapshot;
            int snapshotVersion = snapshot.Version.VersionNumber;
            if (snapshotVersion >= _cacheSnapshotVersion)
            {
                ITagSpan<IBlockTag>[] tags;
                lock (_cacheSync)
                {
                    if (snapshotVersion > _cacheSnapshotVersion)
                    {
                        _cachedTags = GetTags(snapshot);
                        _cacheSnapshotVersion = snapshotVersion;
                    }
                    tags = _cachedTags;
                }
                var filteredTags = tags.Where(x => spans.IntersectsWith(x.Span));
                return filteredTags;
            }
            else
            {
                return Enumerable.Empty<ITagSpan<IBlockTag>>();
            }
        }

        private ITagSpan<IBlockTag>[] GetTags(ITextSnapshot snapshot)
        {
            var extentSpan = new SnapshotSpan(snapshot, 0, snapshot.Length);
            var tags = GetTags(null, extentSpan).ToArray();
            return tags;
        }

        private IEnumerable<ITagSpan<IBlockTag>> GetTags(IBlockTag parent, SnapshotSpan span)
        {
            string text = span.GetText();
            int firstBraceIndex = -1;
            int firstBraceLine = -1;
            int level = 0;
            int depth = 0;
            int lineIndex = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == '\n')
                {
                    lineIndex++;
                }
                else if (c == '{')
                {
                    if (level == 0)
                    {
                        firstBraceIndex = i;
                        firstBraceLine = lineIndex;
                    }
                    level++;
                    depth++;
                }
                else if (c == '}')
                {
                    level--;
                    if (level == 0 && firstBraceLine != lineIndex)
                    {
                        int length = i - firstBraceIndex + 1;
                        var blockSpan = new SnapshotSpan(span.Snapshot, span.Start + firstBraceIndex, length);
                        var blockTag = new BraceBlockTag(blockSpan, blockSpan, parent);
                        yield return new TagSpan<IBlockTag>(blockSpan, blockTag);
                        if (depth > 1)
                        {
                            var innerBlockSpan = new SnapshotSpan(span.Snapshot, blockSpan.Start + 1, blockSpan.Length - 2);
                            var innerTags = GetTags(blockTag, innerBlockSpan).ToArray();
                            foreach (var tag in innerTags)
                            {
                                yield return tag;
                            }
                        }
                        depth = 0;
                    }
                }
            }
        }
    }
}
