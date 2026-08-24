import {
    useMutation,
    useQueryClient,
} from '@tanstack/react-query'

import { workspacesApi } from './workspacesApi'
import type { UpdateWorkspaceRequest } from './Workspace'

type UpdateWorkspaceVariables = {
    workspaceId: string
    request: UpdateWorkspaceRequest
}

export function useUpdateWorkspace() {
    const queryClient = useQueryClient()

    return useMutation({
        mutationFn: ({
            workspaceId,
            request,
        }: UpdateWorkspaceVariables) =>
            workspacesApi.update(
                workspaceId,
                request,
            ),

        onSuccess: async (_, variables) => {
            await Promise.all([
                queryClient.invalidateQueries({
                    queryKey: ['workspaces'],
                }),

                queryClient.invalidateQueries({
                    queryKey: [
                        'workspace',
                        variables.workspaceId,
                    ],
                }),
            ])
        },
    })
}