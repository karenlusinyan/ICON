import type { TaskStatusCode } from "../../constants/constants";
import { Guid } from "../../types/types";

export interface ITask {
   id: Guid;
   name: string;
   code: string;
   description: string;
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
   statusId: Guid = Guid.Empty;
   statusName = "";
   statusCode: TaskStatusCode = "NEW";
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
   statusId: Guid.Empty,
   statusName: "",
   statusCode: "NEW",
   disabled: false,
   createdAt: new Date(),
   modifiedAt: new Date(),
   deleted: false,
};
