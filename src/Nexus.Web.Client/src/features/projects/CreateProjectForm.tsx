import {
    useState,
    type FormEvent,
} from 'react'

import { useSelectedWorkspace } from '../workspaces/WorkspaceContext'
import { useCreateProject } from './useCreateProject'

export function CreateProjectForm() {
    const {
        selectedWorkspace,
        selectedWorkspaceId,
    } = useSelectedWorkspace()

    const createProjectMutation =
        useCreateProject()

    const [name, setName] = useState('')

    async function handleSubmit(
        event: FormEvent<HTMLFormElement>,
    ) {
        event.preventDefault()

        const trimmedName = name.trim()

        if (!selectedWorkspaceId || !trimmedName) {
            return
        }

        try {
            await createProjectMutation.mutateAsync({
                workspaceId: selectedWorkspaceId,
                name: trimmedName,
            })

            setName('')
        }
        catch {
            // Error is displayed below.
        }
    }

    if (!selectedWorkspace) {
        return null
    }

    return (
        <form
            className="nexus-create-project"
            onSubmit={handleSubmit}
        >
            <div>
                <strong>New project</strong>

                <p>
                    Create a project in{' '}
                    {selectedWorkspace.name}.
                </p>
            </div>

            <div className="nexus-create-project-controls">
                <input
                    type="text"
                    value={name}
                    placeholder="Project name"
                    disabled={
                        createProjectMutation.isPending
                    }
                    onChange={(event) =>
                        setName(event.target.value)
                    }
                />

                <button
                    type="submit"
                    className="nexus-primary-button"
                    disabled={
                        !name.trim() ||
                        createProjectMutation.isPending
                    }
                >
                    {createProjectMutation.isPending
                        ? 'Creating...'
                        : 'Create project'}
                </button>
            </div>

            {createProjectMutation.isError && (
                <p className="nexus-form-error">
                    Unable to create project.
                </p>
            )}

            {createProjectMutation.isSuccess && (
                <p className="nexus-form-success">
                    Project created successfully.
                </p>
            )}
        </form>
    )
}