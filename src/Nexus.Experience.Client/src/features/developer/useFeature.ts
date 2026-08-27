import { useQuery } from '@tanstack/react-query'

import { featuresApi } from './featuresApi'

export function useFeature(
    featureId: string | null | undefined,
) {
    return useQuery({
        queryKey: ['feature', featureId],
        queryFn: () => featuresApi.get(featureId!),
        enabled: Boolean(featureId),
    })
}
