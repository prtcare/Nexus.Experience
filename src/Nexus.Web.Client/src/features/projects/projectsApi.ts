import type {
    CreateProjectRequest,
    CreateProjectResponse,
    Project,
    ProjectDetails,
    UpdateProjectRequest,
    UpdateProjectResponse,
} from './Project'

const apiUrl = import.meta.env.VITE_NEXUS_API_URL

export async function getProjects(
    workspaceId: string,
): Promise<Project[]> {
    const response = await fetch(
        `${apiUrl}/api/workspaces/${workspaceId}/projects`,
    )

    if (!response.ok) {
        throw new Error(
            `Failed to load projects: ${response.status}`,
        )
    }

    return response.json() as Promise<Project[]>
}

export async function getProject(
    projectId: string,
): Promise<ProjectDetails> {
    const response = await fetch(
        `${apiUrl}/api/projects/${projectId}`,
    )

    if (!response.ok) {
        throw new Error(
            `Failed to load project: ${response.status}`,
        )
    }

    return response.json() as Promise<ProjectDetails>
}

export async function createProject(
    request: CreateProjectRequest,
): Promise<CreateProjectResponse> {
    const response = await fetch(
        `${apiUrl}/api/projects`,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(request),
        },
    )

    if (!response.ok) {
        const message = await response.text()

        throw new Error(
            message ||
            `Failed to create project: ${response.status}`,
        )
    }

    return response.json() as Promise<CreateProjectResponse>
}

export async function updateProject(
    projectId: string,
    request: UpdateProjectRequest,
): Promise<UpdateProjectResponse> {
    const response = await fetch(
        `${apiUrl}/api/projects/${projectId}`,
        {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(request),
        },
    )

    if (!response.ok) {
        const message = await response.text()

        throw new Error(
            message ||
            `Failed to update project: ${response.status}`,
        )
    }

    return response.json() as Promise<UpdateProjectResponse>
}