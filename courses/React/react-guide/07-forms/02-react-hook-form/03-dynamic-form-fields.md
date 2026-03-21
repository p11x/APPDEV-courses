# Dynamic Form Fields with useFieldArray

## Overview

Real-world applications often require forms with dynamic fields—think invoice line items, todo lists, employee skill lists, or event attendee registration. React Hook Form provides the `useFieldArray` hook to handle these variable-length form collections elegantly. This guide covers adding, removing, reordering fields and how to validate dynamic field arrays with Zod.

## Prerequisites

- Completed the React Hook Form setup guide
- Completed the Zod validation guide
- Understanding of React Hook Form's register and handleSubmit
- Familiarity with array manipulation in JavaScript

## Core Concepts

### Introducing useFieldArray

The useFieldArray hook manages a collection of form fields where the number of entries can change at runtime. It's perfect for scenarios where users can add multiple items.

```tsx
// File: src/components/BasicFieldArray.tsx

// Import useFieldArray along with useForm from react-hook-form
import { useForm, useFieldArray } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';

// Define schema for a single todo item
const todoItemSchema = z.object({
  // Each todo needs an ID for tracking (we'll generate this)
  id: z.string(),
  // The todo text
  text: z.string().min(1, "Task description is required"),
  // Whether the task is completed
  completed: z.boolean()
});

// Array of todos wrapped in an object (recommended structure)
const todosSchema = z.object({
  // The items array contains todo objects
  items: z.array(todoItemSchema).min(1, "Add at least one task")
});

type TodosFormData = z.infer<typeof todosSchema>;

function BasicFieldArray() {
  const { 
    register, 
    handleSubmit, 
    control, 
    formState: { errors } 
  } = useForm<TodosFormData>({
    resolver: zodResolver(todosSchema),
    defaultValues: {
      // Start with one empty todo item
      items: [
        { id: crypto.randomUUID(), text: "", completed: false }
      ]
    }
  });

  // useFieldArray manages the array of fields
  // fields: array of field objects with name, onBlur, onChange, ref
  // append: function to add new fields
  // remove: function to remove fields by index
  // swap: function to swap positions of two fields
  // move: function to move a field to a different position
  const { 
    fields, 
    append, 
    remove, 
    swap, 
    move 
  } = useFieldArray({
    control, // Required: pass the control from useForm
    name: "items" // The name of the field array in your form data
  });

  const onSubmit = (data: TodosFormData) => {
    console.log("Form data:", data);
    alert(`You have ${data.items.length} todo(s)!`);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <h2>Todo List</h2>

      {/* Map over fields to render each todo item */}
      {fields.map((field, index) => (
        <div key={field.id} className="todo-item">
          {/* 
            For field arrays, the name includes the index:
            items.0.text, items.1.text, etc.
          */}
          <input
            {...register(`items.${index}.text` as const)}
            placeholder="What needs to be done?"
          />
          {/* Show error for this specific field if validation fails */}
          {errors.items?.[index]?.text && (
            <span className="error">{errors.items[index]?.text?.message}</span>
          )}

          {/* Checkbox for completed status */}
          <label>
            <input
              type="checkbox"
              {...register(`items.${index}.completed` as const)}
            />
            Done
          </label>

          {/* Remove button - disable if only one item remains */}
          <button 
            type="button" 
            onClick={() => remove(index)}
            disabled={fields.length === 1}
          >
            Remove
          </button>
        </div>
      ))}

      {/* Add new todo button */}
      <button 
        type="button" 
        onClick={() => append({ 
          id: crypto.randomUUID(), 
          text: "", 
          completed: false 
        })}
      >
        Add Todo
      </button>

      {/* Show form-level errors */}
      {errors.items && (
        <p className="error">{errors.items.message}</p>
      )}

      <button type="submit">Save Todos</button>
    </form>
  );
}

export default BasicFieldArray;
```

### Using watch() with Field Arrays

The watch function allows you to monitor field values in real-time, useful for calculating totals or showing conditional content based on field values.

