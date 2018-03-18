using System;
using System.IO;
using System.Windows;

namespace PhotoBooth
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        void OnApplicationStartup(object sender, StartupEventArgs args)
        {
            if (DateTime.Now > DateTime.ParseExact("31.12.2017", "dd.MM.yyyy", null))
            {
                MessageBox.Show("Trial period ended !");
                Current.Shutdown();
            }

            //Create images directory to store sessions in it
            if (!Directory.Exists("Images"))
            {
                Directory.CreateDirectory("Images");
            }
        }
    }
}
