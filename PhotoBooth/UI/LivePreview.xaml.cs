using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PhotoBooth
{
    /// <summary>
    /// Interaction logic for LivePreview.xaml
    /// </summary>
    public partial class LivePreview : Page
    {
        int timerTicks = 0;
        BitmapFrame lastFrame;
        int snapshotIndex = -1;
        VideoCaptureDevice videoSource;
        DispatcherTimer dispatcherTimer;
        List<Photo> snapshots = new List<Photo>();
        SessionTemplateCollection photoCollection;

        public LivePreview(string deviceMoniker)
        {
            InitializeComponent();
            photoCollection = (SessionTemplateCollection)(Resources["SessionTemplatePhotos"] as ObjectDataProvider).Data;
            //TODO: Update collection count property based on the selected session template
            //photoCollection.PhotoCount = Get Current Session Template placeholders count.
            // enumerate video devices

            photoCollection.PhotoCount = Settings.CurrentTemplate.Poses.Count;

            // create video source from selected device
            videoSource = new VideoCaptureDevice(deviceMoniker);

            // set NewFrame event handler
            videoSource.NewFrame += delegate (object source, NewFrameEventArgs args)
            {
                try
                {
                    args.Frame.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    var img = (Bitmap)args.Frame.Clone();

                    Dispatcher.BeginInvoke(new ThreadStart(delegate
                    {
                        lastFrame = BitmapFrame.Create(ToBitmapImage(img)); ;
                        frameHolder.Source = lastFrame;
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };

            videoSource.SnapshotFrame += delegate (object source, NewFrameEventArgs args)
            {
                args.Frame.RotateFlip(RotateFlipType.RotateNoneFlipX);
                var img = (Bitmap)args.Frame.Clone();
                lastFrame = BitmapFrame.Create(ToBitmapImage(img));
                UpdateTemplateStackData(lastFrame);
            };

            try
            {
                // start the video source
                videoSource.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            App.Current.Exit += delegate
            {
                videoSource?.SignalToStop();

                dispatcherTimer?.Stop();
            };

            Loaded += delegate
            {
                NavigationService.Navigated += delegate
                {
                    videoSource?.SignalToStop();

                    dispatcherTimer?.Stop();
                };
            };

            if (!string.IsNullOrWhiteSpace(deviceMoniker))
            {
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 500);
                dispatcherTimer.Start();
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player;
            
            if(timerTicks < 5)
            {
                player = new System.Media.SoundPlayer(@"Resources\count_blip.wav");
            }
            else
            {
                player = new System.Media.SoundPlayer(@"Resources\camera_shutter.wav");


                if (videoSource.IsRunning)
                {
                    if (videoSource.ProvideSnapshots)
                    {
                        videoSource.SimulateTrigger();
                    }
                    else
                    {
                        UpdateTemplateStackData(lastFrame);
                    }
                    snapshotIndex++;
                }
            }
            
            player.Play();
            timerTicks++;
            if (timerTicks == 5 + photoCollection.Count)
            {
                Task.Delay(1000).ContinueWith(delegate {
                    ((DispatcherTimer)sender).Stop();

                    Dispatcher.BeginInvoke(new Action(delegate {
                        PhotoSessionComplete();
                    }));
                });
            }
        }

        private void PhotoSessionComplete()
        {
            //TODO: transition to photo effects page
            NavigationService.Navigate(new SharePhotos(photoCollection));
        }

        private void UpdateTemplateStackData(BitmapFrame frame)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                (lvSnapshots.Items[snapshotIndex] as Photo).Image = lastFrame;
            }));
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            videoSource.SignalToStop();
            base.OnLostFocus(e);
        }

        private BitmapImage ToBitmapImage(Bitmap image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            bi.Freeze();
            return bi;
        }
    }
}
