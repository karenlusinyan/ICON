import createAxiosInstance from "../factories/axiosFactory";

// Create Axios instances for different base URLs
export const authAxios = createAxiosInstance(
   `${import.meta.env.VITE_AUTH_API_URL}`,
);
export const taskAxios = createAxiosInstance(
   `${import.meta.env.VITE_TASK_API_URL}`,
);

export const removeAxiosHeaders = () => {
   // => Remove header [Authorization]
   authAxios.defaults.headers.Authorization = null;
};
