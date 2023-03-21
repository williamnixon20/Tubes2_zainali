using System;
using System.Drawing;

namespace Tubes2_zainali
{
    public class BFSPlayer : Player
    {
        // CTOR
        public BFSPlayer(Maze loadedMaze) : base(loadedMaze)
        {
        }

        /* INNER CLASS: BFSPoint */
        public class BFSPoint
        {
            private Point _currentPosition;
            private string _movementSteps;
            private int _treasureCount;
            private List<Point> _pointSteps;

            public BFSPoint(Point node, string movementHistory, int treasure)
            {
                this._currentPosition = new Point(node.X, node.Y);
                this._movementSteps = movementHistory;
                this._treasureCount = treasure;
                this._pointSteps = new List<Point>();
            }

            public BFSPoint(Point node, string movementHistory, int treasure, List<Point> pointsteps)
            {
                this._currentPosition = new Point(node.X, node.Y);
                this._movementSteps = movementHistory;
                this._treasureCount = treasure;
                this._pointSteps = new List<Point>();
                foreach (Point step in pointsteps)
                {
                    this._pointSteps.Add(step);   
                }
            }

            public Point Point
            {
                get { return this._currentPosition; }
            }

            public string Steps
            {
                get { return this._movementSteps; }
            }

            public int TreasureCount
            {
                get { return this._treasureCount; }
                set { this._treasureCount = value; }
            }

            public List<Point> PointSteps
            {
                get { return this._pointSteps; }
            }

            public void AddSelfAsStep()
            {
                this._pointSteps.Add(this.Point);
            }

            public bool IsAnExploredStep(Point step)
            {
                return this._pointSteps.Contains(step);
            }
        }
        
        /* BFS Solution Methods */
        public void StartBFS()
        {
            IterateBFS(this._mazeMap.StartPoint);
        }

        public void IterateBFS(Point startNode)
        {
            BFSPoint mazeStart = new BFSPoint(this._mazeMap.StartPoint, "", 0);
            mazeStart.AddSelfAsStep();
            Queue<BFSPoint> searchQueue = new Queue<BFSPoint>();

            searchQueue.Enqueue(mazeStart);

            while (searchQueue.Any() && !this._isGoalFinished)
            {
                BFSPoint currentNode = searchQueue.Dequeue();
                this.BackupDirectionState(currentNode.Steps);
                this.AddExploredNode(currentNode.Point);

                // new treasure check
                if (this._mazeMap.GetMazeTile(currentNode.Point) == 'T' && !currentNode.IsAnExploredStep(currentNode.Point))
                {
                    currentNode.TreasureCount++;
                }

                // goal check
                if (currentNode.TreasureCount == this._mazeMap.TreasureCount)
                {
                    this._isGoalFinished = true;
                    break;
                }


                List<Point> neighbors = this._mazeMap.GetNeighbors(currentNode.Point);
                int validNeighbors = 0;
                for (int i = 0; i < neighbors.Count; i++)
                {
                    char nextDirection = 'X';
                    switch (i)
                    {
                        case 0:     // L
                            nextDirection = 'L';
                            break;
                        case 1:     // R
                            nextDirection = 'R';
                            break;
                        case 2:     // U
                            nextDirection = 'U';
                            break;
                        case 3:     // D
                            nextDirection = 'D';
                            break;
                    }

                    if (this._mazeMap.IsWalkable(neighbors[i]) && !currentNode.IsAnExploredStep(neighbors[i]))
                    {
                        validNeighbors++;
                        string route = currentNode.Steps + nextDirection;

                        currentNode.AddSelfAsStep();    // mark self as a visited node in Point Self history, add neighbor to queue
                        BFSPoint n = new BFSPoint(neighbors[i], route, currentNode.TreasureCount, currentNode.PointSteps);
                        searchQueue.Enqueue(n);                    
                    }
                }

                // if (validNeighbors == 0)        // todo: backtrack
                // {

                // }

                
            }
        }
    }
}