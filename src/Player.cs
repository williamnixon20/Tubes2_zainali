using System.Linq;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System;
using System.IO;

namespace Tubes2_zainali
{
    public abstract class Player
    {
        /* Properties & State Fields */
        protected List<Point> _exploredNodes;
        protected Maze _mazeMap;
        public List<string> _playerDirectionState;
        public List<string> _numSteps;
        public List<string> _numNodes;
        public float _time;
        protected bool _isGoalFinished;
        protected bool _isTspStarted;
        protected bool _isTspFinished;

        /* Config Fields */
        protected bool _branchPruningEnabled;
        protected bool _tspEnabled;

        /* Logger-Utility Fields */
        private string _filename;
        private List<int[,]> _mazeStateLog;
        private string _solutionRoute;
        private int[,] _mazeStateTemplate;

        /* CTOR */
        public Player(Maze loadedMaze, bool enableBranchPrune = true)
        {
            this._exploredNodes = new List<Point>();
            this._mazeMap = loadedMaze;
            this._playerDirectionState = new List<String>();
            this._branchPruningEnabled = enableBranchPrune;

            this._filename = (DateTime.Now.ToString() + ".txt").Replace('/', '-').Replace(':', '-').Replace(' ', '_');
            this._mazeStateLog = new List<int[,]>();
            this._solutionRoute = "";
            this._mazeStateTemplate = new int[_mazeMap.RowCount, _mazeMap.ColCount];
            for (int i = 0; i < _mazeMap.RowCount; i++)
            {
                for (int j = 0; j < _mazeMap.ColCount; j++)
                {
                    if (_mazeMap.GetMazeTile(i, j) == 'X')
                    {
                        _mazeStateTemplate[i, j] = -1;
                    }
                    else
                    {
                        _mazeStateTemplate[i, j] = 0;
                    }
                }
            }
            this._numNodes = new List<string>();
            this._numSteps = new List<string>();
        }


        /* LOGGER METHODS */
        public string FileName
        {
            get { return this._filename; }
        }
        public int[,] GenerateColoringState(string directionsTaken)
        {
            int[,] colorState = (int[,])this._mazeStateTemplate.Clone();
            Point playerPosition = _mazeMap.StartPoint;
            Point currPoint = playerPosition;
            colorState[playerPosition.X, playerPosition.Y]++;
            foreach (char direction in directionsTaken)
            {
                playerPosition = Maze.GetNextPoint(playerPosition, direction);
                colorState[playerPosition.X, playerPosition.Y]++;
                currPoint = playerPosition;
            }
            colorState[currPoint.X, currPoint.Y] = -2;
            return colorState;
        }
        public void BackupColoringState()
        {
            this._mazeStateLog.Add(this._mazeStateTemplate);
            foreach (string route in this._playerDirectionState)
            {
                int[,] colorState = this.GenerateColoringState(route);
                this._mazeStateLog.Add(colorState);
            }
            this._solutionRoute = this._playerDirectionState.Last();
        }
        public int CountingNode(int[,] MazeState, int nRows, int nCols)
        {
            int count = 0;
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    if (MazeState[i, j] >= 1)
                    {
                        count += MazeState[i, j];
                    }
                }
            }
            return count;
        }

        public string Log
        {
            get
            {
                StringBuilder log = new StringBuilder();
                int[] NumOfSteps = new int[this._playerDirectionState.Count];
                for (int i = 0; i < this._playerDirectionState.Count; i++)
                {
                    NumOfSteps[i] = this._playerDirectionState[i].Length;
                }
                foreach (int[,] mazeColorState in _mazeStateLog)
                {
                    log.AppendLine('#'.ToString());
                    StringBuilder colorState = new StringBuilder();
                    for (int i = 0; i < _mazeMap.RowCount; i++)
                    {
                        for (int j = 0; j < _mazeMap.ColCount; j++)
                        {
                            colorState.Append(_mazeMap.GetMazeTile(i, j));
                            colorState.Append(mazeColorState[i, j]);
                            colorState.Append(' ');
                        }
                        colorState.AppendLine();
                    }
                    log.AppendLine(colorState.ToString());
                }
                log.AppendLine("$");
                //for (int i = 0; i < this._playerDirectionState.Count; i++)
                //{
                //    log.AppendLine(_playerDirectionState[i].ToString());
                //}
                //log.AppendLine("STEPS");
                _numSteps = new List<string>();
                for (int i = 0; i < this._playerDirectionState.Count; i++)
                {
                    _numSteps.Add(NumOfSteps[i].ToString());
                }
                //log.AppendLine("NODES");
                _numNodes = new List<string>();
                for (int i = 0; i < this._playerDirectionState.Count; i++)
                {
                    _numNodes.Add((i + 1).ToString());
                }

                return log.ToString();
            }
        }
        public string SaveLog(string directory)
        {
            string path = Path.Combine(directory, this.FileName);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(this.Log);
            }
            return path;
        }


        /* GET PLAYER CONFIG: branch-pruning, tsp */
        public bool IsBranchPruningEnabled
        {
            get { return this._branchPruningEnabled; }
        }
        public bool IsTspEnabled
        {
            get { return this._tspEnabled; }
        }


        /* EXPLORED-NODES METHODS */
        public void AddExploredNode(Point node)
        {
            this._exploredNodes.Add(node);
        }
        public int ExploredNodesCount
        {
            get { return this._exploredNodes.Count; }
        }
        public bool IsNodeExplored(Point node)
        {
            return this._exploredNodes.Contains(node);
        }


        /* DIRECTION-STATE BACKUP METHODS */
        public void BackupDirectionState(string directions)
        {
            this._playerDirectionState.Add(directions);
        }
        public void DeleteLastState()
        {
            this._playerDirectionState.RemoveAt(this.BackupCount - 1);
            this._exploredNodes.RemoveAt(this.ExploredNodesCount - 1);
        }
        public string GetStateBackup(int i)
        {
            return this._playerDirectionState[i];
        }
        public void PrintState()
        {
            for (int i = 0; i < this._playerDirectionState.Count; i++)
            {
                Console.WriteLine(GetStateBackup(i));
            }
        }
        public int BackupCount
        {
            get { return this._playerDirectionState.Count; }
        }


        /* BACKTRACK ROUTING */
        static public string GenerateBacktrackRoute(string route)
        {
            char[] reversedRoute = route.ToCharArray();
            Array.Reverse(reversedRoute);
            string backtrack = new string(reversedRoute);

            return backtrack.Replace('L', 'r').Replace('R', 'l').Replace('U', 'd').Replace('D', 'u').ToUpper();
        }

        /* SOLVER */
        public abstract void StartSearch();
    }
}