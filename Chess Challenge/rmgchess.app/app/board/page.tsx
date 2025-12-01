export default function Board() {
  const files = ["a", "b", "c", "d", "e", "f", "g", "h"];
  const ranks = [8, 7, 6, 5, 4, 3, 2, 1];

  return (
    <div className="flex flex-col w-screen h-screen bg-gray-100">
      <header className="bg-gray-800 text-white p-4 shadow-lg">
        <h1 className="text-2xl font-bold">Chess Board</h1>
      </header>
      <div className="flex-1 flex items-center justify-center p-4">
        <div className="aspect-square w-full h-full max-w-[min(90vw,90vh)] max-h-[min(100vw,100vh)] flex flex-col border-4 border-gray-800">
          {ranks.map((rank) => (
            <div key={rank} className="flex flex-1">
              {files.map((file, fileIndex) => {
                const isLight = (rank + fileIndex) % 2 === 0;
                return (
                  <div
                    key={`${file}${rank}`}
                    className="flex-1 flex items-center justify-center text-7xl font-semibold"
                    style={{ backgroundColor: isLight ? 'var(--chess-light-square)' : 'var(--chess-dark-square)' }}
                  >
                    <span
                      className={`fas fa-chess-king fa-solid ${
                        isLight ? "text-gray-900" : "text-gray-200"
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
