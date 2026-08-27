import { useState } from 'react'
import { NavLink } from 'react-router-dom'

import { formatApiError } from '../../api/ApiError'
import { ConvertConversationForm } from '../developer/ConvertConversationForm'
import { useConversations } from './useConversations'

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

    const [convertConversationId, setConvertConversationId] =
        useState<string | null>(null)

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

                const isConverting =
                    convertConversationId === conversationId

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

                            <button
                                type="button"
                                className="nexus-secondary-button"
                                onClick={() =>
                                    setConvertConversationId(
                                        isConverting
                                            ? null
                                            : conversationId,
                                    )
                                }
                            >
                                {isConverting
                                    ? 'Close'
                                    : 'Convert to Feature'}
                            </button>
                        </div>

                        {isConverting && (
                            <ConvertConversationForm
                                conversationId={conversationId}
                                conversationTitle={
                                    conversation.title
                                }
                                onCancel={() =>
                                    setConvertConversationId(
                                        null,
                                    )
                                }
                            />
                        )}
                    </div>
                )
            })}
        </nav>
    )
}
