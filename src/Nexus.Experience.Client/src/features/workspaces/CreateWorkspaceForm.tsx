import {
    useState,
    type FormEvent,
} from 'react'

import { formatApiError } from '../../api/ApiError'
import { useCreateWorkspace } from './useCreateWorkspace'

type CreateWorkspaceFormProps = {
    onCreated?: () => void
}

export function CreateWorkspaceForm({
    onCreated,
}: CreateWorkspaceFormProps) {
    const createWorkspaceMutation =
        useCreateWorkspace()

    const [name, setName] = useState('')
    const [owner, setOwner] = useState('')
    const [description, setDescription] =
        useState('')

    async function handleSubmit(
        event: FormEvent<HTMLFormElement>,
    ) {
        event.preventDefault()

        const trimmedName = name.trim()
        const trimmedOwner = owner.trim()
        const trimmedDescription =
            description.trim()

        if (!trimmedName || !trimmedOwner) {
            return
        }

        try {
            await createWorkspaceMutation.mutateAsync({
                name: trimmedName,
                owner: trimmedOwner,
                description: trimmedDescription,
            })

            setName('')
            setOwner('')
            setDescription('')

            onCreated?.()
        }
        catch {
            // Error rendered below.
        }
    }

    return (
        <form
            className="nexus-create-workspace"
            onSubmit={handleSubmit}
        >
            <div className="nexus-form-field">
                <label htmlFor="workspace-name">
                    Name
                </label>

                <input
                    id="workspace-name"
                    type="text"
                    value={name}
                    placeholder="Workspace name"
                    disabled={
                        createWorkspaceMutation.isPending
                    }
                    onChange={(event) =>
                        setName(event.target.value)
                    }
                />
            </div>

            <div className="nexus-form-field">
                <label htmlFor="workspace-owner">
                    Owner
                </label>

                <input
                    id="workspace-owner"
                    type="text"
                    value={owner}
                    placeholder="Owner"
                    disabled={
                        createWorkspaceMutation.isPending
                    }
                    onChange={(event) =>
                        setOwner(event.target.value)
                    }
                />
            </div>

            <div className="nexus-form-field">
                <label htmlFor="workspace-description">
                    Description
                </label>

                <textarea
                    id="workspace-description"
                    value={description}
                    placeholder="Description"
                    disabled={
                        createWorkspaceMutation.isPending
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
                    createWorkspaceMutation.isPending
                }
            >
                {createWorkspaceMutation.isPending
                    ? 'Creating...'
                    : 'Create workspace'}
            </button>

            {createWorkspaceMutation.isError && (
                <p className="nexus-form-error">
                    Unable to create workspace —{' '}
                    {formatApiError(
                        createWorkspaceMutation.error,
                    )}
                </p>
            )}

            {createWorkspaceMutation.isSuccess && (
                <p className="nexus-form-success">
                    Workspace created successfully.
                </p>
            )}
        </form>
    )
}