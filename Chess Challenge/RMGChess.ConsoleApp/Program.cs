﻿using RMGChess.Core;
//using System.IO.Pipelines;

namespace RMGChess.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            int whiteMoves = 0, blackMoves = 0;

            Game game = new Game();

            while (true)
            {
                Console.Write("Algebra: ");
                string algebra = Console.ReadLine();
                Move move = Move.DecodeAlgebra(algebra, game.Board, Colour.White);
                Console.WriteLine($"Moving {move.Piece} from {move.From} to {move.To} {(move.TakesPiece ? "taking " + move.PieceToTake : "")}");
            }

            //foreach (Piece piece in game.Board.Pieces.Where(x => x.IsWhite))
            //{
            //    whiteMoves += TestGamePiece(piece, game.Board);
            //}
            //foreach (Piece piece in game.Board.Pieces.Where(x => x.IsBlack))
            //{
            //    blackMoves += TestGamePiece(piece, game.Board);
            //}

            WriteBoardStringToConsole(game.Board, "h5");
            bool moveMade = game.MakeMove(game.Board["h7"].Piece, "h5");
            WriteBoardStringToConsole(game.Board, "h5");

            //Console.WriteLine($"Total number of valid moves for white: {whiteMoves}");
            //Console.WriteLine($"Total number of valid moves for black: {blackMoves}");
            
            Console.ReadLine();
        }

        //static int TestGamePiece(Piece piece, Board board)
        //{
        //    IEnumerable<Move> validMoves = board.GetValidMoves(piece);
        //    if (validMoves != null)
        //    {
        //        foreach (Move move in validMoves)
        //        {
        //            Board cloneBoard = board.Clone();
        //            cloneBoard.MovePiece(move);
        //            Console.WriteLine($"{piece.GetType().Name} ({piece.Colour}) {move.From}:");
        //            WriteBoardStringToConsole(cloneBoard, move.From);
        //        }
        //    }

        //    return validMoves.Count();
        //}

        static void TestPiece<T>(Colour colour, Board board = null, Position position = null) where T : Piece
        {
            //position = position ?? new Position('e', 5);
            //T piece = (T)Activator.CreateInstance(typeof(T), colour);
            //board = board.Clone() ?? new Board();

            //board[position].PlacePiece(piece);
            //var moves = board.GetValidMoves(piece) ?? piece.GetPotentialMoves();
            //foreach (var move in moves)
            //{
            //    move.To.PlacePiece(new T() { Colour = piece.Colour });
            //}

            //Console.WriteLine($"{piece.GetType().Name} ({piece.Colour}) {position}:");
            //WriteBoardStringToConsole(board, position);
        }

        static void WriteBoardStringToConsole(Board board, Position highlight)
        {
            for (int rank = 8; rank >= 1; rank--)
            {
                Console.Write(rank);
                Console.Write(" ");
                for (char file = 'a'; file <= 'h'; file++)
                {
                    Square square = board[file, rank];
                    if (square.IsOccupied)
                    {
                        if (square.Piece.IsBlack)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                        }

                        if (square.Is(highlight))
                        {
                            if (square.Piece.Colour == Colour.Black)
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                        }

                        Console.Write(square.Piece.Symbol);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;

                        Console.Write(".");
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("  a b c d e f g h");
            Console.WriteLine();
        }
    }
}
