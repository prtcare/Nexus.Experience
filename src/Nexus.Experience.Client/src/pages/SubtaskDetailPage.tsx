import { useParams } from 'react-router-dom'

import { formatApiError } from '../api/ApiError'
import { useObjectChatLinksByTarget } from '../features/developer/useObjectChatLinksByTarget'
import { useSubtask } from '../features/developer/useSubtask'

export function SubtaskDetailPage() {
    const { subtaskId } = useParams<{
        subtaskId: string
    }>()

    const {
        data: subtask,
        isPending: subtaskPending,
        isError: subtaskIsError,
        error: subtaskError,
    } = useSubtask(subtaskId)

    const {
        data: links,
        isPending: linksPending,
        isError: linksIsError,
        error: linksError,
    } = useObjectChatLinksByTarget(
        'Subtask',
        subtaskId,
    )

    if (subtaskPending) {
        return (
            <div className="nexus-empty-state">
                Loading subtask...
            </div>
        )
    }

    if (subtaskIsError) {
        return (
            <div className="nexus-empty-state">
                Unable to load subtask —{' '}
                {formatApiError(subtaskError)}
            </div>
        )
    }

    if (!subtask) {
        return (
            <div className="nexus-empty-state">
                This subtask could not be found.
            </div>
        )
    }

    const hasLinks =
        links !== undefined && links.length > 0

    return (
        <div>
            <header className="nexus-page-header">
                <div>
                    <span className="nexus-page-eyebrow">
                        Subtask · {subtask.reference}
                    </span>

                    <h1>{subtask.title}</h1>

                    <p>
                        Subtask ID: {subtask.subtaskId}
                    </p>
                </div>
            </header>

            <div className="nexus-card">
                <div className="nexus-card-content">
                    <p className="nexus-detail-content">
                        {subtask.description ||
                            'No description.'}
                    </p>

                    <p className="nexus-muted">
                        Task ID: {subtask.taskId}
                    </p>
                </div>
            </div>

            <section className="nexus-project-conversations">
                <h2>Linked conversations</h2>

                {linksPending && (
                    <div className="nexus-empty-state">
                        Loading linked conversations...
                    </div>
                )}

                {linksIsError && (
                    <div className="nexus-empty-state">
                        Unable to load linked conversations —{' '}
                        {formatApiError(linksError)}
                    </div>
                )}

                {!linksPending &&
                    !linksIsError &&
                    !hasLinks && (
                    <div className="nexus-empty-state">
                        <strong>No linked conversations</strong>

                        <p>
                            No chat conversations are linked to
                            this subtask yet.
                        </p>
                    </div>
                )}

                {!linksPending &&
                    !linksIsError &&
                    hasLinks && (
                    <div>
                        <ul className="nexus-settings-list">
                            {links.map((link) => (
                                <li
                                    key={link.objectChatLinkId}
                                >
                                    <div>
                                        <strong>
                                            Conversation{' '}
                                            {link.conversationId}
                                        </strong>

                                        <span className="nexus-muted">
                                            Linked{' '}
                                            {new Date(
                                                link.linkedAt,
                                            ).toLocaleString()}
                                        </span>
                                    </div>
                                </li>
                            ))}
                        </ul>

                        <p className="nexus-muted">
                            Opening a linked conversation needs
                            its project ID, which the
                            object-chat-link record does not
                            carry — deep links land in a later
                            slice.
                        </p>
                    </div>
                )}
            </section>
        </div>
    )
}
