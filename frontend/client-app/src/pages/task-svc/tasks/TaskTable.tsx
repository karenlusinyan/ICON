import Tag from "antd/es/tag";
import Space from "antd/es/space";
import Button from "antd/es/button";
import Table from "antd/es/table";
import { DeleteOutlined } from "@ant-design/icons";
import type { ColumnsType } from "antd/es/table";
import { taskStatusColorMap } from "../../../types/types";
import type { ITask, ITaskStatus } from "../../../models/task-svc";
import { TaskStatusCode } from "../../../constants/constants";
import dayjs from "dayjs";

interface Props {
   tasks: ITask[];
   taskStatuses?: ITaskStatus[];
   onUpdate: (task: ITask) => void;
   onDelete: (id: string) => void;
   onEdit: (task: ITask) => void;
   loading: boolean;
}

export default function TasksTable({
   tasks,
   onUpdate,
   onDelete,
   onEdit,
   loading,
}: Props) {
   const changeStatus = (task: ITask) => {
      if (task.statusCode === TaskStatusCode.COMPLETED) {
         return;
      }
      onUpdate({ ...task, statusCode: TaskStatusCode.COMPLETED });
   };

   const columns: ColumnsType<ITask> = [
      {
         title: "Title",
         dataIndex: "name",
         key: "title",
      },
      {
         title: "Description",
         dataIndex: "description",
         key: "description",
      },
      {
         title: "Status",
         dataIndex: "statusName",
         key: "statusName",
         render: (_: string, record) => (
            <Tag color={taskStatusColorMap[record.statusCode]}>
               {record.statusName}
            </Tag>
         ),
      },
      {
         title: "Created",
         dataIndex: "createdAt",
         key: "createdAt",
         render: (value: string) =>
            value ? dayjs(value).format("YYYY-MM-DD HH:mm") : "-",
      },
      {
         title: "Modified",
         dataIndex: "modifiedAt",
         key: "modifiedAt",
         render: (value: string) =>
            value ? dayjs(value).format("YYYY-MM-DD HH:mm") : "-",
      },
      {
         title: "Actions",
         key: "actions",
         align: "right",
         render: (_, task) => (
            <Space>
               <Button
                  type="primary"
                  disabled={task.statusCode === TaskStatusCode.COMPLETED}
                  onClick={(e) => {
                     e.stopPropagation();
                     changeStatus(task);
                  }}
               >
                  {task.statusCode !== TaskStatusCode.COMPLETED
                     ? "Complete Task"
                     : "Completed"}
               </Button>
               <Button
                  danger
                  icon={<DeleteOutlined />}
                  onClick={(e) => {
                     e.stopPropagation();
                     onDelete(task.id);
                  }}
               />
            </Space>
         ),
      },
   ];

   return (
      <Table
         rowKey="id"
         tableLayout="fixed"
         columns={columns}
         dataSource={tasks}
         pagination={false}
         loading={loading}
         onRow={(record) => ({
            onClick: () => onEdit(record),
         })}
      />
   );
}
