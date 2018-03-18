using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace PhotoBooth
{
    public class LayoutTemplateManager
    {
        Canvas _canvas;
        bool mouseDown = false;
        SessionTemplate _template;
        List<LayoutElement> elements = new List<LayoutElement>();

        private UIElement _selected;
        public UIElement Selected
        {
            get
            {
                return _selected;
            }
        }

        public LayoutElement SelectedLayoutElement
        {
            get
            {
                if (Selected != null)
                {
                    return elements.First(elm => elm.Name == (Selected as FrameworkElement).Name);
                }

                return null;
            }
        }

        private UIElement _target;
        public LayoutElement TargetLayoutElement
        {
            get
            {
                return elements.First(elm => elm.Name == (_target as FrameworkElement).Name);
            }
        }

        public void DeleteSelected()
        {
            SelectedLayoutElement.RestartSelectionState();
            elements.Remove(SelectedLayoutElement);
            _canvas.Children.Remove(Selected);
            _selected = null;
        }

        public LayoutTemplateManager()
        {

        }

        public void RegisterTemplate(SessionTemplate template)
        {
            _canvas = template.LayoutPanel;
            _template = template;

            foreach (var item in _canvas.Children)
            {
                elements.Add(new LayoutElement(item as UIElement, _canvas));
            }

            try
            {
                _canvas.MouseEnter += _canvas_MouseEnter;
                _canvas.MouseDown += _canvas_MouseDown;
                _canvas.MouseUp += _canvas_MouseUp;
                _canvas.MouseMove += _canvas_MouseMove;
                _canvas.MouseLeave += _canvas_MouseLeave;
                _canvas.MouseWheel += _canvas_MouseWheel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            _template.ChildElementAdded += delegate (object sender, EventArgs a)
            {
                elements.Add(new LayoutElement(sender as UIElement, _canvas));
            };
        }

        public void UnregisterTemplate()
        {
            try
            {
                _canvas.MouseEnter -= _canvas_MouseEnter;
                _canvas.MouseDown -= _canvas_MouseDown;
                _canvas.MouseUp -= _canvas_MouseUp;
                _canvas.MouseMove -= _canvas_MouseMove;
                _canvas.MouseLeave -= _canvas_MouseLeave;
                _canvas.MouseWheel -= _canvas_MouseWheel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void _canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Selected != null)
            {
                SelectedLayoutElement.Rotate(e.Delta > 0 ? 10 : -10);
            }
        }

        private void _canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (SelectedLayoutElement != null)
            {
                SelectedLayoutElement.RestartSelectionState();
            }
        }

        private void _canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //Handle template mouse hover event
            if (!mouseDown)
            {
                var result = _canvas.InputHitTest(e.GetPosition(_canvas));

                if (result is UIElement)
                {
                    _target = result as UIElement;
                    if (_template.IsEditable(_target))
                    {
                        Mouse.OverrideCursor = TargetLayoutElement.HoverCursor(e.GetPosition(result));
                    }
                    else
                    {
                        Mouse.OverrideCursor = Cursors.Arrow;
                    }
                }
            }
            else if (mouseDown && Selected != null)
            {
                SelectedLayoutElement.HandleMouseInteraction(e);
            }
        }

        private void _canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
            if (SelectedLayoutElement != null)
            {
                SelectedLayoutElement.RestartSelectionState();
            }
        }

        private void _canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseDown = true;
            var result = _canvas.InputHitTest(e.GetPosition(_canvas));

            Deselect();

            if (result is UIElement)
            {
                SelectElement(result as UIElement);
            }
        }

        private void _canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the passed element is the currently selected one.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool IsSelected(UIElement element)
        {
            return (element as FrameworkElement).Name == (Selected as FrameworkElement).Name;
        }

        public void SelectElement(UIElement element)
        {
            try
            {
                if (_template.IsEditable(element))
                {
                    _selected = element;

                    var adornerLayer = AdornerLayer.GetAdornerLayer(_selected);
                    var adorners = adornerLayer.GetAdorners(_selected);

                    if (adorners == null)
                    {
                        adornerLayer.Add(new SimpleCircleAdorner(_selected));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Deselects the currently selected element.
        /// </summary>
        public void Deselect()
        {
            try
            {
                if (Selected != null && _template.IsEditable(Selected))
                {
                    var canvas = Selected as Canvas;

                    var adLayer = AdornerLayer.GetAdornerLayer(_selected);
                    var adorners = adLayer.GetAdorners(_selected);

                    if (adorners != null)
                    {
                        adLayer.Remove(adorners.First());
                    }

                    _selected = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Adorners must subclass the abstract base class Adorner.
        public class SimpleCircleAdorner : Adorner
        {
            // Be sure to call the base class constructor.
            public SimpleCircleAdorner(UIElement adornedElement)
              : base(adornedElement)
            {
            }

            // A common way to implement an adorner's rendering behavior is to override the OnRender
            // method, which is called by the layout system as part of a rendering pass.
            protected override void OnRender(DrawingContext drawingContext)
            {
                Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

                // Some arbitrary drawing implements.
                SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
                renderBrush.Opacity = 0.2;
                Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
                double renderRadius = 5.0;

                // Draw a circle at each corner.
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
                drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
            }
        }
    }
}
