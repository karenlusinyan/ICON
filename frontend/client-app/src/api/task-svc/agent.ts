import type { ITask } from "../../models/task-svc";
import { taskAxios } from "../agent";

export const Tasks = {
  getTasks: (signal?: AbortSignal) =>
    taskAxios.get<ITask[]>("/api/tasks", { signal }),
  getTask: (id: string, signal?: AbortSignal) =>
    taskAxios.get<ITask>(`/api/tasks/${id}`, { signal }),
  create: (device: ITask, signal?: AbortSignal) =>
    taskAxios.post<ITask>("/api/tasks/create/", device, {
      signal,
    }),
  update: (id: string, task: ITask, signal?: AbortSignal) =>
    taskAxios.put<ITask>(`/api/tasks/update/${id}`, task, {
      signal,
    }),
  remove: <T>(id: string, signal?: AbortSignal) =>
    taskAxios.delete<T>(`/api/tasks/delete/${id}`, { signal }),
};

const agent = {
  Tasks,
};

export default agent;
