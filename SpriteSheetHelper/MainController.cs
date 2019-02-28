using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpriteSheetHelper
{
    public class MainController : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private List<Animation> _animations;
        private Animation _selectedAnimation;

        public List<Animation> Animations
        {
            get => _animations;
            private set { _animations = value; OnPropertyChanged(); }
        }
        public Animation SelectedAnimation
        {
            get => _selectedAnimation;
            set { _selectedAnimation = value; OnPropertyChanged(); }
        }

        public MainController()
        {
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
    }
}