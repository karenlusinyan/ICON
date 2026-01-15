import "./HomePage.scss";
import Button from "antd/es/Button";
import Typography from "antd/es/typography";
import { useNavigate } from "react-router-dom";

const { Title } = Typography;

export default function HomePage() {
   const navigate = useNavigate();

   return (
      <div className="content">
         <Title level={2}>Welcome!</Title>
         <Button type="primary" onClick={() => navigate("/tasks")}>
            Show Tasks
         </Button>
      </div>
   );
}
