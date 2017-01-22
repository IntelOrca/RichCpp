using System.Threading.Tasks;
using Microsoft.VisualStudio.CodeSense;

namespace RichCpp.IntelliSense.CodeLens
{
    internal class TestDataPoint : DataPoint<TestCodeLensDescriptor>
    {
        public TestCodeLensDescriptor Descriptor { get; }

        public TestDataPoint(TestCodeLensDescriptor descriptor)
        {
            Descriptor = descriptor;
        }

        public override Task<TestCodeLensDescriptor> GetDataAsync()
        {
            return Task.FromResult(Descriptor);
        }
    }
}
