using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpriteSheetHelper
{
    public class MainController : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region Viewport
        private Size _viewPortSize;
        public Size ViewportSize
        {
            get => _viewPortSize;
            set
            {
                _viewPortSize = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ViewportWidth));
                OnPropertyChanged(nameof(ViewportHeight));
                OnPropertyChanged(nameof(ViewportCenter));
                OnPropertyChanged(nameof(CanvasSize));
                OnPropertyChanged(nameof(CanvasCenter));
                OnPropertyChanged(nameof(ImagePosition));
                OnPropertyChanged(nameof(ViewportCenterOnImage));
            }
        }
        public double ViewportWidth
        {
            get => ViewportSize.Width;
            set => ViewportSize = new Size(value, ViewportHeight);
        }
        public double ViewportHeight
        {
            get => ViewportSize.Height;
            set => ViewportSize = new Size(ViewportWidth, value);
        }
        public Point ViewportCenter { get => new Point(ViewportWidth / 2.0, ViewportHeight / 2.0); }
        #endregion

        #region Canvas
        public Size CanvasSize { get => ImageSource == null ? ViewportSize : new Size(ImageSize.Width + Math.Max((ViewportSize.Width - 64.0), 0.0) * 2.0, ImageSize.Height + Math.Max((ViewportSize.Height - 64.0), 0.0) * 2.0); }
        public Point CanvasCenter { get => new Point(CanvasSize.Width / 2.0, CanvasSize.Height / 2.0); }
        #endregion

        #region Image
        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            private set
            {
                _imagePath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ImageSource));
                OnPropertyChanged(nameof(ImageSize));
                OnPropertyChanged(nameof(ImageCenter));
                OnPropertyChanged(nameof(CanvasSize));
                OnPropertyChanged(nameof(CanvasCenter));
                OnPropertyChanged(nameof(ImagePosition));
                OnPropertyChanged(nameof(ViewportCenterOnImage));
            }
        }
        public ImageSource ImageSource
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                    return null;

                try
                {
                    return new BitmapImage(new Uri(ImagePath));
                }
                catch
                {
                    return null;
                }
            }
        }
        public Size ImageSize { get => ImageSource == null ? new Size() : new Size(ImageSource.Width * ScaleValue, ImageSource.Height * ScaleValue); }
        public Point ImageCenter { get => new Point(ImageSize.Width / 2.0, ImageSize.Height / 2.0); }
        public Vector ImagePosition { get => CanvasCenter - ImageCenter; } // Top, Left
        #endregion

        #region Scale
        private readonly double[] _scaleValues = { 0.03125, 0.0625, 0.125, 0.25, 0.33, 0.50, 0.75, 1.0, 1.5, 2.0, 3.0, 4.0, 8.0, 16.0, 32.0 };
        private int _scaleIndex;
        public int ScaleIndex
        {
            get => _scaleIndex;
            set
            {
                PrevScaleValue = _scaleValues[ScaleIndex];

                if (value < 0 || value > _scaleValues.Count() - 1)
                    return;

                PrevViewportCenterOnImage = ViewportCenterOnImage;
                PrevMousePositionOnImage = MousePositionOnImage;
                PrevScrollOffset = ScrollOffset;

                _scaleIndex = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(ScaleValue));
                OnPropertyChanged(nameof(ScaleText));
                OnPropertyChanged(nameof(ImageSize));
                OnPropertyChanged(nameof(ImageCenter));
                OnPropertyChanged(nameof(CanvasSize));
                OnPropertyChanged(nameof(CanvasCenter));
                OnPropertyChanged(nameof(ImagePosition));
                OnPropertyChanged(nameof(ViewportCenterOnImage));

                ScaleChanged = ScaleValue != PrevScaleValue;
            }
        }
        public double ScaleValue { get => _scaleValues[ScaleIndex]; }
        public double ScaleText { get => ScaleValue * 100.0; }

        private double _prevScaleValue;
        public double PrevScaleValue
        {
            get => _prevScaleValue;
            set
            {
                _prevScaleValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ScaleDelta));
            }
        }
        public double ScaleDelta { get => ScaleValue / PrevScaleValue; }
        #endregion

        private Point _mousePositionOnImage;
        public Point MousePositionOnImage
        {
            get => _mousePositionOnImage;
            set
            {
                _mousePositionOnImage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ScaledMousePositionOnImage));
            }
        }
        public Point ScaledMousePositionOnImage { get => new Point(Math.Round(MousePositionOnImage.X / ScaleValue), Math.Round(MousePositionOnImage.Y / ScaleValue)); }

        private Point _prevMousePositionOnImage;
        public Point PrevMousePositionOnImage
        {
            get => _prevMousePositionOnImage;
            set { _prevMousePositionOnImage = value; OnPropertyChanged(); }
        }

        private Point _mousePositionOnCanvas;
        public Point MousePositionOnCanvas
        {
            get => _mousePositionOnCanvas;
            set
            {
                _mousePositionOnCanvas = value;
                OnPropertyChanged();
            }
        }

        private Point _mousePositionOnViewport;
        public Point MousePositionOnViewport
        {
            get => _mousePositionOnViewport;
            set { _mousePositionOnViewport = value; OnPropertyChanged(); }
        }
        private Point _prevMousePositionOnViewport;
        public Point PrevMousePositionViewport
        {
            get => _prevMousePositionOnViewport;
            set { _prevMousePositionOnViewport = value; OnPropertyChanged(); }
        }
        public Point ViewportCenterOnImage { get => new Point(HorizontalOffset + ViewportCenter.X - ImagePosition.X, VerticalOffset + ViewportCenter.Y - ImagePosition.Y); }
        private Point _prevViewportCenterOnImage;
        public Point PrevViewportCenterOnImage
        {
            get => _prevViewportCenterOnImage;
            set
            {
                _prevViewportCenterOnImage = value;
                OnPropertyChanged();
            }
        }

        #region Status
        private bool _loaded;
        public bool Loaded
        {
            get => _loaded;
            set { _loaded = value; OnPropertyChanged(); }
        }
        private bool _fileOpened;
        public bool FileOpened
        {
            get => _fileOpened;
            set { _fileOpened = value; OnPropertyChanged(); }
        }
        private bool _scaleChanged;
        public bool ScaleChanged
        {
            get => _scaleChanged;
            set { _scaleChanged = value; OnPropertyChanged(); }
        }
        private bool _mouseScrolled;
        public bool MouseScrolled
        {
            get => _mouseScrolled;
            set { _mouseScrolled = value; OnPropertyChanged(); }
        }
        private bool _sliderChanged;
        public bool SliderChanged
        {
            get => _sliderChanged;
            set { _sliderChanged = value; OnPropertyChanged(); }
        }
        private bool _sizeChanged;
        public bool SizeChanged
        {
            get => _sizeChanged;
            set { _sizeChanged = value; OnPropertyChanged(); }
        }
        #endregion

        #region Scrollable Size
        private Size _scrollableSize;
        public Size ScrollableSize
        {
            get => _scrollableSize;
            set
            {
                if (_scrollableSize == value)
                    return;

                _scrollableSize = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ScrollableWidth));
                OnPropertyChanged(nameof(ScrollableHeight));

                if (FileOpened)
                {
                    ScrollOffset = new Point(ScrollableWidth / 2.0, ScrollableHeight / 2.0);

                    FileOpened = false;
                }

                if (ScaleChanged)
                {
                    if (MouseScrolled)
                    {
                        Point scaledMousePositionOnImageAnchor = new Point(PrevMousePositionOnImage.X * ScaleDelta, PrevMousePositionOnImage.Y * ScaleDelta);
                        Point deltaMousePositionOnImage = (Point)(scaledMousePositionOnImageAnchor - MousePositionOnImage);

                        ScrollOffset = new Point(Math.Min(HorizontalOffset, ScrollableWidth) + deltaMousePositionOnImage.X, Math.Min(VerticalOffset, ScrollableHeight) + deltaMousePositionOnImage.Y);

                        MouseScrolled = false;
                    }

                    else if (SliderChanged)
                    {
                        Point scaledMousePositionOnImageAnchor = new Point(PrevViewportCenterOnImage.X * ScaleDelta, PrevViewportCenterOnImage.Y * ScaleDelta);
                        Point deltaMousePositionOnImage = (Point)(scaledMousePositionOnImageAnchor - ViewportCenterOnImage);

                        ScrollOffset = new Point(HorizontalOffset + deltaMousePositionOnImage.X, VerticalOffset + deltaMousePositionOnImage.Y);

                        SliderChanged = false;
                    }

                    ScaleChanged = false;
                }

                if (SizeChanged)
                {
                    var ratio = new Point(PrevScrollOffset.X / PrevScrollableSize.Width, PrevScrollOffset.Y / PrevScrollableSize.Height);

                    ScrollOffset = new Point(ScrollableWidth * ratio.X, ScrollableHeight * ratio.Y);

                    SizeChanged = false;
                }
            }
        }
        public double ScrollableWidth { get => ScrollableSize.Width; }
        public double ScrollableHeight { get => ScrollableSize.Height; }
        private Size _prevScrollableSize;
        public Size PrevScrollableSize
        {
            get => _prevScrollableSize;
            set { _prevScrollableSize = value; OnPropertyChanged(); }
        }
        #endregion

        #region Scroll Offset
        private Point _scrollOffset;
        public Point ScrollOffset
        {
            get => _scrollOffset;
            set
            {
                _scrollOffset = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HorizontalOffset));
                OnPropertyChanged(nameof(VerticalOffset));
                OnPropertyChanged(nameof(ViewportCenterOnImage));
            }
        }
        public double HorizontalOffset
        {
            get => ScrollOffset.X;
            set => ScrollOffset = new Point(value, VerticalOffset);
        }
        public double VerticalOffset
        {
            get => ScrollOffset.Y;
            set => ScrollOffset = new Point(HorizontalOffset, value);
        }
        private Point _prevScrollOffset;
        public Point PrevScrollOffset
        {
            get => _prevScrollOffset;
            set { _prevScrollOffset = value; OnPropertyChanged(); }
        }
        #endregion

        #region Frame
        private Size _frameSize;
        public Size FrameSize
        {
            get => _frameSize;
            set
            {
                _frameSize = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FrameWidth));
                OnPropertyChanged(nameof(FrameHeight));
                OnPropertyChanged(nameof(FrameCenter));
            }
        }
        public double FrameWidth
        {
            get => FrameSize.Width;
            set { FrameSize = new Size(value, FrameSize.Height); }
        }
        public double FrameHeight
        {
            get => FrameSize.Height;
            set { FrameSize = new Size(FrameSize.Width, value); }
        }
        public Point FrameCenter { get => new Point(-FrameSize.Width / 2.0, -FrameSize.Height / 2.0); }
        #endregion

        public enum ModKeys { None, CanZoom, CanPan, Panning };
        private List<ModKeys> _modifiers;
        public ModKeys ActiveModifier { get => _modifiers.Any() ? _modifiers.Last() : ModKeys.None; }



        private List<Animation> _animations;
        public List<Animation> Animations
        {
            get => _animations;
            private set { _animations = value; OnPropertyChanged(); }
        }

        private Animation _selectedAnimation;
        public Animation SelectedAnimation
        {
            get => _selectedAnimation;
            set { _selectedAnimation = value; OnPropertyChanged(); }
        }


        public MainController()
        {
            FrameSize = new Size(100, 100);
            _modifiers = new List<ModKeys>();

            Animations = new List<Animation>()
            {
                new Animation("Idle1"),
                new Animation("Idle2"),
                new Animation("Walk1a"),
                new Animation("Walk1b"),
                new Animation("Walk2a"),
                new Animation("Walk2b")
            };


        }
        public void AddModifier(ModKeys key)
        {
            if (_modifiers.Contains(key))
                return;

            _modifiers.Add(key);
            OnPropertyChanged(nameof(ActiveModifier));
        }
        public void RemoveModifier(ModKeys key)
        {
            if (!_modifiers.Contains(key))
                return;

            _modifiers.Remove(key);
            OnPropertyChanged(nameof(ActiveModifier));
        }
        public bool OpenFile(string fileName)
        {
            ImagePath = fileName;

            if (ImageSource == null)
                return false;

            ScaleIndex = 7;
            PrevScaleValue = 1.0;
            FileOpened = true;
            return true;
        }
        public void CloseFile()
        {
            ImagePath = string.Empty;
        }

        public void Scroll(int delta)
        {
            ScaleIndex += delta / Math.Abs(delta);

            MouseScrolled = PrevScaleValue != ScaleValue;
        }

    }
}