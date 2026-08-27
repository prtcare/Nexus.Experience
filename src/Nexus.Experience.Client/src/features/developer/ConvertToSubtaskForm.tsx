import {
    useState,
    type FormEvent,
} from 'react'

import { useNavigate } from 'react-router-dom'

import { formatApiError } from '../../api/ApiError'
import { useConvertConversationToSubtask } from './useConvertConversationToSubtask'

interface ConvertToSubtaskFormProps {
    conversationId: string
    conversationTitle: string
    onCancel: () => void
}

// No authentication/identity slice exists in this frontend yet, so the
// convert flow reports the acting user as the all-zeros GUID. The Developer
// API stores it without validation; replace once real users exist.
const UNKNOWN_USER_ID =
    '00000000-0000-0000-0000-000000000000'

export function ConvertToSubtaskForm({
    conversationId,
    conversationTitle,
    onCancel,
}: ConvertToSubtaskFormProps) {
    const navigate = useNavigate()

    const convertMutation =
        useConvertConversationToSubtask()

    const [taskId, setTaskId] = useState('')
    const [title, setTitle] = useState('')
    const [description, setDescription] = useState('')

    async function handleSubmit(
        event: FormEvent<HTMLFormElement>,
    ) {
        event.preventDefault()

        const trimmedTaskId = taskId.trim()
        const trimmedTitle = title.trim()

        if (!trimmedTaskId || !trimmedTitle) {
            return
        }

        try {
            const result =
                await convertMutation.mutateAsync({
                    conversationId,
                    request: {
                        taskId: trimmedTaskId,
                        title: trimmedTitle,
                        description: description.trim(),
                        createdByUserId: UNKNOWN_USER_ID,
                    },
                })

            navigate(
                `/developer/subtasks/${result.subtaskId}`,
            )
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
                    Subtask
                </strong>

                <p>
                    Creates a Subtask in Nexus.Developer
                    and links it back to this
                    conversation.
                </p>
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-subtask-task-id">
                    Task ID
                </label>

                <input
                    id="convert-subtask-task-id"
                    type="text"
                    value={taskId}
                    placeholder="00000000-0000-0000-0000-000000000000"
                    disabled={convertMutation.isPending}
                    onChange={(event) =>
                        setTaskId(event.target.value)
                    }
                />

                <span className="nexus-muted">
                    Temporary manual entry — Task
                    selection does not exist in the UI
                    yet, so paste a task ID for now.
                    This is expected, not a bug.
                </span>
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-subtask-title">
                    Title
                </label>

                <input
                    id="convert-subtask-title"
                    type="text"
                    value={title}
                    placeholder="Subtask title"
                    disabled={convertMutation.isPending}
                    onChange={(event) =>
                        setTitle(event.target.value)
                    }
                />
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-subtask-description">
                    Description
                </label>

                <textarea
                    id="convert-subtask-description"
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
                        !taskId.trim() ||
                        !title.trim() ||
                        convertMutation.isPending
                    }
                >
                    {convertMutation.isPending
                        ? 'Converting...'
                        : 'Convert to Subtask'}
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
