using System.Linq;
using System.Drawing;
using System.Text;

namespace MazeGame
{   
    public class Player
    {
        protected List<Point> exploredNodes;
        protected Maze mazeMap;
        protected List<string> playerDirectionState;
        protected bool isGoalFinished;

        public Player(Maze loadedMaze)
        {
            this.exploredNodes = new List<Point>();
            this.mazeMap = loadedMaze;
            this.playerDirectionState = new List<String>();
        }

        public void AddExploredNode(Point node)
        {
            this.exploredNodes.Add(node);
        }

        public int GetNodeExploredCount()
        {
            return this.exploredNodes.Count;
        }

        public bool IsNodeExplored(Point node)
        {
            return this.exploredNodes.Contains(node);
        }

        public void BackupDirectionState(string directions)
        {
            this.playerDirectionState.Add(directions);
        }

        public string GetStateBackup(int i)
        {
            return this.playerDirectionState[i];
        }

        public int GetBackupCount()
        {
            return this.playerDirectionState.Count;
        }

        static public string GenerateBacktrackRoute(string route)
        {
            char[] reversedRoute = route.ToCharArray();
            Array.Reverse(reversedRoute);
            string backtrack = new string(reversedRoute);

            return backtrack.Replace('L', 'r').Replace('R', 'l').Replace('U', 'd').Replace('D', 'u').ToUpper();
        }
    }
}