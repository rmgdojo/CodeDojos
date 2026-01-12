import { API_URL } from "./config";
import { GameRecord } from "@/app/models/GameRecord";

export async function fetchGameState() {
  return await fetch(`${API_URL}/gamestate`).then(response => response.json());
}

export async function fetchGame(gameId: string): Promise<GameRecord> {
  return await fetch(`${API_URL}/games/${gameId}`).then(response => response.json());
}
