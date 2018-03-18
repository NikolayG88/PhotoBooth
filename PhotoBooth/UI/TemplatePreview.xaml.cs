using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoBooth
{
    /// <summary>
    /// Interaction logic for TemplatePreview.xaml
    /// </summary>
    public partial class TemplatePreview : Page
    {
        SessionTemplate _template;

        public TemplatePreview(SessionTemplate template, SessionTemplateCollection sessionPhotos)
        {
            InitializeComponent();
            _template = template;
            template.AttachToControl(frPreview);

            var i = 0;
            foreach(Canvas pose in template.Poses)
            {
                var image = new Image();
                image.Width = pose.Width;
                image.Height = pose.Height;
                image.Source = sessionPhotos[i].Image;
                image.Stretch = Stretch.Uniform;
                pose.Children.Add(image);
                pose.Background = new SolidColorBrush(Colors.Transparent);

                i++;
            }
        }
    }
}
