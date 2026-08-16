import { useQuery } from '@tanstack/react-query'
import { platformApi } from './platformApi'
import type { PlatformHealth } from '../../types/PlatformHealth'

export function usePlatformHealth() {
    return useQuery<PlatformHealth>({
        queryKey: ['platform-health'],
        queryFn: () => platformApi.getHealth(),
        retry: 1,
        refetchInterval: 30_000,
    })
}