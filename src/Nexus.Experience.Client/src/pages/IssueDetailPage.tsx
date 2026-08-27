import { useParams } from 'react-router-dom'

import { formatApiError } from '../api/ApiError'
import { useIssue } from '../features/developer/useIssue'
import { useObjectChatLinksByTarget } from '../features/developer/useObjectChatLinksByTarget'

export function IssueDetailPage() {
    const { issueId } = useParams<{
        issueId: string
    }>()

    const {
        data: issue,
        isPending: issuePending,
        isError: issueIsError,
        error: issueError,
    } = useIssue(issueId)

    const {
        data: links,
        isPending: linksPending,
        isError: linksIsError,
        error: linksError,
    } = useObjectChatLinksByTarget(
        'Issue',
        issueId,
    )

    if (issuePending) {
        return (
            <div className="nexus-empty-state">
                Loading issue...
            </div>
        )
    }

    if (issueIsError) {
        return (
            <div className="nexus-empty-state">
                Unable to load issue —{' '}
                {formatApiError(issueError)}
            </div>
        )
    }

    if (!issue) {
        return (
            <div className="nexus-empty-state">
                This issue could not be found.
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
                        Issue · {issue.reference}
                    </span>

                    <h1>{issue.title}</h1>

                    <p>
                        Issue ID: {issue.issueId}
                    </p>
                </div>
            </header>

            <div className="nexus-card">
                <div className="nexus-card-content">
                    <p className="nexus-detail-content">
                        {issue.description ||
                            'No description.'}
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
                            this issue yet.
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
