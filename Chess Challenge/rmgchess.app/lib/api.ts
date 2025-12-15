import { API_URL } from "./config";

export async function fetchGames() {
  return await fetch(`${API_URL}/games`).then(response => response.json());
}

export async function fetchGame(id: string) {
  return await fetch(`${API_URL}/games/${id}`).then(response => response.json());
}