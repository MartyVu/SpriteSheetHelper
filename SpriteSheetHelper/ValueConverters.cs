using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace SpriteSheetHelper
{
    public class DifferenceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool calculate = true;

            if (parameter != null)
            {
                if (parameter.ToString().ToLower() == "true")
                    calculate = true;
                else if (parameter.ToString().ToLower() == "false")
                    calculate = false;
            }

            return calculate ? (double)values[0] - (double)values[1] : (double)values[0];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SumConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double total = 0.0;

            foreach (var value in values)
                total += (double)value;

            return total;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MinValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Math.Min((double)values[0], (double)values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ThumbnailConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length < 3)
                return null;

            if (!(values[0] is BitmapSource) || !(values[1] is Point) || !(values[2] is Size))
                return null;

            var bitmapSource = (BitmapSource)values[0];
            var position = (Point)values[1];
            var sourceRectSize = (Size)values[2];
            var sourceRectCenter = new Point(sourceRectSize.Width / 2.0, sourceRectSize.Height / 2.0);
            var sourceRectPosition = new Point((position.X - sourceRectCenter.X).Clamp(0.0, bitmapSource.Width), (position.Y - sourceRectCenter.Y).Clamp(0.0, bitmapSource.Height));
            var sourceRect = new Int32Rect((int)Math.Round(sourceRectPosition.X), (int)Math.Round(sourceRectPosition.Y), (int)Math.Round(sourceRectSize.Width), (int)Math.Round(sourceRectSize.Height));

            return new CroppedBitmap(bitmapSource, sourceRect);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var toolsType = value as ToolsType;
            if (toolsType == null)
                return null;

            switch (toolsType.Value)
            {
                case 1:
                    return Application.Current.Resources["MouseToolIcon"];
                case 2:
                    return Application.Current.Resources["FrameToolIcon"];
                case 3:
                    return Application.Current.Resources["ZoomToolIcon"];
                case 4:
                    return Application.Current.Resources["PanToolIcon"];
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
