using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Tubes2_zainali
{
    public class BFSPlayer : Player
    {
        // CTOR
        public BFSPlayer(Maze loadedMaze, bool enableTsp, bool enableBranchPrune = true) : base(loadedMaze, enableBranchPrune)
        {
            this._tspEnabled = enableTsp;
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
            private List<Point> _tspSteps;

            public BFSPoint(Point node, string movementHistory, int treasure, int branchGain, int backtrackFlag)
            {
                this._currentPosition = new Point(node.X, node.Y);
                this._movementSteps = movementHistory;
                this._treasureCount = treasure;
                this._branchTreasureGain = branchGain;
                this._backtrackFlag = backtrackFlag;
                this._pointSteps = new List<Point>();
                this._tspSteps = new List<Point>();
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

            public void AddSelfAsTspStep()
            {
                this._tspSteps.Add(this.Point);
            }

            public bool HasExploredBfsStep(Point step)
            {
                return this._pointSteps.Contains(step);
            }

            public bool HasExploredTspStep(Point step)
            {
                return this._tspSteps.Contains(step);
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
        public override void StartSearch()
        {
            IterateBFS(this._mazeMap.StartPoint);
        }

        public void IterateBFS(Point startNode)
        {
            BFSPoint mazeStart = new BFSPoint(this._mazeMap.StartPoint, "", 0, 0, 0);
            mazeStart.AddSelfAsStep();
            Queue<BFSPoint> searchQueue = new Queue<BFSPoint>();

            searchQueue.Enqueue(mazeStart);

            while (searchQueue.Any() && (!(!this._isTspStarted) || !this._isGoalFinished) && (!this._isTspStarted || !this._isTspFinished))
            {
                /* SETUP */
                BFSPoint currentNode = searchQueue.Dequeue();
                this.BackupDirectionState(currentNode.Steps);
                this.AddExploredNode(currentNode.Point);        // mark point as visited in Player

                if (this._mazeMap.GetMazeTile(currentNode.Point) == 'T' && !currentNode.HasExploredBfsStep(currentNode.Point))
                {
                    currentNode.TreasureCount++;
                    currentNode.BranchGain++;
                }
                currentNode.AddSelfAsStep();    // mark self as visited bfs step in currentNode BFSPoint
                if (this._isTspStarted)         // mark self as visited tsp step in currentNode BFSPoint
                {
                    currentNode.AddSelfAsTspStep();
                }

                /* GOAL CHECK */
                // check for goal treasure count when not in TSP mode
                if (currentNode.TreasureCount == this._mazeMap.TreasureCount && !this._isTspStarted)
                {
                    Console.WriteLine("bfs selesai");
                    this._isGoalFinished = true;
                    if (this.IsTspEnabled)      // reset searchQueue, redo BFS to StartPoint
                    {
                        searchQueue.Clear();
                        searchQueue.Enqueue(currentNode);
                        this._isTspStarted = true;
                        this.DeleteLastState();
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                // TSP: check if current point is startpoint
                else if (this._isGoalFinished && this._isTspStarted && currentNode.Point == this._mazeMap.StartPoint)
                {
                    Console.WriteLine("tsp selesai");
                    this._isTspFinished = true;
                    break;
                }

                /* BFS ROUTINE: goal (bfs or tsp) is unsatisfied */
                // get valid neighbors
                List<Point> neighbors = this._mazeMap.GetNeighbors(currentNode.Point);
                List<Point> validNeighbors = new List<Point>();
                for (int i = 0; i < neighbors.Count; i++)
                {
                    // restrict validNeighbors to walkable nodes; and if not isTspStarted => restrict validNeighbors to unexplored nodes from BFSPoint; and if TspStarted => restrict to unvisited from new startpoint
                    if (this._mazeMap.IsWalkable(neighbors[i]) && (!(!this._isTspStarted) || !currentNode.HasExploredBfsStep(neighbors[i])) && (!this._isTspStarted || !currentNode.HasExploredTspStep(neighbors[i])))
                    {
                        validNeighbors.Add(neighbors[i]);
                    }
                }

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
                // Backtracking: queue backtrack when current route has collected treasure; pruningEnabled => check treasure gain
                //               disable backtracking when doing TSP
                if (validNeighbors.Count == 0 && (!this.IsBranchPruningEnabled || currentNode.BranchGain != 0) && !this._isTspStarted)
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