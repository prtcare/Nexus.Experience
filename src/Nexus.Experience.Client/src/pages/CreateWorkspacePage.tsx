import { useNavigate } from 'react-router-dom'
import { CreateWorkspaceForm } from '../features/workspaces/CreateWorkspaceForm'

export function CreateWorkspacePage() {
    const navigate = useNavigate()

    return (
        <div>
            <header className="nexus-page-header">
                <div>
                    <span className="nexus-page-eyebrow">
                        Workspaces
                    </span>

                    <h1>New workspace</h1>

                    <p>
                        Create a new Nexus workspace.
                    </p>
                </div>
            </header>

            <CreateWorkspaceForm
                onCreated={() =>
                    navigate('/workspaces')
                }
            />
        </div>
    )
}