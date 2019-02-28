using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpriteSheetHelper
{
    public partial class MainWindow : Window
    {
        private MainController Controller { get; }

        public MainWindow()
        {
            InitializeComponent();
            Controller = (MainController)Application.Current.Resources["MainController"];
        }        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Controller.OpenFile(@"E:\Marty V\Pictures\CU_logo_black.png");
            Controller.Loaded = true;
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize == new Size())
                return;

            Controller.PrevScrollOffset = Controller.ScrollOffset;
            Controller.PrevScrollableSize = Controller.ScrollableSize;

            Controller.SizeChanged = true;
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Controller.ActiveModifier == MainController.ModKeys.Panning)
                return;

            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
                Controller.AddModifier(MainController.ModKeys.CanZoom);

            if (e.Key == Key.Space)
                Controller.AddModifier(MainController.ModKeys.CanPan);
        }
        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) <= 0)
                Controller.RemoveModifier(MainController.ModKeys.CanZoom);

            if (e.Key == Key.Space)
                Controller.RemoveModifier(MainController.ModKeys.CanPan);
        }
        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Controller.MousePositionOnImage = Mouse.GetPosition(Image);
            Controller.MousePositionOnCanvas = Mouse.GetPosition(Canvas);
            Controller.MousePositionOnViewport = Mouse.GetPosition(ScrollViewer);

            if (Controller.ActiveModifier == MainController.ModKeys.Panning)
            {
                Point mouseDelta = (Point)(Controller.MousePositionOnViewport - Controller.PrevMousePositionViewport);

                Controller.ScrollOffset = new Point(Controller.PrevScrollOffset.X - mouseDelta.X, Controller.PrevScrollOffset.Y - mouseDelta.Y);
            }
        }
        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Controller.ActiveModifier == MainController.ModKeys.CanPan)
            {
                if (Controller.MousePositionOnViewport.X >= 0.0 && Controller.MousePositionOnViewport.X <= Controller.ViewportSize.Width && Controller.MousePositionOnViewport.Y >= 0.0 && Controller.MousePositionOnViewport.Y <= Controller.ViewportSize.Height)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        if (Controller.ActiveModifier != MainController.ModKeys.Panning)
                        {
                            Controller.AddModifier(MainController.ModKeys.Panning);
                            Controller.PrevMousePositionViewport = Controller.MousePositionOnViewport;
                            Controller.PrevScrollOffset = Controller.ScrollOffset;
                            CaptureMouse();
                        }
                    }
                }
            }
        }
        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (Controller.ActiveModifier == MainController.ModKeys.Panning)
                {
                    Controller.RemoveModifier(MainController.ModKeys.Panning);
                    ReleaseMouseCapture();
                }
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            Controller.MousePositionOnImage = Mouse.GetPosition(Image);
        }
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Controller.ActiveModifier == MainController.ModKeys.CanZoom)
            {
                if (Controller.MousePositionOnViewport.X >= 0.0 && Controller.MousePositionOnViewport.X <= Controller.ViewportSize.Width && Controller.MousePositionOnViewport.Y >= 0.0 && Controller.MousePositionOnViewport.Y <= Controller.ViewportSize.Height)
                {
                    if (Controller.ImageSource != null)
                        Controller.Scroll(e.Delta);

                    e.Handled = true;
                }
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Controller.ActiveModifier == MainController.ModKeys.Panning || Controller.ActiveModifier == MainController.ModKeys.CanZoom)
                return;

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            //Controller.AddFrame(Controller.ScaledMousePositionOnImage);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Controller == null)
                return;

            if (!Controller.Loaded)
                return;

            Controller.SliderChanged = true;
        }

        private void OpenFile(object sender, ExecutedRoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog { Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png" };
            if (!openDialog.ShowDialog(this).Value)
                return;

            if (!Controller.OpenFile(openDialog.FileName))
                MessageBox.Show("Cannot read image file: " + Environment.NewLine + openDialog.FileName);
        }
        private void CanCloseFile(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = Controller.ImageSource != null;
        private void CloseFile(object sender, ExecutedRoutedEventArgs e) => Controller.CloseFile();
        private void Exit(object sender, ExecutedRoutedEventArgs e) => Close();
    }
}