using System.Drawing;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Tubes2_zainali
{
    public class TSPPlayer : DFSPlayer
    {
        private Point firstNode;

        public TSPPlayer(Maze loadedMaze, bool enableBranchPrune = true) : base(loadedMaze, enableBranchPrune)
        {
            firstNode = loadedMaze.StartPoint;
        }
        public void DFStoBack(Point currentNode, Point endNode, string routeTaken, string backtrackRoute)
        {
            if (!this._isGoalFinished)
            {
                this.BackupDirectionState(routeTaken);

                if (currentNode == endNode)
                {
                    this._isGoalFinished = true;
                    return;
                }
                else
                {
                    this.AddExploredNode(currentNode);

                    List<Point> neighbors = this._mazeMap.GetNeighbors(currentNode);
                    List<Point> validNeighbors = new List<Point>();
                    for (int i = 0; i < neighbors.Count; i++)
                    {
                        if (!this.IsNodeExplored(neighbors[i]) && this._mazeMap.IsWalkable(neighbors[i]))
                        {
                            validNeighbors.Add(neighbors[i]);
                        }
                    }

                    for (int i = 0; i < validNeighbors.Count; i++)
                    {
                        char nextDirection = Maze.GetDirectionBetween(currentNode, validNeighbors[i]);

                        string nextRoute = routeTaken + nextDirection;
                        DFStoBack(validNeighbors[i], endNode, nextRoute, "");

                    }

                    if (validNeighbors.Count == 0)  // do backtrack when current route has collected treasure; pruningEnabled => check treasure gain
                    {
                        string rRoute;
                        if (backtrackRoute != "")   // is already in backtrack mode
                        {
                            rRoute = backtrackRoute;
                        }
                        else
                        {
                            rRoute = Player.GenerateBacktrackRoute(routeTaken);
                        }

                        Point nextPoint = Maze.GetNextPoint(currentNode, rRoute[0]);
                        if (rRoute.Length > 1)
                        {

                            DFStoBack(nextPoint, endNode, routeTaken + rRoute[0], rRoute.Substring(1));
                        }
                        else
                        {

                            DFStoBack(nextPoint, endNode, routeTaken + rRoute[0], "");
                        }
                    }

                }
            }

        }

        public void StartTSPDFS()
        {
            TSPSolverForDFS(this._mazeMap.StartPoint, 0, 0, "", "");
        }
        public void TSPSolverForDFS(Point currentNode, int treasureCount, int treasureGain, string routeTaken, string backtrackRoute)
        {
            RecurseDFS(currentNode, treasureCount, treasureGain, routeTaken, backtrackRoute);
            this._isGoalFinished = false;
            routeTaken = this._playerDirectionState[this._playerDirectionState.Count - 1];
            currentNode = this._exploredNodes[this._exploredNodes.Count - 1];
            this._playerDirectionState.RemoveAt(this._playerDirectionState.Count - 1);
            this._exploredNodes.Clear();
            this._isGoalFinished = false;
            DFStoBack(currentNode, firstNode, routeTaken, "");
        }

        static void Main(string[] args)
        {
            // string directory = "../test/";
            // string filename = "peta.txt";
            // Maze maze = new Maze(directory, filename);
            // TSPPlayer t1 = new TSPPlayer(maze);
            // t1.StartTSPDFS();
            // t1.PrintState();
            // t1.BackupColoringState(t1.PlayerLog);
            // Console.Write(t1.Log);
            // t1.SaveLog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));

        }

    }
}
