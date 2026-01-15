import axios, {
   type AxiosInstance,
   type AxiosError,
   type AxiosResponse,
} from "axios";
import store from "../stores/store";
import message from "antd/es/message";

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

export const requests = (axiosInstance: AxiosInstance) => ({
   get: <T>(url: string, signal?: AbortSignal): Promise<T> =>
      axiosInstance.get<T>(url, { signal }).then(responseBody),

   post: <T, B>(url: string, body: B, signal?: AbortSignal): Promise<T> =>
      axiosInstance.post<T>(url, body, { signal }).then(responseBody),

   put: <T, B>(url: string, body: B, signal?: AbortSignal): Promise<T> =>
      axiosInstance.put<T>(url, body, { signal }).then(responseBody),

   delete: <T>(url: string, signal?: AbortSignal): Promise<T> =>
      axiosInstance.delete<T>(url, { signal }).then(responseBody),
});

const createAxiosInstance = (baseURL: string): AxiosInstance => {
   //-----------------------------------------------------------------------
   // => Create Axios instance
   //-----------------------------------------------------------------------
   const instance = axios.create({
      baseURL,
      headers: {
         "Content-Type": "application/json", // Set default headers
      },
   });
   //-----------------------------------------------------------------------

   //-----------------------------------------------------------------------
   // => Attach request interceptor
   //-----------------------------------------------------------------------
   instance.interceptors.request.use(
      (config) => {
         const state = store.getState();
         const token = state.auth.user?.token;

         if (config.headers) {
            if (token) {
               config.headers.Authorization = `Bearer ${token}`;
            }
         }
         return config;
      },
      (error) => {
         return Promise.reject(error);
      }
   );
   //-----------------------------------------------------------------------

   //-----------------------------------------------------------------------
   // => Attach response interceptor
   //-----------------------------------------------------------------------
   instance.interceptors.response.use(
      (response) => response,
      (error: AxiosError) => {
         if (error.response) {
            const { status, data } = error.response as {
               status: number;
               data: unknown;
            };

            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            const payload = data as any;

            let messageText = "An unexpected error occurred";

            if (status === 400 && payload?.errors) {
               messageText = Object.values(payload.errors).flat().join("\n");
            } else if (payload?.title) {
               messageText = payload.title;
            } else if (typeof payload === "string") {
               messageText = payload;
            }

            message.error(messageText);

            return Promise.reject(error);
         }

         return Promise.reject(error);
      }
   );
   //-----------------------------------------------------------------------

   return instance;
};

export default createAxiosInstance;
