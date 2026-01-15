import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { IUser } from "../models/auth-svc/user";

interface AuthState {
   user: IUser | undefined;
   isAuthenticated?: boolean;
}

const savedUser: IUser | undefined = (() => {
   const user = localStorage.getItem("user");

   if (!user) return undefined;

   try {
      return JSON.parse(user);
   } catch (error) {
      console.log(`=> Error parsing user from localStorage: ${error}`);
      return undefined;
   }
})();

const initialState: AuthState = {
   user: savedUser,
   isAuthenticated: savedUser ? true : false,
};

const authSlice = createSlice({
   name: "auth",
   initialState,
   reducers: {
      setUser(state, action: PayloadAction<{ user?: IUser }>) {
         const user = action.payload.user;

         state.user = user;
         state.isAuthenticated = Boolean(user);

         if (user) {
            localStorage.setItem("user", JSON.stringify(user));
            return;
         }

         localStorage.removeItem("user");
      },

      logout(state) {
         state.user = undefined;
         state.isAuthenticated = false;

         localStorage.removeItem("user");
      },
   },
});

export const { setUser, logout } = authSlice.actions;
export default authSlice.reducer;
