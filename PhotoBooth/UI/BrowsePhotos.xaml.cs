using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace PhotoBooth
{
    /// <summary>
    /// Interaction logic for BrowsePhotos.xaml
    /// </summary>
    public partial class BrowsePhotos : Page
    {
        public PhotoCollection Photos;
        private Photo _sessionTemplate;

        public BrowsePhotos()
        {
            InitializeComponent();
        }

        public BrowsePhotos(Photo sessionTemplate)
        {
            _sessionTemplate = sessionTemplate;
            InitializeComponent();

            Photos = (PhotoCollection)(App.Current.Resources["Photos"] as ObjectDataProvider).Data;
            var segments = sessionTemplate.Source.Split('\\').ToList();
            segments.Remove(segments.Last());
            Photos.Path = string.Join("\\", segments);
        }
    }
}
