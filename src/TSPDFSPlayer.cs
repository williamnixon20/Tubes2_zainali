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
        public void DFStoBack(Point currentNode, Point endNode, string routeTaken, string backtrackRoute, int count)
        {
            if (!this._isGoalFinished)
            {
                this.BackupDirectionState(routeTaken);

                if (currentNode == endNode)
                {
                    this._isGoalFinished = true;
                    // DeleteAfterLastState();
                    return;
                }
                else
                {
                    if (count > 0)
                    {
                        VisitNode(currentNode.X, currentNode.Y);
                        StringCurrentMazeCondition();
                    }

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
                        count++;
                        DFStoBack(validNeighbors[i], endNode, nextRoute, "", count);

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

                        count++;
                        if (rRoute.Length > 1)
                        {

                            DFStoBack(nextPoint, endNode, routeTaken + rRoute[0], rRoute.Substring(1), count);
                        }
                        else
                        {

                            DFStoBack(nextPoint, endNode, routeTaken + rRoute[0], "", count);
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
            int count = 0;
            RecurseDFS(currentNode, treasureCount, treasureGain, routeTaken, backtrackRoute);
            this._isGoalFinished = false;
            routeTaken = this._playerDirectionState[this._playerDirectionState.Count - 1];
            currentNode = this._exploredNodes[this._exploredNodes.Count - 1];
            this._exploredNodes.Clear();
            this._isGoalFinished = false;
            DFStoBack(currentNode, firstNode, routeTaken, "", count);
        }

        static void Main(string[] args)
        {
            // NewFileOutput();
            // string directory = "../test/";
            // string filename = "peta.txt";
            // Maze maze = new Maze(directory, filename);
            // TSPPlayer t = new TSPPlayer(maze);
            // t.StartTSPDFS();
            // t.PrintState();
            // AppendFileOutput(GetStringFile());
            // AppendFileOutput(t.GetStateBackup(t._playerDirectionState.Count - 1));   // langkah
            // AppendFileOutput((t.GetStateBackup(t._playerDirectionState.Count - 1).Length).ToString());



        }

    }
}
