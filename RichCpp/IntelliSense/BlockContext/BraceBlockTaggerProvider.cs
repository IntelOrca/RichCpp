using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace RichCpp.IntelliSense.BlockContext
{
    [Export(typeof(ITaggerProvider))]
    [Name("RichCpp Brace Block Tagger")]
    [ContentType(ContentTypes.CPlusPlus)]
    [TagType(typeof(IBlockTag))]
    internal class BraceBlockTaggerProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            var tagger = new BraceBlockTagger(buffer) as ITagger<T>;
            return tagger;
        }
    }
}
