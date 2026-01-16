import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

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
   plugins: [
      react({
         babel: {
            plugins: [
               [
                  "import",
                  {
                     libraryName: "antd",
                     libraryDirectory: "es",
                     style: true,
                  },
                  "antd",
               ],
            ],
         },
      }),
   ],
   build: {
      chunkSizeWarningLimit: 1200,
      rollupOptions: {
         output: {
            manualChunks(id) {
               if (!id.includes("node_modules")) return;
               if (id.includes("/node_modules/react/")) return "vendor";
               if (id.includes("/node_modules/react-dom/")) return "vendor";
               if (id.includes("/node_modules/react-router")) return "vendor";
            },
         },
      },
   },
});
