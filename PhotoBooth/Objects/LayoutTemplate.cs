using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media.Imaging;


namespace PhotoBooth
{
    /// <summary>
    /// A WPF Framework element wich exposes manipulation services to its child elements.
    /// </summary>
    public class LayoutTemplate
    {
        public Canvas LayoutPanel;
        public string TemplateName;
        protected DependencyObject rootObject;
        public EventHandler ChildElementAdded;
        public EventHandler ChildElementRemoved;
        protected string templatesPath = "LayoutTemplates";

        public LayoutTemplate(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                MessageBox.Show("Template name should not be empty !");

                return;
            }
            
            if (!Directory.Exists(templatesPath))
            {
                Directory.CreateDirectory(templatesPath);
            }

            TemplateName = templateName.Split('.')[0];

            var templates = Directory.GetFiles(templatesPath, "*.xaml");

            //Check if template exists
            string path = "";
            if (!string.IsNullOrWhiteSpace(path = templates.FirstOrDefault(tmp => tmp.Split('\\').Last().ToLower() == templateName.ToLower())))
            {
                //Load the template xaml
                LoadXAML(path);
            }
            else//if not it's a new template
            {
                //Create new layout panel
                LayoutPanel = new Canvas();
                LayoutPanel.Name = "LayoutPanel";
                LayoutPanel.VerticalAlignment = VerticalAlignment.Stretch;
                LayoutPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                LayoutPanel.Background = new SolidColorBrush(Colors.White);
                
                rootObject = LayoutPanel;
            }
        }
        
        public void AttachToControl(ContentControl control)
        {
            try
            {
                control.Content = rootObject;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        protected void LoadXAML(string templatePath)
        {
            try
            {
                StreamReader mysr = new StreamReader(templatePath);
                rootObject = XamlReader.Load(mysr.BaseStream) as DependencyObject;
                LayoutPanel = rootObject as Canvas;
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveXAML()
        {
            try
            {
                var fs = new FileStream(Path.Combine(templatesPath, TemplateName + ".xaml"), FileMode.Create);

                var sw = new StreamWriter(fs);

                var result = XamlWriter.Save(LayoutPanel);
                sw.Write(result);

                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
