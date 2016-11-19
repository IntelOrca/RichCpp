using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Language.Intellisense;

namespace RichCpp.IntelliSense.Suggestions
{
    internal abstract class SuggestedActionBase : ISuggestedAction
    {
        public abstract string DisplayText { get; }
        public abstract ImageMoniker IconMoniker { get; }

        public virtual bool HasActionSets => false;
        public virtual bool HasPreview => false;
        public string IconAutomationText => DisplayText;
        public string InputGestureText => null;

        public void Dispose()
        {
        }

        public abstract void Invoke(CancellationToken cancellationToken);

        public Task<IEnumerable<SuggestedActionSet>> GetActionSetsAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public Task<object> GetPreviewAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            telemetryId = default(Guid);
            return false;
        }
    }
}