```tsx
// File: src/components/InvoiceForm.tsx

import { useForm, useFieldArray, useWatch } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';

// Define a line item in an invoice
const lineItemSchema = z.object({
  description: z.string().min(1, "Description required"),
  quantity: z.coerce.number().min(1, "Quantity must be at least 1"),
  unitPrice: z.coerce.number().min(0, "Price cannot be negative")
});

// Invoice schema with array of line items
const invoiceSchema = z.object({
  customerName: z.string().min(1, "Customer name required"),
  items: z.array(lineItemSchema).min(1, "At least one item required"),
  // Note: total is calculated, not user-entered
  taxRate: z.coerce.number().min(0).max(100).default(10)
});

type InvoiceData = z.infer<typeof invoiceSchema>;

function InvoiceForm() {
  const { 
    register, 
    handleSubmit, 
    control,
    formState: { errors, isSubmitting } 
  } = useForm<InvoiceData>({
    resolver: zodResolver(invoiceSchema),
    defaultValues: {
      customerName: "",
      taxRate: 10,
      items: [
        { description: "", quantity: 1, unitPrice: 0 }
      ]
    }
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: "items"
  });

  // Watch the items array to calculate totals in real-time
  // This re-renders when any item changes
  const watchedItems = useWatch({
    control,
    name: "items"
  });

  // Calculate subtotal from watched items
  const subtotal = watchedItems?.reduce((sum, item) => {
    const qty = Number(item?.quantity) || 0;
    const price = Number(item?.unitPrice) || 0;
    return sum + (qty * price);
  }, 0) ?? 0;

  // Calculate tax
  const taxRate = watchedItems ? (subtotal * 0.1) : 0;
  
  // Calculate grand total
  const total = subtotal + taxRate;

  const onSubmit = async (data: InvoiceData) => {
    // Add calculated total before sending
    const invoiceData = { ...data, total };
    console.log("Invoice:", invoiceData);
    await new Promise(r => setTimeout(r, 1000));
    alert("Invoice created!");
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <h2>Create Invoice</h2>

      {/* Customer Name */}
      <div className="field">
        <label>Customer Name</label>
        <input {...register("customerName")} />
        {errors.customerName && (
          <span>{errors.customerName.message}</span>
        )}
      </div>

      {/* Line Items */}
      <h3>Line Items</h3>
      
      {fields.map((field, index) => (
        <div key={field.id} className="line-item">
          <div className="field">
            <label>Description</label>
            <input
              {...register(`items.${index}.description` as const)}
              placeholder="Item description"
            />
            {errors.items?.[index]?.description && (
              <span>{errors.items[index]?.description?.message}</span>
            )}
          </div>

          <div className="field small">
            <label>Qty</label>
            <input
              type="number"
              min="1"
              {...register(`items.${index}.quantity` as const)}
            />
          </div>

          <div className="field small">
            <label>Unit Price</label>
            <input
              type="number"
              min="0"
              step="0.01"
              {...register(`items.${index}.unitPrice` as const)}
            />
          </div>

          <div className="line-total">
            ${((Number(fields[index]?.quantity) || 0) * 
               (Number(fields[index]?.unitPrice) || 0)).toFixed(2)}
          </div>

          <button 
            type="button" 
            onClick={() => remove(index)}
            disabled={fields.length === 1}
          >
            ×
          </button>
        </div>
      ))}

      {/* Add Item Button */}
      <button
        type="button"
        onClick={() => append({
          description: "",
          quantity: 1,
          unitPrice: 0
        })}
      >
        + Add Line Item
      </button>

      {/* Totals Section */}
      <div className="totals">
        <div className="total-row">
          <span>Subtotal:</span>
          <span>${subtotal.toFixed(2)}</span>
        </div>
        <div className="total-row">
          <span>Tax (10%):</span>
          <span>${taxRate.toFixed(2)}</span>
        </div>
        <div className="total-row grand-total">
          <span>Total:</span>
          <span>${total.toFixed(2)}</span>
        </div>
      </div>

      {errors.items && (
        <p className="error">{errors.items.message}</p>
      )}

      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? "Creating..." : "Create Invoice"}
      </button>
    </form>
  );
}

export default InvoiceForm;
```

### Nested Field Arrays

For complex forms, you can have nested field arrays—useful for forms with sections that contain multiple repeatable entries.

