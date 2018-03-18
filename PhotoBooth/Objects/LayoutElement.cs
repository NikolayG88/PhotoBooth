using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PhotoBooth
{
    /// <summary>
    /// An element of the LayoutTemplte class.
    /// </summary>
    public class LayoutElement
    {
        Canvas _canvas;
        UIElement _element = null;
        Size startSize = Size.Empty;
        Point dCenter = Utils.SystemPoint.Empty;
        Point localPos = Utils.SystemPoint.Empty;
        Point resizeStart = Utils.SystemPoint.Empty;
        TransformGroup transformGroup = new TransformGroup();
        uint _interactionStateMask = (uint)ElementInteractionState.Drag;

        public string Name
        {
            get
            {
                return (_element as FrameworkElement).Name;
            }
        }

        public uint InteractionStateMask
        {
            get
            {
                return _interactionStateMask;
            }
        }

        public enum ElementInteractionState
        {
            Drag = 0,
            ResizeTop = 2,
            ResizeBot = 4,
            ResizeLeft = 8,
            ResizeRight = 16
        }

        public LayoutElement(UIElement element, Canvas parent)
        {
            _element = element;
            _canvas = parent;
            _element.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        public Cursor HoverCursor(Point localPos)
        {
            var width = (_element as FrameworkElement).ActualWidth;
            var height = (_element as FrameworkElement).ActualHeight;
            var offset = 10;

            bool left = localPos.X < offset;
            bool right = localPos.X > (width - offset);
            bool top = localPos.Y < offset;
            bool bot = localPos.Y > (height - offset);

            Cursor result;

            _interactionStateMask = (uint)ElementInteractionState.Drag;
            result = Cursors.SizeAll;
            if (left || right)
            {
                result = Cursors.SizeWE;
                if (left)
                {
                    _interactionStateMask = _interactionStateMask | (uint)ElementInteractionState.ResizeLeft;
                }
                else if (right)
                {
                    _interactionStateMask = _interactionStateMask | (uint)ElementInteractionState.ResizeRight;
                }
            }

            if (top || bot)
            {
                result = Cursors.SizeNS;

                if (top)
                {
                    _interactionStateMask = _interactionStateMask | (uint)ElementInteractionState.ResizeTop;
                }
                else if (bot)
                {
                    _interactionStateMask = _interactionStateMask | (uint)ElementInteractionState.ResizeBot;
                }
            }

            if ((left && top) || (right && bot))
            {
                result = Cursors.SizeNWSE;
            }
            else if ((left && bot) || (right && top))
            {
                result = Cursors.SizeNESW;
            }

            return result;
        }

        private void Drag(Point pos)
        {
            Point localCenter = new Point((_element as FrameworkElement).Width / 2, (_element as FrameworkElement).Height / 2);
            Point center = _element.TransformToAncestor(_canvas).Transform(localCenter);

            if (Utils.SystemPoint.IsEmpty(dCenter))
            {
                dCenter = new Point(pos.X - center.X, pos.Y - center.Y);
            }

            Canvas.SetLeft(_element, (pos.X - localCenter.X) - dCenter.X);
            Canvas.SetTop(_element, (pos.Y - localCenter.Y) - dCenter.Y);
        }

        private void ResizeLeft(Point pos)
        {
            try
            {
                var dLeft = (pos.X - resizeStart.X);

                (_element as FrameworkElement).Width = startSize.Width + (dLeft * -1);

                Canvas.SetLeft(_element, resizeStart.X + dLeft);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ResizeRight(Point pos)
        {
            var dRight = (pos.X - resizeStart.X);

            (_element as FrameworkElement).Width = startSize.Width + dRight;
        }

        private void ResizeTop(Point pos)
        {
            var dTop = (pos.Y - resizeStart.Y);

            (_element as FrameworkElement).Height = startSize.Height + (dTop * -1);
            Canvas.SetTop(_element, resizeStart.Y + dTop);
        }

        private void ResizeBot(Point pos)
        {
            var dBot = (pos.Y - resizeStart.Y);

            (_element as FrameworkElement).Height = startSize.Height + dBot;
        }

        public void Rotate(double angle)
        {
            if (_element != null)
            {
                transformGroup.Children.Add(new RotateTransform(angle));
                _element.RenderTransform = transformGroup;
                _element.InvalidateVisual();
            }
        }

        public void HandleMouseInteraction(MouseEventArgs e)
        {
            var mousePos = e.GetPosition(_canvas);

            if (Utils.SystemPoint.IsEmpty(localPos))
            {
                localPos = e.GetPosition(_element);
            }

            if (Utils.SystemPoint.IsEmpty(resizeStart) && startSize == Size.Empty)
            {
                var element = _element as FrameworkElement;
                startSize = new Size(element.Width, element.Height);
                resizeStart = mousePos;
            }

            if (InteractionStateMask == (uint)ElementInteractionState.Drag)
            {
                Drag(mousePos);
            }
            else if (InteractionStateMask == (uint)ElementInteractionState.ResizeLeft)
            {
                ResizeLeft(mousePos);
            }
            else if (InteractionStateMask == (uint)ElementInteractionState.ResizeRight)
            {
                ResizeRight(mousePos);
            }
            else if (InteractionStateMask == (uint)ElementInteractionState.ResizeBot)
            {
                ResizeBot(mousePos);
            }
            else if (InteractionStateMask == (uint)ElementInteractionState.ResizeTop)
            {
                ResizeTop(mousePos);
            }
            else if (InteractionStateMask == ((uint)ElementInteractionState.ResizeBot | (uint)ElementInteractionState.ResizeLeft))
            {
                ResizeBot(mousePos);
                ResizeLeft(mousePos);
            }
            else if (InteractionStateMask == ((uint)ElementInteractionState.ResizeBot | (uint)ElementInteractionState.ResizeRight))
            {
                ResizeBot(mousePos);
                ResizeRight(mousePos);
            }
            else if (InteractionStateMask == ((uint)ElementInteractionState.ResizeTop | (uint)ElementInteractionState.ResizeLeft))
            {
                ResizeTop(mousePos);
                ResizeLeft(mousePos);
            }
            else if (InteractionStateMask == ((uint)ElementInteractionState.ResizeTop | (uint)ElementInteractionState.ResizeRight))
            {
                ResizeTop(mousePos);
                ResizeRight(mousePos);
            }
        }

        public void RestartSelectionState()
        {
            localPos = Utils.SystemPoint.Empty;
            resizeStart = Utils.SystemPoint.Empty;
            startSize = Size.Empty;
            dCenter = Utils.SystemPoint.Empty;
        }

        
    }
}
