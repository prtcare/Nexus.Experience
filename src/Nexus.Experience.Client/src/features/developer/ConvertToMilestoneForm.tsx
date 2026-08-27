import {
    useState,
    type FormEvent,
} from 'react'

import { useNavigate } from 'react-router-dom'

import { formatApiError } from '../../api/ApiError'
import { useConvertConversationToMilestone } from './useConvertConversationToMilestone'

import type { ConvertConversationToMilestoneRequest } from './Feature'

interface ConvertToMilestoneFormProps {
    conversationId: string
    conversationTitle: string
    onCancel: () => void
}

// No authentication/identity slice exists in this frontend yet, so the
// convert flow reports the acting user as the all-zeros GUID. The Developer
// API stores it without validation; replace once real users exist.
const UNKNOWN_USER_ID =
    '00000000-0000-0000-0000-000000000000'

export function ConvertToMilestoneForm({
    conversationId,
    conversationTitle,
    onCancel,
}: ConvertToMilestoneFormProps) {
    const navigate = useNavigate()

    const convertMutation =
        useConvertConversationToMilestone()

    const [subprojectId, setSubprojectId] = useState('')
    const [name, setName] = useState('')
    const [description, setDescription] = useState('')
    const [targetDate, setTargetDate] = useState('')

    async function handleSubmit(
        event: FormEvent<HTMLFormElement>,
    ) {
        event.preventDefault()

        const trimmedSubprojectId = subprojectId.trim()
        const trimmedName = name.trim()

        if (!trimmedSubprojectId || !trimmedName) {
            return
        }

        const request: ConvertConversationToMilestoneRequest =
            {
                subprojectId: trimmedSubprojectId,
                name: trimmedName,
                description: description.trim(),
                createdByUserId: UNKNOWN_USER_ID,
                // Blank date input -> omit the field; an empty
                // string would not parse as a DateTimeOffset.
                ...(targetDate ? { targetDate } : {}),
            }

        try {
            const result =
                await convertMutation.mutateAsync({
                    conversationId,
                    request,
                })

            navigate(
                `/developer/milestones/${result.milestoneId}`,
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
                    Milestone
                </strong>

                <p>
                    Creates a Milestone in
                    Nexus.Developer and links it back to
                    this conversation.
                </p>
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-milestone-subproject-id">
                    Subproject ID
                </label>

                <input
                    id="convert-milestone-subproject-id"
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
                    selection does not exist in the UI
                    yet, so paste a subproject ID for
                    now. This is expected, not a bug.
                </span>
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-milestone-name">
                    Name
                </label>

                <input
                    id="convert-milestone-name"
                    type="text"
                    value={name}
                    placeholder="Milestone name"
                    disabled={convertMutation.isPending}
                    onChange={(event) =>
                        setName(event.target.value)
                    }
                />
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-milestone-description">
                    Description
                </label>

                <textarea
                    id="convert-milestone-description"
                    value={description}
                    placeholder="Optional description"
                    disabled={convertMutation.isPending}
                    onChange={(event) =>
                        setDescription(event.target.value)
                    }
                />
            </div>

            <div className="nexus-form-field">
                <label htmlFor="convert-milestone-target-date">
                    Target date
                </label>

                <input
                    id="convert-milestone-target-date"
                    type="date"
                    value={targetDate}
                    disabled={convertMutation.isPending}
                    onChange={(event) =>
                        setTargetDate(event.target.value)
                    }
                />

                <span className="nexus-muted">
                    Optional — leave blank if there is no
                    target date.
                </span>
            </div>

            <div className="nexus-create-project-controls">
                <button
                    type="submit"
                    className="nexus-primary-button"
                    disabled={
                        !subprojectId.trim() ||
                        !name.trim() ||
                        convertMutation.isPending
                    }
                >
                    {convertMutation.isPending
                        ? 'Converting...'
                        : 'Convert to Milestone'}
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
