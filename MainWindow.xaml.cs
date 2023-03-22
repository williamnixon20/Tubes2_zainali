using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using Tubes2_zainali;
using System.ComponentModel;

namespace Tubes2_zainali
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        int index = 0;
        public string mode = "DFS";
        bool TSP = false;
        public string fileName = "";
        Maze _maze;
        List<Element> _board;
        List<List<Element>> _states;
        int nRows = 1;
        int nCols = 2;
        public int NRows
        {
            get { return nRows; }
            set
            {
                nRows = value;
                RaisePropertyChanged(nameof(NRows));
            }
        }
        public int NCols
        {
            get { return nCols; }
            set
            {
                nCols = value;
                RaisePropertyChanged(nameof(NCols));
            }
        }
        public MainWindow()
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
            DataContext = this;
        }

        private void fileTextChanged(object sender, TextChangedEventArgs e)
        {
            var template = fileInput.Template;
            var fileBox = (TextBox)template.FindName("FileBox", fileInput);
            fileName = fileBox.Text;
            //NRows = 5;
            //NCols = 5;
            //_board = new List<Element>();
            //for (int r = 0; r < nRows; r++)
            //    for (int c = 0; c < nCols; c++)
            //        if (r == 0)
            //        {
            //            _board.Add(new Element(r, c, -1));
            //        }
            //        else
            //            _board.Add(new Element(r, c, r));
            //Board.ItemsSource = _board;
        }
        private void btnNextClick(object sender, RoutedEventArgs e)
        {
            if (index + 1 < _states.Count())
            {
                index += 1;
                Board.ItemsSource = _states.ElementAt(index);
            }
        }
        private void btnPrevClick(object sender, RoutedEventArgs e)
        {
            if (index - 1 >= 0)
            {
                index -= 1;
                Board.ItemsSource = _states.ElementAt(index);
            }
        }
        private void RadioClick(object sender, RoutedEventArgs e)
        {
            var radio = (RadioButton)sender;
            mode = (string)radio.Content;

        }
        private void TSPClick(object sender, RoutedEventArgs e)
        {
            if (TSP == false)
            {
                TSP = true;
            }
            else
            {
                TSP = false;
            }
        }
        private void btnOpenFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
                //MessageBox.Show(openFileDialog.FileName);
                //LogReader baru = new LogReader(fileName);
                //_states = baru._logBoard;
                index = 0;
                fileInput.Text = fileName;
            }
        }
        private void btnViz(object sender, RoutedEventArgs e)
        {
            try
            {
                _maze = new Maze("", fileName);
                _board = _maze.GetGridRepresentation();
                NRows = _maze.RowCount;
                NCols = _maze.ColCount;
                Board.ItemsSource = _board;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private void btnSearch(object sender, RoutedEventArgs e)
        {
            if (_maze != null)
            {
                string logFile = "";
                if (TSP)
                {
                    if (mode == "DFS")
                    {
                        TSPPlayer tspPlayer = new TSPPlayer(_maze);
                        tspPlayer.StartTSPDFS();
                        tspPlayer.BackupColoringState(tspPlayer.PlayerLog);
                        logFile = tspPlayer.SaveLog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
                    }
                }
                else
                {
                    if (mode == "DFS")
                    {
                        DFSPlayer dfsPlayer = new DFSPlayer(_maze);
                        dfsPlayer.StartDFS();
                        dfsPlayer.BackupColoringState(dfsPlayer.PlayerLog);
                        logFile = dfsPlayer.SaveLog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
                    }
                    else
                    {
                        BFSPlayer bfsPlayer = new BFSPlayer(_maze);
                        bfsPlayer.StartBFS();
                        bfsPlayer.BackupColoringState(bfsPlayer.PlayerLog);
                        logFile = bfsPlayer.SaveLog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
                    }
                }
                MessageBox.Show(logFile);
                LogReader baru = new LogReader(logFile, _board);
                MessageBox.Show(baru.path);
                _states = baru._logBoard;
                index = 0;
                fileInput.Text = fileName;
            }
        }
        //private void CellClick(object sender, MouseButtonEventArgs e)
        //{
        //    var border = (Border)sender;
        //    // each point has unique {X;Y} coordinates
        //    var point = (Element)border.Tag;
        //    // changing color in item view model
        //    // view is notified by binding
        //    point.Color = "#00BFFF";
        //}
        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
