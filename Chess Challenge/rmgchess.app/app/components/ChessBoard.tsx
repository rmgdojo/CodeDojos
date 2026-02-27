"use client";

import { useState, useEffect } from "react";
import Link from "next/link";
import { fetchGameState, fetchGameRecordState, fetchMovesForPiece, makeMove } from "@/lib/api";
import { mapGameStateToBoard, getPieceAtSquare, getPieceIcon } from "@/lib/boardMapper";
import { GameStateModel, MoveModel } from "@/app/models";

type LiveGameMode = {
  mode: "live";
  gameId: string;
};

type RecordMode = {
  mode: "record";
  libraryName: string;
  gameIndex: number;
  moveIndex: number;
};

type ChessBoardProps = LiveGameMode | RecordMode;

export default function ChessBoard(props: ChessBoardProps) {
  const [gameState, setGameState] = useState<GameStateModel | null>(null);
  const [currentMoveIndex, setCurrentMoveIndex] = useState(
    props.mode === "record" ? props.moveIndex : 0
  );
  const [initialLoading, setInitialLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedSquare, setSelectedSquare] = useState<string | null>(null);
  const [validMoves, setValidMoves] = useState<MoveModel[]>([]);
  const [processing, setProcessing] = useState(false);
  const [isAutoPlaying, setIsAutoPlaying] = useState(false);
  const [autoPlayHighlight, setAutoPlayHighlight] = useState<{ square: string, stage: 'pre' | 'post' } | null>(null);

  const files = ["a", "b", "c", "d", "e", "f", "g", "h"];
  const ranks = [8, 7, 6, 5, 4, 3, 2, 1];

  useEffect(() => {
    if (!isAutoPlaying || props.mode !== "record" || !gameState) return;

    let active = true;
    const delay = (ms: number) => new Promise((resolve) => setTimeout(resolve, ms));

    const playMoves = async () => {
      let moveIndex = currentMoveIndex;

      try {
        while (active) {
          if (moveIndex >= gameState.recordMoveCount) {
            setIsAutoPlaying(false);
            break;
          }

          // Stage 1: Peek at next move target
          const nextState = await fetchGameRecordState(props.libraryName, props.gameIndex, moveIndex + 1);
          if (!active || !nextState.lastMove) break;

          setAutoPlayHighlight({
            square: nextState.lastMove.to.notation,
            stage: "pre",
          });

          // Delay before move
          await delay(800);
          if (!active) break;

          // Stage 2: Move made
          moveIndex += 1;
          setCurrentMoveIndex(moveIndex);
          setAutoPlayHighlight({
            square: nextState.lastMove.to.notation,
            stage: "post",
          });

          // Short delay after move before next auto-play step
          await delay(500);
          if (!active) break;

          setAutoPlayHighlight(null);
        }
      } catch (err) {
        setIsAutoPlaying(false);
      }
    };

    playMoves();
    return () => { active = false; };
  }, [isAutoPlaying, props.mode, props.mode === "record" ? props.libraryName : null, props.mode === "record" ? props.gameIndex : null, gameState?.recordMoveCount]);

  useEffect(() => {
    async function loadState() {
      try {
        setError(null);
        const state =
          props.mode === "live"
            ? await fetchGameState(props.gameId)
            : await fetchGameRecordState(props.libraryName, props.gameIndex, currentMoveIndex);
        setGameState(state);
      } catch (err) {
        const errorMessage = err instanceof Error ? err.message : "Failed to load game";
        setError(errorMessage);
      } finally {
        setInitialLoading(false);
      }
    }

    loadState();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [
    props.mode,
    props.mode === "live" ? props.gameId : null,
    props.mode === "record" ? props.libraryName : null,
    props.mode === "record" ? props.gameIndex : null,
    currentMoveIndex,
  ]);

  const goToPreviousMove = () => {
    setIsAutoPlaying(false);
    setCurrentMoveIndex((prev) => (prev > 0 ? prev - 1 : prev));
  };

  const goToNextMove = () => {
    setIsAutoPlaying(false);
    setCurrentMoveIndex((prev) => prev + 1);
  };

  const validMoveTargets = validMoves.map((m) => m.to.notation);

  const handleSquareClick = async (square: string) => {
    if (props.mode !== "live" || !gameState || gameState.isGameEnded || processing) return;

    // If clicking a valid move target, execute the move
    if (selectedSquare && validMoveTargets.includes(square)) {
      try {
        setProcessing(true);
        const updated = await makeMove(props.gameId, selectedSquare, square);
        setGameState(updated);
        setSelectedSquare(null);
        setValidMoves([]);
      } catch (err) {
        // Keep selection so user can retry
      } finally {
        setProcessing(false);
      }
      return;
    }

    // If clicking a square with the current player's piece, select it
    const boardState = mapGameStateToBoard(gameState);
    const piece = boardState[square];
    if (piece && piece.color === gameState.colourPlaying.toLowerCase()) {
      try {
        setProcessing(true);
        setSelectedSquare(square);
        const moves = await fetchMovesForPiece(props.gameId, square);
        setValidMoves(moves);
      } catch (err) {
        setSelectedSquare(null);
        setValidMoves([]);
      } finally {
        setProcessing(false);
      }
      return;
    }

    // Otherwise, clear selection
    setSelectedSquare(null);
    setValidMoves([]);
  };

  if (initialLoading) {
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
        <Link href="/" className="mt-4 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700">
          Back to Home
        </Link>
      </div>
    );
  }

  // Build the board state from pieces in play when available, otherwise empty
  const boardState = gameState ? mapGameStateToBoard(gameState) : {};

  return (
    <div className="flex flex-col h-screen bg-gray-100">
      <header className="bg-gray-800 text-white p-4 shadow-lg flex-shrink-0">
        <div className="flex justify-between items-center">
          <div>
            <h1 className="text-2xl font-bold">
              {props.mode === "live"
                ? (gameState?.id ?? "Loading...")
                : "Game Record Viewer"}
            </h1>
            {props.mode === "record" && (
              <p className="text-sm text-gray-300">
                {props.libraryName} - Game #{props.gameIndex} - Move {currentMoveIndex}
              </p>
            )}
          </div>
          {gameState && (
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
          )}
        </div>
        <Link className="inline-block mt-2 text-blue-300 hover:text-blue-100" href="/">
          ← Back to Home
        </Link>
      </header>

      <div className="flex-1 flex items-center justify-center p-4 overflow-hidden">
        <div
          className="grid w-full h-full"
          style={{
            gridTemplateColumns: "auto 1fr",
            gridTemplateRows: "minmax(0, 1fr) auto",
            maxWidth: "min(80vw, calc(100vh - 200px))",
            maxHeight: "min(80vw, calc(100vh - 200px))",
          }}
        >
          {/* Rank labels */}
          <div className="flex flex-col">
            {ranks.map((rank) => (
              <div
                key={rank}
                className="flex-1 flex items-center justify-center font-mono font-semibold text-gray-600 select-none pr-[12px]"
                style={{ fontSize: "calc(min(80vw, calc(100vh - 200px)) / 8 * 0.35)" }}
              >
                {rank}
              </div>
            ))}
          </div>

          {/* Board squares */}
          <div className="aspect-square h-full min-h-0 flex flex-col border-4 border-gray-800 shadow-2xl">
            {ranks.map((rank) => (
              <div key={rank} className="flex flex-1">
                {files.map((file, fileIndex) => {
                  const isLight = (rank + fileIndex) % 2 === 0;
                  const square = `${file}${rank}`;
                  const pieceData = getPieceAtSquare(boardState, file, rank);
                  const isSelected = square === selectedSquare;
                  const isValidTarget = validMoveTargets.includes(square);
                  const isCapture = isValidTarget && pieceData !== null;
                  const isAutoPre = autoPlayHighlight?.square === square && autoPlayHighlight.stage === 'pre';
                  const isAutoPost = autoPlayHighlight?.square === square && autoPlayHighlight.stage === 'post';

                  return (
                    <div
                      key={square}
                      onClick={() => handleSquareClick(square)}
                      className={`flex-1 flex items-center justify-center font-semibold relative overflow-hidden${
                        props.mode === "live" ? " cursor-pointer hover:opacity-80 transition-opacity" : ""
                      }`}
                      style={{
                        backgroundColor: isLight
                          ? "var(--chess-light-square)"
                          : "var(--chess-dark-square)",
                        fontSize: "calc(min(80vw, calc(100vh - 200px)) / 8 * 0.7)",
                        lineHeight: 1,
                      }}
                    >
                      {isSelected && (
                        <div
                          style={{
                            position: "absolute",
                            inset: 0,
                            backgroundColor: "rgba(255, 255, 80, 0.7)",
                            pointerEvents: "none",
                          }}
                        />
                      )}
                      {isAutoPre && (
                        <div
                          style={{
                            position: "absolute",
                            inset: 0,
                            backgroundColor: "rgba(130, 190, 255, 0.5)",
                            pointerEvents: "none",
                            zIndex: 1,
                          }}
                        />
                      )}
                      {isAutoPost && (
                        <div
                          style={{
                            position: "absolute",
                            inset: 0,
                            backgroundColor: "rgba(255, 210, 80, 0.7)",
                            pointerEvents: "none",
                            zIndex: 1,
                          }}
                        />
                      )}
                      {isValidTarget && !isCapture && (
                        <div
                          style={{
                            position: "absolute",
                            width: "28%",
                            height: "28%",
                            borderRadius: "50%",
                            backgroundColor: "rgba(0, 0, 0, 0.25)",
                            pointerEvents: "none",
                          }}
                        />
                      )}
                      {isCapture && (
                        <div
                          style={{
                            position: "absolute",
                            inset: "2px",
                            borderRadius: "50%",
                            border: "4px solid rgba(0, 0, 0, 0.25)",
                            pointerEvents: "none",
                          }}
                        />
                      )}
                      {pieceData && (
                        <span
                          className={`fas ${getPieceIcon(pieceData.piece)} ${
                            pieceData.color === "white"
                              ? "text-[#f0f0f0] [text-shadow:0_0_0_1px_#000] [-webkit-text-stroke:1px_#000]"
                              : "text-[#3d3d3d] [text-shadow:0_0_0_1px_#000] [-webkit-text-stroke:1px_#000]"
                          }`}
                          style={{ position: "relative", zIndex: 1 }}
                        ></span>
                      )}
                    </div>
                  );
                })}
              </div>
            ))}
          </div>

          {/* Empty bottom-left corner */}
          <div />

          {/* File labels */}
          <div className="flex pt-[4px]">
            {files.map((file) => (
              <div
                key={file}
                className="flex-1 flex items-center justify-center font-mono font-semibold text-gray-600 select-none"
                style={{ fontSize: "calc(min(80vw, calc(100vh - 200px)) / 8 * 0.35)" }}
              >
                {file}
              </div>
            ))}
          </div>
        </div>
      </div>

      {gameState && (
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

          {props.mode === "record" && (
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
                onClick={() => setIsAutoPlaying(!isAutoPlaying)}
                className={`px-4 py-2 text-white rounded transition-colors ${
                  isAutoPlaying ? "bg-red-600 hover:bg-red-700" : "bg-green-600 hover:bg-green-700"
                }`}
              >
                {isAutoPlaying ? "Stop Auto-Play ⏹" : "🚀 Auto-Play"}
              </button>
              <button
                type="button"
                onClick={goToNextMove}
                className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition-colors"
              >
                Next Move →
              </button>
            </div>
          )}
        </footer>
      )}
    </div>
  );
}
