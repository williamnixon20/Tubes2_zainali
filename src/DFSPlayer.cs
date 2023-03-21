using System.Drawing;
using System.Collections.Generic;
using System;

namespace Tubes2_zainali
{
    public class DFSPlayer : Player
    {
        private bool tsp = true;

        public void setTsp(bool _tsp)
        {
            tsp = _tsp;
        }
        public DFSPlayer(Maze loadedMaze) : base(loadedMaze)
        {
        }

        public void StartDFS()
        {
            RecurseDFS(this._mazeMap.GetStartPoint(), 0, "", "", tsp);
        }

        public void RecurseDFS(Point currentNode, int treasureCount, string routeTaken, string backtrackRoute, bool tsp)
        {
            if (!this._isGoalFinished)
            {
                this.BackupDirectionState(routeTaken);
                if (tsp == false && treasureCount == this._mazeMap.GetTreasureCount())
                {
                    this._isGoalFinished = true;
                    DeleteAfterLastState();
                    return;
                }
                else if (tsp == true && treasureCount == this._mazeMap.GetTreasureCount() && this._mazeMap.GetMazeTile(currentNode) == 'K')
                {
                    this._isGoalFinished = true;
                    return;
                }
                else
                {
                    if (this._mazeMap.GetMazeTile(currentNode) == 'T' && !this.IsNodeExplored(currentNode))   // found new treasure
                    {

                        treasureCount++;
                    }
                    this.AddExploredNode(currentNode);

                    List<Point> neighbors = this._mazeMap.GetNeighbors(currentNode);


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

                        if (!this.IsNodeExplored(neighbors[i]) && this._mazeMap.IsWalkable(neighbors[i]))
                        {
                            validNeighbors++;
                            string nextRoute = routeTaken + nextDirection;

                            RecurseDFS(neighbors[i], treasureCount, nextRoute, "", tsp);
                        }
                    }

                    if (validNeighbors == 0)  // mentok, do backtrack
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
                            RecurseDFS(nextPoint, treasureCount, routeTaken + rRoute[0], rRoute.Substring(1), tsp);
                        }
                        else
                        {
                            RecurseDFS(nextPoint, treasureCount, routeTaken + rRoute[0], "", tsp);
                        }
                    }

                }
            }
        }
        // TEST
        static void Main(string[] args)
        {
            string directory = "../test/";
            string filename = "sample.txt";
            Maze maze = new Maze(directory, filename);
            DFSPlayer d = new DFSPlayer(maze);
            d.StartDFS();
            d.printState();
        }

    }

}