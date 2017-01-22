using Microsoft.VisualStudio.Language.Intellisense;

namespace RichCpp.IntelliSense.CodeLens
{
    internal class TestCodeLensDescriptor : ICodeLensDescriptor
    {
        public string Path { get; }
        public string ElementDescription => Path;

        public TestCodeLensDescriptor(string line)
        {
            string path = GetPath(line, "\"", "\"");
            if (path == null)
            {
                path = GetPath(line, "<", ">");
            }
            Path = path;
        }

        private string GetPath(string line, string startDelimiter, string endDelimiter)
        {
            int startIndex = line.IndexOf(startDelimiter);
            if (startIndex != -1)
            {
                int endIndex = line.LastIndexOf(endDelimiter);
                if (endIndex != -1)
                {
                    string filename = line.Substring(startIndex + 1, endIndex - startIndex - 1);
                    return filename;
                }
            }
            return null;
        }
    }
}
