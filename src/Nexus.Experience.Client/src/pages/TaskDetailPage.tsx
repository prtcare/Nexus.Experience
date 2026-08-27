import { useParams } from 'react-router-dom'

import { formatApiError } from '../api/ApiError'
import { useObjectChatLinksByTarget } from '../features/developer/useObjectChatLinksByTarget'
import { useTask } from '../features/developer/useTask'

export function TaskDetailPage() {
    const { taskId } = useParams<{
        taskId: string
    }>()

    const {
        data: task,
        isPending: taskPending,
        isError: taskIsError,
        error: taskError,
    } = useTask(taskId)

    const {
        data: links,
        isPending: linksPending,
        isError: linksIsError,
        error: linksError,
    } = useObjectChatLinksByTarget('Task', taskId)

    if (taskPending) {
        return (
            <div className="nexus-empty-state">
                Loading task...
            </div>
        )
    }

    if (taskIsError) {
        return (
            <div className="nexus-empty-state">
                Unable to load task —{' '}
                {formatApiError(taskError)}
            </div>
        )
    }

    if (!task) {
        return (
            <div className="nexus-empty-state">
                This task could not be found.
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
                        Task · {task.reference}
                    </span>

                    <h1>{task.title}</h1>

                    <p>
                        Task ID: {task.taskId}
                    </p>
                </div>
            </header>

            <div className="nexus-card">
                <div className="nexus-card-content">
                    <p className="nexus-detail-content">
                        {task.description ||
                            'No description.'}
                    </p>

                    <p className="nexus-muted">
                        Feature ID: {task.featureId}
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
                            this task yet.
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
