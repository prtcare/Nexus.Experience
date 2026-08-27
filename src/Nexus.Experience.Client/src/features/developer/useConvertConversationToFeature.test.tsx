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
import { useConvertConversationToFeature } from './useConvertConversationToFeature'
import type {
    ConvertConversationToFeatureRequest,
    ConvertConversationToFeatureResponse,
} from './Feature'

vi.mock('./convertConversationApi', () => ({
    convertConversationApi: {
        convertConversationToFeature: vi.fn(),
    },
}))

const mockConvert = vi.mocked(
    convertConversationApi.convertConversationToFeature,
)

const request: ConvertConversationToFeatureRequest = {
    subprojectId:
        '00000000-0000-0000-0000-000000000000',
    title: 'Fix login flow',
    description: 'Addresses the login regression',
    createdByUserId:
        '00000000-0000-0000-0000-000000000000',
}

const response: ConvertConversationToFeatureResponse = {
    featureId: '7f12ab00-1111-2222-3333-444455556666',
    featureReference: 'FEAT-1',
    title: 'Fix login flow',
    objectChatLinkId:
        '8a12ab00-1111-2222-3333-444455556666',
    conversationId:
        '9a12ab00-1111-2222-3333-444455556666',
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

describe('useConvertConversationToFeature', () => {
    beforeEach(() => {
        mockConvert.mockReset()
    })

    it('converts a conversation and returns the created feature', async () => {
        mockConvert.mockResolvedValue(response)

        const { result } = renderHook(
            () => useConvertConversationToFeature(),
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
        const failure = new Error('feature creation failed')

        mockConvert.mockRejectedValue(failure)

        const { result } = renderHook(
            () => useConvertConversationToFeature(),
            { wrapper: createWrapper() },
        )

        await expect(
            result.current.mutateAsync({
                conversationId: response.conversationId,
                request,
            }),
        ).rejects.toThrow('feature creation failed')

        expect(mockConvert).toHaveBeenCalledWith(
            response.conversationId,
            request,
        )

        await waitFor(() => {
            expect(result.current.isError).toBe(true)
        })
    })
})
