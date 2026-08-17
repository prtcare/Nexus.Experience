import {
    createContext,
    useContext,
    useEffect,
    useMemo,
    useState,
    type ReactNode,
} from 'react'

import { useWorkspaces } from './useWorkspaces'
import type { Workspace } from './Workspace'

const STORAGE_KEY = 'nexus.selectedWorkspaceId'

type WorkspaceContextValue = {
    workspaces: Workspace[]
    selectedWorkspace?: Workspace
    selectedWorkspaceId?: string
    selectWorkspace: (workspaceId: string) => void
    isPending: boolean
    isError: boolean
}

const WorkspaceContext =
    createContext<WorkspaceContextValue | undefined>(undefined)

export function WorkspaceProvider({
    children,
}: {
    children: ReactNode
}) {
    const {
        data,
        isPending,
        isError,
    } = useWorkspaces()

    const workspaces = data?.workspaces ?? []

    const [selectedWorkspaceId, setSelectedWorkspaceId] =
        useState<string | undefined>(() =>
            localStorage.getItem(STORAGE_KEY) ?? undefined,
        )

    useEffect(() => {
        if (workspaces.length === 0) {
            return
        }

        const exists = workspaces.some(
            workspace =>
                workspace.workspaceId === selectedWorkspaceId,
        )

        if (!exists) {
            const firstId = workspaces[0].workspaceId

            setSelectedWorkspaceId(firstId)
            localStorage.setItem(STORAGE_KEY, firstId)
        }
    }, [workspaces, selectedWorkspaceId])

    function selectWorkspace(workspaceId: string) {
        setSelectedWorkspaceId(workspaceId)
        localStorage.setItem(STORAGE_KEY, workspaceId)
    }

    const selectedWorkspace = workspaces.find(
        workspace =>
            workspace.workspaceId === selectedWorkspaceId,
    )

    const value = useMemo(
        () => ({
            workspaces,
            selectedWorkspace,
            selectedWorkspaceId,
            selectWorkspace,
            isPending,
            isError,
        }),
        [
            workspaces,
            selectedWorkspace,
            selectedWorkspaceId,
            isPending,
            isError,
        ],
    )

    return (
        <WorkspaceContext.Provider value={value}>
            {children}
        </WorkspaceContext.Provider>
    )
}

export function useSelectedWorkspace() {
    const context = useContext(WorkspaceContext)

    if (!context) {
        throw new Error(
            'useSelectedWorkspace must be used inside WorkspaceProvider',
        )
    }

    return context
}