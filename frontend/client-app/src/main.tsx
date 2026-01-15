import "./index.scss";
import "antd/dist/reset.css";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { Provider } from "react-redux";
import store from "./stores/store";
import App from "./App";
import ConfigProvider from "antd/es/config-provider";

createRoot(document.getElementById("root")!).render(
   <StrictMode>
      <Provider store={store}>
         <ConfigProvider
            theme={{
               token: {
                  fontFamily: "Manrope",
                  fontSize: 14,
                  fontWeightStrong: 600,
               },
            }}
         >
            <App />
         </ConfigProvider>
      </Provider>
   </StrictMode>
);
