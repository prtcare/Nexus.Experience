import {
    useState,
    type FormEvent,
} from 'react'

import { useNavigate } from 'react-router-dom'

import { formatApiError } from '../../api/ApiError'
import { useConvertConversationToFeature } from './useConvertConversationToFeature'

interface ConvertConversationFormProps {
    conversationId: string
    conversationTitle: string
    onCancel: () => void
}

// No authentication/identity slice exists in this frontend yet, so the
// convert flow reports the acting user as the all-zeros GUID. The Developer
// API stores it without validation; replace once real users exist.
const UNKNOWN_USER_ID =
    '00000000-0000-0000-0000-000000000000'

export function ConvertConversationForm({
    conversationId,
    conversationTitle,
    onCancel,
}: ConvertConversationFormProps) {
    const navigate = useNavigate()

    const convertMutation =
        useConvertConversationToFeature()

    const [subprojectId, setSubprojectId] = useState('')
    const [title, setTitle] = useState('')
    const [description, setDescription] = useState('')

    async function handleSubmit(
        event: FormEvent<HTMLFormElement>,
    ) {
        event.preventDefault()

        const trimmedSubprojectId = subprojectId.trim()
        const trimmedTitle = title.trim()

        if (!trimmedSubprojectId || !trimmedTitle) {
            return
        }

        try {
            const result =
                await convertMutation.mutateAsync({
                    conversationId,
                    request: {
                        subprojectId: trimmedSubprojectId,
                        title: trimmedTitle,
                        description: description.trim(),
                        createdByUserId: UNKNOWN_USER_ID,
                    },
                })

            navigate(
                `/developer/features/${result.featureId}`,
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
                    Feature
                </strong>

                <p>
                    Creates a Feature in Nexus.Developer and
                    links it back to this conversation.
                </p>
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-subproject-id">
                    Subproject ID
                </label>

                <input
                    id="convert-subproject-id"
                    type="text"
                    value={subprojectId}
                    placeholder="00000000-0000-0000-0000-000000000000"
                    disabled={convertMutation.isPending}
                    onChange={(event) =>
                        setSubprojectId(event.target.value)
                    }
                />

                <span className="nexus-muted">
                    Temporary manual entry — Subproject
                    selection does not exist in the UI yet,
                    so paste a subproject ID for now. This
                    is expected, not a bug.
                </span>
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-title">
                    Title
                </label>

                <input
                    id="convert-title"
                    type="text"
                    value={title}
                    placeholder="Feature title"
                    disabled={convertMutation.isPending}
                    onChange={(event) =>
                        setTitle(event.target.value)
                    }
                />
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-description">
                    Description
                </label>

                <textarea
                    id="convert-description"
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
                        !subprojectId.trim() ||
                        !title.trim() ||
                        convertMutation.isPending
                    }
                >
                    {convertMutation.isPending
                        ? 'Converting...'
                        : 'Convert to Feature'}
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
