using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PhotoBooth
{
    /// <summary>
    /// Interaction logic for TemplateSettings.xaml
    /// </summary>
    public partial class TemplateSettings : Page
    {
        SessionTemplate template;
        LayoutTemplateManager layoutManager;

        public TemplateSettings()
        {
            InitializeComponent();

            try
            {
                KeyDown += TemplateSettings_KeyDown;

                template = Settings.CurrentTemplate;

                layoutManager = new LayoutTemplateManager();
                layoutManager.RegisterTemplate(template);

                template.AttachToControl(svPageContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Loaded += delegate
            {
                NavigationService.Navigating += NavigationService_Navigating;
            };
        }

        private void NavigationService_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (!(e.Content is TemplateSettings))
            {
                layoutManager.UnregisterTemplate();
            }
        }

        private void TemplateSettings_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    layoutManager.DeleteSelected();
                    break;
            }
        }

        private void LayoutPanel_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void btnPholder_Click(object sender, RoutedEventArgs e)
        {
            template.AddPhotoPose();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            template.SaveXAML();
        }

        private void btnBgrImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

                if (ofd.ShowDialog() ?? false)
                {
                    template.SetBackgroundImage(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddImg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

                if (ofd.ShowDialog() ?? false)
                {
                    template.AddImage(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            var previewWindow = new Window();
            previewWindow.Padding = new Thickness(30);

            var image = new Image();
            image.Source = template.RenderToBitmap();
            image.Stretch = System.Windows.Media.Stretch.Uniform;

            image.Width = previewWindow.Width;
            image.Height = previewWindow.Height;

            previewWindow.Content = image;

            previewWindow.Show();
        }
    }
}
