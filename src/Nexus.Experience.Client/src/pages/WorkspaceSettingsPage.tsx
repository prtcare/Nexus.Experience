import { UpdateWorkspaceForm } from '../features/workspaces/UpdateWorkspaceForm'
import { WorkspaceSelector } from '../features/workspaces/WorkspaceSelector'

export function WorkspaceSettingsPage() {
    return (
        <div>
            <header className="nexus-page-header">
                <div>
                    <span className="nexus-page-eyebrow">
                        Workspaces
                    </span>

                    <h1>Workspace settings</h1>

                    <p>
                        Update the selected workspace.
                    </p>
                </div>

                <WorkspaceSelector />
            </header>

            <UpdateWorkspaceForm />
        </div>
    )
}