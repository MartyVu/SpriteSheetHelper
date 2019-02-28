using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpriteSheetHelper
{
    public class MyVector : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private double _x;
        public double X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LengthSquared));
                OnPropertyChanged(nameof(Length));
            }
        }
        private double _y;
        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LengthSquared));
                OnPropertyChanged(nameof(Length));
            }
        }        
        public double LengthSquared { get => Math.Pow(X, 2) + Math.Pow(Y, 2); }
        public double Length { get => Math.Sqrt(LengthSquared); }

        public MyVector() : this(0.0, 0.0)
        {

        }
        public MyVector(double value) : this(value, value)
        {

        }
        public MyVector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object value)
        {
            if (!(value is MyVector vector))
                return false;

            return X == vector.X && Y == vector.Y;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public double AngleBetween(MyVector vector) { return Math.Acos((this * vector) / (Length + vector.Length)); }

        public static MyVector operator +(MyVector vector1, MyVector vector2)
        {
            return new MyVector(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }
        public static MyVector operator -(MyVector vector)
        {
            return new MyVector(-vector.X, -vector.Y);
        }
        public static MyVector operator -(MyVector vector, double scalar)
        {
            return new MyVector(vector.X - scalar, vector.Y - scalar);
        }
        public static MyVector operator -(MyVector vector1, MyVector vector2)
        {
            return vector1 + -vector2;
        }
        public static MyVector operator *(MyVector vector, double scalar)
        {
            return new MyVector(vector.X * scalar, vector.Y * scalar);
        }
        public static MyVector operator *(double scalar, MyVector vector)
        {
            return vector * scalar;
        }
        public static double operator *(MyVector vector1, MyVector vector2)
        {
            return (vector1.X * vector2.X) + (vector1.Y * vector2.Y);
        }
        public static MyVector operator /(MyVector vector, double scalar)
        {
            return new MyVector(vector.X / scalar, vector.Y / scalar);
        }
        public static bool operator ==(MyVector vector1, MyVector vector2)
        {
            return vector1.Equals(vector2);
        }
        public static bool operator !=(MyVector vector1, MyVector vector2)
        {
            return !vector1.Equals(vector2);
        }
    }
}