using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;

namespace PhotoBooth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += delegate
            {
                MainFrame.Navigate(new Photobooth());
            };
        }

        private void btnBooth_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Photobooth());
        }

        private void btnTemplateSettings_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TemplateSettings());
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
           
            pd.ShowDialog();
        }
    }
}
