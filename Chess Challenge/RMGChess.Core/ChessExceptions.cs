using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMGChess.Core
{
    public class ChessException : Exception
    {
        public ChessException(string message) : base(message)
        {
        }

        public ChessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class InvalidMoveException : ChessException
    {
        public InvalidMoveException(string message) : base(message)
        {
        }
        public InvalidMoveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class InvalidPositionException : ChessException
    {
        public InvalidPositionException(string message) : base(message)
        {
        }
        public InvalidPositionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class ShouldNeverHappenException : ChessException
    {
        public ShouldNeverHappenException(string message) : base(message)
        {
        }
        public ShouldNeverHappenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
