using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PhotoBooth
{
    /// <summary>
    /// Interaction logic for PreviewGif.xaml
    /// </summary>
    public partial class PreviewGif : Page
    {
        public PreviewGif(Stream gifStream)
        {
            InitializeComponent();

            gifStream.Seek(0, SeekOrigin.Begin);

            var decoder = GifBitmapDecoder.Create(gifStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            var frameIdx = 0;


            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += delegate {
                Dispatcher.BeginInvoke(new Action(delegate {
                    imgFrameHolder.Source = decoder.Frames[frameIdx];
                    frameIdx++;
                    if (frameIdx == decoder.Frames.Count)
                    {
                        frameIdx = 0;
                    }
                }));
            };
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            dispatcherTimer.Start(); 
        }
    }
}
