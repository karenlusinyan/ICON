import { useEffect, useState } from "react";
import { BrowserRouter } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "./hooks/hooks";
import { setUser } from "./stores/authStore";
import { getUser } from "./api/auth-svc/authApi";
import { AppRoutes } from "./AppRoutes";
import Spin from "antd/es/spin";

export default function App() {
   const [loading, setLoading] = useState(false);
   const { isAuthenticated } = useAppSelector((state) => state.auth);
   const dispatch = useAppDispatch();

   useEffect(() => {
      if (isAuthenticated) {
         const checkAuth = async () => {
            try {
               setLoading(true);
               const response = await getUser();
               dispatch(setUser({ user: response?.data || undefined }));
            } catch {
               dispatch(setUser({ user: undefined }));
            } finally {
               setLoading(false);
            }
         };
         checkAuth();
      }
   }, [dispatch, isAuthenticated]);

   if (loading) {
      return <Spin fullscreen />;
   }

   return (
      <BrowserRouter>
         <AppRoutes />
      </BrowserRouter>
   );
}
