export default function Board() {
    const files = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];
    const ranks = [8, 7, 6, 5, 4, 3, 2, 1];

    return (
        <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100">
            <div className="inline-block border-4 border-gray-800">
                {ranks.map((rank) => (
                    <div key={rank} className="flex">
                        {files.map((file, fileIndex) => {
                            const isLight = (rank + fileIndex) % 2 === 0;
                            return (
                                <div
                                    key={`${file}${rank}`}
                                    className={`w-16 h-16 flex items-center justify-center text-xs font-semibold ${
                                        isLight ? 'bg-amber-100' : 'bg-amber-700'
                                    }`}
                                >
                                    <span className={`fas fa-chess-king text-3xl ${
                                        isLight ? 'text-black' : 'text-white'
                                    }`}></span>
                                </div>
                            );
                        })}
                    </div>
                ))}
            </div>
        </div>
    );
}