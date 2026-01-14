import { useRoutes } from "react-router-dom";
import { useAppSelector } from "./hooks/hooks";
import { routes } from "./routes";

export function AppRoutes() {
   const user = useAppSelector((s) => s.auth.user);
   return useRoutes(routes(user));
}
