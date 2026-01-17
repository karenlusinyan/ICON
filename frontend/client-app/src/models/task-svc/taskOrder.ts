import { Guid } from "../../types/types";

export interface ITaskOrder {
   id: Guid;
   orderIndex: number;
}

export class TaskOrder implements ITaskOrder {
   id: Guid = Guid.Empty;
   orderIndex = 0;
}

export const InitialTaskOrder: ITaskOrder = {
   id: Guid.Empty,
   orderIndex: 0,
};
