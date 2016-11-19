using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using RichCpp.Language;

namespace RichCpp.IntelliSense.QuickInfo
{
    /// <summary>
    /// A QuickInfo source that hijacks previous sources and colourises it.
    /// </summary>
    internal class RichCppQuickInfoSource : IQuickInfoSource
    {
        private readonly ITextStructureNavigatorSelectorService _navigatorService;
        private readonly ITextBuffer _subjectBuffer;

        public RichCppQuickInfoSource(ITextStructureNavigatorSelectorService navigatorService,
                                      ITextBuffer subjectBuffer)
        {
            _navigatorService = navigatorService;
            _subjectBuffer = subjectBuffer;
        }

        public void Dispose()
        {
        }

        public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> quickInfoContent, out ITrackingSpan applicableToSpan)
        {
            // Map the trigger point down to our buffer
            ITextSnapshot snapshot = _subjectBuffer.CurrentSnapshot;
            SnapshotPoint? subjectTriggerPoint = session.GetTriggerPoint(snapshot);
            if (!subjectTriggerPoint.HasValue)
            {
                applicableToSpan = null;
                return;
            }

            ITextStructureNavigator navigator = _navigatorService.GetTextStructureNavigator(_subjectBuffer);
            TextExtent extent = navigator.GetExtentOfWord(subjectTriggerPoint.Value);
            string extentText = extent.Span.GetText();

            // What we are effectively doing here is placing this QuickInfo source at the end so that it
            // can hijack the other previous QuickInfo sources. We replace them with colourised versions.

            applicableToSpan = null;
            if (quickInfoContent.Count > 0)
            {
                ITextBuffer cppQuickInfoContentBuffer = quickInfoContent[0] as ITextBuffer;
                string cppQuickInfoText = cppQuickInfoContentBuffer.CurrentSnapshot.GetText();
                TextBlock newContent = CreateColourisedContent(cppQuickInfoText);

                quickInfoContent.RemoveAt(0);
                quickInfoContent.Add(newContent);

                applicableToSpan = snapshot.CreateTrackingSpan(extent.Span.Start, extent.Span.Length, SpanTrackingMode.EdgeInclusive);
            }
        }

        private ColourisedQuickInfoContent CreateColourisedContent(string content)
        {
            // TODO Investigate using C++'s colouriser instead of tokenising the content
            //      ourselves. We might then be able to colour types and identifiers.

            var lexer = new CppLexer();
            CppToken[] tokens = lexer.GetTokens(content);
            return new ColourisedQuickInfoContent(tokens);
        }
    }
}
