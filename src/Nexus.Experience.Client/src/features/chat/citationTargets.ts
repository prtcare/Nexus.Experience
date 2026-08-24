import { nexusApi } from '../../api/ApiClient'

// GET /knowledge/{id} returns GetKnowledgeResult as-is - both ids are the
// wrapped struct shape, not flattened, same as the conversation/message
// list endpoints. There is no Status field here; GetKnowledgeResult never
// mapped it, unlike the domain model that backs it.
export interface KnowledgeDetail {
    knowledgeId: { value: string }
    workspaceId: { value: string }
    title: string
    content: string
    type: string
    createdAt: string
}

// GET /workitems/{id} goes through GetWorkItemResponse, which flattens the
// ids to plain strings but - unlike Knowledge's `type` - sends Type and
// Status as raw integers cast from the enum, not enum names. Mirrored, not
// normalized: WorkItemType 1-8 (Task..Spike), WorkItemStatus 1-5
// (New..Cancelled).
export interface WorkItemDetail {
    workItemId: string
    projectId: string
    title: string
    description: string
    type: number
    status: number
    createdAt: string
}

export const citationTargetsApi = {
    getKnowledge(id: string): Promise<KnowledgeDetail> {
        return nexusApi.get<KnowledgeDetail>(`/knowledge/${id}`)
    },

    getWorkItem(id: string): Promise<WorkItemDetail> {
        return nexusApi.get<WorkItemDetail>(`/workitems/${id}`)
    },
}
