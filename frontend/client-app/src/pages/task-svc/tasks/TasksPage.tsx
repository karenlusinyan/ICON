import "./TasksPage.scss";
import Space from "antd/es/space";
import Button from "antd/es/button";
import TaskModal from "./TaskModal";
import Select from "antd/es/select";
import { useCallback, useEffect, useMemo, useState } from "react";
import { PlusOutlined, ReloadOutlined } from "@ant-design/icons";
import TasksTable from "./TaskTable";
import type { ITask } from "../../../models/task-svc";
import type { ITaskStatus } from "../../../models/task-svc/taskStatus";
import {
   create,
   getTasks,
   remove,
   update,
} from "../../../api/task-svc/taskApi";
import { getTaskStatuses } from "../../../api/task-svc/taskStatusApi";

export default function TasksPage() {
   const [tasks, setTasks] = useState<ITask[]>([]);
   const [statusFilter, setStatusFilter] = useState<string | null>(null);
   const [taskStatuses, setTaskStatuses] = useState<ITaskStatus[]>([]);
   const [editingTask, setEditingTask] = useState<ITask | null>(null);
   const [loading, setLoading] = useState(false);
   const [open, setOpen] = useState(false);

   const filteredTasks = useMemo(() => {
      return statusFilter
         ? tasks.filter((t) => t.statusId === statusFilter)
         : tasks;
   }, [tasks, statusFilter]);

   // ----------------------------------------------------------------------
   // => Fetch data from API
   // ----------------------------------------------------------------------
   const fetchTaskStatuses = useCallback(async () => {
      const response = await getTaskStatuses();
      console.log("Fetch task statuses completed", response);
      if (response?.data) {
         setTaskStatuses(response.data);
         console.log(response.data);
      }
   }, []);

   const fetchTasks = useCallback(async () => {
      try {
         setLoading(true);
         const response = await getTasks();
         if (response?.data) {
            setTasks(response.data);
            console.log(response.data);
         }
      } catch (error) {
         console.error(error);
      } finally {
         console.log("Fetch tasks completed");
         setLoading(false);
      }
   }, []);

   useEffect(() => {
      fetchTaskStatuses();
      fetchTasks();
   }, [fetchTaskStatuses, fetchTasks]);
   // ----------------------------------------------------------------------

   // ----------------------------------------------------------------------
   // => Handlers
   // ----------------------------------------------------------------------
   const createTask = useCallback(
      async (task: ITask) => {
         await create(task);
         await fetchTasks();
      },
      [fetchTasks]
   );

   const updateTask = useCallback(
      async (task: ITask) => {
         await update(task);
         await fetchTasks();
      },
      [fetchTasks]
   );

   const removeTask = useCallback(
      async (id: string) => {
         await remove(id);
         await fetchTasks();
      },
      [fetchTasks]
   );

   const editTask = (task: ITask) => {
      setEditingTask(task);
      setOpen(true);
   };

   return (
      <div className="tasks-page">
         <div className="tasks-toolbar">
            <Space className="tasks-toolbar-left">
               <Select
                  className="task-toolbar-filter"
                  allowClear
                  placeholder="Filter by status"
                  value={statusFilter}
                  onChange={setStatusFilter}
                  options={taskStatuses.map((s) => ({
                     label: s.name,
                     value: s.id,
                  }))}
               />
            </Space>

            <Space className="tasks-toolbar-right">
               <Button
                  type="default"
                  icon={<ReloadOutlined />}
                  onClick={fetchTasks}
               >
                  Refresh
               </Button>
               <Button
                  type="primary"
                  icon={<PlusOutlined />}
                  onClick={() => setOpen(true)}
               >
                  New Task
               </Button>
            </Space>
         </div>

         <TasksTable
            tasks={filteredTasks}
            onUpdate={updateTask}
            onDelete={removeTask}
            onEdit={editTask}
            loading={loading}
         />

         <TaskModal
            open={open}
            task={editingTask}
            onClose={() => {
               setOpen(false);
               setEditingTask(null);
            }}
            onSubmit={editingTask ? updateTask : createTask}
         />
      </div>
   );
}
