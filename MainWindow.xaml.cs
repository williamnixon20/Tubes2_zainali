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
using Tubes2_zainali;
using System.ComponentModel;

namespace Tubes2_zainali
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        int index = 1;
        public string mode = "DFS";
        bool TSP = false;
        public string fileName = "";
        Maze _maze;
        List<Element> _board;
        List<List<Element>> _states;
        List<string> _steps;
        List<string> _numNodes;
        List<string> _numSteps;
        string _time;
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
        }
        private void btnNextClick(object sender, RoutedEventArgs e)
        {
            if (index + 1 < _states.Count())
            {
                index += 1;
                Board.ItemsSource = _states.ElementAt(index);
                numSteps.Text = "Num Steps: " + _numSteps[index-1];
                steps.Text = "Steps: " + _steps[index-1];
                numNodes.Text = "Num Nodes: " + _numNodes[index-1];
            } else
            {
                MessageBox.Show("Sudah di state terakhir!");
            }
        }
        private void btnPrevClick(object sender, RoutedEventArgs e)
        {
            if (index - 1 >= 1)
            {
                index -= 1;
                Board.ItemsSource = _states.ElementAt(index);
                numSteps.Text = "Num Steps: " + _numSteps[index - 1];
                steps.Text = "Steps: " + _steps[index-1];
                numNodes.Text = "Num Nodes: " + _numNodes[index-1];
            } else
            {
                MessageBox.Show("Sudah di state paling awal!");
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
                buttonSearch.Visibility = Visibility.Visible;
                showSearchInfo(false);
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

                Player krustyKrab;
                if (mode == "DFS")
                {
                    krustyKrab = new DFSPlayer(_maze, TSP);
                }
                else
                {
                    krustyKrab = new BFSPlayer(_maze, TSP);
                }
                krustyKrab.StartSearch();
                krustyKrab.BackupColoringState();
                logFile = krustyKrab.SaveLog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
                this._numNodes = krustyKrab._numNodes;
                this._numSteps = krustyKrab._numSteps;
                this._time = krustyKrab._time.ToString();
                this._steps = krustyKrab._playerDirectionState;      

                // if (TSP)
                // {
                //     if (mode == "DFS")
                //     {
                //         TSPPlayer tspPlayer = new TSPPlayer(_maze);
                //         tspPlayer.StartTSPDFS();
                //         tspPlayer.BackupColoringState();
                //         logFile = tspPlayer.SaveLog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
                //         this._numNodes = tspPlayer._numNodes;
                //         this._numSteps = tspPlayer._numSteps;
                //         this._time = tspPlayer._time.ToString();
                //         this._steps = tspPlayer._playerDirectionState;
                //     } else
                //     {
                //         MessageBox.Show("Maaf, mode TSP hanya dapat dilakukan dengan DFS.");
                //         return;
                //     }
                // }
                // else
                // {
                //     if (mode == "DFS")
                //     {
                //         DFSPlayer dfsPlayer = new DFSPlayer(_maze);
                //         dfsPlayer.StartSearch();
                //         dfsPlayer.BackupColoringState();
                //         logFile = dfsPlayer.SaveLog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
                //         this._numNodes = dfsPlayer._numNodes;
                //         this._numSteps = dfsPlayer._numSteps;
                //         this._time = dfsPlayer._time.ToString();
                //         this._steps = dfsPlayer._playerDirectionState;
                //     }
                //     else
                //     {
                //         BFSPlayer bfsPlayer = new BFSPlayer(_maze);
                //         bfsPlayer.StartSearch();
                //         bfsPlayer.BackupColoringState();
                //         logFile = bfsPlayer.SaveLog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
                //         this._numNodes = bfsPlayer._numNodes;
                //         this._numSteps = bfsPlayer._numSteps;
                //         this._time = bfsPlayer._time.ToString();
                //         this._steps = bfsPlayer._playerDirectionState;
                //     }
                // }

                LogReader baru = new LogReader(logFile);
                _states = baru._logBoard;
                index = 1;
                fileInput.Text = fileName;
                Board.ItemsSource = _states.ElementAt(index);
                numSteps.Text = "Num Steps: " + _numSteps[index-1];
                steps.Text = "Steps: " + _steps[index-1];
                time.Text = "Tot time: " + _time;
                numNodes.Text = "Num Nodes: " + _numNodes[index - 1];
                showSearchInfo(true);
            }
        }

        private void showSearchInfo(bool isVisible)
        {
            if (isVisible)
            {
                btnPrev.Visibility = Visibility.Visible;
                btnNext.Visibility = Visibility.Visible;
                numNodes.Visibility = Visibility.Visible;
                time.Visibility = Visibility.Visible;
                numSteps.Visibility = Visibility.Visible;
                steps.Visibility = Visibility.Visible;
            } else
            {
                btnPrev.Visibility = Visibility.Hidden;
                btnNext.Visibility = Visibility.Hidden;
                numNodes.Visibility = Visibility.Hidden;
                time.Visibility = Visibility.Hidden;
                numSteps.Visibility = Visibility.Hidden;
                steps.Visibility = Visibility.Hidden;
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
