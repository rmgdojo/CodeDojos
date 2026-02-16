import ChessBoard from "@/app/components/ChessBoard"

export default async function GameRecord({ params }: {
    params: Promise<{ libraryName: string; gameIndex: string; moveIndex: string }>
}) {
    const { libraryName, gameIndex, moveIndex } = await params
    return (
        <ChessBoard 
            mode="record"
            libraryName={libraryName} 
            gameIndex={parseInt(gameIndex)} 
            moveIndex={parseInt(moveIndex)} 
        />
    )
}
