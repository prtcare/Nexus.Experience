import { useQuery } from '@tanstack/react-query'
import { useParams } from 'react-router-dom'

import { citationTargetsApi } from '../features/chat/citationTargets'

export function KnowledgeItemPage() {
    const { knowledgeId } = useParams<{
        knowledgeId: string
    }>()

    const { data, isPending, isError } = useQuery({
        queryKey: ['knowledge-detail', knowledgeId],
        queryFn: () =>
            citationTargetsApi.getKnowledge(knowledgeId!),
        enabled: Boolean(knowledgeId),
    })

    if (isPending) {
        return (
            <div className="nexus-empty-state">
                Loading knowledge item...
            </div>
        )
    }

    if (isError || !data) {
        return (
            <div className="nexus-empty-state">
                Unable to load this knowledge item.
            </div>
        )
    }

    return (
        <div>
            <header className="nexus-page-header">
                <div>
                    <span className="nexus-page-eyebrow">
                        Knowledge · {data.type}
                    </span>

                    <h1>{data.title}</h1>
                </div>
            </header>

            <div className="nexus-card">
                <div className="nexus-card-content">
                    <p className="nexus-detail-content">
                        {data.content}
                    </p>
                </div>
            </div>
        </div>
    )
}