```tsx
// File: src/components/CourseForm.tsx

import { useForm, useFieldArray, useWatch, Control } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';

// Define a single answer option
const answerSchema = z.object({
  text: z.string().min(1, "Answer text required"),
  isCorrect: z.boolean()
});

// Define a question (contains array of answers)
const questionSchema = z.object({
  id: z.string(),
  questionText: z.string().min(1, "Question required"),
  // Nested array of answers
  answers: z.array(answerSchema).min(2, "At least 2 answers required"),
  points: z.coerce.number().min(1).default(1)
});

// Full course schema with nested arrays
const courseSchema = z.object({
  title: z.string().min(1, "Course title required"),
  // Array of sections
  sections: z.array(z.object({
    title: z.string().min(1, "Section title required"),
    // Each section has questions, and each question has answers
    questions: z.array(questionSchema)
  }))
});

type CourseData = z.infer<typeof courseSchema>;

function CourseForm() {
  const { 
    register, 
    handleSubmit, 
    control,
    formState: { errors } 
  } = useForm<CourseData>({
    resolver: zodResolver(courseSchema),
    defaultValues: {
      title: "",
      sections: [
        {
          title: "Section 1",
          questions: [
            { 
              id: crypto.randomUUID(), 
              questionText: "", 
              answers: [
                { text: "", isCorrect: false },
                { text: "", isCorrect: true }
              ],
              points: 1
            }
          ]
        }
      ]
    }
  });

  // Access the nested field arrays
  const { 
    fields: sectionFields, 
    append: appendSection, 
    remove: removeSection 
  } = useFieldArray({
    control,
    name: "sections"
  });

  return (
    <form onSubmit={handleSubmit(console.log)}>
      <h2>Create Course</h2>

      {/* Course Title */}
      <div className="field">
        <label>Course Title</label>
        <input {...register("title")} />
        {errors.title && <span>{errors.title.message}</span>}
      </div>

      {/* Sections */}
      {sectionFields.map((section, sectionIndex) => (
        <div key={section.id} className="section">
          <h3>Section {sectionIndex + 1}</h3>
          
          <div className="field">
            <label>Section Title</label>
            <input 
              {...register(`sections.${sectionIndex}.title` as const)} 
            />
            {errors.sections?.[sectionIndex]?.title && (
              <span>{errors.sections[sectionIndex]?.title?.message}</span>
            )}
          </div>

          {/* Nested Questions Array */}
          <QuestionsFieldArray 
            sectionIndex={sectionIndex}
            control={control}
            register={register}
            errors={errors}
          />

          <button
            type="button"
            onClick={() => appendSection({
              title: `Section ${sectionFields.length + 1}`,
              questions: []
            })}
          >
            Add Section
          </button>

          {sectionFields.length > 1 && (
            <button type="button" onClick={() => removeSection(sectionIndex)}>
              Remove Section
            </button>
          )}
        </div>
      ))}

      <button type="submit">Save Course</button>
    </form>
  );
}

// Separate component for nested questions array
// This keeps the code cleaner and is a React best practice
function QuestionsFieldArray({ 
  sectionIndex, 
  control, 
  register,
  errors 
}: { 
  sectionIndex: number;
  control: Control<CourseData>;
  register: any;
  errors: any;
}) {
  const { fields, append, remove } = useFieldArray({
    control,
    name: `sections.${sectionIndex}.questions` as const
  });

  return (
    <div className="questions">
      {fields.map((question, questionIndex) => (
        <div key={question.id} className="question">
          <div className="field">
            <label>Question {questionIndex + 1}</label>
            <input
              {...register(
                `sections.${sectionIndex}.questions.${questionIndex}.questionText` as const
              )}
            />
          </div>

          {/* Answers for this question */}
          <AnswersFieldArray
            sectionIndex={sectionIndex}
            questionIndex={questionIndex}
            control={control}
            register={register}
            errors={errors}
          />

          <button type="button" onClick={() => remove(questionIndex)}>
            Remove Question
          </button>
        </div>
      ))}

      <button
        type="button"
        onClick={() => append({
          id: crypto.randomUUID(),
          questionText: "",
          answers: [
            { text: "", isCorrect: false },
            { text: "", isCorrect: true }
          ],
          points: 1
        })}
      >
        Add Question
      </button>
    </div>
  );
}

// Third-level nested array for answers
function AnswersFieldArray({
  sectionIndex,
  questionIndex,
  control,
  register,
  errors
}: {
  sectionIndex: number;
  questionIndex: number;
  control: Control<CourseData>;
  register: any;
  errors: any;
}) {
  const { fields, append, remove } = useFieldArray({
    control,
    name: `sections.${sectionIndex}.questions.${questionIndex}.answers` as const
  });

  return (
    <div className="answers">
      {fields.map((answer, answerIndex) => (
        <div key={answer.id} className="answer">
          <input
            {...register(
              `sections.${sectionIndex}.questions.${questionIndex}.answers.${answerIndex}.text` as const
            )}
            placeholder={`Answer ${answerIndex + 1}`}
          />
          <label>
            <input
              type="checkbox"
              {...register(
                `sections.${sectionIndex}.questions.${questionIndex}.answers.${answerIndex}.isCorrect` as const
              )}
            />
            Correct
          </label>
          {fields.length > 2 && (
            <button type="button" onClick={() => remove(answerIndex)}>
              ×
            </button>
          )}
        </div>
      ))}

      <button
        type="button"
        onClick={() => append({ text: "", isCorrect: false })}
      >
        Add Answer
      </button>
    </div>
  );
}

export default CourseForm;
```

