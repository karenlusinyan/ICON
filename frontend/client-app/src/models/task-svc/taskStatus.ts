import type { TaskStatusCode } from "../../constants/constants";
import { Guid } from "../../types/types";

export interface ITaskStatus {
   id: Guid;
   code: TaskStatusCode;
   name: string;
}

export class TaskStatus implements ITaskStatus {
   id: Guid = Guid.Empty;
   code: TaskStatusCode = "INCOMPLETE";
   name = "";
}

export const InitialTaskStatus: ITaskStatus = {
   id: Guid.Empty,
   code: "INCOMPLETE",
   name: "",
};
