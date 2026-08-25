import { useChatTelemetry } from '../features/chat/ChatTelemetryContext'

export function InsightsPage() {
    const { turns } = useChatTelemetry()

    const sortedTurns = [...turns].reverse()

    return (
        <div>
            <header className="nexus-page-header">
                <div>
                    <span className="nexus-page-eyebrow">
                        Usage
                    </span>

                    <h1>Insights</h1>

                    <p>
                        Usage is not persisted by the product
                        API - this reflects only turns sent
                        from this browser during the current
                        session. Reloading the page resets it.
                    </p>
                </div>
            </header>

            <div className="nexus-card">
                <div className="nexus-card-header">
                    <h2>Decision trace</h2>
                </div>

                <div className="nexus-card-content">
                    <p>
                        Not available from this product's API.
                        Nexus.Intelligence exposes{' '}
                        <code>
                            GET /intelligence/v1/turns/{'{id}'}
                            /explanation
                        </code>
                        , but no product may call Intelligence
                        routes directly (NEXUS_ARCHITECTURE_V2.md
                        §2.3), and the Chat product API has no
                        equivalent route of its own yet.
                    </p>
                </div>
            </div>

            {sortedTurns.length === 0 ? (
                <div className="nexus-empty-state">
                    <strong>No turns yet</strong>

                    <p>
                        Send a chat message to see usage here.
                    </p>
                </div>
            ) : (
                <div className="nexus-card">
                    <div className="nexus-card-header">
                        <h2>Usage per turn</h2>
                    </div>

                    <div className="nexus-card-content">
                        <div className="nexus-insights-table-wrap">
                            <table className="nexus-insights-table">
                                <thead>
                                    <tr>
                                        <th>Sent</th>
                                        <th>Prompt</th>
                                        <th>Model</th>
                                        <th>Tokens in</th>
                                        <th>Tokens out</th>
                                        <th>Estimated cost</th>
                                        <th>Citations</th>
                                        <th>Outcome</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    {sortedTurns.map(
                                        (turn) => (
                                            <tr key={turn.id}>
                                                <td>
                                                    {new Date(
                                                        turn.sentAt,
                                                    ).toLocaleTimeString()}
                                                </td>

                                                <td className="nexus-insights-prompt">
                                                    {turn.prompt}
                                                </td>

                                                <td>
                                                    {turn.usage
                                                        .modelUsed ||
                                                        '—'}
                                                </td>

                                                <td>
                                                    {
                                                        turn
                                                            .usage
                                                            .tokensIn
                                                    }
                                                </td>

                                                <td>
                                                    {
                                                        turn
                                                            .usage
                                                            .tokensOut
                                                    }
                                                </td>

                                                <td>
                                                    $
                                                    {turn.usage.estimatedCost.toFixed(
                                                        4,
                                                    )}
                                                </td>

                                                <td>
                                                    {
                                                        turn
                                                            .citations
                                                            .length
                                                    }
                                                </td>

                                                <td>
                                                    {turn.success ? (
                                                        <span className="nexus-status nexus-status-ok">
                                                            Success
                                                        </span>
                                                    ) : (
                                                        <span
                                                            className="nexus-status nexus-status-error"
                                                            title={
                                                                turn.error ??
                                                                undefined
                                                            }
                                                        >
                                                            Failed
                                                        </span>
                                                    )}
                                                </td>
                                            </tr>
                                        ),
                                    )}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            )}
        </div>
    )
}
