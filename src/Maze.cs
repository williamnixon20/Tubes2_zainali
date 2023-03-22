using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.IO;
namespace Tubes2_zainali
{
    public class Maze
    {
        private List<List<char>> _mapMatrix;
        private int _nRows;
        private int _nCols;
        private Point _mazeStart;
        private HashSet<Point> _treasureSet;

        /* CTOR */
        public Maze(string mapConfig)
        {
            mapConfig = mapConfig.TrimEnd('\r', '\n', ' ');      // remove trailing newline from config text
            this._mapMatrix = new List<List<char>>();
            this._mazeStart = new Point(-1, -1);
            this._treasureSet = new HashSet<Point>();

            // Check syntax of file
            string mapConfigSyntax = @"^([KTRX][\s\n])+$";
            if (!Regex.IsMatch(mapConfig, mapConfigSyntax, RegexOptions.Multiline))
            {
                throw new MapFileException("Map Configuration File Has Illegal Characters.");
            }

            // Process map matrix
            string[] mapRows = mapConfig.Split('\n');
            int colCount = -1;
            for (int i = 0; i < mapRows.Length; i++)
            {
                string row = mapRows[i];
                string[] mapColumns = row.Trim().Split(' ');

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
                        if (this._mazeStart.X == -1 && this._mazeStart.Y == -1)   // uninitialized start
                        {
                            this._mazeStart = new Point(i, j);
                        }
                        else
                        {
                            throw new MapFileException("Map Configuration has multiple start points.");
                        }
                    }

                    if (tile == "T")    // list treasures
                    {
                        this._treasureSet.Add(new Point(i, j));
                    }
                }

                this._mapMatrix.Add(currentRow);
            }

            this._nRows = this._mapMatrix.Count;
            this._nCols = colCount;
        }

        public Maze(string directory, string fileName) : this(File.ReadAllText(Path.Combine(directory, fileName)))
        {
        }

        /* MAP INFO */
        public int RowCount
        {
            get { return this._nRows; }
        }

        public int ColCount
        {
            get { return this._nCols; }
        }

        public char GetMazeTile(int i, int j)
        {
            if (i < 0 || i >= this._nRows || j < 0 || j >= this._nCols)       // Point ot of bounds
            {
                return 'X';
            }
            else
            {
                return this._mapMatrix[i][j];
            }
        }
        public Point StartPoint
        {
            get { return this._mazeStart; }
        }

        public int TreasureCount
        {
            get { return this._treasureSet.Count; }
        }

        public void PrintMazeInfo()
        {
            foreach (List<char> row in this._mapMatrix)
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
            Console.WriteLine(this.RowCount);

            Console.Write("Columns : ");
            Console.WriteLine(this.ColCount);
            
            Console.Write("Start Point : ");
            Console.WriteLine(this.StartPoint.ToString());

            Console.Write("Treasure Count : ");
            Console.WriteLine(this.TreasureCount);
            foreach (Point treasure in _treasureSet)
            {
                Console.Write("  ");
                Console.WriteLine(treasure.ToString());
            }
        }

        /* MAZE TILE INFO */
        public char GetMazeTile(Point tileCoordinate)
        {
            return this.GetMazeTile(tileCoordinate.X, tileCoordinate.Y);
        }

        public List<Point> GetNeighbors(Point tile)
        {
            List<Point> neighbors = new List<Point>();

            // NEIGHBOR PRIORITY: L R U D
            Point left = GetNextPoint(tile, 'L');
            Point right = GetNextPoint(tile, 'R');
            Point up = GetNextPoint(tile, 'U');
            Point down = GetNextPoint(tile, 'D');

            neighbors.Add(left);
            neighbors.Add(right);
            neighbors.Add(up);
            neighbors.Add(down);

            return neighbors;
        }

        public bool IsWalkable(Point currentPoint)
        {
            return this.GetMazeTile(currentPoint) != 'X';
        }


        /* GET NEXT-POINT FROM DIRECTION */
        public static Point GetNextPoint(Point currentPoint, char direction)
        {
            // assumes the next direction is valid

            /*
                (X,Y)
                X are rows, Y are columns
                                (X0, Y1)
                                    ^  
                                    |
                                    U
                (X1, Y0)    <-L (X1, Y1) R->    (X1, Y2)
                                    D
                                    |
                                    v  
                                (X2, Y1)
            
            */
            Point nextPoint;
            switch (direction)
            {
                case 'L':
                    nextPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                    return nextPoint;
                case 'R':
                    nextPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                    return nextPoint;
                case 'U':
                    nextPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                    return nextPoint;
                case 'D':
                    nextPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                    return nextPoint;
                default:
                    return currentPoint;
            }
        }
        public static Point GetNextPoint(Point currentPoint, string steps)
        {
            Point nextPoint = currentPoint;
            foreach (char direction in steps)
            {
                nextPoint = Maze.GetNextPoint(nextPoint, direction);
            }
            return nextPoint;
        }

        public static char GetDirectionBetween(Point start, Point finish)
        {
            if (start.X == finish.X)
            {
                if (finish.Y < start.Y)
                {
                    return 'L';
                }
                if (start.Y < finish.Y)
                {
                    return 'R';
                }

            }
            if (start.Y == finish.Y)
            {
                if (finish.X < start.X)
                {
                    return 'U';
                }
                if (start.X < finish.X)
                {
                    return 'D';
                }
            }
            return 'X';
        }

        public List<Element> GetGridRepresentation()
        {
            List<Element> ls = new List<Element>();
            foreach (List<char> row in this._mapMatrix)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i] == 'K')
                    {
                        ls.Add(new Element(1, 1, -2, row[i]));
                    } else if (row[i] == 'R')
                    {
                        ls.Add(new Element(1, 1, 0, row[i]));
                    } else if (row[i] == 'T')
                    {
                        ls.Add(new Element(1, 1, 1, row[i]));
                    } else
                    {
                        ls.Add(new Element(1, 1, -1, row[i]));
                    }
                }
            }
            return ls;
        }
    }
}
