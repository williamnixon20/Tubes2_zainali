using System.Drawing;

namespace MazeGame
{
    public class DFSPlayer : Player
    {
        public DFSPlayer(Maze loadedMaze) : base(loadedMaze)
        {
        }

        public void StartDFS()
        {
            RecurseDFS(this.mazeMap.GetStartPoint(), 0, "", "");
        }

        public void RecurseDFS(Point currentNode, int treasureCount, string routeTaken, string backtrackRoute)
        {
            if (!this.isGoalFinished)
            {
                this.BackupDirectionState(routeTaken);
                if (treasureCount == this.mazeMap.GetTreasureCount() - 1 && this.mazeMap.GetMazeTile(currentNode) == 'T')
                {
                    this.isGoalFinished = true;
                    return;
                }
                else
                {
                    this.AddExploredNode(currentNode);

                    List<Point> neighbors = this.mazeMap.GetNeighbors(currentNode);

                    if (this.mazeMap.GetMazeTile(currentNode) == 'T')   // found new treasure
                    {
                        treasureCount++;
                    }


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

                        if (!this.IsNodeExplored(neighbors[i]) && this.mazeMap.IsWalkable(neighbors[i]))
                        {
                            validNeighbors++;
                            string nextRoute = routeTaken + nextDirection;
                            RecurseDFS(neighbors[i], treasureCount, nextRoute, "");
                        }
                    }

                    // if (validNeighbors == 0)  // mentok, do backtrack
                    // {
                    //     string rRoute = Player.GenerateBacktrackRoute(routeTaken);
                    //     // Console.WriteLine(rRoute);
                    //     Point nextPoint = Maze.GetNextPoint(currentNode, rRoute[0]);
                    //     if (rRoute.Length > 1)
                    //     {
                    //         RecurseDFS(nextPoint, treasureCount, routeTaken + rRoute[0], rRoute.Substring(1));
                    //     }
                    //     else
                    //     {
                    //         RecurseDFS(nextPoint, treasureCount, routeTaken + rRoute[0], "");
                    //     }
                    // }

                }
            }
        }
        
    }
    
}