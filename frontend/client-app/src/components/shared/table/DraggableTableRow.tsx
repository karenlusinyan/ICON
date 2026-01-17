import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import React from "react";

interface DraggableRowProps extends React.HTMLAttributes<HTMLTableRowElement> {
   "data-row-key": string;
}

export default function DraggableTableRow(props: DraggableRowProps) {
   const { setNodeRef, attributes, listeners, transform, transition } =
      useSortable({ id: props["data-row-key"] });

   return (
      <tr
         ref={setNodeRef}
         {...props}
         {...attributes}
         {...listeners}
         style={{
            ...props.style,
            transform: CSS.Transform.toString(transform),
            transition,
            cursor: "move",
         }}
      />
   );
}
