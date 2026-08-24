import { useQuery } from '@tanstack/react-query'

import { citationTargetsApi } from './citationTargets'

export type CitationTarget =
    | { kind: 'knowledge'; href: string; label: string }
    | { kind: 'workitem'; href: string; label: string }
    | { kind: 'unresolved' }

// The Citation contract carries only a bare contextItemId - no Kind, no
// type prefix (see ChatContextBundleMapper: every domain entity's raw
// Guid becomes the id, with nothing to tell them apart). Knowledge and
// WorkItem are the only entities with a frontend-facing GET-by-id route,
// so "does this citation map to something we can route to" can only be
// answered by asking each of those routes in turn. Decision (Adr) has no
// read route at all - see the F3 report - so it can never resolve here,
// and neither can Message or Project ids, which were never meant to be
// looked up this way.
//
// An id that belongs to neither fails both lookups. As found while
// testing, a missing entity does not 404 cleanly - Dataverse throws and
// the API leaks it as an unhandled HTTP 500. Either failure is treated
// the same way here: try the next candidate, then give up.
export function useCitationTarget(contextItemId: string): {
    target: CitationTarget
    isResolving: boolean
} {
    const knowledgeQuery = useQuery({
        queryKey: ['citation-target-knowledge', contextItemId],
        queryFn: () => citationTargetsApi.getKnowledge(contextItemId),
        retry: false,
        staleTime: Infinity,
    })

    const workItemQuery = useQuery({
        queryKey: ['citation-target-workitem', contextItemId],
        queryFn: () => citationTargetsApi.getWorkItem(contextItemId),
        retry: false,
        staleTime: Infinity,
        enabled: knowledgeQuery.isError,
    })

    if (knowledgeQuery.data) {
        return {
            isResolving: false,
            target: {
                kind: 'knowledge',
                href: `/knowledge/${contextItemId}`,
                label: knowledgeQuery.data.title,
            },
        }
    }

    if (workItemQuery.data) {
        return {
            isResolving: false,
            target: {
                kind: 'workitem',
                href: `/workitems/${contextItemId}`,
                label: workItemQuery.data.title,
            },
        }
    }

    const isResolving =
        knowledgeQuery.isPending ||
        (knowledgeQuery.isError && workItemQuery.isPending)

    return {
        isResolving,
        target: { kind: 'unresolved' },
    }
}
