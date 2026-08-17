import { useSelectedWorkspace } from '../workspaces/useSelectedWorkspace'
import { useProjects } from './useProjects'

export function WorkspaceProjects() {
    const {
        selectedWorkspace,
        selectedWorkspaceId,
        isPending: workspacesPending,
    } = useSelectedWorkspace()

    const {
        data: projects,
        isPending: projectsPending,
        isError,
    } = useProjects(selectedWorkspaceId)

    if (workspacesPending) {
        return <div>Loading workspace...</div>
    }

    if (!selectedWorkspace) {
        return <div>No workspace selected.</div>
    }

    if (projectsPending) {
        return <div>Loading projects...</div>
    }

    if (isError) {
        return <div>Unable to load projects.</div>
    }

    if (!projects || projects.length === 0) {
        return (
            <div>
                No projects in {selectedWorkspace.name}.
            </div>
        )
    }

    return (
        <div>
            <h2>{selectedWorkspace.name}</h2>

            {projects.map(project => (
                <div key={project.projectId}>
                    <strong>{project.name}</strong>
                    <div>
                        {new Date(
                            project.createdAt,
                        ).toLocaleString()}
                    </div>
                </div>
            ))}
        </div>
    )
}