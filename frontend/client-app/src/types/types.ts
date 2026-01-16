import type { TaskStatusCode } from "../constants/constants";

export type Guid = string & { readonly __guidBrand: unique symbol };

export const Guid = {
   Empty: "00000000-0000-0000-0000-000000000000" as Guid,

   NewGuid(): Guid {
      if (typeof crypto?.randomUUID === "function") {
         return crypto.randomUUID() as Guid;
      }

      // Fallback using getRandomValues
      const bytes = crypto.getRandomValues(new Uint8Array(16));

      // RFC 4122 version 4 UUID
      bytes[6] = (bytes[6] & 0x0f) | 0x40;
      bytes[8] = (bytes[8] & 0x3f) | 0x80;

      const toHex = (n: number) => n.toString(16).padStart(2, "0");

      const guid = [
         toHex(bytes[0]) + toHex(bytes[1]) + toHex(bytes[2]) + toHex(bytes[3]),
         toHex(bytes[4]) + toHex(bytes[5]),
         toHex(bytes[6]) + toHex(bytes[7]),
         toHex(bytes[8]) + toHex(bytes[9]),
         Array.from(bytes.slice(10), toHex).join(""),
      ].join("-");

      return guid as Guid;
   },

   IsGuidEmpty(id: Guid): boolean {
      return id === Guid.Empty;
   },
} as const;

export interface IResponse<T> {
   data?: T;
   error?: unknown;
}

export const taskStatusColorMap: Record<TaskStatusCode, string> = {
   INCOMPLETE: "orange",
   COMPLETED: "green",
};
