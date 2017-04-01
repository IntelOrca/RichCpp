using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace RichCpp.IntelliSense.BlockContext
{
    internal class BraceBlockContext : IBlockContext
    {
        public ITextView TextView { get; }
        public IBlockTag BlockTag { get; }
        public object Content { get; }

        public BraceBlockContext(ITextView textView, IBlockTag blockTag, object content)
        {
            TextView = textView;
            BlockTag = blockTag;
            Content = content;
        }
    }
}
