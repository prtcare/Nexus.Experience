export interface Project {
    projectId: string
    name: string
    createdAt: string
}

export interface ProjectDetails {
    projectId: string
    workspaceId: string
    name: string
    createdAt: string
}

export interface CreateProjectRequest {
    workspaceId: string
    name: string
}

export interface CreateProjectResponse {
    projectId: string
    name: string
}

export interface UpdateProjectRequest {
    name: string
}

export interface UpdateProjectResponse {
    projectId: string
    name: string
}