import { nexusDeveloperApi } from '../../api/DeveloperApiClient'

import type { ObjectChatLink } from './Feature'

export const objectChatLinksApi = {
    listByTarget(
        targetType: string,
        targetId: string,
    ): Promise<ObjectChatLink[]> {
        return nexusDeveloperApi.get<ObjectChatLink[]>(
            `/object-chat-links/by-target?targetType=${targetType}&targetId=${targetId}`,
        )
    },
}
