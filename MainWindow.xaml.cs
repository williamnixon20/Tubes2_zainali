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
        int _stateViewIndex = 1;
        public string _searchMode = "DFS";
        bool _tspToggle = false;
        public string _configFileName = "";
        Maze? _maze;
        List<Element>? _board;
        List<List<Element>> _states;
        List<string> _steps;
        List<string> _numNodes;
        List<string> _numSteps;
        string _displayTime;
        int _nGridRows = 1;
        int _nGridCols = 2;
        public int NRows
        {
            get { return _nGridRows; }
            set
            {
                _nGridRows = value;
                RaisePropertyChanged(nameof(NRows));
            }
        }
        public int NCols
        {
            get { return _nGridCols; }
            set
            {
                _nGridCols = value;
                RaisePropertyChanged(nameof(NCols));
            }
        }
        public MainWindow()
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
            DataContext = this;
            this._steps = new List<string>();
            this._states = new List<List<Element>>();
            this._numNodes = new List<string>();
            this._numSteps = new List<string>();
            this._displayTime = ""; 
        }

        private void fileTextChanged(object sender, TextChangedEventArgs e)
        {
            var template = fileInput.Template;
            var fileBox = (TextBox)template.FindName("FileBox", fileInput);
            _configFileName = fileBox.Text;
        }
        private void btnNextClick(object sender, RoutedEventArgs e)
        {
            if (_stateViewIndex + 1 < _states.Count())
            {
                _stateViewIndex += 1;
                Board.ItemsSource = _states.ElementAt(_stateViewIndex);
                numSteps.Text = "Num Steps: " + _numSteps[_stateViewIndex-1];
                steps.Text = "Steps: " + _steps[_stateViewIndex-1];
                numNodes.Text = "Num Nodes: " + _numNodes[_stateViewIndex-1];
            } else
            {
                MessageBox.Show("Sudah di state terakhir!");
            }
        }
        private void btnPrevClick(object sender, RoutedEventArgs e)
        {
            if (_stateViewIndex - 1 >= 1)
            {
                _stateViewIndex -= 1;
                Board.ItemsSource = _states.ElementAt(_stateViewIndex);
                numSteps.Text = "Num Steps: " + _numSteps[_stateViewIndex - 1];
                steps.Text = "Steps: " + _steps[_stateViewIndex-1];
                numNodes.Text = "Num Nodes: " + _numNodes[_stateViewIndex-1];
            } else
            {
                MessageBox.Show("Sudah di state paling awal!");
            }
        }
        private void RadioClick(object sender, RoutedEventArgs e)
        {
            var radio = (RadioButton)sender;
            _searchMode = (string)radio.Content;

        }
        private void TSPClick(object sender, RoutedEventArgs e)
        {
            if (_tspToggle == false)
            {
                _tspToggle = true;
            }
            else
            {
                _tspToggle = false;
            }
        }
        private void btnOpenFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _configFileName = openFileDialog.FileName;
                _stateViewIndex = 0;
                fileInput.Text = _configFileName;
            }
        }
        private void btnViz(object sender, RoutedEventArgs e)
        {
            try
            {
                _maze = new Maze("", _configFileName);
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
                if (_searchMode == "DFS")
                {
                    krustyKrab = new DFSPlayer(_maze, _tspToggle);
                }
                else
                {
                    krustyKrab = new BFSPlayer(_maze, _tspToggle);
                }
                krustyKrab.StartSearch();
                krustyKrab.BackupColoringState();
                logFile = krustyKrab.SaveLog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
                this._numNodes = krustyKrab._numNodes;
                this._numSteps = krustyKrab._numSteps;
                this._displayTime = krustyKrab._time.ToString();
                this._steps = krustyKrab._playerDirectionState;      


                LogReader baru = new LogReader(logFile);
                _states = baru._logBoard;
                _stateViewIndex = 1;
                fileInput.Text = _configFileName;
                Board.ItemsSource = _states.ElementAt(_stateViewIndex);
                numSteps.Text = "Num Steps: " + _numSteps[_stateViewIndex-1];
                steps.Text = "Steps: " + _steps[_stateViewIndex-1];
                time.Text = "Tot time: " + _displayTime;
                numNodes.Text = "Num Nodes: " + _numNodes[_stateViewIndex - 1];
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
        
        public event PropertyChangedEventHandler? PropertyChanged;
        void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
