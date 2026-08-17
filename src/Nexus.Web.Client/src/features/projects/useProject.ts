import { useQuery } from '@tanstack/react-query'
import { getProject } from './projectsApi'

export function useProject(
    projectId: string | null | undefined,
) {
    return useQuery({
        queryKey: ['project', projectId],
        queryFn: () => getProject(projectId!),
        enabled: Boolean(projectId),
    })
}