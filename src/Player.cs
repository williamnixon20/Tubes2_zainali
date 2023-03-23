using System.Linq;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System;
using System.IO;

namespace Tubes2_zainali
{
    public class Player
    {
        /* Properties & State Fields */
        protected List<Point> _exploredNodes;
        protected List<string> _playerDirectionState;
        protected bool _isGoalFinished;
        protected bool _isTspStarted;
        protected bool _isTspFinished;

        /* Config Fields */
        protected Maze _mazeMap;
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

            colorState[playerPosition.X, playerPosition.Y]++;
            foreach (char direction in directionsTaken)
            {
                playerPosition = Maze.GetNextPoint(playerPosition, direction);
                colorState[playerPosition.X, playerPosition.Y]++;
            }
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
                            colorState.Append(mazeColorState[i, j]);
                            colorState.Append(' ');
                        }
                        colorState.AppendLine();
                    }
                    log.AppendLine(colorState.ToString());
                }
                log.AppendLine("$");
                log.AppendLine(this._solutionRoute);
                log.AppendLine("STEPS");
                log.AppendLine("0");
                for (int i = 0; i < this._playerDirectionState.Count; i++)
                {
                    log.AppendLine(NumOfSteps[i].ToString());
                }
                log.AppendLine("NODES");
                for (int i = 0; i < this._playerDirectionState.Count; i++)
                {
                    log.AppendLine((i + 1).ToString());
                }

                return log.ToString();
            }
        }
        public void SaveLog(string directory)
        {
            string path = Path.Combine(directory, this.FileName);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(this.Log);
            }
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
        public void DeleteAfterLastState()
        {
            this._playerDirectionState.RemoveAt(this.BackupCount - 1);
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
    }
}