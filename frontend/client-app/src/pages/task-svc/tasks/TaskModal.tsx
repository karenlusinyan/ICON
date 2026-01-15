import { Modal, Form, Input } from "antd";
import type { ITask } from "../../../models/task-svc";
import { useEffect } from "react";

interface Props {
   open: boolean;
   task?: ITask | null;
   onClose: () => void;
   onSubmit: (task: ITask) => void;
}

export default function TaskModal({ open, task, onClose, onSubmit }: Props) {
   const [form] = Form.useForm();

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
            name: values.title,
            description: values.description,
         } as ITask);

         form.resetFields();
         onClose();
      });
   };

   return (
      <Modal
         title={task ? "Edit Task" : "New Task"}
         open={open}
         onOk={submit}
         onCancel={onClose}
         okText="Create"
      >
         <Form form={form} layout="vertical">
            <Form.Item name="title" label="Title" rules={[{ required: true }]}>
               <Input />
            </Form.Item>

            <Form.Item name="description" label="Description">
               <Input.TextArea rows={3} />
            </Form.Item>
         </Form>
      </Modal>
   );
}
