import { PieceModel } from "./PieceModel";
import { MoveModel } from "./MoveModel";

/**
 * Represents the complete state of a chess game
 */
export interface GameStateModel {
    /**
     * Unique identifier for the game instance
     */
    id: string;

    /**
     * The color of the player whose turn it is to move
     */
    colourPlaying: string;

    /**
     * All pieces currently in play on the board
     */
    piecesInPlay: PieceModel[];

    /**
     * All pieces that have been captured
     */
    capturedPieces: PieceModel[];

    /**
     * The last move made in the game
     */
    lastMove: MoveModel | null;

    /**
     * Move history for White player
     */
    whiteMoveHistory: MoveModel[];

    /**
     * Move history for Black player
     */
    blackMoveHistory: MoveModel[];

    /**
     * Whether White is currently in check
     */
    isWhiteInCheck: boolean;

    /**
     * Whether Black is currently in check
     */
    isBlackInCheck: boolean;

    /**
     * Whether the game has ended
     */
    isGameEnded: boolean;

    /**
     * The reason the game ended (if applicable)
     */
    gameEndReason: string | null;

    /**
     * Total number of moves made in the game
     */
    totalMoves: number;

    /**
     * Current round number
     */
    currentRound: number;

    /**
     * Whether this game state belongs to a historical game record
     */
    isRecord: boolean;

    /**
     * Total moves in the record (if applicable)
     */
    recordMoveCount: number;
}
