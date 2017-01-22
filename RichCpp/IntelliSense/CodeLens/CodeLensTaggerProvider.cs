using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace RichCpp.IntelliSense.CodeLens
{
    [Export(typeof(ITaggerProvider))]
    [Name("C/C++ RichCPP CodeLens")]
    [ContentType(ContentTypes.CPlusPlus)]
    [TagType(typeof(ICodeLensTag))]
    internal class CodeLensTaggerProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new CodeLensTagger(buffer) as ITagger<T>;
        }
    }
}
