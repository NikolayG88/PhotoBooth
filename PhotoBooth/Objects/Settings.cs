using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Windows;

namespace PhotoBooth
{
    public class Settings
    {
        // private static SessionTemplate _currentTemplate = null;
        public static SessionTemplate CurrentTemplate
        {
            get
            {
                try
                {
                    //Load the current template from settings
                    return new SessionTemplate(LastSelectedTemplate.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                return null;
            }
            set
            {
                try
                {
                    LastSelectedTemplate.RemoveAll();
                    LastSelectedTemplate.Add(value.TemplateName + ".xaml");
                    SettingsDocument.Save("settings.xml");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static void InitSettings()
        {
            try
            {
                var root = new XElement("settings");

                root.Add(new XElement("layout_template", "DefaultTemplate.xaml"));

                _document.Add(root);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private static XDocument _document = null;
        private static XDocument SettingsDocument
        {
            get
            {
                if (_document == null)
                {
                    try
                    {
                        _document = XDocument.Load(File.OpenRead("settings"));
                    }
                    catch
                    {
                    }

                    if (_document == null)
                    {
                        _document = new XDocument();
                    }
                }

                return _document;
            }
        }

        //private static XElement _root = null;
        private static XElement SettingsRoot
        {
            get
            {
                XElement root = null;
                try
                {
                    if (SettingsDocument.Root != null)
                    {
                        root = SettingsDocument.Root;
                    }
                    else
                    {
                        root = new XElement("settigs");
                        SettingsDocument.Add(root);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                return root;
            }
        }

        /// <summary>
        /// Returns the last saved template
        /// </summary>
        private static XElement LastSelectedTemplate
        {
            get
            {
                XElement xName = null;
                try
                {
                    xName = SettingsRoot.Elements().FirstOrDefault(elm => elm.Name.ToString().ToLower() == "layout_template");

                    if (xName == null)
                    {
                        xName = new XElement("layout_template", "DefaultTemplate.xaml");
                        SettingsRoot.Add(xName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                return xName;
            }
        }

        public static void SaveSettings()
        {
            SettingsDocument.Save(File.Create("settings"));
        }
    }
}
