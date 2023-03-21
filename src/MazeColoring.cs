using System;
using System.Drawing;
using System.Text;
using System.IO;

namespace Tubes2_zainali
{
    public class MazeColoring
    {
        private string _filename;
        private Maze _maze;
        private List<int[,]> _mazeStateLog;
        private string _solutionRoute;
        private int[,] _mazeStateTemplate;

        public MazeColoring(Maze maze)
        {
            this._filename = (DateTime.Now.ToString() + ".txt").Replace('/', '-').Replace(':', '-').Replace(' ', '_');
            
            this._maze = maze;
            this._mazeStateLog = new List<int[,]>();
            this._solutionRoute = "";

            this._mazeStateTemplate = new int[_maze.RowCount, _maze.ColCount];
            for (int i = 0; i < _maze.RowCount; i++)
            {
                for (int j = 0; j < _maze.ColCount; j++)
                {
                    if (_maze.GetMazeTile(i, j) == 'X')
                    {
                        _mazeStateTemplate[i, j] = -1;
                    }
                }
                
            }
        }

        public string FileName
        {
            get { return this._filename; }
        }

        public int[,] GenerateColoringState(string directionsTaken)
        {
            int[,] colorState = (int[,])this._mazeStateTemplate.Clone();
            Point playerPosition = _maze.StartPoint;

            colorState[playerPosition.X, playerPosition.Y]++;
            foreach (char direction in directionsTaken)
            {
                playerPosition = Maze.GetNextPoint(playerPosition, direction);                
                colorState[playerPosition.X, playerPosition.Y]++;
            }
            return colorState;
        }

        public void BackupColoringState(List<string> directionState)
        {
            this._mazeStateLog.Add(this._mazeStateTemplate);
            foreach (string route in directionState)
            {
                int[,] colorState = this.GenerateColoringState(route);
                this._mazeStateLog.Add(colorState);
            }
            this._solutionRoute = directionState.Last();
        }

        public string Log
        {
            get
            {
                StringBuilder log = new StringBuilder();

                foreach (int[,] mazeColorState in _mazeStateLog)
                {
                    log.AppendLine('#'.ToString());
                    StringBuilder colorState = new StringBuilder();
                    for (int i = 0; i < _maze.RowCount; i++)
                    {
                        for (int j = 0; j < _maze.ColCount; j++)
                        {
                            colorState.Append(mazeColorState[i, j]);
                            colorState.Append(' ');
                        }
                        colorState.AppendLine();
                    }
                    log.AppendLine(colorState.ToString());
                }
                log.AppendLine("$");
                log.AppendLine(this._solutionRoute);
                return log.ToString();
            }
        }

        public void SaveLog(string directory)
        {
            string path = Path.Combine(directory, this.FileName);
            using (StreamWriter writer = new StreamWriter(path)) {
                writer.WriteLine(this.Log);
            }
        }
    }
}