import {
    useMutation,
    useQueryClient,
} from '@tanstack/react-query'

import { projectsApi } from './projectsApi'
import type { CreateProjectRequest } from './Project'

export function useCreateProject() {
    const queryClient = useQueryClient()

    return useMutation({
        mutationFn: (
            request: CreateProjectRequest,
        ) => projectsApi.create(request),

        onSuccess: async (_, request) => {
            await queryClient.invalidateQueries({
                queryKey: [
                    'projects',
                    request.workspaceId,
                ],
            })
        },
    })
}