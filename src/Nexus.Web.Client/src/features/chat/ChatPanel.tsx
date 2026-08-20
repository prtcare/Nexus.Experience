import { useState, type FormEvent, type KeyboardEvent } from 'react'

import {
    useChatTelemetry,
    type ChatTurnRecord,
} from './ChatTelemetryContext'
import { CitationsPanel } from './CitationsPanel'
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

const ZERO_USAGE = {
    tokensIn: 0,
    tokensOut: 0,
    estimatedCost: 0,
    modelUsed: '',
}

function createTurnId() {
    return typeof crypto !== 'undefined' &&
        typeof crypto.randomUUID === 'function'
        ? crypto.randomUUID()
        : `${Date.now()}-${Math.random()}`
}

export function ChatPanel({ conversationId }: ChatPanelProps) {
    const messagesQuery = useConversationMessages(conversationId)
    const sendChat = useSendChat(conversationId)
    const { turns, recordTurn } = useChatTelemetry()

    const [prompt, setPrompt] = useState('')
    const [pendingPrompt, setPendingPrompt] =
        useState<string | null>(null)
    const [failedTurn, setFailedTurn] =
        useState<FailedTurn | null>(null)
    const [selectedTurnId, setSelectedTurnId] =
        useState<string | null>(null)

    const conversationTurns = turns.filter(
        (turn) => turn.conversationId === conversationId,
    )

    const selectedTurn: ChatTurnRecord | null =
        conversationTurns.find(
            (turn) => turn.id === selectedTurnId,
        ) ??
        conversationTurns[conversationTurns.length - 1] ??
        null

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

        const turnId = createTurnId()
        const sentAt = new Date().toISOString()

        sendChat.mutate(trimmed, {
            onSuccess: (response) => {
                setPendingPrompt(null)

                recordTurn({
                    id: turnId,
                    conversationId,
                    prompt: trimmed,
                    sentAt,
                    reply: response.reply,
                    citations: response.citations,
                    usage: response.usage,
                    requiresClarification:
                        response.requiresClarification,
                    success: true,
                    error: null,
                })

                setSelectedTurnId(turnId)
            },
            onError: (error) => {
                setPendingPrompt(null)

                const message =
                    error instanceof Error
                        ? error.message
                        : 'The turn failed for an unknown reason.'

                setFailedTurn({ prompt: trimmed, message })

                recordTurn({
                    id: turnId,
                    conversationId,
                    prompt: trimmed,
                    sentAt,
                    reply: null,
                    citations: [],
                    usage: ZERO_USAGE,
                    requiresClarification: false,
                    success: false,
                    error: message,
                })

                setSelectedTurnId(turnId)
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
        <div className="nexus-chat-workspace">
            <div className="nexus-chat-panel">
                <MessageThread
                    messages={messagesQuery.data ?? []}
                    pendingPrompt={pendingPrompt}
                    failedTurn={failedTurn}
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
                            !prompt.trim() ||
                            sendChat.isPending
                        }
                    >
                        {sendChat.isPending
                            ? 'Sending...'
                            : 'Send'}
                    </button>
                </form>
            </div>

            <aside className="nexus-chat-citations-sidebar">
                <span className="nexus-page-eyebrow">
                    Turns this session
                </span>

                {conversationTurns.length === 0 ? (
                    <p className="nexus-citations-empty">
                        Send a prompt to see the context
                        behind the reply. Citations and usage
                        are not persisted by the API, so only
                        turns sent this session are available
                        here.
                    </p>
                ) : (
                    <>
                        <div className="nexus-chat-turn-selector">
                            {conversationTurns.map(
                                (turn, index) => (
                                    <button
                                        key={turn.id}
                                        type="button"
                                        className={`nexus-chat-turn-chip${
                                            turn.id ===
                                            selectedTurn?.id
                                                ? ' active'
                                                : ''
                                        }${
                                            turn.success
                                                ? ''
                                                : ' failed'
                                        }`}
                                        onClick={() =>
                                            setSelectedTurnId(
                                                turn.id,
                                            )
                                        }
                                    >
                                        #{index + 1}
                                        {!turn.success &&
                                            ' ⚠'}
                                    </button>
                                ),
                            )}
                        </div>

                        {selectedTurn && (
                            <>
                                <div className="nexus-chat-turn-usage">
                                    <span>
                                        {selectedTurn.usage
                                            .modelUsed ||
                                            'No model recorded'}
                                    </span>

                                    <span>
                                        {selectedTurn.usage
                                            .tokensIn +
                                            selectedTurn.usage
                                                .tokensOut}{' '}
                                        tokens
                                    </span>

                                    <span>
                                        $
                                        {selectedTurn.usage.estimatedCost.toFixed(
                                            4,
                                        )}
                                    </span>
                                </div>

                                <CitationsPanel
                                    citations={
                                        selectedTurn.citations
                                    }
                                />
                            </>
                        )}
                    </>
                )}
            </aside>
        </div>
    )
}
