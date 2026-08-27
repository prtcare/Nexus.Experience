import {
    QueryClient,
    QueryClientProvider,
} from '@tanstack/react-query'
import { renderHook, waitFor } from '@testing-library/react'
import type { ReactNode } from 'react'
import {
    beforeEach,
    describe,
    expect,
    it,
    vi,
} from 'vitest'

import { convertConversationApi } from './convertConversationApi'
import { useConvertConversationToTask } from './useConvertConversationToTask'
import type {
    ConvertConversationToTaskRequest,
    ConvertConversationToTaskResponse,
} from './Feature'

vi.mock('./convertConversationApi', () => ({
    convertConversationApi: {
        convertConversationToTask: vi.fn(),
    },
}))

const mockConvert = vi.mocked(
    convertConversationApi.convertConversationToTask,
)

const request: ConvertConversationToTaskRequest = {
    featureId: '00000000-0000-0000-0000-000000000000',
    title: 'Implement login flow',
    description: 'Covers the login regression',
    createdByUserId: '00000000-0000-0000-0000-000000000000',
}

const response: ConvertConversationToTaskResponse = {
    taskId: '7f12ab00-1111-2222-3333-444455556666',
    taskReference: 'TASK-1',
    title: 'Implement login flow',
    objectChatLinkId: '8a12ab00-1111-2222-3333-444455556666',
    conversationId: '9a12ab00-1111-2222-3333-444455556666',
}

function createWrapper() {
    const queryClient = new QueryClient({
        defaultOptions: {
            mutations: { retry: false },
        },
    })

    return function Wrapper({
        children,
    }: {
        children: ReactNode
    }) {
        return (
            <QueryClientProvider client={queryClient}>
                {children}
            </QueryClientProvider>
        )
    }
}

describe('useConvertConversationToTask', () => {
    beforeEach(() => {
        mockConvert.mockReset()
    })

    it('converts a conversation and returns the created task', async () => {
        mockConvert.mockResolvedValue(response)

        const { result } = renderHook(
            () => useConvertConversationToTask(),
            { wrapper: createWrapper() },
        )

        const outcome = await result.current.mutateAsync({
            conversationId: response.conversationId,
            request,
        })

        expect(mockConvert).toHaveBeenCalledWith(
            response.conversationId,
            request,
        )

        expect(outcome).toEqual(response)

        await waitFor(() => {
            expect(result.current.isSuccess).toBe(true)
        })
    })

    it('surfaces the error when conversion fails', async () => {
        const failure = new Error('task creation failed')

        mockConvert.mockRejectedValue(failure)

        const { result } = renderHook(
            () => useConvertConversationToTask(),
            { wrapper: createWrapper() },
        )

        await expect(
            result.current.mutateAsync({
                conversationId: response.conversationId,
                request,
            }),
        ).rejects.toThrow('task creation failed')

        expect(mockConvert).toHaveBeenCalledWith(
            response.conversationId,
            request,
        )

        await waitFor(() => {
            expect(result.current.isError).toBe(true)
        })
    })
})
