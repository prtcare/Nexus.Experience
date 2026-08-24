import { useParams } from 'react-router-dom'

import { formatApiError } from '../api/ApiError'
import { ChatPanel } from '../features/chat/ChatPanel'
import { ConversationList } from '../features/chat/ConversationList'
import { useConversation } from '../features/chat/useConversation'

export function ChatPage() {
    const { projectId, conversationId } = useParams<{
        projectId: string
        conversationId: string
    }>()

    const {
        data: conversation,
        isPending,
        isError,
        error,
    } = useConversation(conversationId)

    if (!projectId || !conversationId) {
        return (
            <div className="nexus-empty-state">
                Missing project or conversation.
            </div>
        )
    }

    return (
        <div className="nexus-chat-layout">
            <aside className="nexus-chat-sidebar">
                <span className="nexus-page-eyebrow">
                    Conversations
                </span>

                <ConversationList projectId={projectId} />
            </aside>

            <div className="nexus-chat-main">
                <header className="nexus-chat-header">
                    {/*
                        Conversation carries a human-readable Reference
                        (CON-00000005 style) once SQL stage 2a lands - show
                        it here, small and muted, next to the title. It is
                        not in the contract yet, so there is nothing to
                        render.
                    */}
                    <h1>
                        {isPending
                            ? 'Loading...'
                            : isError
                                ? 'Unable to load conversation'
                                : (conversation?.title ??
                                      'Conversation')}
                    </h1>

                    {isError && (
                        <p className="nexus-chat-header-error">
                            {formatApiError(error)}
                        </p>
                    )}
                </header>

                <ChatPanel conversationId={conversationId} />
            </div>
        </div>
    )
}
