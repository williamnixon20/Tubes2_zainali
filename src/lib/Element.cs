using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tubes2_zainali
{
    public class Element 
    {
        private string _color;
        private char _type;

        public Element(int x, int y)
        {
            X = x;
            Y = y;
            _color = "Green";
        }

        public Element(int x, int y, int color)
        {
            X = x;
            Y = y;
            switch (color)
            {
                case -2:
                    _color = "Red";
                    break;
                case -1:
                    _color = "Gray";
                    break;
                case 0:
                    _color = "LightGray";
                    break;
                case 1:
                    _color = "#99F4FF";
                    break;
                case 2:
                    _color = "#5BCBF7";
                    break;
                case 3:
                    _color = "#399CDF";
                    break;
                case 4:
                    _color = "#2186D1";
                    break;
                case 5:
                    _color = "#0052A2";
                    break;
                case 6:
                    _color = "#00498D";
                    break;
                case 7:
                    _color = "#02386E";
                    break;
                case 8:
                    _color = "#00264D";
                    break;
                case 9:
                    _color = "##00172D";
                    break;
                default:
                    _color = "#000B18";
                    break;
            }
        }

        public Element(int x, int y, int color, char type)
        {
            X = x;
            Y = y;
            _type = type;
            switch (color)
            {
                case -2:
                    _color = "Red";
                    break;
                case -1:
                    _color = "Gray";
                    break;
                case 0:
                    _color = "LightGray";
                    break;
                case 1:
                    _color = "#99F4FF";
                    break;
                case 2:
                    _color = "#5BCBF7";
                    break;
                case 3:
                    _color = "#399CDF";
                    break;
                case 4:
                    _color = "#2186D1";
                    break;
                case 5:
                    _color = "#0052A2";
                    break;
                case 6:
                    _color = "#00498D";
                    break;
                case 7:
                    _color = "#02386E";
                    break;
                case 8:
                    _color = "#00264D";
                    break;
                case 9:
                    _color = "##00172D";
                    break;
                default:
                    _color = "#000B18";
                    break;
            }
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public string ColorTile
        {
            get { Debug.Write(_color.ToString()); return _color;  }
            set
            {
                _color = value;
                if (ColorChanged != null)
                    ColorChanged(this, EventArgs.Empty);
            }
        }

        public char CellType
        {
            get { return _type; }
            set
            {
                _type = value;
            }
        }

        public BitmapImage? CellImage
        {
            get
            {
                switch (_type)
                {
                    case 'K':
                        return new BitmapImage(new Uri("pack://application:,,,/assets/start.png"));
                    case 'T':
                        return new BitmapImage(new Uri("pack://application:,,,/assets/treasure.png"));
                    default:
                        return null;
                };
            }
            set
            {

            }
        }

        public event EventHandler? ColorChanged;
    }
}
