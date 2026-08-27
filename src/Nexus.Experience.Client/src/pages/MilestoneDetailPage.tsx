import { useParams } from 'react-router-dom'

import { formatApiError } from '../api/ApiError'
import { useMilestone } from '../features/developer/useMilestone'
import { useObjectChatLinksByTarget } from '../features/developer/useObjectChatLinksByTarget'

export function MilestoneDetailPage() {
    const { milestoneId } = useParams<{
        milestoneId: string
    }>()

    const {
        data: milestone,
        isPending: milestonePending,
        isError: milestoneIsError,
        error: milestoneError,
    } = useMilestone(milestoneId)

    const {
        data: links,
        isPending: linksPending,
        isError: linksIsError,
        error: linksError,
    } = useObjectChatLinksByTarget(
        'Milestone',
        milestoneId,
    )

    if (milestonePending) {
        return (
            <div className="nexus-empty-state">
                Loading milestone...
            </div>
        )
    }

    if (milestoneIsError) {
        return (
            <div className="nexus-empty-state">
                Unable to load milestone —{' '}
                {formatApiError(milestoneError)}
            </div>
        )
    }

    if (!milestone) {
        return (
            <div className="nexus-empty-state">
                This milestone could not be found.
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
                        Milestone · {milestone.reference}
                    </span>

                    <h1>{milestone.name}</h1>

                    <p>
                        Milestone ID: {milestone.milestoneId}
                    </p>
                </div>
            </header>

            <div className="nexus-card">
                <div className="nexus-card-content">
                    <p className="nexus-detail-content">
                        {milestone.description ||
                            'No description.'}
                    </p>

                    <p className="nexus-muted">
                        Subproject ID: {milestone.subprojectId}
                    </p>

                    <p className="nexus-muted">
                        Target date:{' '}
                        {milestone.targetDate
                            ? new Date(
                                  milestone.targetDate,
                              ).toLocaleDateString()
                            : 'None'}
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
                            this milestone yet.
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
