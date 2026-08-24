import { useQuery } from '@tanstack/react-query'
import { projectsApi } from './projectsApi'

export function useProject(
    projectId: string | null | undefined,
) {
    return useQuery({
        queryKey: ['project', projectId],
        queryFn: () => projectsApi.get(projectId!),
        enabled: Boolean(projectId),
    })
}