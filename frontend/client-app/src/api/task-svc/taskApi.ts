import type { IResponse } from "../../types/types";
import type { ITask } from "../../models/task-svc";
import agent from "../task-svc/agent";

export async function getTasks(
   signal?: AbortSignal
): Promise<IResponse<ITask[]>> {
   try {
      const response = await agent.Tasks.getTasks(signal);
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
   id: string,
   task: ITask,
   signal?: AbortSignal
): Promise<IResponse<ITask>> {
   try {
      const response = await agent.Tasks.update(id, task, signal);
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
