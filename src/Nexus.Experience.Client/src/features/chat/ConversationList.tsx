import { useState } from 'react'
import { NavLink } from 'react-router-dom'

import { formatApiError } from '../../api/ApiError'
import { ConvertConversationForm } from '../developer/ConvertConversationForm'
import { ConvertToIssueForm } from '../developer/ConvertToIssueForm'
import { ConvertToMilestoneForm } from '../developer/ConvertToMilestoneForm'
import { ConvertToSubtaskForm } from '../developer/ConvertToSubtaskForm'
import { ConvertToTaskForm } from '../developer/ConvertToTaskForm'
import { useConversations } from './useConversations'

import type { ConvertTargetType } from '../developer/Feature'

const CONVERT_TARGET_TYPES: ConvertTargetType[] = [
    'Feature',
    'Task',
    'Subtask',
    'Milestone',
    'Issue',
]

interface ConvertSelection {
    conversationId: string
    targetType: ConvertTargetType
}

interface ConvertFormProps {
    conversationId: string
    conversationTitle: string
    onCancel: () => void
}

function renderConvertForm(
    targetType: ConvertTargetType,
    props: ConvertFormProps,
) {
    switch (targetType) {
        case 'Task':
            return <ConvertToTaskForm {...props} />
        case 'Subtask':
            return <ConvertToSubtaskForm {...props} />
        case 'Milestone':
            return <ConvertToMilestoneForm {...props} />
        case 'Issue':
            return <ConvertToIssueForm {...props} />
        case 'Feature':
        default:
            return <ConvertConversationForm {...props} />
    }
}

interface ConversationListProps {
    projectId: string
}

export function ConversationList({
    projectId,
}: ConversationListProps) {
    const {
        data: conversations,
        isPending,
        isError,
        error,
    } = useConversations(projectId)

    const [convertSelection, setConvertSelection] =
        useState<ConvertSelection | null>(null)

    if (isPending) {
        return (
            <div className="nexus-empty-state">
                Loading conversations...
            </div>
        )
    }

    if (isError) {
        return (
            <div className="nexus-empty-state">
                Unable to load conversations —{' '}
                {formatApiError(error)}
            </div>
        )
    }

    if (!conversations || conversations.length === 0) {
        return (
            <div className="nexus-empty-state">
                <strong>No conversations</strong>

                <p>
                    This project has no conversations yet.
                </p>
            </div>
        )
    }

    return (
        <nav className="nexus-chat-conversation-list">
            {conversations.map((conversation) => {
                const conversationId =
                    conversation.conversationId.value

                const selection = convertSelection
                const isConverting =
                    selection !== null &&
                    selection.conversationId ===
                        conversationId
                const selectedType = isConverting
                    ? selection.targetType
                    : ''

                return (
                    <div
                        key={conversationId}
                        className="nexus-chat-conversation-row-wrap"
                    >
                        <div className="nexus-chat-conversation-row-inner">
                            <NavLink
                                to={`/projects/${projectId}/conversations/${conversationId}`}
                                className={({ isActive }) =>
                                    `nexus-chat-conversation-row${isActive ? ' active' : ''}`
                                }
                            >
                                <strong>
                                    {conversation.title}
                                </strong>

                                <span>
                                    {new Date(
                                        conversation.createdAt,
                                    ).toLocaleString()}
                                </span>
                            </NavLink>

                            <select
                                className="nexus-convert-picker"
                                aria-label="Convert conversation to"
                                value={selectedType}
                                onChange={(event) => {
                                    const targetType =
                                        event.target
                                            .value as
                                        ConvertTargetType | ''

                                    if (targetType === '') {
                                        setConvertSelection(
                                            null,
                                        )
                                    }
                                    else {
                                        setConvertSelection({
                                            conversationId,
                                            targetType,
                                        })
                                    }
                                }}
                            >
                                <option value="">
                                    Convert to…
                                </option>

                                {CONVERT_TARGET_TYPES.map(
                                    (targetType) => (
                                        <option
                                            key={targetType}
                                            value={targetType}
                                        >
                                            {targetType}
                                        </option>
                                    ),
                                )}
                            </select>

                            {isConverting && (
                                <button
                                    type="button"
                                    className="nexus-secondary-button"
                                    onClick={() =>
                                        setConvertSelection(
                                            null,
                                        )
                                    }
                                >
                                    Close
                                </button>
                            )}
                        </div>

                        {isConverting && (
                            renderConvertForm(
                                selection.targetType,
                                {
                                    conversationId,
                                    conversationTitle:
                                        conversation.title,
                                    onCancel: () =>
                                        setConvertSelection(
                                            null,
                                        ),
                                },
                            )
                        )}
                    </div>
                )
            })}
        </nav>
    )
}
