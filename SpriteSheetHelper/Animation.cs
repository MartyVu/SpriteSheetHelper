using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpriteSheetHelper
{
    public class Animation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string _name;
        private List<Frame> _frames;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public List<Frame> Frames
        {
            get => _frames;
            set { _frames = value; OnPropertyChanged(); }
        }

        public Animation(string name)
        {
            Name = name;
            Frames = new List<Frame>()
            {
                new Frame(),
                new Frame(),
                new Frame(),
                new Frame(),
                new Frame(),
            };
        }
    }
}