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
    public class SessionTemplate : LayoutTemplate
    {
        private PrintSizing _printSizing = new PrintSizing();
        
        public int Scale
        {
            get
            {
                try
                {
                    if (LayoutPanel.Name.Contains("_"))
                    {
                        _printSizing.Scale = int.Parse(LayoutPanel.Name.Split('_')[1].Replace('n', '-'));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                return _printSizing.Scale;
            }
            set
            {
                LayoutPanel.Name = LayoutPanel.Name + "_" + (_printSizing.Scale < 0 ? "n" + _printSizing.Scale.ToString() : _printSizing.Scale.ToString());

                _printSizing.Scale = value;
            }
        }

        private List<UIElement> _poses;
        public List<UIElement> Poses
        {
            get
            {
                if (_poses == null)
                {
                    _poses = new List<UIElement>();

                    _poses.AddRange(getChildren("pose"));
                }

                return _poses;
            }
        }

        private List<UIElement> _images;
        public List<UIElement> Images
        {
            get
            {
                if (_images == null)
                {
                    _images = new List<UIElement>();

                    _images.AddRange(getChildren("img"));
                }

                return _images;
            }
        }

        private List<UIElement> getChildren(string contains)
        {
            List<UIElement> result = new List<UIElement>();
            foreach (UIElement child in LayoutPanel.Children)
            {
                var name = child.GetValue(FrameworkElement.NameProperty) as string;

                if (name.Contains(contains))
                {
                    result.Add(child);
                }
            }

            return result;
        }

        public void SetBackgroundImage(string fullPath)
        {
            var brush = new ImageBrush();

            brush.ImageSource = new BitmapImage(new Uri(fullPath));

            LayoutPanel.Background = brush;
        }
        
        /// <summary>
        /// Adds new pose to the template.
        /// </summary>
        public void AddPhotoPose()
        {
            try
            {
                var canvas = new Canvas();

                canvas.Width = 250;
                canvas.Height = 150;

                canvas.Background = new SolidColorBrush(Colors.Aqua);
                canvas.Name = "pose_" + Poses.Count;

                LayoutPanel.Children.Add(canvas);

                _poses.Add(canvas);
                ChildElementAdded?.Invoke(canvas, new EventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddImage(string fullPath)
        {
            var canvas = new Canvas();

            canvas.Width = 250;
            canvas.Height = 150;

            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(fullPath));
            canvas.Background = brush;

            canvas.Name = "img_" + Images.Count;

            LayoutPanel.Children.Add(canvas);
            _images.Add(canvas);

            ChildElementAdded?.Invoke(canvas, new EventArgs());
        }

        public bool IsEditable(UIElement element)
        {
            try
            {
                var name = (element as FrameworkElement).Name;
                return name.Contains("pose") || name.Contains("img");
            }
            catch
            {
                return false;
            }
        }
        
        public SessionTemplate(string templateName) : base(templateName)
        {
            Scale = -3;
            var size = _printSizing.RenderSizeScaled;
            LayoutPanel.Width = size.Width;
            LayoutPanel.Height = size.Height;
        }

        public BitmapFrame RenderToBitmap()
        {
            try
            {
                LayoutPanel.UpdateLayout();

                return new ImageFactory().RenderVisualToBitmap(LayoutPanel, _printSizing.RenderSizeScaled,
                    _printSizing.SelectedDensityScaled, _printSizing.SelectedDensityScaled);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return null;
        }
    }
}
