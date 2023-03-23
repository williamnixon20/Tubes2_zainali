using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Tubes2_zainali
{
    public class LogReader
    {
        public List<List<Element>> _logBoard;
        string mapConfig;

        public LogReader(string completePath)
        {
            _logBoard = new List<List<Element>>();

            mapConfig = File.ReadAllText(completePath);
            // Process map matrix
            string[] mapRows = mapConfig.Split('\n');
            List<Element> currentState = new List<Element>();
            int stage = 0;
            for (int i = 0; i < mapRows.Length && stage != 4; i++)
            {
                string row = mapRows[i];
                string[] mapColumns = row.Trim().Split(' ');
                // Read map config
                for (int j = 0; j < mapColumns.Length && stage == 0; j++)
                {
                    string tile = mapColumns[j];
            
                    switch (tile)
                    {
                        case "":
                            _logBoard.Add(currentState);
                            currentState = new List<Element>();
                            break;
                        case "$":
                            stage = 1;
                            break;
                        default:
                            char tileType = tile[0];
                            tile = tile.Substring(1);
                            int tileColor;
                            var isNumeric = int.TryParse(tile, out tileColor);
                            if (isNumeric)
                            {
                                currentState.Add(new Element(i, j, tileColor, tileType));
                            }
                            break;

                    }
                }
                //// Read step history
                //if (stage == 1)
                //{
                //    Console.WriteLine("HI2");
                //    if (row[0] == 'S')
                //    {
                //        stage = 2;
                //        continue;
                //    }
                //    if (row[0] != '$') {
                //        steps.Add(row);
                //    } 
                //}
                //if (stage == 2)
                //{
                //    if (row[0] == 'N')
                //    {
                //        stage = 3;
                //        continue;
                //    }
                //    steps.Add(row);
                //}
                //if (stage == 3)
                //{
                //    if (row == "")
                //    {
                //        stage = 4;
                //        continue;
                //    }
                //    history.Add(row);
                //}
            }
        }

        //static void Main(string[] args)
        //{
        //    Console.WriteLine("HI");
        //    LogReader log = new LogReader("C:\\Users\\ASUS\\source\\repos\\Tubes2_zainali\\Tubes2_zainali\\src\\3-23-2023_1-58-32_AM.txt");
        //    Console.WriteLine(log._logBoard.Count());
        //    for (int i = 0; i < log._logBoard.Count(); i++)
        //    {
        //        Console.WriteLine("New State!");
        //        log._logBoard[i].ForEach(x => Console.WriteLine(x.ToString()));
        //        //Console.WriteLine(log.steps[i]);
        //        //Console.WriteLine(log.history[i]);
        //        //Console.WriteLine(log.nodes[i]);
        //    }
        //}

    }
}
