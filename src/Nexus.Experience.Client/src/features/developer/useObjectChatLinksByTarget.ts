import { useQuery } from '@tanstack/react-query'

import { objectChatLinksApi } from './objectChatLinksApi'

export function useObjectChatLinksByTarget(
    targetType: string,
    targetId: string | null | undefined,
) {
    return useQuery({
        queryKey: [
            'object-chat-links',
            targetType,
            targetId,
        ],
        queryFn: () =>
            objectChatLinksApi.listByTarget(
                targetType,
                targetId!,
            ),
        enabled: Boolean(targetId),
    })
}
