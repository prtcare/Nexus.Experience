import { formatApiError } from '../../api/ApiError'
import { useSelectedWorkspace } from './WorkspaceContext'

export function WorkspaceSelector() {
    const {
        workspaces,
        selectedWorkspaceId,
        selectWorkspace,
        isPending,
        isError,
        error,
    } = useSelectedWorkspace()

    if (isPending) {
        return (
            <select disabled>
                <option>Loading workspaces...</option>
            </select>
        )
    }

    if (isError) {
        return (
            <select
                disabled
                title={formatApiError(error)}
            >
                <option>Unable to load workspaces</option>
            </select>
        )
    }

    if (workspaces.length === 0) {
        return (
            <select disabled>
                <option>No workspaces</option>
            </select>
        )
    }

    return (
        <select
            value={selectedWorkspaceId ?? ''}
            onChange={(event) =>
                selectWorkspace(event.target.value)
            }
        >
            {workspaces.map(workspace => (
                <option
                    key={workspace.workspaceId}
                    value={workspace.workspaceId}
                >
                    {workspace.name}
                </option>
            ))}
        </select>
    )
}