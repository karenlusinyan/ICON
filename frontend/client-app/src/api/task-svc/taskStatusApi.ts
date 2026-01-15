import type { IResponse } from "../../types/types";
import type { ITaskStatus } from "../../models/task-svc/taskStatus";
import agent from "./agent";

export async function getTaskStatuses(
   signal?: AbortSignal
): Promise<IResponse<ITaskStatus[]>> {
   try {
      const response = await agent.TaskStatuses.getTaskStatuses(signal);
      return {
         data: response.data,
      };
   } catch (error) {
      return {
         error: error,
      };
   }
}
