import { useQuery } from '@tanstack/react-query'
import { projectsApi } from './projectsApi'
import type { Project } from './Project'

export function useProjects(
    workspaceId: string | undefined,
) {
    return useQuery<Project[]>({
        queryKey: ['projects', workspaceId],
        queryFn: () =>
            projectsApi.list(workspaceId!),
        enabled: Boolean(workspaceId),
    })
}