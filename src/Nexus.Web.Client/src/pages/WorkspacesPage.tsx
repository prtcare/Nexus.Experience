import { CreateProjectForm } from '../features/projects/CreateProjectForm'
import { WorkspaceProjects } from '../features/projects/WorkspaceProjects'
import { WorkspaceSelector } from '../features/workspaces/WorkspaceSelector'

export function WorkspacesPage() {
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
            </header>

            <CreateProjectForm />

            <WorkspaceProjects />
        </div>
    )
}