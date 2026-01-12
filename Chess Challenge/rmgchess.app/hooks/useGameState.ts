import { useState, useEffect } from 'react';
import { GameStateModel } from '@/app/models';
import { API_URL } from '@/lib/config';

/**
 * Hook to fetch and manage game state from the API
 * @param gameId - Optional game ID to load a specific game
 * @returns Game state, loading status, error, and setter function
 */
export function useGameState(gameId?: string) {
  const [gameState, setGameState] = useState<GameStateModel | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function fetchGameState() {
      try {
        setLoading(true);
        setError(null);

        // Determine the API endpoint
        const endpoint = gameId
          ? `${API_URL}/api/game/${gameId}`
          : `${API_URL}/api/game/new`;

        const response = await fetch(endpoint);

        if (!response.ok) {
          throw new Error(`Failed to fetch game state: ${response.statusText}`);
        }

        const data: GameStateModel = await response.json();
        setGameState(data);
      } catch (err) {
        const errorMessage = err instanceof Error ? err.message : 'Unknown error occurred';
        setError(errorMessage);
        console.error('Error fetching game state:', err);
      } finally {
        setLoading(false);
      }
    }

    fetchGameState();
  }, [gameId]);

  return { gameState, loading, error, setGameState };
}
