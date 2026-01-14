import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { IUser } from "../models/auth-svc/user";
import { removeAxiosHeaders } from "../api/agent";

interface AuthState {
  user: IUser | undefined;
  loading: boolean;
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
  loading: false,
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setLoading(state, action: PayloadAction<boolean>) {
      state.loading = action.payload;
    },
    setUser(state, action: PayloadAction<{ user?: IUser }>) {
      const user = action.payload.user;

      state.user = user;

      if (user) {
        localStorage.setItem("user", JSON.stringify(user));
      } else {
        localStorage.removeItem("user");
      }
    },
    logout(state) {
      state.user = undefined;
      state.loading = false;

      // => Remove from localStorage
      localStorage.removeItem("user");

      // => Remove axios headers
      removeAxiosHeaders();
    },
  },
});

export const { setLoading, setUser, logout } = authSlice.actions;
export default authSlice.reducer;
