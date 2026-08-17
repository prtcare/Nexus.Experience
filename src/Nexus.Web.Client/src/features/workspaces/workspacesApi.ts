import { nexusApi } from '../../api/ApiClient'

import type {
    CreateWorkspaceRequest,
    CreateWorkspaceResponse,
    ListWorkspacesResponse,
    UpdateWorkspaceRequest,
    UpdateWorkspaceResponse,
    Workspace,
} from './Workspace'

export const workspacesApi = {
    list(): Promise<ListWorkspacesResponse> {
        return nexusApi.get<ListWorkspacesResponse>(
            '/api/workspaces',
        )
    },

    get(workspaceId: string): Promise<Workspace> {
        return nexusApi.get<Workspace>(
            `/api/workspaces/${workspaceId}`,
        )
    },

    create(
        request: CreateWorkspaceRequest,
    ): Promise<CreateWorkspaceResponse> {
        return nexusApi.post<
            CreateWorkspaceResponse,
            CreateWorkspaceRequest
        >('/api/workspaces', request)
    },

    update(
        workspaceId: string,
        request: UpdateWorkspaceRequest,
    ): Promise<UpdateWorkspaceResponse> {
        return nexusApi.put<
            UpdateWorkspaceResponse,
            UpdateWorkspaceRequest
        >(`/api/workspaces/${workspaceId}`, request)
    },
}