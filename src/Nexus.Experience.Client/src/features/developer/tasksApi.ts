import { nexusDeveloperApi } from '../../api/DeveloperApiClient'

import type { Task } from './Feature'

export const tasksApi = {
    get(taskId: string): Promise<Task> {
        return nexusDeveloperApi.get<Task>(
            `/tasks/${taskId}`,
        )
    },
}
