import { useQuery } from '@tanstack/react-query'
import { workspacesApi } from './workspacesApi'
import type { Workspace } from './Workspace'

export function useWorkspace(
    workspaceId: string | undefined,
) {
    return useQuery<Workspace>({
        queryKey: ['workspace', workspaceId],
        queryFn: () => workspacesApi.get(workspaceId!),
        enabled: Boolean(workspaceId),
    })
}