"use client";

import { useState, useEffect } from "react";
import Link from "next/link";
import { fetchGameRecordState } from "@/lib/api";
import { mapGameStateToBoard, getPieceAtSquare, getPieceIcon } from "@/lib/boardMapper";
import { GameStateModel } from "@/app/models";

interface RecordBoardProps {
  libraryName: string;
  gameIndex: number;
  moveIndex: number;
}

export default function RecordBoard({ libraryName, gameIndex, moveIndex: initialMoveIndex }: RecordBoardProps) {
  const [gameState, setGameState] = useState<GameStateModel | null>(null);
  const [currentMoveIndex, setCurrentMoveIndex] = useState(initialMoveIndex);
  const [initialLoading, setInitialLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const files = ["a", "b", "c", "d", "e", "f", "g", "h"];
  const ranks = [8, 7, 6, 5, 4, 3, 2, 1];

  useEffect(() => {
    async function loadGameRecord() {
      try {
        setError(null);
        console.log('Fetching game record:', { libraryName, gameIndex, currentMoveIndex });
        const state = await fetchGameRecordState(libraryName, gameIndex, currentMoveIndex);
        console.log('Received game state:', state);
        console.log('Pieces in play:', state.piecesInPlay?.length);
        console.log('Total moves:', state.totalMoves);
        console.log('Last move:', state.lastMove?.moveNotation);
        setGameState(state);
        console.log('Game state updated, should trigger re-render');
      } catch (err) {
        const errorMessage = err instanceof Error ? err.message : "Failed to load game record";
        setError(errorMessage);
        console.error("Error fetching game record:", err);
      } finally {
        setInitialLoading(false);
      }
    }

    loadGameRecord();
  }, [libraryName, gameIndex, currentMoveIndex]);

  const goToPreviousMove = () => {
    console.log('Going to previous move, current:', currentMoveIndex);
    setCurrentMoveIndex((prev) => {
      const newIndex = prev > 0 ? prev - 1 : prev;
      console.log('Previous move index:', prev, '->', newIndex);
      return newIndex;
    });
  };

  const goToNextMove = () => {
    console.log('Going to next move, current:', currentMoveIndex);
    setCurrentMoveIndex((prev) => {
      const newIndex = prev + 1;
      console.log('Next move index:', prev, '->', newIndex);
      return newIndex;
    });
  };

  if (initialLoading) {
    return (
      <div className="flex items-center justify-center w-screen h-screen bg-gray-100">
        <div className="text-xl font-semibold text-gray-700">Loading game record...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex flex-col items-center justify-center w-screen h-screen bg-gray-100">
        <div className="text-xl font-semibold text-red-600 mb-4">Error loading game record</div>
        <div className="text-sm text-gray-600">{error}</div>
        <Link href="/" className="mt-4 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700">
          Back to Home
        </Link>
      </div>
    );
  }

  if (!gameState) {
    return null;
  }

  // Convert game state to board representation
  const boardState = mapGameStateToBoard(gameState);
  console.log('Rendering board with move index:', currentMoveIndex, 'Total pieces:', boardState ? Object.keys(boardState).length : 0);

  return (
    <div className="flex flex-col h-screen bg-gray-100">
      <header className="bg-gray-800 text-white p-4 shadow-lg flex-shrink-0">
        <div className="flex justify-between items-center">
          <div>
            <h1 className="text-2xl font-bold">Game Record Viewer</h1>
            <p className="text-sm text-gray-300">
              {libraryName} - Game #{gameIndex} - Move {currentMoveIndex}
            </p>
          </div>
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
        <Link className="inline-block mt-2 text-blue-300 hover:text-blue-100" href="/">
          ← Back to Home
        </Link>
      </header>

      <div className="flex-1 flex items-center justify-center p-4 overflow-hidden">
        <div className="aspect-square w-full h-full max-w-[min(80vw,calc(100vh-200px))] max-h-[min(80vw,calc(100vh-200px))] flex flex-col border-4 border-gray-800 shadow-2xl">
          {ranks.map((rank) => (
            <div key={rank} className="flex flex-1">
              {files.map((file, fileIndex) => {
                const isLight = (rank + fileIndex) % 2 === 0;
                const square = `${file}${rank}`;
                const pieceData = getPieceAtSquare(boardState, file, rank);

                return (
                  <div
                    key={square}
                    className="flex-1 flex items-center justify-center text-7xl font-semibold relative"
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

      <footer className="bg-gray-200 p-4 text-center flex-shrink-0">
        <div className="flex justify-center gap-8 text-sm text-gray-600 mb-4">
          <span>
            Game ID: <code className="text-xs bg-gray-300 px-2 py-1 rounded">{gameState.id}</code>
          </span>
          {gameState.lastMove && (
            <span>
              Last Move: <strong>{gameState.lastMove.moveNotation}</strong>
            </span>
          )}
        </div>

        {/* Navigation controls */}
        <div className="flex justify-center gap-4">
          <button
            type="button"
            onClick={goToPreviousMove}
            disabled={currentMoveIndex === 0}
            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition-colors disabled:bg-gray-400 disabled:cursor-not-allowed"
          >
            ← Previous Move
          </button>
          <button
            type="button"
            onClick={goToNextMove}
            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition-colors"
          >
            Next Move →
          </button>
        </div>
      </footer>
    </div>
  );
}
