"use client";

import { useRouter } from "next/navigation";
import { createGameState } from "@/lib/api";
import { useState } from "react";

export function CreateGameButton() {
  const router = useRouter();
  const [isCreating, setIsCreating] = useState(false);

  const handleCreateGame = async () => {
    setIsCreating(true);
    try {
      const gameState = await createGameState();
      router.push(`/game/${gameState.id}`);
    } catch (error) {
      console.error("Failed to create game:", error);
      setIsCreating(false);
    }
  };

  return (
    <button
      id="createGame"
      onClick={handleCreateGame}
      disabled={isCreating}
      className="px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors"
    >
      {isCreating ? "Creating..." : "Create New Game"}
    </button>
  );
}
