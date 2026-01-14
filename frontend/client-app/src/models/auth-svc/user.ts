import { Guid } from "../../types/types";

export interface IUser {
  id: Guid;
  email: string;
  username: string;
  password?: string;
  token: string;
  roles: string[];
}

export class User implements IUser {
   id: Guid = Guid.Empty;
   email = "";
   username = "";
   password? = "";
   token = "";
   roles = [];
}

export const InitialUser: IUser = {
   id: Guid.Empty,
   email: "",
   username: "",
   password: "",
   token: "",
   roles: [],
};
