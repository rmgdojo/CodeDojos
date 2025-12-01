export default function Board() {
  const files = ["a", "b", "c", "d", "e", "f", "g", "h"];
  const ranks = [8, 7, 6, 5, 4, 3, 2, 1];

  return (
    <div className="flex flex-col w-screen h-screen bg-gray-100">
      <header className="bg-gray-800 text-white p-4 shadow-lg">
        <h1 className="text-2xl font-bold">Chess Board</h1>
      </header>
      <div className="flex-1 flex items-center justify-center">
        <div className="w-full h-full max-w-screen max-h-screen flex flex-col border-4 border-gray-800">
          {ranks.map((rank) => (
            <div key={rank} className="flex flex-1">
              {files.map((file, fileIndex) => {
                const isLight = (rank + fileIndex) % 2 === 0;
                return (
                  <div
                    key={`${file}${rank}`}
                    className={`flex-1 flex items-center justify-center text-3xl font-semibold ${
                      isLight ? "bg-amber-100" : "bg-amber-700"
                    }`}
                  >
                    <span
                      className={`fas fa-chess-king ${
                        isLight ? "text-black" : "text-white"
                      }`}
                    ></span>
                  </div>
                );
              })}
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
