import Board from "./components/Board"

export default async function Game({ params }: {
    params: Promise<{ id: string }>
}) {
    const { id } = await params
    return <Board gameId={id} />
}
