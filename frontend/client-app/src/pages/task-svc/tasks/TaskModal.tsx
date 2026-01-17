import Form from "antd/es/form";
import Modal from "antd/es/modal";
import Input from "antd/es/input";
import type { ITask } from "../../../models/task-svc";
import { useEffect } from "react";
import { TaskStatusCode } from "../../../constants/constants";

interface Props {
   open: boolean;
   task?: ITask | null;
   onClose: () => void;
   onSubmit: (task: ITask) => void;
}

export default function TaskModal({ open, task, onClose, onSubmit }: Props) {
   const [form] = Form.useForm();

   const isCompleted = task?.statusCode === TaskStatusCode.COMPLETED;

   useEffect(() => {
      if (task) {
         form.setFieldsValue({
            title: task.name,
            description: task.description,
         });
      } else {
         form.resetFields();
      }
   }, [task, form]);

   const submit = () => {
      form.validateFields().then((values) => {
         onSubmit({
            ...task,
            name: values.title.trim(),
            description: values.description?.trim() || null,
         } as ITask);

         form.resetFields();
         onClose();
      });
   };

   return (
      <Modal
         title={task ? "Edit Task" : "New Task"}
         open={open}
         forceRender
         onOk={submit}
         onCancel={onClose}
         okText={task ? "Save" : "Create"}
         cancelText={task ? "Close" : "Cancel"}
         okButtonProps={{ disabled: isCompleted }}
      >
         <Form form={form} layout="vertical" disabled={isCompleted}>
            <Form.Item
               name="title"
               label="Title"
               rules={[
                  {
                     required: true,
                     whitespace: true,
                     message: "Title is required",
                  },
               ]}
            >
               <Input />
            </Form.Item>

            <Form.Item name="description" label="Description">
               <Input.TextArea rows={3} />
            </Form.Item>
         </Form>
      </Modal>
   );
}
