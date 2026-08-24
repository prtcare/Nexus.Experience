import {
    createContext,
    useCallback,
    useContext,
    useState,
    type PropsWithChildren,
} from 'react'

import type { ChatCitation, ChatUsage } from './chat.types'

// Neither citations nor usage are persisted anywhere the API will hand
// back later - GET /conversations/{id}/messages returns only
// messageId/role/content/createdOn (see chat.types.ts), and there is no
// history endpoint for usage at all. The only citations and usage that
// will ever exist for a turn are the ones in that turn's own POST /chat
// response, the moment it arrives. This context is that memory, scoped to
// the current browser session - it is not a substitute for a real API,
// it is what makes turn-to-turn comparison possible without one.
export interface ChatTurnRecord {
    id: string
    conversationId: string
    prompt: string
    sentAt: string
    reply: string | null
    citations: ChatCitation[]
    usage: ChatUsage
    requiresClarification: boolean
    success: boolean
    error: string | null
}

interface ChatTelemetryContextValue {
    turns: ChatTurnRecord[]
    recordTurn: (turn: ChatTurnRecord) => void
}

const MAX_TRACKED_TURNS = 100

const ChatTelemetryContext =
    createContext<ChatTelemetryContextValue | null>(null)

export function ChatTelemetryProvider({
    children,
}: PropsWithChildren) {
    const [turns, setTurns] = useState<ChatTurnRecord[]>([])

    const recordTurn = useCallback((turn: ChatTurnRecord) => {
        setTurns((current) =>
            [...current, turn].slice(-MAX_TRACKED_TURNS),
        )
    }, [])

    return (
        <ChatTelemetryContext.Provider
            value={{ turns, recordTurn }}
        >
            {children}
        </ChatTelemetryContext.Provider>
    )
}

export function useChatTelemetry() {
    const context = useContext(ChatTelemetryContext)

    if (!context) {
        throw new Error(
            'useChatTelemetry must be used within a ChatTelemetryProvider',
        )
    }

    return context
}
