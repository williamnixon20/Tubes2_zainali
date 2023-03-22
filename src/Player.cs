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
        protected List<Point> _exploredNodes;
        protected Maze _mazeMap;
        protected List<string> _playerDirectionState;
        protected bool _isGoalFinished;
        protected bool _branchPruningEnabled;

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
        public void BackupColoringState(List<string> _playerDirectionState)
        {
            this._mazeStateLog.Add(this._mazeStateTemplate);
            foreach (string route in _playerDirectionState)
            {
                int[,] colorState = this.GenerateColoringState(route);
                this._mazeStateLog.Add(colorState);
            }
            this._solutionRoute = _playerDirectionState.Last();
        }

        public string Log
        {
            get
            {
                StringBuilder log = new StringBuilder();

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

        /* EXPLORED-NODES METHODS */
        public void AddExploredNode(Point node)
        {
            this._exploredNodes.Add(node);
        }

        public int ExploredNodesCount
        {
            get { return this._exploredNodes.Count; }
        }

        public List<string> PlayerLog
        {
            get { return this._playerDirectionState; }
        }

        public bool IsNodeExplored(Point node)
        {
            return this._exploredNodes.Contains(node);
        }

        /* STATE-BACKUPS METHODS */
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