### Reordering Fields

The swap and move functions allow users to reorder dynamic fields, useful for priority lists, ranked items, or playlist management.

```tsx
// File: src/components/PrioritizedList.tsx

import { useForm, useFieldArray } from 'react-hook-form';

interface PriorityItem {
  id: string;
  title: string;
  priority: number;
}

function PrioritizedList() {
  const { register, handleSubmit, control, formState: { errors } } = useForm<{ items: PriorityItem[] }>({
    defaultValues: {
      items: [
        { id: crypto.randomUUID(), title: "First priority", priority: 1 },
        { id: crypto.randomUUID(), title: "Second priority", priority: 2 },
        { id: crypto.randomUUID(), title: "Third priority", priority: 3 }
      ]
    }
  });

  const { fields, swap, move, remove, append } = useFieldArray({
    control,
    name: "items"
  });

  const onSubmit = (data: { items: PriorityItem[] }) => {
    // Update priorities based on current order
    const reorderedItems = data.items.map((item, index) => ({
      ...item,
      priority: index + 1
    }));
    console.log("Reordered:", reorderedItems);
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <h2>Priority List</h2>
      <p>Drag and drop or use buttons to reorder items</p>

      {fields.map((field, index) => (
        <div key={field.id} className="priority-item">
          {/* Priority number */}
          <span className="priority-number">{index + 1}</span>
          
          {/* Item title */}
          <input
            {...register(`items.${index}.title` as const)}
            className="item-title"
          />

          {/* Move up button - disabled for first item */}
          <button
            type="button"
            onClick={() => swap(index, index - 1)}
            disabled={index === 0}
            title="Move up"
          >
            ↑
          </button>

          {/* Move down button - disabled for last item */}
          <button
            type="button"
            onClick={() => swap(index, index + 1)}
            disabled={index === fields.length - 1}
            title="Move down"
          >
            ↓
          </button>

          {/* Move to specific position */}
          <button
            type="button"
            onClick={() => move(index, 0)}
            disabled={index === 0}
            title="Move to top"
          >
            ↟
          </button>

          {/* Remove button */}
          <button
            type="button"
            onClick={() => remove(index)}
            disabled={fields.length <= 1}
          >
            ×
          </button>
        </div>
      ))}

      <button
        type="button"
        onClick={() => append({
          id: crypto.randomUUID(),
          title: "",
          priority: fields.length + 1
        })}
      >
        Add Item
      </button>

      <button type="submit">Save Priority Order</button>
    </form>
  );
}

export default PrioritizedList;
```

## Common Mistakes

### Mistake 1: Not Using the Key Properly

When mapping field arrays, always use the field.id as the key. Using index as a key can cause issues when removing items from the middle of the array.

```tsx
// ❌ WRONG - Using index as key causes bugs when removing items
fields.map((field, index) => (
  <div key={index}>...</div>
))

// ✅ CORRECT - Always use field.id as the key
fields.map((field, index) => (
  <div key={field.id}>...</div>
))
```

### Mistake 2: Forgetting control Prop

The useFieldArray hook requires the control prop from useForm. Forgetting it will cause the hook to not work properly.

```tsx
// ❌ WRONG - Missing control prop
const { fields, append, remove } = useFieldArray({
  name: "items"
  // control is required!
});

// ✅ CORRECT - Always pass control
const { control } = useForm();
const { fields, append, remove } = useFieldArray({
  control,
  name: "items"
});
```

### Mistake 3: Not Using as const for Dynamic Paths

When registering nested fields in field arrays, TypeScript needs the `as const` assertion to properly infer the path type.

