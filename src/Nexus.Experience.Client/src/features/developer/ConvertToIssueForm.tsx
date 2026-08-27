import {
    useState,
    type FormEvent,
} from 'react'

import { useNavigate } from 'react-router-dom'

import { formatApiError } from '../../api/ApiError'
import { useConvertConversationToIssue } from './useConvertConversationToIssue'

interface ConvertToIssueFormProps {
    conversationId: string
    conversationTitle: string
    onCancel: () => void
}

// No authentication/identity slice exists in this frontend yet, so the
// convert flow reports the acting user as the all-zeros GUID. The Developer
// API stores it without validation; replace once real users exist.
const UNKNOWN_USER_ID =
    '00000000-0000-0000-0000-000000000000'

// Issue is universally attachable (no parent id of its own), so this form
// carries no parent field - matching ConvertConversationToIssueCommand.
export function ConvertToIssueForm({
    conversationId,
    conversationTitle,
    onCancel,
}: ConvertToIssueFormProps) {
    const navigate = useNavigate()

    const convertMutation =
        useConvertConversationToIssue()

    const [title, setTitle] = useState('')
    const [description, setDescription] = useState('')

    async function handleSubmit(
        event: FormEvent<HTMLFormElement>,
    ) {
        event.preventDefault()

        const trimmedTitle = title.trim()

        if (!trimmedTitle) {
            return
        }

        try {
            const result =
                await convertMutation.mutateAsync({
                    conversationId,
                    request: {
                        title: trimmedTitle,
                        description: description.trim(),
                        createdByUserId: UNKNOWN_USER_ID,
                    },
                })

            navigate(`/developer/issues/${result.issueId}`)
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
                    Convert “{conversationTitle}” to an
                    Issue
                </strong>

                <p>
                    Creates an Issue in Nexus.Developer
                    and links it back to this
                    conversation.
                </p>
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-issue-title">
                    Title
                </label>

                <input
                    id="convert-issue-title"
                    type="text"
                    value={title}
                    placeholder="Issue title"
                    disabled={convertMutation.isPending}
                    onChange={(event) =>
                        setTitle(event.target.value)
                    }
                />
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-issue-description">
                    Description
                </label>

                <textarea
                    id="convert-issue-description"
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
                        !title.trim() ||
                        convertMutation.isPending
                    }
                >
                    {convertMutation.isPending
                        ? 'Converting...'
                        : 'Convert to Issue'}
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
