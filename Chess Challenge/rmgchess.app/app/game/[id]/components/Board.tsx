"use client";

import Link from "next/dist/client/link";
import { useGame } from "../hooks/useGame";

export default function Board({ gameId }: { gameId: string }) {
  const { gameState } = useGame(gameId);

  const files = ["a", "b", "c", "d", "e", "f", "g", "h"];
  const ranks = [8, 7, 6, 5, 4, 3, 2, 1];

  const initialSetup: Record<
    string,
    { piece: string; color: "white" | "black" }
  > = {
    // Black pieces
    a8: { piece: "rook", color: "black" },
    b8: { piece: "knight", color: "black" },
    c8: { piece: "bishop", color: "black" },
    d8: { piece: "queen", color: "black" },
    e8: { piece: "king", color: "black" },
    f8: { piece: "bishop", color: "black" },
    g8: { piece: "knight", color: "black" },
    h8: { piece: "rook", color: "black" },
    a7: { piece: "pawn", color: "black" },
    b7: { piece: "pawn", color: "black" },
    c7: { piece: "pawn", color: "black" },
    d7: { piece: "pawn", color: "black" },
    e7: { piece: "pawn", color: "black" },
    f7: { piece: "pawn", color: "black" },
    g7: { piece: "pawn", color: "black" },
    h7: { piece: "pawn", color: "black" },
    // White pieces
    a2: { piece: "pawn", color: "white" },
    b2: { piece: "pawn", color: "white" },
    c2: { piece: "pawn", color: "white" },
    d2: { piece: "pawn", color: "white" },
    e2: { piece: "pawn", color: "white" },
    f2: { piece: "pawn", color: "white" },
    g2: { piece: "pawn", color: "white" },
    h2: { piece: "pawn", color: "white" },
    a1: { piece: "rook", color: "white" },
    b1: { piece: "knight", color: "white" },
    c1: { piece: "bishop", color: "white" },
    d1: { piece: "queen", color: "white" },
    e1: { piece: "king", color: "white" },
    f1: { piece: "bishop", color: "white" },
    g1: { piece: "knight", color: "white" },
    h1: { piece: "rook", color: "white" },
  };

  const getPieceIcon = (piece: string) => {
    const icons: Record<string, string> = {
      king: "fa-chess-king",
      queen: "fa-chess-queen",
      rook: "fa-chess-rook",
      bishop: "fa-chess-bishop",
      knight: "fa-chess-knight",
      pawn: "fa-chess-pawn",
    };
    return icons[piece] || "";
  };

  return (
    <div className="flex flex-col w-screen h-screen bg-gray-100">
      <header className="bg-gray-800 text-white p-4 shadow-lg">
        <h1 className="text-2xl font-bold">{gameState?.id ?? "Loading..."}</h1>
        <Link className="items-center px-5" href="/">{"< Back"}</Link>
      </header>
      <div className="flex-1 flex items-center justify-center p-4">
        <div className="aspect-square w-full h-full max-w-[min(90vw,90vh)] max-h-[min(100vw,100vh)] flex flex-col border-4 border-gray-800">
          {ranks.map((rank) => (
            <div key={rank} className="flex flex-1">
              {files.map((file, fileIndex) => {
                const isLight = (rank + fileIndex) % 2 === 0;
                const square = `${file}${rank}`;
                const pieceData = initialSetup[square];

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

      {gameState &&
        <footer className="bg-gray-200 p-3 text-center text-sm text-gray-600">
          <div className="flex justify-center gap-8">
            <span>Game ID: <code className="text-xs bg-gray-300 px-2 py-1 rounded">{gameState?.id}</code></span>
            {gameState.lastMove && (
              <span>Last Move: <strong>{gameState.lastMove.moveNotation}</strong></span>
            )}
          </div>
        </footer>}
      </div>
  );
}
