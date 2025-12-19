import Link from "next/link";
import { GameRecord } from "./models/GameRecord";
import { Suspense } from "react";
import { fetchGames } from "@/lib/api";

export default async function Home() {
  const games: GameRecord[] = await fetchGames();

  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 font-sans dark:bg-black">
      <main className="flex min-h-screen w-full max-w-3xl flex-col items-center justify-between py-32 px-16 bg-white dark:bg-black sm:items-start">
        <div className="flex flex-col items-center gap-6 text-center sm:items-start sm:text-left">
          <h1 className="max-w-s text-3xl font-semibold leading-10 tracking-tight text-black dark:text-zinc-50">
            Magnus Carlsen games
          </h1>
          <Suspense fallback={<p>Loading..</p>}>
            <ul role="list" className="divide-y divide-white/5">
              {games.map((game: GameRecord) => (
                <li
                  className="flex justify-between gap-x-16 py-4"
                  key={game.id}
                >
                  <div className="flex min-w-0 gap-x-4">
                    <div className="min-w-0 flex-auto">
                      <Link
                        prefetch={false}
                        className="text-sm/6 font-semibold text-white"
                        href={"/game/" + game.id}
                      >
                        {game.playingWhite} vs {game.playingBlack}
                      </Link>
                    </div>
                  </div>
                  <div className="hidden shrink-0 sm:flex sm:flex-col sm:items-end">
                    <p className="text-sm/6 text-white">{game.event}</p>
                    <p className="mt-1 text-xs/5 text-gray-400">
                      {new Date(game.date).toLocaleDateString()}
                    </p>
                  </div>
                </li>
              ))}
            </ul>
          </Suspense>
        </div>
      </main>
    </div>
  );
}
