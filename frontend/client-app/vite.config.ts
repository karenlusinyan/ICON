import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
   server: {
      host: "0.0.0.0",
      allowedHosts: ["host.docker.internal", "localhost", "127.0.0.1"],
      port: 3000,
      strictPort: true,
      hmr: {
         clientPort: 3000,
         overlay: false,
      },
   },
   build: {
      chunkSizeWarningLimit: 800,
      rollupOptions: {
         output: {
            manualChunks: {
               antd: ["antd"],
               vendor: ["react", "react-dom", "react-router-dom"],
            },
         },
      },
   },
   plugins: [react()],
});
