import axios, {
  type AxiosInstance,
  type AxiosError,
  type AxiosResponse,
} from "axios";
import { toast } from "react-toastify";
import store from "../stores/store";

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
      if (
        error.response?.data &&
        error.response?.status &&
        error.response?.config
      ) {
        const statusCode = error.response.status;
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const body = error.response.data as any;

        const message =
          body?.title ||
          body?.detail ||
          body?.message ||
          error.message ||
          "An unexpected error occurred";

        // Handle notifications based on status code
        switch (statusCode) {
          case 204:
            toast.warning(message);
            break;
          case 400:
          case 401:
          case 403:
          case 404:
          case 500:
          default:
            toast.error(message);
            break;
        }

        return Promise.reject(error.response);
      }

      return Promise.reject(error);
    }
  );
  //-----------------------------------------------------------------------

  return instance;
};

export default createAxiosInstance;