```tsx
// ❌ WRONG - Path type may not be inferred correctly
<input {...register(`items.${index}.name`)} />

// ✅ CORRECT - Use as const for proper type inference
<input {...register(`items.${index}.name` as const)} />
```

## Real-World Example

Here's a complete project management form where users can create projects with multiple tasks, assignees, and milestones:

```tsx
// File: src/schemas/projectSchema.ts

import { z } from 'zod';

// Assignee schema
const assigneeSchema = z.object({
  id: z.string(),
  name: z.string().min(1, "Name required"),
  email: z.string().email("Valid email required")
});

// Milestone schema
const milestoneSchema = z.object({
  id: z.string(),
  title: z.string().min(1, "Milestone title required"),
  dueDate: z.string().min(1, "Due date required")
});

// Task with assignees
const taskSchema = z.object({
  id: z.string(),
  title: z.string().min(1, "Task title required"),
  description: z.string().optional(),
  status: z.enum(['todo', 'in_progress', 'done']),
  assignees: z.array(assigneeSchema),
  estimatedHours: z.coerce.number().min(0).optional()
});

// Project schema
export const projectSchema = z.object({
  name: z.string().min(1, "Project name required"),
  description: z.string().optional(),
  startDate: z.string().min(1, "Start date required"),
  endDate: z.string().min(1, "End date required"),
  tasks: z.array(taskSchema).min(1, "At least one task required"),
  milestones: z.array(milestoneSchema)
}).refine(data => new Date(data.endDate) > new Date(data.startDate), {
  message: "End date must be after start date",
  path: ["endDate"]
});

export type ProjectFormData = z.infer<typeof projectSchema>;

// File: src/components/ProjectForm.tsx

import { useForm, useFieldArray } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { projectSchema, type ProjectFormData } from '../schemas/projectSchema';

// Initial task with assignees
const createInitialTask = () => ({
  id: crypto.randomUUID(),
  title: "",
  description: "",
  status: 'todo' as const,
  assignees: [{ id: crypto.randomUUID(), name: "", email: "" }],
  estimatedHours: 1
});

function ProjectForm() {
  const { 
    register, 
    handleSubmit, 
    control,
    formState: { errors, isSubmitting } 
  } = useForm<ProjectFormData>({
    resolver: zodResolver(projectSchema),
    defaultValues: {
      name: "",
      description: "",
      startDate: new Date().toISOString().split('T')[0],
      tasks: [createInitialTask()],
      milestones: []
    }
  });

  // Tasks field array
  const { 
    fields: taskFields, 
    append: appendTask, 
    remove: removeTask 
  } = useFieldArray({
    control,
    name: "tasks"
  });

  // Milestones field array
  const { 
    fields: milestoneFields, 
    append: appendMilestone, 
    remove: removeMilestone 
  } = useFieldArray({
    control,
    name: "milestones"
  });

  const onSubmit = async (data: ProjectFormData) => {
    console.log("Project data:", data);
    await new Promise(r => setTimeout(r, 1500));
    alert("Project created successfully!");
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="project-form">
      <h1>Create New Project</h1>

      {/* Project Basics */}
      <section className="form-section">
        <h2>Project Details</h2>
        
        <div className="field">
          <label>Project Name</label>
          <input {...register("name")} placeholder="Enter project name" />
          {errors.name && <span className="error">{errors.name.message}</span>}
        </div>

        <div className="field">
          <label>Description</label>
          <textarea {...register("description")} placeholder="Project description" />
        </div>

        <div className="field-row">
          <div className="field">
            <label>Start Date</label>
            <input type="date" {...register("startDate")} />
            {errors.startDate && <span className="error">{errors.startDate.message}</span>}
          </div>

          <div className="field">
            <label>End Date</label>
            <input type="date" {...register("endDate")} />
            {errors.endDate && <span className="error">{errors.endDate.message}</span>}
          </div>
        </div>
      </section>

      {/* Tasks Section */}
      <section className="form-section">
        <h2>Tasks</h2>
        
        {taskFields.map((task, taskIndex) => (
          <TaskCard
            key={task.id}
            taskIndex={taskIndex}
            register={register}
            errors={errors}
            onRemove={() => removeTask(taskIndex)}
          />
        ))}

        <button
          type="button"
          className="add-button"
          onClick={() => appendTask(createInitialTask())}
        >
          + Add Task
        </button>

        {errors.tasks && (
          <p className="error">{errors.tasks.message}</p>
        )}
      </section>

      {/* Milestones Section */}
      <section className="form-section">
        <h2>Milestones</h2>
        
        {milestoneFields.map((milestone, index) => (
          <div key={milestone.id} className="milestone-card">
            <input
              {...register(`milestones.${index}.title` as const)}
              placeholder="Milestone title"
            />
            <input
              type="date"
              {...register(`milestones.${index}.dueDate` as const)}
            />
            <button type="button" onClick={() => removeMilestone(index)}>
              Remove
            </button>
            {errors.milestones?.[index] && (
              <span className="error">
                {errors.milestones[index]?.title?.message || 
                 errors.milestones[index]?.dueDate?.message}
              </span>
            )}
          </div>
        ))}

        <button
          type="button"
          className="add-button"
          onClick={() => appendMilestone({
            id: crypto.randomUUID(),
            title: "",
            dueDate: ""
          })}
        >
          + Add Milestone
        </button>
      </section>

      <button type="submit" disabled={isSubmitting} className="submit-button">
        {isSubmitting ? "Creating Project..." : "Create Project"}
      </button>
    </form>
  );
}

// Task card with nested assignee array
function TaskCard({ 
  taskIndex, 
  register, 
  errors, 
  onRemove 
}: { 
  taskIndex: number;
  register: any;
  errors: any;
  onRemove: () => void;
}) {
  const { 
    fields: assigneeFields, 
    append: appendAssignee, 
    remove: removeAssignee 
  } = useFieldArray({
    control,
    name: `tasks.${taskIndex}.assignees` as const
  });

  return (
    <div className="task-card">
      <div className="task-header">
        <h3>Task {taskIndex + 1}</h3>
        <button type="button" onClick={onRemove}>Remove Task</button>
      </div>

      <div className="field">
        <label>Task Title</label>
        <input
          {...register(`tasks.${taskIndex}.title` as const)}
          placeholder="What needs to be done?"
        />
        {errors.tasks?.[taskIndex]?.title && (
          <span className="error">{errors.tasks[taskIndex]?.title?.message}</span>
        )}
      </div>

      <div className="field">
        <label>Description</label>
        <textarea
          {...register(`tasks.${taskIndex}.description` as const)}
          placeholder="Task details"
        />
      </div>

      <div className="field">
        <label>Status</label>
        <select {...register(`tasks.${taskIndex}.status` as const)}>
          <option value="todo">To Do</option>
          <option value="in_progress">In Progress</option>
          <option value="done">Done</option>
        </select>
      </div>

      <div className="field">
        <label>Estimated Hours</label>
        <input
          type="number"
          min="0"
          step="0.5"
          {...register(`tasks.${taskIndex}.estimatedHours` as const)}
        />
      </div>

      {/* Assignees sub-section */}
      <div className="assignees-section">
        <h4>Assignees</h4>
        
        {assigneeFields.map((assignee, assigneeIndex) => (
          <div key={assignee.id} className="assignee-row">
            <input
              {...register(
                `tasks.${taskIndex}.assignees.${assigneeIndex}.name` as const
              )}
              placeholder="Name"
            />
            <input
              {...register(
                `tasks.${taskIndex}.assignees.${assigneeIndex}.email` as const
              )}
              placeholder="Email"
              type="email"
            />
            {assigneeFields.length > 1 && (
              <button
                type="button"
                onClick={() => removeAssignee(assigneeIndex)}
              >
                ×
              </button>
            )}
          </div>
        ))}

        <button
          type="button"
          onClick={() => appendAssignee({
            id: crypto.randomUUID(),
            name: "",
            email: ""
          })}
        >
          + Add Assignee
        </button>
      </div>
    </div>
  );
}

export default ProjectForm;
```

## Key Takeaways

- useFieldArray manages collections of fields that can grow or shrink at runtime
- Always use field.id as the key when mapping—never use index
- The control prop from useForm must be passed to useFieldArray
- Use watch with field arrays to calculate derived values like totals in real-time
- Nested field arrays are supported—create separate components for each level to keep code organized
- The swap, move, and remove functions allow full control over field reordering
- Use Zod with .array() to validate field arrays with minimum/maximum lengths
- Always add unique IDs to dynamic items for proper React key management

## What's Next

Continue to [File Upload Handling](/07-forms/03-form-patterns/01-file-upload-handling.md) to learn how to handle file uploads in React Hook Form, including drag-and-drop interfaces and upload progress tracking.