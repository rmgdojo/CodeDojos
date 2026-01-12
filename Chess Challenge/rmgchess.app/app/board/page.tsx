"use client";

import { useGameState } from "@/hooks/useGameState";
import { mapGameStateToBoard, getPieceAtSquare, getPieceIcon } from "@/lib/boardMapper";
import { API_URL } from '@/lib/config';

export default function Board() {
  const files = ["a", "b", "c", "d", "e", "f", "g", "h"];
  const ranks = [8, 7, 6, 5, 4, 3, 2, 1];

  // Fetch game state from API
  const { gameState, loading, error } = useGameState();

  if (loading) {
    return (
      <div className="flex items-center justify-center w-screen h-screen bg-gray-100">
        <div className="text-xl font-semibold text-gray-700">Loading game...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex flex-col items-center justify-center w-screen h-screen bg-gray-100">
        <div className="text-xl font-semibold text-red-600 mb-4">Error loading game</div>
        <div className="text-sm text-gray-600">{error}</div>
      </div>
    );
  }

  if (!gameState) {
    return null;
  }

  // Convert game state to board representation
  const boardState = mapGameStateToBoard(gameState);

  return (
    <div className="flex flex-col w-screen h-screen bg-gray-100">
      <header className="bg-gray-800 text-white p-4 shadow-lg">
        <div className="flex justify-between items-center">
          <h1 className="text-2xl font-bold">Chess Board</h1>
          <div className="flex gap-6 text-sm">
            <span>
              Turn: <strong className="text-lg">{gameState.colourPlaying}</strong>
            </span>
            <span>
              Round: <strong className="text-lg">{gameState.currentRound}</strong>
            </span>
            <span>
              Moves: <strong className="text-lg">{gameState.totalMoves}</strong>
            </span>
            {gameState.isWhiteInCheck && (
              <span className="text-red-400 font-semibold animate-pulse">
                ⚠ White in Check!
              </span>
            )}
            {gameState.isBlackInCheck && (
              <span className="text-red-400 font-semibold animate-pulse">
                ⚠ Black in Check!
              </span>
            )}
          </div>
        </div>
      </header>

      <div className="flex-1 flex items-center justify-center p-4">
        <div className="aspect-square w-full h-full max-w-[min(90vw,90vh)] max-h-[min(100vw,100vh)] flex flex-col border-4 border-gray-800 shadow-2xl">
          {ranks.map((rank) => (
            <div key={rank} className="flex flex-1">
              {files.map((file, fileIndex) => {
                const isLight = (rank + fileIndex) % 2 === 0;
                const square = `${file}${rank}`;
                const pieceData = getPieceAtSquare(boardState, file, rank);

                return (
                  <div
                    key={square}
                    className="flex-1 flex items-center justify-center text-7xl font-semibold relative cursor-pointer hover:opacity-80 transition-opacity"
                    style={{
                      backgroundColor: isLight
                        ? "var(--chess-light-square)"
                        : "var(--chess-dark-square)",
                    }}
                  >
                    {/* Square label */}
                    <span className="absolute bottom-1 left-1 text-xs opacity-30 font-mono select-none">
                      {square}
                    </span>

                    {/* Piece */}
                    {pieceData && (
                      <span
                        className={`fas ${getPieceIcon(pieceData.piece)} ${
                          pieceData.color === "white"
                            ? "text-[#f0f0f0] [text-shadow:0_0_0_1px_#000] [-webkit-text-stroke:1px_#000]"
                            : "text-[#3d3d3d] [text-shadow:0_0_0_1px_#000] [-webkit-text-stroke:1px_#000]"
                        }`}
                      ></span>
                    )}
                  </div>
                );
              })}
            </div>
          ))}
        </div>
      </div>

      {/* Captured Pieces Display */}
      {gameState.capturedPieces.length > 0 && (
        <aside className="bg-gray-800 text-white p-4">
          <h2 className="text-lg font-semibold mb-3">Captured Pieces</h2>
          <div className="flex flex-wrap gap-3">
            {gameState.capturedPieces.map((piece, index) => (
              <div
                key={index}
                className="flex flex-col items-center"
                title={`${piece.colour} ${piece.type}`}
              >
                <span
                  className={`fas ${getPieceIcon(piece.type)} text-3xl ${
                    piece.colour.toLowerCase() === "white"
                      ? "text-gray-300"
                      : "text-gray-600"
                  }`}
                ></span>
                <span className="text-xs mt-1 opacity-70">{piece.type}</span>
              </div>
            ))}
          </div>
        </aside>
      )}

      {/* Game Info Footer */}
      <footer className="bg-gray-200 p-3 text-center text-sm text-gray-600">
        <div className="flex justify-center gap-8">
          <span>Game ID: <code className="text-xs bg-gray-300 px-2 py-1 rounded">{gameState.id}</code></span>
          {gameState.lastMove && (
            <span>Last Move: <strong>{gameState.lastMove.moveNotation}</strong></span>
          )}
        </div>
      </footer>
    </div>
  );
}
