import {
    useState,
    type FormEvent,
} from 'react'

import { useNavigate } from 'react-router-dom'

import { formatApiError } from '../../api/ApiError'
import { useConvertConversationToTask } from './useConvertConversationToTask'

interface ConvertToTaskFormProps {
    conversationId: string
    conversationTitle: string
    onCancel: () => void
}

// No authentication/identity slice exists in this frontend yet, so the
// convert flow reports the acting user as the all-zeros GUID. The Developer
// API stores it without validation; replace once real users exist.
const UNKNOWN_USER_ID =
    '00000000-0000-0000-0000-000000000000'

export function ConvertToTaskForm({
    conversationId,
    conversationTitle,
    onCancel,
}: ConvertToTaskFormProps) {
    const navigate = useNavigate()

    const convertMutation =
        useConvertConversationToTask()

    const [featureId, setFeatureId] = useState('')
    const [title, setTitle] = useState('')
    const [description, setDescription] = useState('')

    async function handleSubmit(
        event: FormEvent<HTMLFormElement>,
    ) {
        event.preventDefault()

        const trimmedFeatureId = featureId.trim()
        const trimmedTitle = title.trim()

        if (!trimmedFeatureId || !trimmedTitle) {
            return
        }

        try {
            const result =
                await convertMutation.mutateAsync({
                    conversationId,
                    request: {
                        featureId: trimmedFeatureId,
                        title: trimmedTitle,
                        description: description.trim(),
                        createdByUserId: UNKNOWN_USER_ID,
                    },
                })

            navigate(`/developer/tasks/${result.taskId}`)
        }
        catch {
            // Error is displayed below.
        }
    }

    return (
        <form
            className="nexus-create-project"
            onSubmit={handleSubmit}
        >
            <div>
                <strong>
                    Convert “{conversationTitle}” to a
                    Task
                </strong>

                <p>
                    Creates a Task in Nexus.Developer and
                    links it back to this conversation.
                </p>
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-task-feature-id">
                    Feature ID
                </label>

                <input
                    id="convert-task-feature-id"
                    type="text"
                    value={featureId}
                    placeholder="00000000-0000-0000-0000-000000000000"
                    disabled={convertMutation.isPending}
                    onChange={(event) =>
                        setFeatureId(event.target.value)
                    }
                />

                <span className="nexus-muted">
                    Temporary manual entry — Feature
                    selection does not exist in the UI
                    yet, so paste a feature ID for now.
                    This is expected, not a bug.
                </span>
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-task-title">
                    Title
                </label>

                <input
                    id="convert-task-title"
                    type="text"
                    value={title}
                    placeholder="Task title"
                    disabled={convertMutation.isPending}
                    onChange={(event) =>
                        setTitle(event.target.value)
                    }
                />
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-task-description">
                    Description
                </label>

                <textarea
                    id="convert-task-description"
                    value={description}
                    placeholder="Optional description"
                    disabled={convertMutation.isPending}
                    onChange={(event) =>
                        setDescription(event.target.value)
                    }
                />
            </div>

            <div className="nexus-create-project-controls">
                <button
                    type="submit"
                    className="nexus-primary-button"
                    disabled={
                        !featureId.trim() ||
                        !title.trim() ||
                        convertMutation.isPending
                    }
                >
                    {convertMutation.isPending
                        ? 'Converting...'
                        : 'Convert to Task'}
                </button>

                <button
                    type="button"
                    className="nexus-secondary-button"
                    disabled={convertMutation.isPending}
                    onClick={onCancel}
                >
                    Cancel
                </button>
            </div>

            {convertMutation.isError && (
                <p className="nexus-form-error">
                    Unable to convert conversation —{' '}
                    {formatApiError(
                        convertMutation.error,
                    )}
                </p>
            )}
        </form>
    )
}
