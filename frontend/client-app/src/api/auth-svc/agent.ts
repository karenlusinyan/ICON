import type { IUser } from "../../models/auth-svc/user";
import { authAxios } from "../agent";

export const Account = {
   getUser: (signal?: AbortSignal) =>
      authAxios.get<IUser>("/api/account/user", { signal }),
   login: <IUser>(username: string, password: string, signal?: AbortSignal) =>
      authAxios.post<IUser>(
         "/api/account/login",
         {
            username,
            password,
         },
         { signal }
      ),
   register: <T>(user: IUser, signal?: AbortSignal) =>
      authAxios.post<T>("/api/account/register", user, { signal }),
};

const agent = {
   Account,
};

export default agent;
