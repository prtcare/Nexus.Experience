import { NavLink } from 'react-router-dom'

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
    } = useConversations(projectId)

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
                Unable to load conversations.
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

                return (
                    <NavLink
                        key={conversationId}
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
                )
            })}
        </nav>
    )
}
