"use client";

import { useState, useEffect } from "react";
import Link from "next/link";
import { API_URL } from "@/lib/config";

interface GameSummary {
  id: string;
  playingWhite: string;
  playingBlack: string;
  event: string;
  date: string;
}

export default function GamesListPage() {
  const [games, setGames] = useState<GameSummary[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function fetchGames() {
      try {
        setLoading(true);
        const response = await fetch(`${API_URL}/games`);
        if (!response.ok) {
          throw new Error("Failed to fetch games");
        }
        const data = await response.json();
        setGames(data);
      } catch (err) {
        const errorMessage = err instanceof Error ? err.message : "Unknown error";
        setError(errorMessage);
      } finally {
        setLoading(false);
      }
    }

    fetchGames();
  }, []);

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen bg-gray-100">
        <div className="text-xl font-semibold text-gray-700">Loading games...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100">
        <div className="text-xl font-semibold text-red-600 mb-4">Error loading games</div>
        <div className="text-sm text-gray-600">{error}</div>
        <Link href="/" className="mt-4 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700">
          Back to Home
        </Link>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-100">
      <header className="bg-gray-800 text-white p-6 shadow-lg">
        <div className="max-w-6xl mx-auto">
          <h1 className="text-3xl font-bold mb-2">Chess Game Records</h1>
          <p className="text-gray-300">Browse historic chess games</p>
          <Link className="inline-block mt-2 text-blue-300 hover:text-blue-100" href="/">
            ← Back to Home
          </Link>
        </div>
      </header>

      <main className="max-w-6xl mx-auto p-6">
        <div className="bg-white rounded-lg shadow-md overflow-hidden">
          <table className="w-full">
            <thead className="bg-gray-50 border-b">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  #
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Event
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  White
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Black
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Date
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Actions
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {games.map((game, index) => (
                <tr key={game.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {index}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                    {game.event}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {game.playingWhite}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {game.playingBlack}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {new Date(game.date).toLocaleDateString()}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm">
                    <Link
                      href={`/records/MagnusCarlsenGames/${index}/0`}
                      className="text-blue-600 hover:text-blue-800 font-medium"
                    >
                      View Game →
                    </Link>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {games.length === 0 && (
          <div className="text-center py-12 text-gray-500">
            No games found
          </div>
        )}
      </main>
    </div>
  );
}
