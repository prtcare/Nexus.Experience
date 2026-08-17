import { useQuery } from '@tanstack/react-query'
import { getProjects } from './projectsApi'

export function useProjects(
    workspaceId: string | null | undefined,
) {
    return useQuery({
        queryKey: ['projects', workspaceId],
        queryFn: () => getProjects(workspaceId!),
        enabled: Boolean(workspaceId),
    })
}