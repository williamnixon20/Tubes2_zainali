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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tubes2_zainali.MVVM
{
    /// <summary>
    /// Interaction logic for GridView.xaml
    /// </summary>
    public partial class GridView : UserControl
    {
        List<Element> _board;
        public double Rows { get; set; }
        public double Cols { get; set; }
        public GridView()
        {
            InitializeComponent();
            DataContext = this;
            Rows = 15;
            Cols = 15;
            _board = new List<Element>();
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    _board.Add(new Element(0, 0));
            Board.ItemsSource = _board;
            //try
            //{
            //    Maze baru = new Maze("wait");
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //}
        }

        private void CellClick(object sender, MouseButtonEventArgs e)
        {
            var border = (Border)sender;
            // each point has unique {X;Y} coordinates
            var point = (Element)border.Tag;
            // changing color in item view model
            // view is notified by binding
            point.Color = "#00BFFF";
        }
    }
}
