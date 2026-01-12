import { PieceModel } from "./PieceModel";
import { PositionModel } from "./PositionModel";

/**
 * Represents a chess move in the game
 */
export interface MoveModel {
    /**
     * The piece being moved
     */
    piece: PieceModel;

    /**
     * Starting position of the move
     */
    from: PositionModel;

    /**
     * Destination position of the move
     */
    to: PositionModel;

    /**
     * The color of the player making the move
     */
    whoIsMoving: string;

    /**
     * Whether this move captures an opponent's piece
     */
    takesPiece: boolean;

    /**
     * The piece being captured (if any)
     */
    pieceToTake: PieceModel | null;

    /**
     * Whether this move puts the opponent in check
     */
    putsOpponentInCheck: boolean;

    /**
     * Whether this is a pawn promotion move
     */
    isPromotion: boolean;

    /**
     * The piece type to promote to (e.g., "Queen")
     */
    promotesTo: string | null;

    /**
     * String representation of the move
     */
    moveNotation: string;
}
