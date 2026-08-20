import { Link } from 'react-router-dom'

import type { ChatCitation } from './chat.types'
import { useCitationTarget } from './useCitationTarget'

interface CitationsPanelProps {
    citations: ChatCitation[]
}

export function CitationsPanel({ citations }: CitationsPanelProps) {
    if (citations.length === 0) {
        return (
            <div className="nexus-citations-panel">
                <p className="nexus-citations-empty">
                    This turn cited no context.
                </p>
            </div>
        )
    }

    return (
        <div className="nexus-citations-panel">
            <p className="nexus-citations-note">
                Trust level is not part of the citation contract yet -
                only a context item id and an optional span are returned.
            </p>

            <ul className="nexus-citations-list">
                {citations.map((citation, index) => (
                    <CitationRow
                        key={`${citation.contextItemId}-${index}`}
                        citation={citation}
                    />
                ))}
            </ul>
        </div>
    )
}

function CitationRow({ citation }: { citation: ChatCitation }) {
    const { target, isResolving } = useCitationTarget(
        citation.contextItemId,
    )

    return (
        <li className="nexus-citation-row">
            <span className="nexus-citation-kind">
                {isResolving
                    ? 'Resolving...'
                    : target.kind === 'knowledge'
                        ? 'Knowledge'
                        : target.kind === 'workitem'
                            ? 'Work item'
                            : 'Unresolved'}
            </span>

            {target.kind === 'knowledge' ||
            target.kind === 'workitem' ? (
                <Link
                    to={target.href}
                    className="nexus-citation-reference"
                >
                    {target.label}
                </Link>
            ) : (
                <span className="nexus-citation-reference nexus-citation-reference-plain">
                    {citation.contextItemId}
                </span>
            )}

            {citation.span && (
                <p className="nexus-citation-span">
                    "{citation.span}"
                </p>
            )}
        </li>
    )
}
