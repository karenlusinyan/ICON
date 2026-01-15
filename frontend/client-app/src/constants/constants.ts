export const TaskStatusCode = {
   NEW: "NEW",
   INCOMPLETE: "INCOMPLETE",
   COMPLETED: "COMPLETED",
   ON_HOLD: "ON_HOLD",
   CANCELLED: "CANCELLED",
} as const;

export type TaskStatusCode =
   (typeof TaskStatusCode)[keyof typeof TaskStatusCode];
