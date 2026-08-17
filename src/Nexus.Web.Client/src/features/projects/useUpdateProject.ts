import {
    useMutation,
    useQueryClient,
} from '@tanstack/react-query'

import { updateProject } from './projectsApi'
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
            updateProject(projectId, request),

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