import { API_URL } from "./config";
import { GameRecord } from "@/app/models/GameRecord";
import { GameStateModel } from "@/app/models/GameStateModel";

export async function createGameState() {
  return await fetch(`${API_URL}/createNewGameState`, {
    method: "POST"
  }).then(response => response.json());
}

export async function fetchGameState(gameId: string) {
  return await fetch(`${API_URL}/gameStates/${gameId}`).then(response => response.json());
}

export async function fetchGame(gameId: string): Promise<GameRecord> {
  return await fetch(`${API_URL}/games/${gameId}`).then(response => response.json());
}

export async function fetchGameRecordState(
  libraryName: string,
  gameIndex: number,
  moveIndex: number
): Promise<GameStateModel> {
  return await fetch(
    `${API_URL}/gameRecords/${libraryName}/${gameIndex}/${moveIndex}`
  ).then(response => response.json());
}
