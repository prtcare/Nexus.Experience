import { useParams } from 'react-router-dom'

import { formatApiError } from '../api/ApiError'
import { useFeature } from '../features/developer/useFeature'
import { useObjectChatLinksByTarget } from '../features/developer/useObjectChatLinksByTarget'

export function FeatureDetailPage() {
    const { featureId } = useParams<{
        featureId: string
    }>()

    const {
        data: feature,
        isPending: featurePending,
        isError: featureIsError,
        error: featureError,
    } = useFeature(featureId)

    const {
        data: links,
        isPending: linksPending,
        isError: linksIsError,
        error: linksError,
    } = useObjectChatLinksByTarget('Feature', featureId)

    if (featurePending) {
        return (
            <div className="nexus-empty-state">
                Loading feature...
            </div>
        )
    }

    if (featureIsError) {
        return (
            <div className="nexus-empty-state">
                Unable to load feature —{' '}
                {formatApiError(featureError)}
            </div>
        )
    }

    if (!feature) {
        return (
            <div className="nexus-empty-state">
                This feature could not be found.
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
                        Feature · {feature.reference}
                    </span>

                    <h1>{feature.title}</h1>

                    <p>
                        Feature ID: {feature.featureId}
                    </p>
                </div>
            </header>

            <div className="nexus-card">
                <div className="nexus-card-content">
                    <p className="nexus-detail-content">
                        {feature.description ||
                            'No description.'}
                    </p>

                    <p className="nexus-muted">
                        Subproject ID: {feature.subprojectId}
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
                            this feature yet.
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
