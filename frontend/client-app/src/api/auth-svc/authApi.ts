import type { IResponse } from "../../types/types";
import type { IUser } from "../../models/auth-svc/user";
import agent from "./agent";

export async function signIn(
   username: string,
   password: string,
   signal?: AbortSignal
): Promise<IResponse<IUser>> {
   try {
      const response = await agent.Account.login<IUser>(
         username,
         password,
         signal
      );
      return {
         data: response.data,
      };
   } catch (error) {
      return {
         error: error,
      };
   }
}

export async function getUser(signal?: AbortSignal): Promise<IResponse<IUser>> {
   try {
      const response = await agent.Account.getUser(signal);
      return { data: response.data };
   } catch (error) {
      return {
         error: error,
      };
   }
}

export async function createAccount(
   email: string,
   password: string,
   signal?: AbortSignal
): Promise<IResponse<IUser>> {
   try {
      console.log(email, password, signal);
      return {};
   } catch (error) {
      return {
         error: error,
      };
   }
}

export async function changePassword(
   email: string,
   recoveryCode?: string,
   signal?: AbortSignal
): Promise<IResponse<IUser>> {
   try {
      console.log(email, recoveryCode, signal);
      return {};
   } catch (error) {
      return {
         error: error,
      };
   }
}

export async function resetPassword(
   email: string,
   signal?: AbortSignal
): Promise<IResponse<IUser>> {
   try {
      console.log(email, signal);
      return {};
   } catch (error) {
      return {
         error: error,
      };
   }
}
