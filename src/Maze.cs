using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.IO;
namespace Tubes2_zainali
{
    public class Maze
    {
        private List<List<char>> mapMatrix;
        private int nRows;
        private int nCols;
        private Point mazeStart;
        private HashSet<Point> treasureSet;


        public Maze(string mapConfig)
        {
            mapConfig = mapConfig.TrimEnd('\r', '\n', ' ');      // remove trailing newline from config text
            this.mapMatrix = new List<List<char>>();
            this.mazeStart = new Point(-1, -1);
            this.treasureSet = new HashSet<Point>();
            
            // Check syntax of file
            string mapConfigSyntax = @"^([KTRX][\s\n])+$";
            if (!Regex.IsMatch(mapConfig, mapConfigSyntax, RegexOptions.Multiline))
            {
                throw new MapFileException("Map Configuration File Has Illegal Characters.");
            } 

            // Process map matrix
            string[] mapRows = mapConfig.Split("\n");
            int colCount = -1;
            for (int i = 0; i < mapRows.Length; i++)
            {
                string row = mapRows[i];
                string[] mapColumns = row.Trim().Split(" ");

                if (colCount == -1)     // undeclared column length
                {
                    colCount = mapColumns.Length;
                }
                else if (row == "")
                {
                    if (i != mapRows.Length - 1)
                    {
                        throw new MapFileException("Map Configuration File Must Have Rows Split by Only a Newline.");
                    }
                    break;
                }
                else if (mapColumns.Length != colCount)     // a column is not equal in length to previous columns
                {
                    throw new MapFileException("Map Configuration File Must Have Equal Number of Tiles Separated by a Whitespace in Each Row.");
                }

                // process row, add to mapMatrix
                List<char> currentRow = new List<char>();
                for (int j = 0; j < mapColumns.Length; j++)
                {
                    string tile = mapColumns[j];
                    currentRow.Add(tile[0]);

                    if (tile == "K")    // check start tile "K"
                    {
                        if (this.mazeStart.X == -1 && this.mazeStart.Y == -1)   // uninitialized start
                        {
                            this.mazeStart = new Point(i, j);
                        } 
                        else
                        {
                            throw new MapFileException("Map Configuration has multiple start points.");
                        }
                    }

                    if (tile == "T")    // list treasures
                    {
                        this.treasureSet.Add(new Point(i, j));
                    }
                }

                this.mapMatrix.Add(currentRow);
            }

            this.nRows = this.mapMatrix.Count;
            this.nCols = colCount;
        }

        public Maze(string directory, string fileName) : this(File.ReadAllText(Path.Combine(directory, fileName))) 
        {
        }

        public int GetRowCount()
        {
            return this.nRows;
        }

        public int GetColCount()
        {
            return this.nCols;
        }

        public char GetMazeTile(int i, int j)
        {
            if (i < 0 || i >= this.nRows || j < 0 || j >= this.nCols)       // Point ot of bounds
            {
                return 'X';
            } else
            {
                return this.mapMatrix[i][j];
            }
        }

        public char GetMazeTile(Point tileCoordinate)
        {
            return this.GetMazeTile(tileCoordinate.X, tileCoordinate.Y);
        }

        public List<Point> GetNeighbors(int i, int j)
        {
            List<Point> neighbors = new List<Point>();

            // NEIGHBOR PRIORITY: L R U D
            Point left = new Point(i, j - 1);
            Point right = new Point(i, j + 1);
            Point up = new Point(i - 1, j);
            Point down = new Point(i + 1, j);

            neighbors.Add(left);
            neighbors.Add(right);
            neighbors.Add(up);
            neighbors.Add(down);

            return neighbors;
        }

        public List<Point> GetNeighbors(Point tile)
        {
            return this.GetNeighbors(tile.X, tile.Y);
        }

        public Point GetStartPoint()
        {
            return this.mazeStart;
        }

        public int GetTreasureCount()
        {
            return this.treasureSet.Count;
        }
        
        public void PrintMazeInfo()
        {
            foreach (List<char> row in this.mapMatrix)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    Console.Write(row[i]);

                    if (i < row.Count - 1)
                    {
                        Console.Write(' ');
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                }
            }
            Console.Write("Rows : ");
            Console.WriteLine(this.GetRowCount());

            Console.Write("Columns : ");
            Console.WriteLine(this.GetColCount());
            
            Console.Write("Start Point : ");
            Console.WriteLine(this.mazeStart.ToString());

            Console.Write("Treasure Count : ");
            Console.WriteLine(this.treasureSet.Count);
            foreach (Point treasure in treasureSet)
            {
                Console.Write("  ");
                Console.WriteLine(treasure.ToString());
            }
        }

        static public Point GetNextPoint(Point currentPoint, char direction)
        {
            // assume next direction is valid
            Point nextPoint;
            switch (direction)
            {
                case 'L':
                    nextPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                    return nextPoint;
                case 'R':
                    nextPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                    return nextPoint;
                case 'U':
                    nextPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                    return nextPoint;
                case 'D':
                    nextPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                    return nextPoint;
                default:
                    return currentPoint;
            }
        }

        public bool IsWalkable(Point currentPoint)
        {
            return this.GetMazeTile(currentPoint) != 'X';
        }
    }
}
