using AForge.Video.DirectShow;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace PhotoBooth
{
    /// <summary>
    /// Interaction logic for Photobooth.xaml
    /// </summary>
    public partial class Photobooth : Page
    {
        public PhotoCollection Sessions;

        public Photobooth()
        {
            InitializeComponent();

            Sessions = (PhotoCollection)(App.Current.Resources["Photos"] as ObjectDataProvider).Data;
            Sessions.Path = Environment.CurrentDirectory + "\\images";

            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count != 0)
            {
                cbDevices.Items.Clear();

                var devItr = videoDevices.GetEnumerator();
                while (devItr.MoveNext())
                {
                    var current = (FilterInfo)devItr.Current;
                    var item = new ComboBoxItem();
                    item.Content = current.Name;
                    item.Resources.Add("moniker", current.MonikerString);
                    cbDevices.Items.Add(item);
                }

                ((ComboBoxItem)cbDevices.Items[0]).IsSelected = true;
            }
        }

        private void OnPhotoClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var photo = PhotosListBox.SelectedItem as Photo;
            NavigationService.Navigate(new BrowsePhotos(photo));
        }

        private void btnBoothStart_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var cbItem = (ComboBoxItem)cbDevices.SelectedItem;
            NavigationService.Navigate(new LivePreview((string)cbItem.Resources["moniker"]));
        }
    }
}
