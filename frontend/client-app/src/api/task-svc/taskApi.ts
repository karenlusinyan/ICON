import type { IResponse } from "../../types/types";
import type { ITask, ITaskOrder } from "../../models/task-svc";
import type { ITaskFilters } from "../../request/task-svc";
import agent from "../task-svc/agent";

export async function getTasks(
   filters?: ITaskFilters,
   signal?: AbortSignal
): Promise<IResponse<ITask[]>> {
   try {
      const response = await agent.Tasks.getTasks(filters, signal);
      return {
         data: response.data,
      };
   } catch (error) {
      return {
         error: error,
      };
   }
}

export async function GetTask(
   id: string,
   signal?: AbortSignal
): Promise<IResponse<ITask>> {
   try {
      const response = await agent.Tasks.getTask(id, signal);
      return {
         data: response.data,
      };
   } catch (error) {
      return {
         error: error,
      };
   }
}

export async function create(
   task: ITask,
   signal?: AbortSignal
): Promise<IResponse<ITask>> {
   try {
      const response = await agent.Tasks.create(task, signal);
      return {
         data: response.data,
      };
   } catch (error) {
      return {
         error: error,
      };
   }
}

export async function update(
   task: ITask,
   signal?: AbortSignal
): Promise<IResponse<ITask>> {
   try {
      const response = await agent.Tasks.update(task, signal);
      return {
         data: response.data,
      };
   } catch (error) {
      return {
         error: error,
      };
   }
}

export async function remove(
   id: string,
   signal?: AbortSignal
): Promise<IResponse<ITask>> {
   try {
      await agent.Tasks.remove(id, signal);
      return {};
   } catch (error) {
      return {
         error: error,
      };
   }
}

export async function reorder(
   tasks?: ITaskOrder[],
   signal?: AbortSignal
): Promise<IResponse<void>> {
   try {
      await agent.Tasks.reorder(tasks, signal);
      return {};
   } catch (error) {
      return {
         error: error,
      };
   }
}
