using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;

namespace RichCpp.IntelliSense.QuickInfo
{
    [Export(typeof(IQuickInfoSourceProvider))]
    [Name("RichCpp QuickInfo Source")]
    [Order(After = "Default Quick Info Presenter")]
    [ContentType(ContentTypes.CPlusPlus)]
    internal class RichCppQuickInfoSourceProvider : IQuickInfoSourceProvider
    {
        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer)
        {
            return new RichCppQuickInfoSource(NavigatorService, textBuffer);
        }
    }
}
