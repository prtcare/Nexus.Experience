import { useNavigate } from 'react-router-dom'

import { useSelectedWorkspace } from '../workspaces/WorkspaceContext'
import { useProjects } from './useProjects'

export function WorkspaceProjects() {
    const navigate = useNavigate()

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
        return (
            <div className="nexus-empty-state">
                Loading workspace...
            </div>
        )
    }

    if (!selectedWorkspace) {
        return (
            <div className="nexus-empty-state">
                No workspace selected.
            </div>
        )
    }

    if (projectsPending) {
        return (
            <div className="nexus-empty-state">
                Loading projects...
            </div>
        )
    }

    if (isError) {
        return (
            <div className="nexus-empty-state">
                Unable to load projects.
            </div>
        )
    }

    if (!projects || projects.length === 0) {
        return (
            <div className="nexus-empty-state">
                <strong>No projects</strong>

                <p>
                    No projects exist in{' '}
                    {selectedWorkspace.name}.
                </p>
            </div>
        )
    }

    return (
        <div className="nexus-project-list">
            {projects.map((project) => (
                <button
                    key={project.projectId}
                    type="button"
                    className="nexus-project-row"
                    onClick={() =>
                        navigate(
                            `/projects/${project.projectId}`,
                        )
                    }
                >
                    <div>
                        <strong>
                            {project.name}
                        </strong>

                        <span>
                            {new Date(
                                project.createdAt,
                            ).toLocaleString()}
                        </span>
                    </div>

                    <span
                        className="nexus-project-row-action"
                        aria-hidden="true"
                    >
                        ›
                    </span>
                </button>
            ))}
        </div>
    )
}