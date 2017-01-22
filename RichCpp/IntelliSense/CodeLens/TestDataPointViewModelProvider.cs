using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.CodeSense.Editor;
using Microsoft.VisualStudio.Language.Intellisense;

namespace RichCpp.IntelliSense.CodeLens
{
    [DataPointViewModelProvider(typeof(TestDataPoint))]
    internal sealed class TestDataPointViewModelProvider : GlyphDataPointViewModelProvider<TestDataPointViewModel>
    {
        protected override TestDataPointViewModel GetViewModel(ICodeLensDataPoint dataPoint)
        {
            return new TestDataPointViewModel(dataPoint as TestDataPoint);
        }
    }
}
