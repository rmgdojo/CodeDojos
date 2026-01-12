import Link from "next/link";
import { GameStateModel } from "./models/GameStateModel";
import { Suspense } from "react";
import { fetchGameState } from "@/lib/api";

export default async function Home() {
  // const games: GameRecord[] = await fetchGames();
  const game: GameStateModel = await fetchGameState();

  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 font-sans dark:bg-black">
      <main className="flex min-h-screen w-full max-w-3xl flex-col items-center justify-between py-32 px-16 bg-white dark:bg-black sm:items-start">
        <div className="flex flex-col items-center gap-6 text-center sm:items-start sm:text-left">
          <h1 className="max-w-s text-3xl font-semibold leading-10 tracking-tight text-black dark:text-zinc-50">
            Game State
          </h1>
          <Suspense fallback={<p>Loading..</p>}>
            <p>
              {JSON.stringify(game)}
            </p>
          </Suspense>
        </div>
      </main>
    </div>
  );
}
