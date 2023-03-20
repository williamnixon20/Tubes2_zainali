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
        protected HashSet<Point> _collectedTreasure;
        protected Maze _mazeMap;
        protected List<string> _playerDirectionState;
        protected bool _isGoalFinished;

        /* CTOR */
        public Player(Maze loadedMaze)
        {
            this._exploredNodes = new List<Point>();
            this._collectedTreasure = new HashSet<Point>();
            this._mazeMap = loadedMaze;
            this._playerDirectionState = new List<String>();
        }

        /* EXPLORED-NODES METHODS */
        public void AddExploredNode(Point node)
        {
            this._exploredNodes.Add(node);
        }

        public int GetNodeExploredCount()
        {
            return this._exploredNodes.Count;
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

        public string GetStateBackup(int i)
        {
            return this._playerDirectionState[i];
        }

        public int GetBackupCount()
        {
            return this._playerDirectionState.Count;
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