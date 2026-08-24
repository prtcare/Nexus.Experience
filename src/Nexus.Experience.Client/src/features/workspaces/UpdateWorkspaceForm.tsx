import {
    useEffect,
    useState,
    type FormEvent,
} from 'react'

import { formatApiError } from '../../api/ApiError'
import { useSelectedWorkspace } from './WorkspaceContext'
import { useUpdateWorkspace } from './useUpdateWorkspace'

export function UpdateWorkspaceForm() {
    const {
        selectedWorkspace,
        selectedWorkspaceId,
    } = useSelectedWorkspace()

    const updateWorkspaceMutation =
        useUpdateWorkspace()

    const [name, setName] = useState('')
    const [owner, setOwner] = useState('')
    const [description, setDescription] =
        useState('')

    useEffect(() => {
        if (!selectedWorkspace) {
            setName('')
            setOwner('')
            setDescription('')
            return
        }

        setName(selectedWorkspace.name)
        setOwner(selectedWorkspace.owner)
        setDescription(
            selectedWorkspace.description ?? '',
        )
    }, [selectedWorkspace])

    async function handleSubmit(
        event: FormEvent<HTMLFormElement>,
    ) {
        event.preventDefault()

        if (!selectedWorkspaceId) {
            return
        }

        const trimmedName = name.trim()
        const trimmedOwner = owner.trim()
        const trimmedDescription =
            description.trim()

        if (!trimmedName || !trimmedOwner) {
            return
        }

        try {
            await updateWorkspaceMutation.mutateAsync({
                workspaceId: selectedWorkspaceId,

                request: {
                    name: trimmedName,
                    owner: trimmedOwner,
                    description: trimmedDescription,
                },
            })
        }
        catch {
            // Error displayed below.
        }
    }

    if (!selectedWorkspace) {
        return (
            <div className="nexus-empty-state">
                Select a workspace first.
            </div>
        )
    }

    return (
        <form
            className="nexus-update-workspace"
            onSubmit={handleSubmit}
        >
            <div className="nexus-form-field">
                <label htmlFor="edit-workspace-name">
                    Name
                </label>

                <input
                    id="edit-workspace-name"
                    type="text"
                    value={name}
                    disabled={
                        updateWorkspaceMutation.isPending
                    }
                    onChange={(event) =>
                        setName(event.target.value)
                    }
                />
            </div>

            <div className="nexus-form-field">
                <label htmlFor="edit-workspace-owner">
                    Owner
                </label>

                <input
                    id="edit-workspace-owner"
                    type="text"
                    value={owner}
                    disabled={
                        updateWorkspaceMutation.isPending
                    }
                    onChange={(event) =>
                        setOwner(event.target.value)
                    }
                />
            </div>

            <div className="nexus-form-field">
                <label htmlFor="edit-workspace-description">
                    Description
                </label>

                <textarea
                    id="edit-workspace-description"
                    value={description}
                    disabled={
                        updateWorkspaceMutation.isPending
                    }
                    onChange={(event) =>
                        setDescription(event.target.value)
                    }
                />
            </div>

            <button
                type="submit"
                className="nexus-primary-button"
                disabled={
                    !name.trim() ||
                    !owner.trim() ||
                    updateWorkspaceMutation.isPending
                }
            >
                {updateWorkspaceMutation.isPending
                    ? 'Saving...'
                    : 'Save changes'}
            </button>

            {updateWorkspaceMutation.isError && (
                <p className="nexus-form-error">
                    Unable to update workspace —{' '}
                    {formatApiError(
                        updateWorkspaceMutation.error,
                    )}
                </p>
            )}

            {updateWorkspaceMutation.isSuccess && (
                <p className="nexus-form-success">
                    Workspace updated successfully.
                </p>
            )}
        </form>
    )
}