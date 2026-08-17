import { WorkspaceSelector } from '../features/workspaces/WorkspaceSelector'
import { WorkspaceProjects } from '../features/projects/WorkspaceProjects'

export function WorkspacesPage() {
    return (
        <div>
            <div className="nexus-page-heading">
                <div>
                    <div className="nexus-eyebrow">
                        WORKSPACES
                    </div>

                    <h1>Workspaces</h1>

                    <p>
                        Select a workspace and view its projects.
                    </p>
                </div>

                <WorkspaceSelector />
            </div>

            <WorkspaceProjects />
        </div>
    )
}