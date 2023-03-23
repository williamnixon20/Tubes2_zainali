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
        bool _pruningToggle = false;
        public string _configFileName = "";
        Maze? _maze;
        List<Element>? _board;
        List<List<Element>> _states;
        List<string> _steps;
        List<string> _numNodes;
        List<string> _numSteps;
        string _totalTime;
        int _nGridRows = 1;
        int _nGridCols = 2;
        int _autoSpeed = 0;
        int SPEED_STATE = 9;
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
            this._totalTime = ""; 
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
        private void PruningClick(object sender, RoutedEventArgs e)
        {
            if (_pruningToggle == false)
            {
                _pruningToggle = true;
            } else
            {
                _pruningToggle = false;
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
                // True by default
                if (_pruningToggle == false)
                {
                    krustyKrab.setBranchPruning(_pruningToggle);
                }
                krustyKrab.StartSearch();
                krustyKrab.BackupColoringState();
                logFile = krustyKrab.SaveLog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));
                this._numNodes = krustyKrab._numNodes;
                this._numSteps = krustyKrab._numSteps;
                this._totalTime = krustyKrab._recordedSearchTime.ToString();
                this._steps = krustyKrab._playerDirectionState;      


                LogReader baru = new LogReader(logFile);
                _states = baru._logBoard;
                _stateViewIndex = 1;
                fileInput.Text = _configFileName;
                Board.ItemsSource = _states.ElementAt(_stateViewIndex);
                numSteps.Text = "Num Steps: " + _numSteps[_stateViewIndex-1];
                steps.Text = "Steps: " + _steps[_stateViewIndex-1];
                time.Text = "Tot time: " + _totalTime + " ms";
                numNodes.Text = "Num Nodes: " + _numNodes[_stateViewIndex - 1];
                showSearchInfo(true);
            }
        }
        private async void clickAuto(object sender, RoutedEventArgs e)
        {
            _autoSpeed = (_autoSpeed + 1) % SPEED_STATE;
            if (_autoSpeed == 0)
            {
                btnAuto.Content = "Auto : Off";
                return;
            } else
            {
                btnAuto.Content = "Auto : " + ((SPEED_STATE-1) / (float)_autoSpeed * 50).ToString() + " ms";
                await TransitionState(_autoSpeed);
            }
        }

        internal async Task TransitionState(int autoSpeed)
        {
            while (_autoSpeed == autoSpeed)
            {
                if (_stateViewIndex + 1 < _states.Count())
                {
                    _stateViewIndex += 1;
                    Board.ItemsSource = _states.ElementAt(_stateViewIndex);
                    numSteps.Text = "Num Steps: " + _numSteps[_stateViewIndex - 1];
                    steps.Text = "Steps: " + _steps[_stateViewIndex - 1];
                    numNodes.Text = "Num Nodes: " + _numNodes[_stateViewIndex - 1];
                }
                else
                {
                    _stateViewIndex = 1;
                }
                //textBox.Dispatcher.Invoke(() =>
                //{
                //    // UI operation goes inside of Invoke
                //    textBox.Text += ".";
                //    // Note that: 
                //    //    Dispatcher.Invoke() blocks the UI thread anyway
                //    //    but without it you can't modify UI objects from another thread
                //});

                // CPU-bound or I/O-bound operation goes outside of Invoke
                // await won't block UI thread, unless it's run in a synchronous context
                await Task.Delay((int)((SPEED_STATE - 1) / (float)_autoSpeed * 50));
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
                btnAuto.Visibility = Visibility.Visible;
            } else
            {
                btnPrev.Visibility = Visibility.Hidden;
                btnNext.Visibility = Visibility.Hidden;
                numNodes.Visibility = Visibility.Hidden;
                time.Visibility = Visibility.Hidden;
                numSteps.Visibility = Visibility.Hidden;
                steps.Visibility = Visibility.Hidden;
                btnAuto.Visibility = Visibility.Hidden;
                _autoSpeed = 0;
                btnAuto.Content = "Auto : Off";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
