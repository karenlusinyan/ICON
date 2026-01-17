import "./RegisterForm.scss";
import Form from "antd/es/form";
import Input from "antd/es/input";
import Button from "antd/es/button";
import Card from "antd/es/card";
import Typography from "antd/es/typography";
import { UserOutlined, LockOutlined } from "@ant-design/icons";
import { register } from "../../../api/auth-svc/authApi";
import { useCallback } from "react";
import { Link } from "react-router-dom";
import { setUser } from "../../../stores/authStore";
import { useAppDispatch } from "../../../hooks/hooks";

const { Title, Text } = Typography;

export function RegisterForm() {
   const dispatch = useAppDispatch();

   const onFinish = useCallback(
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      async (values: any) => {
         // console.log("Register submitted:", values);
         const { email, username, password } = values;
         const response = await register(email, username, password);
         dispatch(
            setUser({
               user: response?.data,
            })
         );
      },
      [dispatch]
   );

   return (
      <div className="register-container">
         <Card className="register-card">
            <Title className="register-title" level={3}>
               Register
            </Title>

            <Form layout="vertical" onFinish={onFinish}>
               <Form.Item
                  label="Email"
                  name="email"
                  rules={[
                     { required: true, message: "Email is required" },
                     { type: "email", message: "Invalid email address" },
                  ]}
               >
                  <Input prefix={<UserOutlined />} placeholder="Email" />
               </Form.Item>

               <Form.Item
                  label="Username"
                  name="username"
                  rules={[{ required: true, message: "Username is required" }]}
               >
                  <Input prefix={<UserOutlined />} placeholder="Username" />
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
                  Register
               </Button>

               <div className="register-footer">
                  <Text>Already have an account?</Text>
                  <Link to="/login"> Login </Link>
               </div>
            </Form>
         </Card>
      </div>
   );
}
