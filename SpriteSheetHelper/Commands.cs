using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpriteSheetHelper
{
    public static class Commands
    {
        public static readonly RoutedUICommand OpenFile = new RoutedUICommand("Open File", "OpenFile", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.O, ModifierKeys.Control) });
        public static readonly RoutedUICommand CloseFile = new RoutedUICommand("Close File", "CloseFile", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control) });
        public static readonly RoutedUICommand Exit = new RoutedUICommand("Exit", "Exit", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.Q, ModifierKeys.Control) });
    }
}
