using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SpriteSheetHelper
{
    class ScrollViewerObserver
    {
        public static bool GetObserveScrollViewer(FrameworkElement elem) => (bool)elem.GetValue(ObserveScrollViewerProperty);
        public static void SetObserveScrollViewer(FrameworkElement elem, bool value) => elem.SetValue(ObserveScrollViewerProperty, value);
        public static readonly DependencyProperty ObserveScrollViewerProperty = DependencyProperty.RegisterAttached("ObserveScrollViewer", typeof(bool), typeof(ScrollViewerObserver), new UIPropertyMetadata(false, OnObserveScrollViewerProperty));
        static void OnObserveScrollViewerProperty(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = depObj as ScrollViewer;
            if (scrollViewer == null)
                return;

            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
                scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            else
                scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
        }
        private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (!ReferenceEquals(sender, e.OriginalSource))
                return;

            var scrollViewer = e.OriginalSource as ScrollViewer;
            if (scrollViewer == null)
                return;

            if (e.ViewportWidthChange != 0 || e.ViewportHeightChange != 0)
                SetViewportSize(scrollViewer, new Size(e.ViewportWidth, e.ViewportHeight));

            if (e.ExtentWidthChange != 0 || e.ExtentHeightChange != 0)
                SetScrollableSize(scrollViewer, new Size(Math.Max(e.ExtentWidth - e.ViewportWidth, 0.0), Math.Max(e.ExtentHeight - e.ViewportHeight, 0.0)));
            else
            {
                if (e.HorizontalChange != 0)
                    SetHorizontalOffset(scrollViewer, e.HorizontalOffset);
                if (e.VerticalChange != 0)
                    SetVerticalOffset(scrollViewer, e.VerticalOffset);
            }
        }

        public static Size GetViewportSize(DependencyObject obj) => (Size)obj.GetValue(ViewportSizeProperty);
        public static void SetViewportSize(DependencyObject obj, Size value) => obj.SetValue(ViewportSizeProperty, value);
        public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.RegisterAttached("ViewportSize", typeof(Size), typeof(ScrollViewerObserver), new UIPropertyMetadata(new Size()));

        public static Size GetScrollableSize(DependencyObject obj) => (Size)obj.GetValue(ScrollableSizeProperty);
        public static void SetScrollableSize(DependencyObject obj, Size value) => obj.SetValue(ScrollableSizeProperty, value);
        public static readonly DependencyProperty ScrollableSizeProperty = DependencyProperty.RegisterAttached("ScrollableSize", typeof(Size), typeof(ScrollViewerObserver), new UIPropertyMetadata(new Size()));

        public static double GetVerticalOffset(DependencyObject obj) => (double)obj.GetValue(VerticalOffsetProperty);
        public static void SetVerticalOffset(DependencyObject obj, double value) => obj.SetValue(VerticalOffsetProperty, value);
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ScrollViewerObserver), new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));
        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;
            if (scrollViewer == null)
                return;

            scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
        }

        public static double GetHorizontalOffset(DependencyObject obj) => (double)obj.GetValue(HorizontalOffsetProperty);
        public static void SetHorizontalOffset(DependencyObject obj, double value) => obj.SetValue(HorizontalOffsetProperty, value);
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollViewerObserver), new UIPropertyMetadata(0.0, OnHorizontalOffsetChanged));
        private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;
            if (scrollViewer == null)
                return;

            scrollViewer.ScrollToHorizontalOffset((double)e.NewValue);
        }
    }
}