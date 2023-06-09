﻿using System;

namespace Tubes2_zainali
{
    public class MapFileException : Exception
    {
        public MapFileException() : base("Map configuration file is not formatted correctly.")
        {
        }

        public MapFileException(string message) : base(message)
        {
        }

        public MapFileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
