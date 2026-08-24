import { CreateProjectForm } from '../features/projects/CreateProjectForm'
import { WorkspaceProjects } from '../features/projects/WorkspaceProjects'
import { WorkspaceSelector } from '../features/workspaces/WorkspaceSelector'
import { useNavigate } from 'react-router-dom'

export function WorkspacesPage() {
    const navigate = useNavigate()
    return (
        <div>
            <header className="nexus-page-header">
                <div>
                    <span className="nexus-page-eyebrow">
                        Workspaces
                    </span>

                    <h1>Workspaces</h1>

                    <p>
                        Select a workspace and manage
                        its projects.
                    </p>
                </div>

                <WorkspaceSelector />
                <div className="nexus-page-actions">
                    <WorkspaceSelector />

                    <button
                        type="button"
                        className="nexus-secondary-button"
                        onClick={() =>
                            navigate('/workspaces/settings')
                        }
                    >
                        Edit workspace
                    </button>
                </div>
            </header>

            <CreateProjectForm />

            <WorkspaceProjects />
        </div>

    )
}