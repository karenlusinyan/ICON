import "./TaskTable.scss";
import Tag from "antd/es/tag";
import Space from "antd/es/space";
import Button from "antd/es/button";
import Table from "antd/es/table";
import { DeleteOutlined } from "@ant-design/icons";
import type { ColumnsType } from "antd/es/table";
import { taskStatusColorMap } from "../../../types/types";
import type { ITask, ITaskOrder, ITaskStatus } from "../../../models/task-svc";
import { TaskStatusCode } from "../../../constants/constants";
import dayjs from "dayjs";
import { useEffect, useRef, useState } from "react";
import { DraggableTableRow } from "../../../components";

// ----------------------------------------------------------------------
// Sortable rows (Drag & Drop support)
// ----------------------------------------------------------------------
import {
   DndContext,
   closestCenter,
   PointerSensor,
   useSensor,
   useSensors,
} from "@dnd-kit/core";

import {
   SortableContext,
   verticalListSortingStrategy,
   arrayMove,
} from "@dnd-kit/sortable";
// ----------------------------------------------------------------------

interface Props {
   tasks: ITask[];
   taskStatuses?: ITaskStatus[];
   onUpdate: (task: ITask) => void;
   onDelete: (id: string) => void;
   onReorder: (tasks: ITaskOrder[]) => void;
   onEdit: (task: ITask) => void;
   loading: boolean;
}

export default function TasksTable({
   tasks,
   onUpdate,
   onDelete,
   onReorder,
   onEdit,
   loading,
}: Props) {
   // ----------------------------------------------------------------------
   // WORKAROUND: => Table scroll height calculation
   // ----------------------------------------------------------------------
   const containerRef = useRef<HTMLDivElement>(null);
   const [scrollY, setScrollY] = useState<number>(0);

   useEffect(() => {
      if (!containerRef.current) return;

      const observer = new ResizeObserver(([entry]) => {
         setScrollY(Math.floor(entry.contentRect.height));
      });

      observer.observe(containerRef.current);
      return () => observer.disconnect();
   }, []);
   // ----------------------------------------------------------------------

   // ----------------------------------------------------------------------
   // Sortable rows (Drag & Drop support)
   // ----------------------------------------------------------------------
   const [data, setData] = useState<ITask[]>(tasks);

   useEffect(() => {
      setData(tasks);
   }, [tasks]);

   const sensors = useSensors(
      useSensor(PointerSensor, {
         activationConstraint: {
            distance: 5,
         },
      })
   );

   // eslint-disable-next-line @typescript-eslint/no-explicit-any
   const onDragEnd = ({ active, over }: any) => {
      if (!over || active.id === over.id) return;

      const reordered = arrayMove(
         data,
         data.findIndex((i) => i.id === active.id),
         data.findIndex((i) => i.id === over.id)
      );

      // => Update local state
      setData(reordered);

      // => Syncronize to server
      onReorder(
         reordered.map((x, index) => {
            return { id: x.id, name: x.name, orderIndex: index };
         })
      );
   };
   // ----------------------------------------------------------------------

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
      <div ref={containerRef} className="table-container">
         <DndContext
            sensors={sensors}
            collisionDetection={closestCenter}
            onDragEnd={onDragEnd}
         >
            <SortableContext
               items={data.map((t) => t.id)}
               strategy={verticalListSortingStrategy}
            >
               <Table
                  rowKey="id"
                  tableLayout="fixed"
                  columns={columns}
                  dataSource={data}
                  pagination={false}
                  loading={loading}
                  scroll={{ y: scrollY }}
                  components={{
                     body: {
                        row: DraggableTableRow,
                     },
                  }}
                  onRow={(record) => ({
                     onClick: () => onEdit(record),
                  })}
               />
            </SortableContext>
         </DndContext>
      </div>
   );
}
