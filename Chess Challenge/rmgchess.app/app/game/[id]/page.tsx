import ChessBoard from "@/app/components/ChessBoard"

export default async function Game({ params }: {
    params: Promise<{ id: string }>
}) {
    const { id } = await params
    return <ChessBoard mode="live" gameId={id} />
}
