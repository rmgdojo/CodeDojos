/**
 * Represents a position on the chess board (e.g., "e4", "a1")
 */
export interface PositionModel {
    /**
     * The file (column) of the position ('a' through 'h')
     */
    file: string;

    /**
     * The rank (row) of the position (1 through 8)
     */
    rank: number;

    /**
     * String representation in algebraic notation (e.g., "e4")
     */
    notation: string;
}
