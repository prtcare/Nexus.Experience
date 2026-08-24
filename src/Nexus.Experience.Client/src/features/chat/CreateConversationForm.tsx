import { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'

import { formatApiError } from '../../api/ApiError'
import { useCreateConversation } from './useCreateConversation'

interface CreateConversationFormProps {
    projectId: string
    workspaceId: string
}

// ConversationType.Standalone (0) and ConversationVisibility.Private (0) -
// the most conservative defaults the contract offers. There is no picker
// in the UI for either yet; this form only exists to close the "no way to
// start a conversation" dead end, not to expose the full create contract.
const DEFAULT_CONVERSATION_TYPE = 0
const DEFAULT_CONVERSATION_VISIBILITY = 0

export function CreateConversationForm({
    projectId,
    workspaceId,
}: CreateConversationFormProps) {
    const navigate = useNavigate()

    const createConversationMutation =
        useCreateConversation()

    const [title, setTitle] = useState('')

    async function handleSubmit(
        event: FormEvent<HTMLFormElement>,
    ) {
        event.preventDefault()

        const trimmedTitle = title.trim()

        if (!trimmedTitle) {
            return
        }

        try {
            const response =
                await createConversationMutation.mutateAsync({
                    projectId,
                    workspaceId,
                    title: trimmedTitle,
                    description: '',
                    type: DEFAULT_CONVERSATION_TYPE,
                    visibility:
                        DEFAULT_CONVERSATION_VISIBILITY,
                })

            navigate(
                `/projects/${projectId}/conversations/${response.conversationId}`,
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
                <strong>New conversation</strong>

                <p>
                    Start a new conversation in this
                    project.
                </p>
            </div>

            <div className="nexus-create-project-controls">
                <input
                    type="text"
                    value={title}
                    placeholder="Conversation title"
                    disabled={
                        createConversationMutation.isPending
                    }
                    onChange={(event) =>
                        setTitle(event.target.value)
                    }
                />

                <button
                    type="submit"
                    className="nexus-primary-button"
                    disabled={
                        !title.trim() ||
                        createConversationMutation.isPending
                    }
                >
                    {createConversationMutation.isPending
                        ? 'Starting...'
                        : 'Start conversation'}
                </button>
            </div>

            {createConversationMutation.isError && (
                <p className="nexus-form-error">
                    Unable to start conversation —{' '}
                    {formatApiError(
                        createConversationMutation.error,
                    )}
                </p>
            )}
        </form>
    )
}
