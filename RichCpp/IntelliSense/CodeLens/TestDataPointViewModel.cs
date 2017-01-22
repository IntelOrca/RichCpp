using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.CodeSense.Editor;

namespace RichCpp.IntelliSense.CodeLens
{
    internal class TestDataPointViewModel : GlyphDataPointViewModel
    {
        private readonly TestDataPoint _dataPoint;
        private BitmapImage _bitmap;

        public TestDataPointViewModel(TestDataPoint dataPoint)
            : base(dataPoint)
        {
            _dataPoint = dataPoint;

            HasDetails = true;
            Descriptor = dataPoint.Descriptor.Path;
        }

        public override ImageSource GlyphSource
        {
            get
            {
                if (_bitmap == null)
                {
                    _bitmap = new BitmapImage(new Uri(@"C:\Users\Ted\Pictures\intelorca.png"));
                }
                return _bitmap;
            }
        }
    }
}
