import "./LoginForm.scss";
import Form from "antd/es/form";
import Input from "antd/es/input";
import Button from "antd/es/button";
import Card from "antd/es/card";
import Typography from "antd/es/typography";
import { UserOutlined, LockOutlined } from "@ant-design/icons";
import { login } from "../../../api/auth-svc/authApi";
import { useAppDispatch } from "../../../hooks/hooks";
import { useCallback } from "react";
import { setUser } from "../../../stores/authStore";
import { Link } from "react-router-dom";

const { Title, Text } = Typography;

export function LoginForm() {
   const dispatch = useAppDispatch();

   const onFinish = useCallback(
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      async (values: any) => {
         console.log("Login submitted:", values);
         const { email, password } = values;
         const response = await login(email, password);
         dispatch(
            setUser({
               user: response?.data,
            })
         );
      },
      [dispatch]
   );

   return (
      <div className="login-container">
         <Card className="login-card">
            <Title className="login-title" level={3}>
               Login
            </Title>

            <Form layout="vertical" onFinish={onFinish}>
               <Form.Item
                  label="Email/Username"
                  name="email"
                  rules={[
                     { required: true, message: "Email/Username is required" },
                     // { type: "email", message: "Invalid email address" },
                  ]}
               >
                  <Input prefix={<UserOutlined />} placeholder="Email" />
               </Form.Item>

               <Form.Item
                  label="Password"
                  name="password"
                  rules={[{ required: true, message: "Password is required" }]}
               >
                  <Input.Password
                     prefix={<LockOutlined />}
                     placeholder="Password"
                  />
               </Form.Item>

               <Button type="primary" htmlType="submit" block>
                  Sign In
               </Button>

               <div className="login-footer">
                  <Text>Don't have an account?</Text>
                  <Link to="/register"> Register </Link>
               </div>
            </Form>
         </Card>
      </div>
   );
}
