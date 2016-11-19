using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace RichCpp.IntelliSense.Suggestions
{
    [Export(typeof(ISuggestedActionsSourceProvider))]
    [Name("C/C++ RichCPP Suggestions")]
    [ContentType(ContentTypes.CPlusPlus)]
    internal class RichCppSuggestedActionsSourceProvider : ISuggestedActionsSourceProvider
    {
        public ISuggestedActionsSource CreateSuggestedActionsSource(ITextView textView, ITextBuffer textBuffer)
        {
            return new RichCppSuggestedActionsSource(textView, textBuffer);
        }
    }
}
