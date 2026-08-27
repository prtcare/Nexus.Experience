import { nexusDeveloperApi } from '../../api/DeveloperApiClient'

import type { Subtask } from './Feature'

export const subtasksApi = {
    get(subtaskId: string): Promise<Subtask> {
        return nexusDeveloperApi.get<Subtask>(
            `/subtasks/${subtaskId}`,
        )
    },
}
