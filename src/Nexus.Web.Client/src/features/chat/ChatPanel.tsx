import { useState, type FormEvent, type KeyboardEvent } from 'react'

import type { ChatCitation, ChatUsage } from './chat.types'
import { MessageThread } from './MessageThread'
import { useConversationMessages } from './useConversationMessages'
import { useSendChat } from './useSendChat'

interface ChatPanelProps {
    conversationId: string
}

interface FailedTurn {
    prompt: string
    message: string
}

interface LastReply {
    citations: ChatCitation[]
    usage: ChatUsage
}

export function ChatPanel({ conversationId }: ChatPanelProps) {
    const messagesQuery = useConversationMessages(conversationId)
    const sendChat = useSendChat(conversationId)

    const [prompt, setPrompt] = useState('')
    const [pendingPrompt, setPendingPrompt] =
        useState<string | null>(null)
    const [failedTurn, setFailedTurn] =
        useState<FailedTurn | null>(null)
    const [lastReply, setLastReply] =
        useState<LastReply | null>(null)

    function handleSubmit(
        event: FormEvent<HTMLFormElement>,
    ) {
        event.preventDefault()

        const trimmed = prompt.trim()

        if (!trimmed || sendChat.isPending) {
            return
        }

        setPrompt('')
        setPendingPrompt(trimmed)
        setFailedTurn(null)

        sendChat.mutate(trimmed, {
            onSuccess: (response) => {
                setPendingPrompt(null)
                setLastReply({
                    citations: response.citations,
                    usage: response.usage,
                })
            },
            onError: (error) => {
                setPendingPrompt(null)
                setLastReply(null)
                setFailedTurn({
                    prompt: trimmed,
                    message:
                        error instanceof Error
                            ? error.message
                            : 'The turn failed for an unknown reason.',
                })
            },
        })
    }

    function handleKeyDown(
        event: KeyboardEvent<HTMLTextAreaElement>,
    ) {
        if (event.key === 'Enter' && !event.shiftKey) {
            event.preventDefault()
            event.currentTarget.form?.requestSubmit()
        }
    }

    if (messagesQuery.isPending) {
        return (
            <div className="nexus-chat-panel nexus-empty-state">
                Loading conversation...
            </div>
        )
    }

    if (messagesQuery.isError) {
        return (
            <div className="nexus-chat-panel nexus-empty-state">
                Unable to load this conversation.
            </div>
        )
    }

    return (
        <div className="nexus-chat-panel">
            <MessageThread
                messages={messagesQuery.data ?? []}
                pendingPrompt={pendingPrompt}
                failedTurn={failedTurn}
                lastReply={lastReply}
            />

            <form
                className="nexus-chat-composer"
                onSubmit={handleSubmit}
            >
                <textarea
                    value={prompt}
                    placeholder="Message Nexus..."
                    disabled={sendChat.isPending}
                    onChange={(event) =>
                        setPrompt(event.target.value)
                    }
                    onKeyDown={handleKeyDown}
                />

                <button
                    type="submit"
                    className="nexus-primary-button"
                    disabled={
                        !prompt.trim() || sendChat.isPending
                    }
                >
                    {sendChat.isPending
                        ? 'Sending...'
                        : 'Send'}
                </button>
            </form>
        </div>
    )
}
