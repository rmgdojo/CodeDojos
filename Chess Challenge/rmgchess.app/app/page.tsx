import { GameStateModel } from "./models/GameStateModel";
import { createGameState } from "@/lib/api";
import { redirect } from "next/navigation";

export default async function Home() {
  async function handleCreateGame() {
    "use server";
    const gameState: GameStateModel = await createGameState();
    redirect(`/game/${gameState.id}`);
  }

  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 font-sans dark:bg-black">
      <main className="flex min-h-screen w-full max-w-3xl flex-col items-center justify-between py-32 px-16 bg-white dark:bg-black sm:items-start">
        <div className="flex flex-col items-center gap-6 text-center sm:items-start sm:text-left">
          <h1>
            <form action={handleCreateGame}>
              <button 
                type="submit" 
                id="createGame" 
                className="px-6 py-3 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors"
              >
                Create New Game
              </button>
            </form>
          </h1>
        </div>
      </main>
    </div>
  );
}
