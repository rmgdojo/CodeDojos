import { PositionModel } from "./PositionModel";

/**
 * Represents a chess piece in the game
 */
export interface PieceModel {
    /**
     * The color of the piece ("White" or "Black")
     */
    colour: string;

    /**
     * The type of piece (e.g., "King", "Queen", "Rook", "Bishop", "Knight", "Pawn")
     */
    type: string;

    /**
     * The piece's symbol (e.g., 'K', 'Q', 'R', 'B', 'N', 'P')
     */
    symbol: string;

    /**
     * Current position of the piece on the board
     */
    position: PositionModel | null;

    /**
     * Original starting position of the piece
     */
    origin: PositionModel | null;

    /**
     * Whether the piece has moved from its starting position
     */
    hasMoved: boolean;

    /**
     * The point value of the piece
     */
    value: number;
}
