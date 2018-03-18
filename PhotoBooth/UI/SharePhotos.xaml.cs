using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace PhotoBooth
{
    /// <summary>
    /// Interaction logic for SharePhotos.xaml
    /// </summary>
    public partial class SharePhotos : Page
    {
        MemoryStream gifStream;
        SessionTemplate _template;
        TemplatePreview _templatePreview;
        SessionTemplateCollection _sessionPhotos;
        
        public SharePhotos(SessionTemplateCollection sessionPhotos)
        {
            InitializeComponent();
            _sessionPhotos = sessionPhotos;

            _template = Settings.CurrentTemplate;

            _templatePreview = new TemplatePreview(_template, sessionPhotos);
            frPreview.Navigate(_templatePreview);
            
            var brush = new ImageBrush();
            var sMgr = new SessionManager();
            gifStream = sMgr.GenerateGifStream(sessionPhotos);
            brush.ImageSource = sMgr.ImageFromStream(gifStream);

            btnPreviewGif.Background = brush;
            
            var templateBrush = new ImageBrush();
            templateBrush.ImageSource = _template.RenderToBitmap();

            btnPreviewTemplate.Background = templateBrush;
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            var sMgr = new SessionManager();
            
            sMgr.CreateSessionFiles(_sessionPhotos);

            NavigationService.Navigate(new Photobooth());
        }

        private void btnPreviewGif_Click(object sender, RoutedEventArgs e)
        {
            frPreview.Navigate(new PreviewGif(gifStream));
        }

        private void btnPreviewTemplate_Click(object sender, RoutedEventArgs e)
        {
            frPreview.Navigate(_templatePreview);
        }
    }
}
