using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace RichCpp.IntelliSense.Suggestions
{
    internal class RichCppSuggestedActionsSource : ISuggestedActionsSource
    {
        private readonly ITextView _textView;
        private readonly ITextBuffer _textBuffer;

#pragma warning disable 67
        public event EventHandler<EventArgs> SuggestedActionsChanged;
#pragma warning restore 67

        public RichCppSuggestedActionsSource(ITextView textView, ITextBuffer textBuffer)
        {
            _textView = textView;
            _textBuffer = textBuffer;
        }

        public void Dispose()
        {
        }

        public async Task<bool> HasSuggestedActionsAsync(ISuggestedActionCategorySet requestedActionCategories,
                                                         SnapshotSpan range,
                                                         CancellationToken ct)
        {
            if (await SortIncludesSuggestedAction.CheckAvailabilityAsync(range, ct))
            {
                return true;
            }
            return false;
        }

        public IEnumerable<SuggestedActionSet> GetSuggestedActions(ISuggestedActionCategorySet requestedActionCategories,
                                                                   SnapshotSpan range,
                                                                   CancellationToken ct)
        {
            var suggestedActions = new List<ISuggestedAction>();
            if (SortIncludesSuggestedAction.CheckAvailabilityAsync(range, ct).Result)
            {
                suggestedActions.Add(new SortIncludesSuggestedAction(_textView, _textBuffer, range.Start));
            }
            yield return new SuggestedActionSet(suggestedActions);
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            telemetryId = default(Guid);
            return false;
        }
    }
}
