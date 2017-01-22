using System;
using Microsoft.VisualStudio.Language.Intellisense;

namespace RichCpp.IntelliSense.CodeLens
{
    internal class CodeLensTag : ICodeLensTag
    {
        private readonly TestCodeLensDescriptor _descriptor;

#pragma warning disable 67
        public event EventHandler Disconnected;
#pragma warning restore 67

        public ICodeLensDescriptor Descriptor => _descriptor;

        public CodeLensTag(TestCodeLensDescriptor descriptor)
        {
            _descriptor = descriptor;
        }
    }
}
