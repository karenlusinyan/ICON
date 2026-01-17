import type { ITask, ITaskOrder } from "../../models/task-svc";
import type { ITaskFilters } from "../../request/task-svc";
import { TaskStatus } from "../../models/task-svc/taskStatus";
import { taskAxios } from "../agent";

export const TaskStatuses = {
   getTaskStatuses: (signal?: AbortSignal) =>
      taskAxios.get<TaskStatus[]>("/api/taskstatus", { signal }),
};

export const Tasks = {
   getTasks: (filters?: ITaskFilters, signal?: AbortSignal) =>
      taskAxios.get<ITask[]>("/api/tasks", { params: filters, signal }),
   getTask: (id: string, signal?: AbortSignal) =>
      taskAxios.get<ITask>(`/api/tasks/${id}`, { signal }),
   create: (device: ITask, signal?: AbortSignal) =>
      taskAxios.post<ITask>("/api/tasks/create/", device, {
         signal,
      }),
   update: (task: ITask, signal?: AbortSignal) =>
      taskAxios.put<ITask>("/api/tasks/update", task, {
         signal,
      }),
   remove: <T>(id: string, signal?: AbortSignal) =>
      taskAxios.delete<T>(`/api/tasks/delete/${id}`, { signal }),
   reorder: <T>(tasks?: ITaskOrder[], signal?: AbortSignal) => {
      const payload = { tasks };
      // console.log("REORDER PAYLOAD", payload);
      return taskAxios.put<T>("/api/tasks/reorder", payload, { signal });
   },
};

const agent = {
   TaskStatuses,
   Tasks,
};

export default agent;
