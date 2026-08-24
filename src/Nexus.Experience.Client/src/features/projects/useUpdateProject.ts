import {
    useMutation,
    useQueryClient,
} from '@tanstack/react-query'

import { projectsApi } from './projectsApi'
import type { UpdateProjectRequest } from './Project'

type UpdateProjectVariables = {
    projectId: string
    workspaceId: string
    request: UpdateProjectRequest
}

export function useUpdateProject() {
    const queryClient = useQueryClient()

    return useMutation({
        mutationFn: ({
            projectId,
            request,
        }: UpdateProjectVariables) =>
            projectsApi.update(projectId, request),

        onSuccess: async (_, variables) => {
            await Promise.all([
                queryClient.invalidateQueries({
                    queryKey: [
                        'project',
                        variables.projectId,
                    ],
                }),

                queryClient.invalidateQueries({
                    queryKey: [
                        'projects',
                        variables.workspaceId,
                    ],
                }),
            ])
        },
    })
}