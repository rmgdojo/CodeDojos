using RMGChess.Core;
using System.Text;

namespace RMGChess.ConsoleApp
{
    /// <summary>
    /// Handles display of chess moves and game information.
    /// </summary>
    public static class MoveDisplayService
    {
        public static void DisplayGameInfo(int gameIndex, string gameName)
        {
            string title = $"Game {gameIndex}: {gameName}";
            ChessConsole.WriteLine(0, 0, title);
            ChessConsole.WriteLine(new string('-', 120));
            ChessConsole.WriteLine();
        }

        public static void DisplayMoves(GameRecord gameRecord, float currentRound, Colour whoseTurn)
        {
            StringBuilder pgnBuilder = new StringBuilder();
            int currentLineLength = 0;
            RoundRecord[] roundRecords = gameRecord.Rounds.ToArray();

            for (int i = 0; i < roundRecords.Length; i++)
            {
                MoveRecord whiteMoveRecord = roundRecords[i].WhiteMove;
                MoveRecord blackMoveRecord = roundRecords[i].BlackMove;

                bool isCurrentRound = i + 1 == (int)currentRound;

                string roundString = GetRoundString(i + 1, whiteMoveRecord.MoveAsAlgebra, blackMoveRecord?.MoveAsAlgebra ?? "", isCurrentRound, whoseTurn, out string markedUpRoundString);
                if (currentLineLength + roundString.Length > DisplaySettings.ConsoleWidth)
                {
                    pgnBuilder.AppendLine();
                    currentLineLength = 0;
                }
                pgnBuilder.Append(markedUpRoundString);
                currentLineLength += roundString.Length;
            }

            string pgn = pgnBuilder.ToString().TrimEnd();
            ChessConsole.Write(0, DisplaySettings.MovesDisplayLine, pgn);
        }

        private static string GetRoundString(int round, string whiteMove, string blackMove, bool isCurrentRound, Colour whoseTurn, out string markedUpString)
        {
            markedUpString = string.Empty;
            if (isCurrentRound)
            {
                markedUpString = $"[white on green]{round}.\u00A0";

                markedUpString += (whoseTurn == Colour.White) ? "[white]" : "[darkgreen]";
                markedUpString += whiteMove;
                markedUpString += "[/]\u00A0";

                markedUpString += (whoseTurn == Colour.Black) ? "[white]" : "[darkgreen]";
                markedUpString += blackMove;
                markedUpString += "[/][/]";
            }
            else
            {
                markedUpString += $"[grey30]{round}.\u00A0{whiteMove}\u00A0{blackMove}[/]";
            }
            markedUpString += " ";

            return $"{round}.\u00A0{whiteMove}\u00A0{blackMove} ";
        }

        public static void DisplayPreviousMove(Colour whoseTurn, string lastMoveAsAlgebra, Move lastMove, float roundIndex)
        {
            if (lastMove is not null)
            {
                ChessConsole.Write(DisplaySettings.RightHandBlockColumn, DisplaySettings.PreviousMoveLine, $"Previous move by {whoseTurn.Switch()}.", true);
                ChessConsole.Write(DisplaySettings.RightHandBlockColumn, DisplaySettings.PreviousMoveLine + 1, $"[blue]Algebra: {(Math.Ceiling(roundIndex) - 1)}. {lastMoveAsAlgebra}[/]", true);
                ChessConsole.Write(DisplaySettings.RightHandBlockColumn, DisplaySettings.PreviousMoveLine + 2, $"[blue]{GetMoveDescription(lastMove)}[/]", true);
            }
            else
            {
                ChessConsole.Write(DisplaySettings.RightHandBlockColumn, DisplaySettings.PreviousMoveLine, $"[blue]No previous move.[/]", true);
            }
        }

        public static void DisplayNextMove(Colour whoseTurn, string moveAsAlgebra, Move move, float roundIndex, Game game)
        {
            ChessConsole.WriteLine(DisplaySettings.RightHandBlockColumn, DisplaySettings.NextMoveLine, $"{whoseTurn} to play.");
            ChessConsole.WriteLine(DisplaySettings.RightHandBlockColumn, DisplaySettings.NextMoveLine + 1, $"[green]Algebra: {(int)roundIndex}. {moveAsAlgebra}[/]", true);

            ChessConsole.WriteLine(DisplaySettings.RightHandBlockColumn, DisplaySettings.NextMoveLine + 2, $"[green]{GetMoveDescription(move)}[/]", true);
            if (game.IsInCheck(whoseTurn))
            {
                ChessConsole.WriteLine(DisplaySettings.RightHandBlockColumn, DisplaySettings.NextMoveLine + 3, $"[green]{whoseTurn} is in check[/]".ToUpper(), true);
            }
            else
            {
                ChessConsole.ClearLineRight(DisplaySettings.RightHandBlockColumn, DisplaySettings.NextMoveLine + 3);
            }
        }

        public static string GetMoveDescription(Move move)
        {
            string output = string.Empty;
            if (move is CastlingMove castlingMove)
            {
                Move rookMove = castlingMove.RookMove;
                output = $"Castling {(castlingMove.Side == Side.Queenside ? "QS" : "KS")} {move.Piece} from {move.From} to {move.To} ({move.Path.ToString()}) and {rookMove.Piece} from {rookMove.From} to {rookMove.To} ({rookMove.Path.ToString()})";
            }
            else if (move is EnPassantMove)
            {
                output = $"Moving {move.Piece} from {move.From} to {move.To} taking {move.PieceToTake} en passant ({move.Path.ToString()})";
            }
            else
            {
                output = $"Moving {move.Piece} from {move.From} to {move.To}{(move.TakesPiece ? " taking " + move.PieceToTake : "")}{(move.IsPromotion ? " promotes to " + move.PromotesTo : "")} ({move.Path.ToString()})";
            }

            if (move.PutsOpponentInCheck)
            {
                output += " CHECK";
            }

            return output;
        }
    }
}
