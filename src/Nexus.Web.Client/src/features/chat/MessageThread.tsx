import { useEffect, useRef } from 'react'

import type {
    ChatCitation,
    ChatUsage,
    ConversationMessageDto,
} from './chat.types'

interface FailedTurn {
    prompt: string
    message: string
}

interface LastReply {
    citations: ChatCitation[]
    usage: ChatUsage
}

interface MessageThreadProps {
    messages: ConversationMessageDto[]
    pendingPrompt: string | null
    failedTurn: FailedTurn | null
    lastReply: LastReply | null
}

const NEAR_BOTTOM_THRESHOLD_PX = 80

export function MessageThread({
    messages,
    pendingPrompt,
    failedTurn,
    lastReply,
}: MessageThreadProps) {
    const containerRef = useRef<HTMLDivElement>(null)
    const isNearBottomRef = useRef(true)

    // The API gives no ordering guarantee on the messages list - sort
    // defensively so the thread always reads top-to-bottom by time.
    const sortedMessages = [...messages].sort(
        (a, b) =>
            new Date(a.createdOn).getTime() -
            new Date(b.createdOn).getTime(),
    )

    const lastMessage = sortedMessages[sortedMessages.length - 1]

    function handleScroll() {
        const el = containerRef.current

        if (!el) {
            return
        }

        const distanceFromBottom =
            el.scrollHeight - el.scrollTop - el.clientHeight

        isNearBottomRef.current =
            distanceFromBottom < NEAR_BOTTOM_THRESHOLD_PX
    }

    useEffect(() => {
        const el = containerRef.current

        if (el && isNearBottomRef.current) {
            el.scrollTop = el.scrollHeight
        }
    }, [sortedMessages.length, pendingPrompt, failedTurn])

    const isEmpty =
        sortedMessages.length === 0 &&
        !pendingPrompt &&
        !failedTurn

    if (isEmpty) {
        return (
            <div className="nexus-chat-thread nexus-empty-state">
                <strong>No messages yet</strong>

                <p>
                    Send a prompt to start this conversation.
                </p>
            </div>
        )
    }

    return (
        <div
            ref={containerRef}
            className="nexus-chat-thread"
            onScroll={handleScroll}
        >
            {sortedMessages.map((message) => (
                <div
                    key={message.messageId.value}
                    className={`nexus-chat-message nexus-chat-message-${message.role.toLowerCase()}`}
                >
                    <span className="nexus-chat-message-role">
                        {message.role}
                    </span>

                    <p>{message.content}</p>
                </div>
            ))}

            {lastReply && lastMessage?.role === 'Assistant' && (
                <div className="nexus-chat-message-meta">
                    {lastReply.citations.length > 0 && (
                        <span>
                            {lastReply.citations.length}{' '}
                            citation
                            {lastReply.citations.length === 1
                                ? ''
                                : 's'}
                        </span>
                    )}

                    <span>
                        {lastReply.usage.tokensIn +
                            lastReply.usage.tokensOut}{' '}
                        tokens
                        {lastReply.usage.modelUsed
                            ? ` · ${lastReply.usage.modelUsed}`
                            : ''}
                    </span>
                </div>
            )}

            {pendingPrompt && (
                <>
                    <div className="nexus-chat-message nexus-chat-message-user">
                        <span className="nexus-chat-message-role">
                            User
                        </span>

                        <p>{pendingPrompt}</p>
                    </div>

                    <div className="nexus-chat-message nexus-chat-message-assistant nexus-chat-message-pending">
                        <span className="nexus-chat-message-role">
                            Assistant
                        </span>

                        <p>Sending...</p>
                    </div>
                </>
            )}

            {failedTurn && (
                <div className="nexus-chat-message nexus-chat-message-error">
                    <span className="nexus-chat-message-role">
                        Error
                    </span>

                    <p>{failedTurn.message}</p>
                </div>
            )}
        </div>
    )
}
