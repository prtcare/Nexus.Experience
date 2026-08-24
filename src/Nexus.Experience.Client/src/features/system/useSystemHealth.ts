import { useQuery } from '@tanstack/react-query'
import { systemApi } from './systemApi'
import type { SystemHealth } from '../../types/SystemHealth'

export function useSystemHealth() {
    return useQuery<SystemHealth>({
        queryKey: ['system-health'],
        queryFn: () => systemApi.getHealth(),
        retry: 1,
        refetchInterval: 30_000,
    })
}
