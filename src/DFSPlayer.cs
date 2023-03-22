using System.Drawing;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Tubes2_zainali
{
    public class DFSPlayer : Player
    {
        // CTOR

        public DFSPlayer(Maze loadedMaze, bool enableBranchPrune = true) : base(loadedMaze, enableBranchPrune)
        {

        }

        /* DFS Solution Methods */
        public void StartDFS()
        {
            RecurseDFS(this._mazeMap.StartPoint, 0, 0, "", "");
        }

        public void RecurseDFS(Point currentNode, int treasureCount, int treasureGain, string routeTaken, string backtrackRoute)
        {

            if (!this._isGoalFinished)
            {
                this.BackupDirectionState(routeTaken);

                if (treasureCount == this._mazeMap.TreasureCount)
                {
                    this._isGoalFinished = true;
                    DeleteAfterLastState();
                    return;
                }
                else
                {
                    if (this._mazeMap.GetMazeTile(currentNode) == 'T' && !this.IsNodeExplored(currentNode))   // found new treasure
                    {
                        treasureCount++;
                        treasureGain++;
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
                        int branchGain;
                        if (validNeighbors.Count > 1)
                        {
                            branchGain = 0;
                        }
                        else
                        {
                            branchGain = treasureGain;
                        }

                        RecurseDFS(validNeighbors[i], treasureCount, branchGain, nextRoute, "");

                    }

                    if (validNeighbors.Count == 0 && (!this.IsBranchPruningEnabled || treasureGain != 0))  // do backtrack when current route has collected treasure; pruningEnabled => check treasure gain
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
                            RecurseDFS(nextPoint, treasureCount, treasureGain, routeTaken + rRoute[0], rRoute.Substring(1));
                        }
                        else
                        {
                            RecurseDFS(nextPoint, treasureCount, treasureGain, routeTaken + rRoute[0], "");
                        }
                    }

                }
            }
        }

        public static void NewFileOutput()
        {
            StreamWriter sw = new StreamWriter("output.txt");
            sw.Close();

        }
        public static void AppendFileOutput(string s)
        {
            using (StreamWriter sw = File.AppendText("output.txt"))
            {
                sw.WriteLine(s);
            }
        }
        // TEST
        // static void Main(string[] args)
        // {
        //     string directory = "../test/";
        //     string filename = "peta.txt";
        //     Maze maze = new Maze(directory, filename);
        //     DFSPlayer d = new DFSPlayer(maze);
        //     d.StartDFS();
        //     d.PrintState();
        //     d.BackupColoringState(d.PlayerLog);
        //     Console.Write(d.Log);
        //     d.SaveLog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));



        // }

    }

}