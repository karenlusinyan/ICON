import type { TaskStatusCode } from "../../constants/constants";
import { Guid } from "../../types/types";

export interface ITask {
   id: Guid;
   name: string;
   code: string;
   description: string;
   orderIndex: number;
   statusId: Guid;
   statusName: string;
   statusCode: TaskStatusCode;
   disabled: boolean;
   createdAt?: Date;
   modifiedAt?: Date;
   deleted: boolean;
}

export class Task implements ITask {
   id: Guid = Guid.Empty;
   name = "";
   code = "";
   description = "";
   orderIndex = 0;
   statusId: Guid = Guid.Empty;
   statusName = "";
   statusCode: TaskStatusCode = "INCOMPLETE";
   disabled = false;
   createdAt?: Date | undefined;
   modifiedAt?: Date | undefined;
   deleted = false;
}

export const InitialTask: ITask = {
   id: Guid.Empty,
   name: "",
   code: "",
   description: "",
   orderIndex: 0,
   statusId: Guid.Empty,
   statusName: "",
   statusCode: "INCOMPLETE",
   disabled: false,
   createdAt: new Date(),
   modifiedAt: new Date(),
   deleted: false,
};
