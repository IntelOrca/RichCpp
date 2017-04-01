using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Utilities;

namespace RichCpp.IntelliSense.BlockContext
{
    [Export(typeof(IBlockContextProvider))]
    [Name("RichCpp BlockContext Provider")]
    [ContentType(ContentTypes.CPlusPlus)]
    internal class BraceBlockContextProvider : IBlockContextProvider
    {
        public Task<IBlockContextSource> TryCreateBlockContextSourceAsync(ITextBuffer textBuffer, CancellationToken token)
        {
            var source = new BraceBlockContextSource(textBuffer);
            return Task.FromResult<IBlockContextSource>(source);
        }
    }
}
