using System.Linq;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System;
namespace Tubes2_zainali
{
    public class Player
    {
        protected List<Point> _exploredNodes;
        protected Maze _mazeMap;
        protected List<string> _playerDirectionState;
        protected bool _isGoalFinished;
        protected bool _branchPruningEnabled;

        /* CTOR */
        public Player(Maze loadedMaze, bool enableBranchPrune = true)
        {
            this._exploredNodes = new List<Point>();
            this._mazeMap = loadedMaze;
            this._playerDirectionState = new List<String>();
            this._branchPruningEnabled = enableBranchPrune;
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