import { useEffect, useState } from "react";
import { BrowserRouter } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "./hooks/hooks";
import { setUser } from "./stores/authStore";
import { getUser } from "./api/auth-svc/authApi";
import { AppRoutes } from "./AppRoutes";
import Spin from "antd/es/spin";

export default function App() {
   const [loading, setLoading] = useState(false);
   const { user } = useAppSelector((state) => state.auth);
   const dispatch = useAppDispatch();

   useEffect(() => {
      if (!user?.token) {
         dispatch(setUser({ user: undefined }));
         return;
      }
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
      // eslint-disable-next-line react-hooks/exhaustive-deps
   }, [dispatch]);

   if (loading) {
      return <Spin fullscreen />;
   }

   return (
      <BrowserRouter>
         <AppRoutes />
      </BrowserRouter>
   );
}
