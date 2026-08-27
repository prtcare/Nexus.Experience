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
            '/api/v1/workspaces',
        )
    },

    get(
        workspaceId: string,
    ): Promise<Workspace> {
        return nexusApi.get<Workspace>(
            `/api/v1/workspaces/${workspaceId}`,
        )
    },

    create(
        request: CreateWorkspaceRequest,
    ): Promise<CreateWorkspaceResponse> {
        return nexusApi.post<
            CreateWorkspaceResponse,
            CreateWorkspaceRequest
        >(
            '/api/v1/workspaces',
            request,
        )
    },

    update(
        workspaceId: string,
        request: UpdateWorkspaceRequest,
    ): Promise<UpdateWorkspaceResponse> {
        return nexusApi.put<
            UpdateWorkspaceResponse,
            UpdateWorkspaceRequest
        >(
            `/api/v1/workspaces/${workspaceId}`,
            request,
        )
    },
}