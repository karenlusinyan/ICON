import createAxiosInstance from "../factories/axiosFactory";

// Create Axios instances for different base URLs
export const authAxios = createAxiosInstance(
  `${import.meta.env.VITE_BASE_URL}:10001`
);
export const taskAxios = createAxiosInstance(
  `${import.meta.env.VITE_BASE_URL}:10002`
);

export const removeAxiosHeaders = () => {
  // => Remove header [Authorization]
  authAxios.defaults.headers.Authorization = null;
};
