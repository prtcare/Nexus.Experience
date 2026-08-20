import type { PropsWithChildren } from 'react'
import { QueryClientProvider } from '@tanstack/react-query'
import { queryClient } from './queryClient'
import { ChatTelemetryProvider } from '../features/chat/ChatTelemetryContext'
import { WorkspaceProvider } from '../features/workspaces/WorkspaceContext'

export function AppProviders({ children }: PropsWithChildren) {
    return (
        <QueryClientProvider client={queryClient}>
            <WorkspaceProvider>
                <ChatTelemetryProvider>
                    {children}
                </ChatTelemetryProvider>
            </WorkspaceProvider>
        </QueryClientProvider>
    )
}