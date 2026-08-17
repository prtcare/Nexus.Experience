import {
    useMutation,
    useQueryClient,
} from '@tanstack/react-query'

import { workspacesApi } from './workspacesApi'
import type { CreateWorkspaceRequest } from './Workspace'

export function useCreateWorkspace() {
    const queryClient = useQueryClient()

    return useMutation({
        mutationFn: (
            request: CreateWorkspaceRequest,
        ) => workspacesApi.create(request),

        onSuccess: async () => {
            await queryClient.invalidateQueries({
                queryKey: ['workspaces'],
            })
        },
    })
}