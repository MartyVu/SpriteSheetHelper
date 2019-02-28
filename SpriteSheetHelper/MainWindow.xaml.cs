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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpriteSheetHelper
{
    public partial class MainWindow : Window
    {
        private MainController MainController { get; }

        public MainWindow()
        {
            InitializeComponent();
            MainController = (MainController)Application.Current.Resources["MainController"];
        }        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainController.OpenFile(@"E:\Marty V\Pictures\CU_logo_black.png");
            MainController.Loaded = true;
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize == new Size())
                return;

            MainController.PrevScrollOffset = MainController.ScrollOffset;
            MainController.PrevScrollableSize = MainController.ScrollableSize;

            MainController.SizeChanged = true;
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (MainController.ActiveModifier == MainController.ModKeys.Panning)
                return;

            if ((Keyboard.Modifiers & ModifierKeys.Shift) > 0)
                MainController.AddModifier(MainController.ModKeys.ShiftHeld);

            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
                MainController.AddModifier(MainController.ModKeys.CtrlHeld);

            if (e.Key == Key.Space)
                MainController.AddModifier(MainController.ModKeys.SpaceHeld);
        }
        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Shift) <= 0)
                MainController.RemoveModifier(MainController.ModKeys.ShiftHeld);

            if ((Keyboard.Modifiers & ModifierKeys.Control) <= 0)
                MainController.RemoveModifier(MainController.ModKeys.CtrlHeld);

            if (e.Key == Key.Space)
                MainController.RemoveModifier(MainController.ModKeys.SpaceHeld);
        }
        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            MainController.MousePositionOnImage = Mouse.GetPosition(Image);
            MainController.MousePositionOnCanvas = Mouse.GetPosition(Canvas);
            MainController.MousePositionOnViewport = Mouse.GetPosition(ScrollViewer);

            if (MainController.ActiveModifier == MainController.ModKeys.Panning)
            {
                Point mouseDelta = (Point)(MainController.MousePositionOnViewport - MainController.PrevMousePositionViewport);

                MainController.ScrollOffset = new Point(MainController.PrevScrollOffset.X - mouseDelta.X, MainController.PrevScrollOffset.Y - mouseDelta.Y);
            }
        }
        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MainController.ActiveModifier == MainController.ModKeys.SpaceHeld)
            {
                if (MainController.MousePositionOnViewport.X >= 0.0 && MainController.MousePositionOnViewport.X <= MainController.ViewportSize.Width && MainController.MousePositionOnViewport.Y >= 0.0 && MainController.MousePositionOnViewport.Y <= MainController.ViewportSize.Height)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        if (MainController.ActiveModifier != MainController.ModKeys.Panning)
                        {
                            MainController.AddModifier(MainController.ModKeys.Panning);
                            MainController.PrevMousePositionViewport = MainController.MousePositionOnViewport;
                            MainController.PrevScrollOffset = MainController.ScrollOffset;
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
                if (MainController.ActiveModifier == MainController.ModKeys.Panning)
                {
                    MainController.RemoveModifier(MainController.ModKeys.Panning);
                    ReleaseMouseCapture();
                }
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            MainController.MousePositionOnImage = Mouse.GetPosition(Image);
        }
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (MainController.ActiveModifier == MainController.ModKeys.CtrlHeld)
            {
                if (MainController.MousePositionOnViewport.X >= 0.0 && MainController.MousePositionOnViewport.X <= MainController.ViewportSize.Width && MainController.MousePositionOnViewport.Y >= 0.0 && MainController.MousePositionOnViewport.Y <= MainController.ViewportSize.Height)
                {
                    if (MainController.ImageSource != null)
                        MainController.Scroll(e.Delta);

                    e.Handled = true;
                }
            }
        }

       

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MainController == null)
                return;

            if (!MainController.Loaded)
                return;

            MainController.SliderChanged = true;
        }

        

        private void Animations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FramesSuperset.SelectionChanged -= FramesSuperset_SelectionChanged;

            foreach (var frame in MainController.FramesSuperset)
                frame.IsSelected = false;

            MainController.SelectedAnimationCount = Animations.SelectedItems.Count;

            if (MainController.SelectedAnimationCount == 1 && MainController.SelectedAnimation.Frames.Any())
                MainController.SelectedAnimation.Frames.First().IsSelected = true;

            FramesSuperset.SelectionChanged += FramesSuperset_SelectionChanged;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MainController.ActiveModifier == MainController.ModKeys.Panning || MainController.ActiveModifier == MainController.ModKeys.CtrlHeld)
                return;

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            if (MainController.ActiveModifier != MainController.ModKeys.ShiftHeld)
                return;

            MainController.AddFrame(MainController.ScaledMousePositionOnImage);
        }

        private void FramesSuperset_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FramesSuperset.SelectedItems.Count == 0)
                return;

            Animations.SelectionChanged -= Animations_SelectionChanged;

            MainController.SelectedFrameCount = FramesSuperset.SelectedItems.Count;

            foreach (var animation in MainController.Animations)
                animation.IsSelected = animation.Frames.Any(x => FramesSuperset.SelectedItems.Contains(x));

            MainController.SelectedAnimationCount = Animations.SelectedItems.Count;

            Animations.SelectionChanged += Animations_SelectionChanged;
        }

        private void OpenFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog { Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png" };
            if (!openDialog.ShowDialog(this).Value)
                return;

            if (!MainController.OpenFile(openDialog.FileName))
                MessageBox.Show("Cannot read image file: " + Environment.NewLine + openDialog.FileName);
        }
        private void CloseFile_Executed(object sender, ExecutedRoutedEventArgs e) => MainController.CloseFile();
        private void CloseFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MainController == null)
                return;

            e.CanExecute = MainController.ImageSource != null;
        }
        private void Exit_Exexcuted(object sender, ExecutedRoutedEventArgs e) => Close();

        private void SelectFirstFrame_Executed(object sender, ExecutedRoutedEventArgs e) => MainController.SelectFirstFrame();
        private void SelectFirstFrame_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MainController == null)
                return;

            if (MainController.SelectedFrame == null || MainController.AnimationFrames == null)
                return;

            e.CanExecute = MainController.SelectedFrame != MainController.AnimationFrames.First();
        }
        private void SelectPreviousFrame_Executed(object sender, ExecutedRoutedEventArgs e) => MainController.SelectPreviousFrame();
        private void SelectPreviousFrame_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MainController == null)
                return;

            if (MainController.SelectedFrame == null || MainController.AnimationFrames == null)
                return;

            e.CanExecute = MainController.AnimationFrames.Count > 1;

        }
        private void SelectNextFrame_Executed(object sender, ExecutedRoutedEventArgs e) => MainController.SelectNextFrame();
        private void SelectNextFrame_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MainController == null)
                return;

            if (MainController.SelectedFrame == null || MainController.AnimationFrames == null)
                return;

            e.CanExecute = MainController.AnimationFrames.Count > 1;

        }
        private void SelectLastFrame_Executed(object sender, ExecutedRoutedEventArgs e) => MainController.SelectLastFrame();
        private void SelectLastFrame_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MainController == null)
                return;

            if (MainController.SelectedFrame == null || MainController.AnimationFrames == null)
                return;

            e.CanExecute = MainController.SelectedFrame != MainController.AnimationFrames.Last();

        }
        private void PlayAnimation_Executed(object sender, ExecutedRoutedEventArgs e) => MainController.PlayAnimation();
        private void PlayAnimation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MainController == null)
                return;

            if (MainController.AnimationFrames == null)
                return;

            e.CanExecute = MainController.AnimationFrames.Count > 1;
        }

        private void AddAnimation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MainController.AddAnimation(MainController.Animations.Count.ToString());
            Animations.Focus();
        }
        private void AddAnimation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RemoveAnimation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MainController.RemoveAnimations(Animations.SelectedItems.Cast<Animation>());
            Animations.Focus();
        }
        private void RemoveAnimation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (MainController == null)
                return;

            e.CanExecute = MainController.SelectedAnimationCount > 0;
        }

        private void EditAnimation_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}