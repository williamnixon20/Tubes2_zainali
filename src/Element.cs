using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubes2_zainali
{
    public class Element
    {
        private string _color;

        public Element(int x, int y)
        {
            X = x;
            Y = y;
            _color = "Green";
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                if (ColorChanged != null)
                    ColorChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler ColorChanged;
    }
}
