import { Guid } from "../../types/types";

export interface ITask {
  id: Guid;
  name: string;
  description: string;
  statusId?: Guid;
  disabled: boolean;
  createdAt?: Date;
  modifiedAt?: Date;
  deleted: boolean;
}

export class Task implements ITask {
  id: Guid = Guid.Empty;
  name = "";
  description = "";
  statusId?: Guid = undefined;
  disabled = false;
  createdAt?: Date | undefined;
  modifiedAt?: Date | undefined;
  deleted = false;
}

export const InitialTask: ITask = {
  id: Guid.Empty,
  name: "",
  description: "",
  statusId: undefined,
  disabled: false,
  createdAt: new Date(),
  modifiedAt: new Date(),
  deleted: false,
};
