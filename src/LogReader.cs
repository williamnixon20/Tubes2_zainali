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
        public string path = "";

        public LogReader(string completePath)
        {
            _logBoard = new List<List<Element>>();
            mapConfig = File.ReadAllText(completePath);
            // Process map matrix
            string[] mapRows = mapConfig.Split('\n');
            List<Element> currentState = new List<Element>();
            bool done = false;
            for (int i = 0; i < mapRows.Length && !done; i++)
            {
                string row = mapRows[i];
                string[] mapColumns = row.Trim().Split(' ');
                for (int j = 0; j < mapColumns.Length; j++)
                {
                    string tile = mapColumns[j];
            
                    switch (tile)
                    {
                        case "":
                            _logBoard.Add(currentState);
                            currentState = new List<Element>();
                            break;
                        case "$":
                            path = mapRows[i + 1];
                            done = true;
                            break;
                        default:
                            int tileColor;
                            var isNumeric = int.TryParse(tile, out tileColor);
                            if (isNumeric)
                            {
                                currentState.Add(new Element(i, j, tileColor));
                            }
                            break;

                    }
                }
            }
        }

        

        //static void Main(string[] args)
        //{
        //    Console.WriteLine("HI");
        //    LogReader log = new LogReader("C:\\Users\\ASUS\\source\\repos\\Tubes2_zainali\\Tubes2_zainali\\src\\3-22-2023_3-46-55_PM.txt");
        //    Console.WriteLine(log.path);
        //    Console.WriteLine(log._logBoard.Count());
        //    foreach (var sublist in log._logBoard)
        //    {
        //        Console.WriteLine("NEW LIST");
        //        foreach (var obj in sublist)
        //        {
        //            Console.WriteLine(obj);
        //        }
        //    }
        //}

    }
}
