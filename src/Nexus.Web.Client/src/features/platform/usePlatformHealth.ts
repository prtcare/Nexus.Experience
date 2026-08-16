import { useQuery } from '@tanstack/react-query'
import { platformApi } from './platformApi'

export function usePlatformHealth() {
    return useQuery({
        queryKey: ['platform-health'],
        queryFn: platformApi.getHealth,
        retry: 1,
        refetchInterval: 30_000,
    })
}