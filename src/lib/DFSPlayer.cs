using System.Drawing;
using System.Collections.Generic;
using System;
using System.IO;
using System.Diagnostics;

namespace Tubes2_zainali
{
    public class DFSPlayer : Player
    {
        List<Point> _tspVisits;

        /* CTOR */
        public DFSPlayer(Maze loadedMaze, bool enableTsp, bool enableBranchPrune = true) : base(loadedMaze, enableBranchPrune)
        {
            this._tspEnabled = enableTsp;
            this._tspVisits = new List<Point>();
        }
        

        /* DFS Solution Methods */
        public override void StartSearch()
        {
            Stopwatch searchTimer = new Stopwatch();
            searchTimer.Start();
            RecurseDFS(this._mazeMap.StartPoint, 0, 0, "", "");
            if (this.IsTspEnabled)
            {
                string route = this.GetStateBackup(this.BackupCount - 1);
                Point newStart = Maze.GetNextPoint(this._mazeMap.StartPoint, route);
                this._isTspStarted = true;

                DeleteLastState();     // remove duplicate step from before TSP
                RecurseDFS(newStart, this._mazeMap.TreasureCount, 0, route, "");
            }
            searchTimer.Stop();
            this._recordedSearchTime = searchTimer.ElapsedMilliseconds;
        }

        public void RecurseDFS(Point currentNode, int treasureCount, int treasureGain, string routeTaken, string backtrackRoute)
        {
            /* DFS STOP CONDITION: clears call stack on finish */
            // if TSP is not started, continue DFS until goal is finished
            // if TSP started, continue DFS until TSP marked finished
            if ((!(!this._isTspStarted) || !this._isGoalFinished) && (!this._isTspStarted || !this._isTspFinished))
            {
                /* SETUP */
                this.BackupDirectionState(routeTaken);

                if (this._mazeMap.GetMazeTile(currentNode) == 'T' && !this.IsNodeExplored(currentNode))   // found new treasure
                {
                    treasureCount++;
                    treasureGain++;
                }

                this.AddExploredNode(currentNode);      // mark currentNode as visited
                if (this._isTspStarted)
                {
                    this._tspVisits.Add(currentNode);
                }

                /* GOAL CHECK */
                // check for goal treasure count when not in TSP mode
                if (!this._isGoalFinished && treasureCount == this._mazeMap.TreasureCount && !this._isTspStarted)
                {
                    Console.WriteLine("dfs siap");
                    this._isGoalFinished = true;
                    return;
                }
                // TSP: check if current point is startpoint
                else if (this._isGoalFinished && this._isTspStarted && currentNode == this._mazeMap.StartPoint)
                {
                    Console.WriteLine("tsp siap");
                    this._isTspFinished = true;
                    return;
                }

                /* DFS ROUTINE: goal (dfs or tsp) is unsatisfied */
                else
                {
                    List<Point> neighbors = this._mazeMap.GetNeighbors(currentNode);
                    List<Point> validNeighbors = new List<Point>();
                    for (int i = 0; i < neighbors.Count; i++)
                    {
                        // restrict validNeighbors to walkable nodes; and if not isTspStarted => restrict validNeighbors to unexplored nodes; and if TspStarted => restrict to unvisited from new startpoint
                        if (this._mazeMap.IsWalkable(neighbors[i]) && (!(!this._isTspStarted) || !this.IsNodeExplored(neighbors[i])) && (!this._isTspStarted || !this._tspVisits.Contains(neighbors[i])))
                        {
                            validNeighbors.Add(neighbors[i]);
                        }
                    }


                    for (int i = 0; i < validNeighbors.Count; i++)
                    {
                        char nextDirection = Maze.GetDirectionBetween(currentNode, validNeighbors[i]);

                        string nextRoute = routeTaken + nextDirection;
                        int branchGain;
                        if (validNeighbors.Count > 1 && this._mazeMap.GetMazeTile(currentNode) != 'T')
                        {
                            branchGain = 0;
                        }
                        else
                        {
                            branchGain = treasureGain;
                        }

                        RecurseDFS(validNeighbors[i], treasureCount, branchGain, nextRoute, "");

                    }

                    // Backtracking: do backtrack when current route has collected treasure; pruningEnabled => check treasure gain
                    //               disable backtracking when doing TSP
                    if (validNeighbors.Count == 0 && (!this.IsBranchPruningEnabled || treasureGain != 0) && !this._isTspStarted)  
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
    }

}