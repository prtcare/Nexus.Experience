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
    list(workspaceId: string): Promise<Project[]> {
        return nexusApi.get<Project[]>(
            `/api/workspaces/${workspaceId}/projects`,
        )
    },

    get(projectId: string): Promise<ProjectDetails> {
        return nexusApi.get<ProjectDetails>(
            `/api/projects/${projectId}`,
        )
    },

    create(
        request: CreateProjectRequest,
    ): Promise<CreateProjectResponse> {
        return nexusApi.post<
            CreateProjectResponse,
            CreateProjectRequest
        >('/api/projects', request)
    },

    update(
        projectId: string,
        request: UpdateProjectRequest,
    ): Promise<UpdateProjectResponse> {
        return nexusApi.put<
            UpdateProjectResponse,
            UpdateProjectRequest
        >(`/api/projects/${projectId}`, request)
    },
}