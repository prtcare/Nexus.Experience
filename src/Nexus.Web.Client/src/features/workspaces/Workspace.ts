export interface Workspace {
    workspaceId: string
    name: string
    owner: string
    description: string
    status: number
    createdAt: string
}

export interface ListWorkspacesResponse {
    workspaces: Workspace[]
}

export interface CreateWorkspaceRequest {
    name: string
    owner: string
    description: string
}

export interface CreateWorkspaceResponse {
    workspaceId: string
    name: string
}

export interface UpdateWorkspaceRequest {
    name: string
    owner: string
    description: string
}

export interface UpdateWorkspaceResponse {
    workspaceId: string
    name: string
}