import { useQuery } from '@tanstack/react-query'
import { workspacesApi } from './workspacesApi'
import type { ListWorkspacesResponse } from './Workspace'

export function useWorkspaces() {
    return useQuery<ListWorkspacesResponse>({
        queryKey: ['workspaces'],
        queryFn: () => workspacesApi.list(),
    })
}