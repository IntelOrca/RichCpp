using System;
using Microsoft.VisualStudio.Language.Intellisense;

namespace RichCpp.IntelliSense.CodeLens
{
    internal class CodeLensTag : ICodeLensTag
    {
#pragma warning disable 67
        public event EventHandler Disconnected;
#pragma warning restore 67

        public ICodeLensDescriptor Descriptor { get; }

        public CodeLensTag(ICodeLensDescriptor descriptor)
        {
            Descriptor = descriptor;
        }
    }
}
