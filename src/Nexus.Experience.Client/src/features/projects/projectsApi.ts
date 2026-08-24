import { nexusApi } from '../../api/ApiClient'

import type {
    CreateProjectRequest,
    CreateProjectResponse,
    Project,
    ProjectDetails,
    UpdateProjectRequest,
    UpdateProjectResponse,
} from './Project'

export const projectsApi = {
    list(
        workspaceId: string,
    ): Promise<Project[]> {
        return nexusApi.get<Project[]>(
            `/workspaces/${workspaceId}/projects`,
        )
    },

    get(
        projectId: string,
    ): Promise<ProjectDetails> {
        return nexusApi.get<ProjectDetails>(
            `/projects/${projectId}`,
        )
    },

    create(
        request: CreateProjectRequest,
    ): Promise<CreateProjectResponse> {
        return nexusApi.post<
            CreateProjectResponse,
            CreateProjectRequest
        >(
            '/projects',
            request,
        )
    },

    update(
        projectId: string,
        request: UpdateProjectRequest,
    ): Promise<UpdateProjectResponse> {
        return nexusApi.put<
            UpdateProjectResponse,
            UpdateProjectRequest
        >(
            `/projects/${projectId}`,
            request,
        )
    },
}
