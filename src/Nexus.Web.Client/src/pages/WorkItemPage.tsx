import { useQuery } from '@tanstack/react-query'
import { useParams } from 'react-router-dom'

import { citationTargetsApi } from '../features/chat/citationTargets'

// GetWorkItemResponse sends Type/Status as the raw int cast from the
// domain enum (WorkItemType, WorkItemStatus) rather than a name - mirrored
// labels here, not a contract change.
const WORK_ITEM_TYPE_LABELS: Record<number, string> = {
    1: 'Task',
    2: 'Bug',
    3: 'Feature',
    4: 'Epic',
    5: 'Story',
    6: 'Research',
    7: 'Idea',
    8: 'Spike',
}

const WORK_ITEM_STATUS_LABELS: Record<number, string> = {
    1: 'New',
    2: 'Active',
    3: 'Blocked',
    4: 'Completed',
    5: 'Cancelled',
}

export function WorkItemPage() {
    const { workItemId } = useParams<{
        workItemId: string
    }>()

    const { data, isPending, isError } = useQuery({
        queryKey: ['workitem-detail', workItemId],
        queryFn: () =>
            citationTargetsApi.getWorkItem(workItemId!),
        enabled: Boolean(workItemId),
    })

    if (isPending) {
        return (
            <div className="nexus-empty-state">
                Loading work item...
            </div>
        )
    }

    if (isError || !data) {
        return (
            <div className="nexus-empty-state">
                Unable to load this work item.
            </div>
        )
    }

    return (
        <div>
            <header className="nexus-page-header">
                <div>
                    <span className="nexus-page-eyebrow">
                        Work item ·{' '}
                        {WORK_ITEM_TYPE_LABELS[data.type] ??
                            `Type ${data.type}`}{' '}
                        ·{' '}
                        {WORK_ITEM_STATUS_LABELS[
                            data.status
                        ] ?? `Status ${data.status}`}
                    </span>

                    <h1>{data.title}</h1>
                </div>
            </header>

            <div className="nexus-card">
                <div className="nexus-card-content">
                    <p className="nexus-detail-content">
                        {data.description || 'No description.'}
                    </p>
                </div>
            </div>
        </div>
    )
}
