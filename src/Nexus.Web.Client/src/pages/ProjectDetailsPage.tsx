import {
    useEffect,
    useState,
    type FormEvent,
} from 'react'

import {
    useNavigate,
    useParams,
} from 'react-router-dom'

import { formatApiError } from '../api/ApiError'
import { CreateConversationForm } from '../features/chat/CreateConversationForm'
import { ConversationList } from '../features/chat/ConversationList'
import { useProject } from '../features/projects/useProject'
import { useUpdateProject } from '../features/projects/useUpdateProject'

export function ProjectDetailsPage() {
    const navigate = useNavigate()

    const { projectId } = useParams<{
        projectId: string
    }>()

    const {
        data: project,
        isPending,
        isError,
        error,
    } = useProject(projectId)

    const updateProjectMutation =
        useUpdateProject()

    const [name, setName] = useState('')

    useEffect(() => {
        if (project) {
            setName(project.name)
        }
    }, [project])

    async function handleSubmit(
        event: FormEvent<HTMLFormElement>,
    ) {
        event.preventDefault()

        if (!project || !projectId) {
            return
        }

        const trimmedName = name.trim()

        if (!trimmedName) {
            return
        }

        try {
            await updateProjectMutation.mutateAsync({
                projectId,
                workspaceId: project.workspaceId,
                request: {
                    name: trimmedName,
                },
            })
        }
        catch {
            // Error is displayed below.
        }
    }

    if (isPending) {
        return (
            <div className="nexus-empty-state">
                Loading project...
            </div>
        )
    }

    if (isError) {
        return (
            <div className="nexus-empty-state">
                Unable to load project —{' '}
                {formatApiError(error)}
            </div>
        )
    }

    if (!project) {
        return (
            <div className="nexus-empty-state">
                This project could not be found.
            </div>
        )
    }

    return (
        <div>
            <header className="nexus-page-header">
                <div>
                    <span className="nexus-page-eyebrow">
                        Project
                    </span>

                    <h1>{project.name}</h1>

                    <p>
                        Project ID: {project.projectId}
                    </p>
                </div>

                <button
                    type="button"
                    className="nexus-secondary-button"
                    onClick={() =>
                        navigate('/workspaces')
                    }
                >
                    Back to workspaces
                </button>
            </header>

            <form
                className="nexus-update-workspace"
                onSubmit={handleSubmit}
            >
                <div className="nexus-form-field">
                    <label htmlFor="project-name">
                        Project name
                    </label>

                    <input
                        id="project-name"
                        value={name}
                        disabled={
                            updateProjectMutation.isPending
                        }
                        onChange={(event) =>
                            setName(event.target.value)
                        }
                    />
                </div>

                <button
                    type="submit"
                    className="nexus-primary-button"
                    disabled={
                        !name.trim() ||
                        updateProjectMutation.isPending
                    }
                >
                    {updateProjectMutation.isPending
                        ? 'Saving...'
                        : 'Save project'}
                </button>

                {updateProjectMutation.isSuccess && (
                    <p className="nexus-form-success">
                        Project updated successfully.
                    </p>
                )}

                {updateProjectMutation.isError && (
                    <p className="nexus-form-error">
                        Unable to update project —{' '}
                        {formatApiError(
                            updateProjectMutation.error,
                        )}
                    </p>
                )}
            </form>

            <section className="nexus-project-conversations">
                <h2>Conversations</h2>

                <CreateConversationForm
                    projectId={project.projectId}
                    workspaceId={project.workspaceId}
                />

                <ConversationList
                    projectId={project.projectId}
                />
            </section>
        </div>
    )
}