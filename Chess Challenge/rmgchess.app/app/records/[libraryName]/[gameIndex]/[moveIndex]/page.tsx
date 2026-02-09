import RecordBoard from "./components/RecordBoard"

export default async function GameRecord({ params }: {
    params: Promise<{ libraryName: string; gameIndex: string; moveIndex: string }>
}) {
    const { libraryName, gameIndex, moveIndex } = await params
    return (
        <RecordBoard 
            libraryName={libraryName} 
            gameIndex={parseInt(gameIndex)} 
            moveIndex={parseInt(moveIndex)} 
        />
    )
}
