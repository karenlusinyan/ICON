export const TaskStatusCode = {
   INCOMPLETE: "INCOMPLETE",
   COMPLETED: "COMPLETED",
} as const;

export type TaskStatusCode =
   (typeof TaskStatusCode)[keyof typeof TaskStatusCode];
