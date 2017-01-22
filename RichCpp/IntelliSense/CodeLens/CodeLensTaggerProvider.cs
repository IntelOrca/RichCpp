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
        [Import]
        private ITextDocumentFactoryService _textDocumentFactoryService = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            string path = GetFilePath(buffer);
            return new CodeLensTagger(path, buffer) as ITagger<T>;
        }

        private string GetFilePath(ITextBuffer buffer)
        {
            ITextDocument textDocument;
            if (_textDocumentFactoryService.TryGetTextDocument(buffer, out textDocument))
            {
                return textDocument.FilePath;
            }
            return null;
        }
    }
}
