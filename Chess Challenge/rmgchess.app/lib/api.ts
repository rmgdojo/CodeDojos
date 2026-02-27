import { API_URL } from "./config";
import { GameRecord } from "@/app/models/GameRecord";
import { GameStateModel } from "@/app/models/GameStateModel";
import { MoveModel } from "@/app/models/MoveModel";

async function toJsonOrThrow<T>(response: Response): Promise<T> {
  const data = await response.json();
  if (!response.ok) {
    const message = data?.message ?? response.statusText ?? "Request failed";
    throw new Error(message);
  }
  return data as T;
}

export async function createGameState() {
  const res = await fetch(`${API_URL}/createNewGameState`, { method: "POST" });
  return await toJsonOrThrow(res);
}

export async function fetchGameState(gameId: string): Promise<GameStateModel> {
  const res = await fetch(`${API_URL}/gameStates/${gameId}`);
  return await toJsonOrThrow<GameStateModel>(res);
}

export async function fetchGame(gameId: string): Promise<GameRecord> {
  const res = await fetch(`${API_URL}/games/${gameId}`);
  return await toJsonOrThrow<GameRecord>(res);
}

export async function fetchMovesForPiece(gameId: string, position: string): Promise<MoveModel[]> {
  const res = await fetch(`${API_URL}/gameStates/${gameId}/moves/${position}`);
  return await toJsonOrThrow<MoveModel[]>(res);
}

export async function makeMove(gameId: string, from: string, to: string): Promise<GameStateModel> {
  const res = await fetch(`${API_URL}/gameStates/${gameId}/move`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ from, to }),
  });
  return await toJsonOrThrow<GameStateModel>(res);
}

export async function fetchGameRecordState(
  libraryName: string,
  gameIndex: number,
  moveIndex: number
): Promise<GameStateModel> {
  const res = await fetch(`${API_URL}/gameRecords/${libraryName}/${gameIndex}/${moveIndex}`);
  return await toJsonOrThrow<GameStateModel>(res);
}
