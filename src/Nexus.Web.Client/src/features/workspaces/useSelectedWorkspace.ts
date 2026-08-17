import { useEffect, useState } from 'react'
import { useWorkspaces } from './useWorkspaces'

const STORAGE_KEY = 'nexus.selectedWorkspaceId'

export function useSelectedWorkspace() {
    const { data, isPending, isError } = useWorkspaces()

    const workspaces = data?.workspaces ?? []

    const [selectedWorkspaceId, setSelectedWorkspaceId] =
        useState<string | undefined>(() => {
            return localStorage.getItem(STORAGE_KEY) ?? undefined
        })

    useEffect(() => {
        if (workspaces.length === 0) {
            setSelectedWorkspaceId(undefined)
            localStorage.removeItem(STORAGE_KEY)
            return
        }

        const selectedStillExists = workspaces.some(
            workspace =>
                workspace.workspaceId === selectedWorkspaceId,
        )

        if (!selectedStillExists) {
            const firstWorkspaceId = workspaces[0].workspaceId

            setSelectedWorkspaceId(firstWorkspaceId)
            localStorage.setItem(
                STORAGE_KEY,
                firstWorkspaceId,
            )
        }
    }, [workspaces, selectedWorkspaceId])

    function selectWorkspace(workspaceId: string) {
        setSelectedWorkspaceId(workspaceId)

        localStorage.setItem(
            STORAGE_KEY,
            workspaceId,
        )
    }

    const selectedWorkspace = workspaces.find(
        workspace =>
            workspace.workspaceId === selectedWorkspaceId,
    )

    return {
        workspaces,
        selectedWorkspace,
        selectedWorkspaceId,
        selectWorkspace,
        isPending,
        isError,
    }
}