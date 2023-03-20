using System.Text.RegularExpressions;
using System.Drawing;

namespace MazeGame
{
    public class Maze
    {
        private List<List<char>> mapMatrix;
        private int nRows;
        private int nCols;
        private Point mazeStart;
        private List<Point> treasureList;

        public Maze(string mapConfig)
        {
            this.mapMatrix = new List<List<char>>();
            this.mazeStart = new Point(-1, -1);
            this.treasureList = new List<Point>();
            
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
                    if (i != mapRows.Length - 1)        // blank is not in end of file
                    {
                        throw new MapFileException("Map Configuration File Must Have Rows Split by a Newline.");
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
                        this.treasureList.Add(new Point(i, j));
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

        public int getRowCount()
        {
            return this.nRows;
        }

        public int getColCount()
        {
            return this.nCols;
        }

        public char getMazeTile(int i, int j)
        {
            return this.mapMatrix[i][j];
        }

        public Point getStartPoint()
        {
            return this.mazeStart;
        }

        public int getTreasureCount()
        {
            return this.treasureList.Count;
        }
        
        public void printMazeInfo()
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
            Console.WriteLine(this.getRowCount());

            Console.Write("Columns : ");
            Console.WriteLine(this.getColCount());
            
            Console.Write("Start Point : ");
            Console.WriteLine(this.mazeStart.ToString());

            Console.Write("Treasure Count : ");
            Console.WriteLine(this.treasureList.Count);
            foreach (Point treasure in treasureList)
            {
                Console.Write("  ");
                Console.WriteLine(treasure.ToString());
            }
        }
    }
}
