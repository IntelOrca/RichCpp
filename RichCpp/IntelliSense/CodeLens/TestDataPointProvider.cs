using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Utilities;

namespace RichCpp.IntelliSense.CodeLens
{
    [Export(typeof(ICodeLensDataPointProvider))]
    [Name("Test")]
    internal class TestDataPointProvider : ICodeLensDataPointProvider
    {
        public bool CanCreateDataPoint(ICodeLensDescriptor descriptor)
        {
            return (descriptor is TestCodeLensDescriptor);
        }

        public ICodeLensDataPoint CreateDataPoint(ICodeLensDescriptor descriptor)
        {
            var desc = descriptor as TestCodeLensDescriptor;
            return new TestDataPoint(desc);
        }
    }
}
