"use client";

import { useState, useEffect } from "react";
import { fetchGameState } from "@/lib/api";
import { GameStateModel } from "../../../models";

export function useGame(gameId: string) {
  const [gameState, setGameState] = useState<GameStateModel | null>(null);

  useEffect(() => {
    async function loadGame() {
      await fetchGameState(gameId).then(data => setGameState(data));
    }

    loadGame();
  }, [gameId]);

  return { gameState };
}
