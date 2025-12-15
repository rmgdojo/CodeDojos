"use client";

import { useState, useEffect } from "react";
import { fetchGame } from "@/lib/api";
import { GameRecord } from "@/app/models/GameRecord";

export function useGame(gameId: string) {
  const [game, setGame] = useState<GameRecord | null>(null);

  useEffect(() => {
    async function loadGame() {
      await fetchGame(gameId).then(data => setGame(data));
    }

    loadGame();
  }, [gameId]);

  return { game };
}