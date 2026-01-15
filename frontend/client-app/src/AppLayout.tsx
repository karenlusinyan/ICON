import Layout from "antd/es/layout";
import Menu from "antd/es/menu";
import Button from "antd/es/button";
import Space from "antd/es/space";
import Drawer from "antd/es/drawer";
import Grid from "antd/es/grid";
import {
   HomeOutlined,
   UnorderedListOutlined,
   LogoutOutlined,
   MenuOutlined,
} from "@ant-design/icons";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { useState } from "react";
import { useAppDispatch } from "./hooks/hooks";
import { logout } from "./stores/authStore";

const { Header, Content } = Layout;
const { useBreakpoint } = Grid;

const menuItems = [
   {
      key: "/home",
      icon: <HomeOutlined />,
      label: "Home",
   },
   {
      key: "/tasks",
      icon: <UnorderedListOutlined />,
      label: "Tasks",
   },
];

export function AppLayout() {
   const { pathname } = useLocation();
   const navigate = useNavigate();
   const dispatch = useAppDispatch();
   const screens = useBreakpoint();

   const [drawerOpen, setDrawerOpen] = useState(false);

   const handleLogout = () => {
      dispatch(logout());
      navigate("/login", { replace: true });
   };

   const handleMenuClick = ({ key }: { key: string }) => {
      navigate(key);
      setDrawerOpen(false);
   };

   const isMobile = !screens.md;

   return (
      <Layout className="layout">
         <Header className="layout-header">
            {isMobile && (
               <Button
                  type="text"
                  icon={<MenuOutlined />}
                  onClick={() => setDrawerOpen(true)}
               />
            )}

            {!isMobile && (
               <Menu
                  theme="light"
                  mode="horizontal"
                  selectedKeys={[pathname]}
                  items={menuItems}
                  onClick={handleMenuClick}
                  style={{ flex: 1, borderBottom: "none" }}
               />
            )}

            <Space>
               <Button
                  type="text"
                  icon={<LogoutOutlined />}
                  onClick={handleLogout}
               >
                  {!isMobile && "Logout"}
               </Button>
            </Space>
         </Header>

         <Drawer
            placement="left"
            open={drawerOpen}
            onClose={() => setDrawerOpen(false)}
            styles={{ body: { padding: 0 } }}
         >
            <Menu
               mode="vertical"
               selectedKeys={[pathname]}
               items={menuItems}
               onClick={handleMenuClick}
            />
         </Drawer>

         <Content className="layout-content">
            <Outlet />
         </Content>
      </Layout>
   );
}
