import { GameStateModel, PieceModel } from "@/app/models";

/**
 * Represents a piece on a board square
 */
export type BoardSquare = {
  piece: string; // 'king', 'queen', 'rook', 'bishop', 'knight', 'pawn'
  color: 'white' | 'black';
} | null;

/**
 * Board state keyed by square notation (e.g., "e4", "a1")
 */
export type BoardState = Record<string, BoardSquare>;

/**
 * Converts GameStateModel to a board state representation
 * Maps each piece to its position on the board
 */
export function mapGameStateToBoard(gameState: GameStateModel): BoardState {
  const boardState: BoardState = {};

  // Map each piece in play to its position on the board
  gameState.piecesInPlay.forEach((piece) => {
    if (piece.position) {
      const square = piece.position.notation; // e.g., "e4"
      boardState[square] = {
        piece: piece.type.toLowerCase(), // "King" -> "king"
        color: piece.colour.toLowerCase() as 'white' | 'black', // "White" -> "white"
      };
    }
  });

  return boardState;
}

/**
 * Gets piece data for a specific square
 * @param boardState - The current board state
 * @param file - The file (column) of the square ('a'-'h')
 * @param rank - The rank (row) of the square (1-8)
 * @returns The piece at the square, or null if empty
 */
export function getPieceAtSquare(
  boardState: BoardState,
  file: string,
  rank: number
): BoardSquare {
  const square = `${file}${rank}`;
  return boardState[square] || null;
}

/**
 * Gets the Font Awesome icon class for a piece type
 * @param piece - The piece type (e.g., "king", "queen")
 * @returns The Font Awesome icon class
 */
export function getPieceIcon(piece: string): string {
  const icons: Record<string, string> = {
    'king': 'fa-chess-king',
    'queen': 'fa-chess-queen',
    'rook': 'fa-chess-rook',
    'bishop': 'fa-chess-bishop',
    'knight': 'fa-chess-knight',
    'pawn': 'fa-chess-pawn',
  };
  return icons[piece.toLowerCase()] || '';
}
