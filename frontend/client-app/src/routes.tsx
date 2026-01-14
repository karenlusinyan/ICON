import { Navigate, type RouteObject } from "react-router-dom";
import * as pages from "./pages";
import { AppLayout } from "./AppLayout";

export function routes(user: unknown): RouteObject[] {
   return [
      {
         path: "/login",
         element: user ? <Navigate to="/home" replace /> : <pages.LoginPage />,
      },
      {
         path: "/register",
         element: user ? (
            <Navigate to="/home" replace />
         ) : (
            <pages.RegisterPage />
         ),
      },
      {
         element: user ? <AppLayout /> : <Navigate to="/login" replace />,
         children: [
            {
               path: "/home",
               element: <pages.HomePage />,
            },
            {
               path: "/tasks",
               element: <pages.TasksPage />,
            },
         ],
      },
      {
         path: "*",
         element: <Navigate to={user ? "/home" : "/login"} replace />,
      },
   ];
}
