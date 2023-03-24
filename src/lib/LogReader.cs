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
                            if (tileType == 'T' && tileColor != 0)
                            {
                                tileType = 'R';
                            }
                            if (isNumeric)
                            {
                                currentState.Add(new Element(i, j, tileColor, tileType));
                            }
                            break;

                    }
                }
            }
        }
    }
}
