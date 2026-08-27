import { nexusApi } from '../../api/ApiClient'

import type {
    CreateProjectRequest,
    CreateProjectResponse,
    Project,
} from './Project'

export function getProjects(
    workspaceId: string,
): Promise<Project[]> {
    return nexusApi.get<Project[]>(
        `/api/v1/workspaces/${workspaceId}/projects`,
    )
}

export function createProject(
    request: CreateProjectRequest,
): Promise<CreateProjectResponse> {
    return nexusApi.post<
        CreateProjectResponse,
        CreateProjectRequest
    >(
        '/api/v1/projects',
        request,
    )
}
