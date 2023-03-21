using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Tubes2_zainali
{
    public class BFSPlayer : Player
    {
        // CTOR
        public BFSPlayer(Maze loadedMaze, bool enableBranchPrune=true) : base(loadedMaze, enableBranchPrune)
        {
        }

        /* INNER CLASS: BFSPoint */
        public class BFSPoint
        {
            private Point _currentPosition;
            private string _movementSteps;
            private int _treasureCount;
            private int _branchTreasureGain;
            private int _backtrackFlag;
            private List<Point> _pointSteps;

            public BFSPoint(Point node, string movementHistory, int treasure, int branchGain, int backtrackFlag)
            {
                this._currentPosition = new Point(node.X, node.Y);
                this._movementSteps = movementHistory;
                this._treasureCount = treasure;
                this._branchTreasureGain = branchGain;
                this._backtrackFlag = backtrackFlag;
                this._pointSteps = new List<Point>();
            }

            public BFSPoint(Point node, string movementHistory, int treasure, List<Point> pointsteps, int branchGain, int backtrackFlag) : this(node, movementHistory, treasure, branchGain, backtrackFlag)
            {
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

            public int BranchGain
            {
                get { return this._branchTreasureGain; }
                set { this._branchTreasureGain = value; }
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

            public bool IsFlagInactive
            {
                get { return this._backtrackFlag == 0; }
            }

            public int BacktrackFlag
            {
                get { return this._backtrackFlag; }
            }

            public void IncrementBacktrackFlag()
            {
                this._backtrackFlag += 2;
            }

            public char NextBackStep
            {
                get
                {
                    return Player.GenerateBacktrackRoute(this.Steps.Substring(0, this.Steps.Length - this._backtrackFlag))[0]; 
                }
            }
        }
        
        /* BFS Solution Methods */
        public void StartBFS()
        {
            IterateBFS(this._mazeMap.StartPoint);
        }

        public void IterateBFS(Point startNode)
        {
            BFSPoint mazeStart = new BFSPoint(this._mazeMap.StartPoint, "", 0, 0, 0);
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
                    currentNode.BranchGain++;
                }

                // goal check
                if (currentNode.TreasureCount == this._mazeMap.TreasureCount)
                {
                    this._isGoalFinished = true;
                    break;
                }

                // get valid neighbors
                List<Point> neighbors = this._mazeMap.GetNeighbors(currentNode.Point);
                List<Point> validNeighbors = new List<Point>();
                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (!currentNode.IsAnExploredStep(neighbors[i]) && this._mazeMap.IsWalkable(neighbors[i]))
                    {
                        validNeighbors.Add(neighbors[i]);
                    }
                }

                currentNode.AddSelfAsStep();    // mark self as a visited node in self's Point history
                for (int i = 0; i < validNeighbors.Count; i++)
                {
                    char nextDirection = Maze.GetDirectionBetween(currentNode.Point, validNeighbors[i]);

                    int nextBranchGain;
                    if (validNeighbors.Count > 1)
                    {
                        nextBranchGain = 0;
                    }
                    else
                    {
                        nextBranchGain = currentNode.BranchGain;
                    }

                    string route = currentNode.Steps + nextDirection;

                    BFSPoint n = new BFSPoint(validNeighbors[i], route, currentNode.TreasureCount, currentNode.PointSteps, nextBranchGain, 0);
                    searchQueue.Enqueue(n);
                }
                
                if (validNeighbors.Count == 0 && (!this.IsBranchPruningEnabled || currentNode.BranchGain != 0))        // if stuck, do backtrack until adjacent unexplored branch found
                {
                    char backStep = currentNode.NextBackStep;
                    Point nextPoint = Maze.GetNextPoint(currentNode.Point, backStep);
                    BFSPoint nextSearchPoint = new BFSPoint(nextPoint, currentNode.Steps + backStep, currentNode.TreasureCount, currentNode.PointSteps, currentNode.BranchGain, currentNode.BacktrackFlag);
                    nextSearchPoint.IncrementBacktrackFlag();

                    searchQueue.Enqueue(nextSearchPoint);
                }

                
            }
        }
    }
}