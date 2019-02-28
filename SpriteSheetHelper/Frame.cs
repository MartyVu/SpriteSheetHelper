using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpriteSheetHelper
{
    public class Frame : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private Point _position;
        public Point Position
        {
            get => _position;
            set { _position = value; OnPropertyChanged(); }
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }
        // In milliseconds
        private int _delayTime;
        public int DelayTime
        {
            get => _delayTime;
            set { _delayTime = value; OnPropertyChanged(); }
        }

        public Frame(Point position)
        {
            Position = position;
            DelayTime = 100;
        }
    }
}