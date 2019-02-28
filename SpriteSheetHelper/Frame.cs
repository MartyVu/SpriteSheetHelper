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

        Vector _topLeft;
        Vector _bottomRight;
        Vector _origin;

        public Frame()
        {
            _topLeft = new Vector(1.0, 1.0);
            _bottomRight = new Vector(2.0, 2.0);
            _topLeft = new Vector(0.5, 0.5);
        }
    }
}