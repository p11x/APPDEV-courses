# Generics in React

## Overview
Generics allow you to create reusable components and functions that work with any data type while maintaining full type safety. In React, generics are incredibly powerful for building flexible components like data tables, lists, forms, and custom hooks. Instead of writing separate code for each data type, you write it once with generics, and TypeScript ensures type safety at compile time. This guide covers generic components, hooks, and constrained generics with practical React examples.

## Prerequisites
- TypeScript setup and configuration
- Understanding of types vs interfaces
- Familiarity with React hooks
- Basic knowledge of TypeScript utility types (Partial, Pick, Omit)

## Core Concepts

### What Are Generics?
Generics let you write code that works with multiple types while preserving type information. Think of them as variables for types:

```typescript
// [File: src/examples/genericBasics.ts]

// Without generics - we'd need separate functions for each type
function getString(arr: string[]): string[] {
  return arr;
}

function getNumber(arr: number[]): number[] {
  return arr;
}

// With generics - ONE function works with ANY type
function getFirst<T>(arr: T[]): T | undefined {
  return arr[0];
}

// TypeScript infers the type from usage
const strings = getFirst(['a', 'b', 'c']);    // type: string
const numbers = getFirst([1, 2, 3]);          // type: number
const users = getFirst([{name: 'John'}]);    // type: {name: string}

// You can also explicitly specify the type
const explicit = getFirst<number>([1, 2, 3]);
```

### Generic Components
Generic components can render any type of data while maintaining full type safety:

```typescript
// [File: src/components/GenericList.tsx]
import React from 'react';

// Define what a ListItem looks like - can be anything
interface ListItem {
  id: string | number;
  [key: string]: any; // Allow any additional properties
}

// Generic props for a reusable list component
interface GenericListProps<T> {
  // The data to render - an array of any type T
  items: T[];
  
  // Function to render each item - receives item and index
  // Returns React.ReactNode (JSX)
  renderItem: (item: T, index: number) => React.ReactNode;
  
  // Optional key extractor - helps React with reconciliation
  getKey?: (item: T) => string | number;
  
  // Optional empty state
  emptyMessage?: string;
}

// Generic component - T is determined by the items array type
function GenericList<T>({
  items,
  renderItem,
  getKey,
  emptyMessage = 'No items to display',
}: GenericListProps<T>) {
  if (items.length === 0) {
    return <div className="empty-state">{emptyMessage}</div>;
  }

  return (
    <ul className="generic-list">
      {items.map((item, index) => {
        // Use provided key function or fall back to index
        const key = getKey ? getKey(item) : (item.id ?? index);
        
        return (
          <li key={key}>
            {renderItem(item, index)}
          </li>
        );
      })}
    </ul>
  );
}

// Usage with different data types:

// Example 1: User list
interface User {
  id: number;
  name: string;
  email: string;
}

const users: User[] = [
  { id: 1, name: 'Alice', email: 'alice@example.com' },
  { id: 2, name: 'Bob', email: 'bob@example.com' },
];

function UserList() {
  return (
    <GenericList
      items={users}
      getKey={(user) => user.id}
      renderItem={(user) => (
        <div>
          <strong>{user.name}</strong>
          <span>{user.email}</span>
        </div>
      )}
    />
  );
}

// Example 2: Product list
interface Product {
  sku: string;
  name: string;
  price: number;
  inStock: boolean;
}

const products: Product[] = [
  { sku: 'A001', name: 'Laptop', price: 999, inStock: true },
  { sku: 'A002', name: 'Mouse', price: 29, inStock: false },
];

function ProductList() {
  return (
    <GenericList
      items={products}
      getKey={(product) => product.sku}
      renderItem={(product) => (
        <div className={product.inStock ? 'available' : 'out-of-stock'}>
          {product.name} - ${product.price}
        </div>
      )}
    />
  );
}

export default GenericList;
```

### Generic Hooks
Custom hooks often benefit from generics to work with any data type:

```typescript
// [File: src/hooks/useFetch.ts]
import { useState, useEffect } from 'react';

// Generic hook for fetching data - works with any API response type
interface FetchState<T> {
  data: T | null;
  loading: boolean;
  error: Error | null;
}

function useFetch<T>(url: string): FetchState<T> & { refetch: () => void } {
  // State typed with generic T
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  const fetchData = async () => {
    setLoading(true);
    setError(null);
    
    try {
      const response = await fetch(url);
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      // Parse JSON - type assertion needed since we don't know T
      const json = await response.json() as T;
      setData(json);
    } catch (e) {
      setError(e instanceof Error ? e : new Error('Unknown error'));
    } finally {
      setLoading(false);
    }
  };

  // Fetch on mount and when URL changes
  useEffect(() => {
    fetchData();
  }, [url]);

  // Return refetch function
  return { data, loading, error, refetch: fetchData };
}

// Usage:
interface User {
  id: number;
  name: string;
}

function UserProfile({ userId }: { userId: number }) {
  // TypeScript infers T from the API response
  const { data: user, loading, error } = useFetch<User>(
    `/api/users/${userId}`
  );

  if (loading) return <Spinner />;
  if (error) return <Error error={error} />;
  
  return (
    <div>
      <h1>{user?.name}</h1>
    </div>
  );
}
```

### Constrained Generics
Sometimes you need to restrict generics to certain types - this is called "constraining" the generic:

```typescript
// [File: src/components/GenericForm.tsx]

// Constrain T to only objects that have an id
interface WithId {
  id: string | number;
}

// Props requiring items with an ID
interface FormSelectProps<T extends WithId> {
  items: T[];
  label?: string;
  onSelect: (item: T) => void;
  getLabel?: (item: T) => string;
}

// Now T MUST have an id property
function FormSelect<T extends WithId>({
  items,
  label,
  onSelect,
  getLabel = (item) => String(item.id),
}: FormSelectProps<T>) {
  return (
    <div className="form-select">
      {label && <label>{label}</label>}
      <select onChange={(e) => {
        const item = items.find(i => i.id === e.target.value);
        if (item) onSelect(item);
      }}>
        {items.map(item => (
          <option key={item.id} value={item.id}>
            {getLabel(item)}
          </option>
        ))}
      </select>
    </div>
  );
}

// This works:
interface User { id: number; name: string; }
const users: User[] = [{ id: 1, name: 'Alice' }];
<FormSelect items={users} onSelect={(u) => console.log(u.name)} />

// This would ERROR because Product doesn't have id:
// interface Product { sku: string; name: string; }
// const products: Product[] = [{ sku: 'A1', name: 'Thing' }];
// <FormSelect items={products} /> // Error!
```

### Generic Data Table Component
Here's a more complex example combining everything:

```typescript
// [File: src/components/DataTable.tsx]
import React from 'react';

// Column definition - defines how to display each field
interface Column<T> {
  // Which property of T to display
  key: keyof T;
  // Column header text
  header: string;
  // Optional custom render function
  render?: (value: T[keyof T], row: T) => React.ReactNode;
  // Whether this column is sortable
  sortable?: boolean;
}

// DataTable props - generic over the data type T
interface DataTableProps<T> {
  // Array of data to display
  data: T[];
  // Column definitions
  columns: Column<T>[];
  // Optional sort state
  sortColumn?: keyof T;
  sortDirection?: 'asc' | 'desc';
  // Optional sort handler
  onSort?: (column: keyof T) => void;
  // Loading state
  loading?: boolean;
  // Empty state message
  emptyMessage?: string;
}

// Generic DataTable component
function DataTable<T extends object>({
  data,
  columns,
  sortColumn,
  sortDirection,
  onSort,
  loading = false,
  emptyMessage = 'No data available',
}: DataTableProps<T>) {
  if (loading) {
    return <div className="loading">Loading...</div>;
  }

  if (data.length === 0) {
    return <div className="empty">{emptyMessage}</div>;
  }

  return (
    <table className="data-table">
      <thead>
        <tr>
          {columns.map(column => (
            <th
              key={String(column.key)}
              onClick={() => column.sortable && onSort?.(column.key)}
              className={column.sortable ? 'sortable' : ''}
            >
              {column.header}
              {sortColumn === column.key && (
                <span>{sortDirection === 'asc' ? ' ↑' : ' ↓'}</span>
              )}
            </th>
          ))}
        </tr>
      </thead>
      <tbody>
        {data.map((row, index) => (
          <tr key={index}>
            {columns.map(column => (
              <td key={String(column.key)}>
                {column.render 
                  ? column.render(row[column.key], row)
                  : String(row[column.key] ?? '')
                }
              </td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
}

// ======== USAGE EXAMPLE ========

interface Product {
  id: number;
  name: string;
  category: string;
  price: number;
  inStock: boolean;
}

const productColumns: Column<Product>[] = [
  { key: 'id', header: 'ID', sortable: true },
  { key: 'name', header: 'Product Name', sortable: true },
  { key: 'category', header: 'Category' },
  { 
    key: 'price', 
    header: 'Price', 
    sortable: true,
    render: (value) => `$${Number(value).toFixed(2)}`
  },
  { 
    key: 'inStock', 
    header: 'Stock',
    render: (value) => value ? '✓ In Stock' : '✗ Out of Stock'
  },
];

const products: Product[] = [
  { id: 1, name: 'Laptop', category: 'Electronics', price: 999, inStock: true },
  { id: 2, name: 'Mouse', category: 'Electronics', price: 29, inStock: false },
  { id: 3, name: 'Chair', category: 'Furniture', price: 199, inStock: true },
];

function ProductTable() {
  const [sortColumn, setSortColumn] = React.useState<keyof Product>('id');
  const [sortDirection, setSortDirection] = React.useState<'asc' | 'desc'>('asc');

  const handleSort = (column: keyof Product) => {
    if (sortColumn === column) {
      setSortDirection(d => d === 'asc' ? 'desc' : 'asc');
    } else {
      setSortColumn(column);
      setSortDirection('asc');
    }
  };

  const sortedProducts = [...products].sort((a, b) => {
    const aVal = a[sortColumn];
    const bVal = b[sortColumn];
    const direction = sortDirection === 'asc' ? 1 : -1;
    
    if (aVal < bVal) return -1 * direction;
    if (aVal > bVal) return 1 * direction;
    return 0;
  });

  return (
    <DataTable
      data={sortedProducts}
      columns={productColumns}
      sortColumn={sortColumn}
      sortDirection={sortDirection}
      onSort={handleSort}
    />
  );
}

export default DataTable;
```

## Common Mistakes

### Mistake 1: Not Constraining Generics When Needed
```typescript
// ❌ WRONG - T could be anything, including primitives
function getId<T>(item: T): string {
  return item.id; // Error: Property 'id' does not exist on type T
}

// ✅ CORRECT - Constrain T to objects with an id
function getId<T extends { id: string | number }>(item: T): string {
  return String(item.id); // Works!
}
```

### Mistake 2: Overusing Generics
```typescript
// ❌ WRONG - Generic for something that doesn't need it
function wrapInArray<T>(value: T): T[] {
  return [value];
}

// ✅ CORRECT - Just use the type you need
function wrapInArray(value: string): string[] {
  return [value];
}
// Or if you really need generics:
function wrapInArray<T>(value: T): T[] {
  return [value]; // Only if T will vary at call site
}
```

### Mistake 3: Forgetting Default Type Parameters
```typescript
// ❌ WRONG - Must always provide type
interface ApiResponse<T> {
  data: T;
  status: number;
}

// With default - T defaults to unknown if not provided
interface ApiResponse<T = unknown> {
  data: T;
  status: number;
}

// Now works without specifying type:
const response: ApiResponse = { data: 'ok', status: 200 };
```

## Real-World Example

Here's a complete generic modal component with full typing:

```typescript
// [File: src/components/Modal.tsx]
import React, { useEffect, useRef } from 'react';

// Base modal props - works with any content type
interface BaseModalProps {
  isOpen: boolean;
  onClose: () => void;
  title?: string;
  children: React.ReactNode;
}

// Modal size variants
type ModalSize = 'small' | 'medium' | 'large' | 'fullscreen';

// Props for form modals with submit handling
interface FormModalProps<T> extends BaseModalProps {
  type?: 'form';
  onSubmit: (data: T) => void;
  submitLabel?: string;
  initialData?: Partial<T>;
}

// Props for confirmation modals
interface ConfirmModalProps extends BaseModalProps {
  type: 'confirm';
  onConfirm: () => void;
  confirmLabel?: string;
  cancelLabel?: string;
  variant?: 'danger' | 'warning' | 'info';
}

// Union of all modal types - discriminated by 'type' field
type ModalProps<T> = FormModalProps<T> | ConfirmModalProps;

// Generic Modal component
function Modal<T>(props: ModalProps<T>) {
  const {
    isOpen,
    onClose,
    title,
    children,
    type = 'form',
  } = props;

  // Focus trap - return ref for the modal container
  const modalRef = useRef<HTMLDivElement>(null);

  // Close on escape key
  useEffect(() => {
    const handleEscape = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose();
    };

    if (isOpen) {
      document.addEventListener('keydown', handleEscape);
      // Prevent body scroll when modal is open
      document.body.style.overflow = 'hidden';
    }

    return () => {
      document.removeEventListener('keydown', handleEscape);
      document.body.style.overflow = '';
    };
  }, [isOpen, onClose]);

  if (!isOpen) return null;

  // Determine modal size class
  const getSizeClass = () => {
    switch (props.type === 'confirm' ? 'medium' : 'medium') {
      case 'small': return 'modal-sm';
      case 'large': return 'modal-lg';
      case 'fullscreen': return 'modal-fullscreen';
      default: return 'modal-md';
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div 
        ref={modalRef}
        className={`modal ${getSizeClass()}`}
        onClick={(e) => e.stopPropagation()}
        role="dialog"
        aria-modal="true"
        aria-labelledby={title ? 'modal-title' : undefined}
      >
        {title && (
          <div className="modal-header">
            <h2 id="modal-title">{title}</h2>
            <button 
              onClick={onClose}
              aria-label="Close modal"
              className="modal-close"
            >
              ×
            </button>
          </div>
        )}
        
        <div className="modal-body">
          {children}
        </div>

        {type === 'confirm' && 'onConfirm' in props && (
          <div className="modal-footer">
            <button onClick={onClose}>
              {props.cancelLabel ?? 'Cancel'}
            </button>
            <button
              onClick={props.onConfirm}
              className={`btn btn-${props.variant ?? 'primary'}`}
            >
              {props.confirmLabel ?? 'Confirm'}
            </button>
          </div>
        )}
      </div>
    </div>
  );
}

// ======== Usage Examples ========

// Form modal with generic data type
interface UserFormData {
  name: string;
  email: string;
  role: string;
}

function CreateUserModal() {
  const [isOpen, setIsOpen] = React.useState(false);
  const [formData, setFormData] = React.useState<UserFormData>({
    name: '',
    email: '',
    role: 'user',
  });

  return (
    <Modal
      isOpen={isOpen}
      onClose={() => setIsOpen(false)}
      title="Create User"
      type="form"
      initialData={formData}
      onSubmit={(data) => {
        console.log('Creating user:', data);
        setIsOpen(false);
      }}
      submitLabel="Create"
    >
      <form>
        <input
          value={formData.name}
          onChange={(e) => setFormData(d => ({ ...d, name: e.target.value }))}
        />
        <input
          value={formData.email}
          onChange={(e) => setFormData(d => ({ ...d, email: e.target.value }))}
        />
      </form>
    </Modal>
  );
}

// Confirmation modal - no generic needed
function DeleteConfirmModal() {
  const [isOpen, setIsOpen] = React.useState(false);

  return (
    <Modal
      isOpen={isOpen}
      onClose={() => setIsOpen(false)}
      title="Delete Item"
      type="confirm"
      variant="danger"
      onConfirm={() => {
        console.log('Item deleted');
        setIsOpen(false);
      }}
    >
      <p>Are you sure you want to delete this item?</p>
    </Modal>
  );
}

export default Modal;
```

## Key Takeaways
- Generics allow components and functions to work with any type while maintaining type safety
- Use `<T>` syntax to declare a generic type parameter
- Constrain generics with `extends` to restrict what types are allowed
- Generic hooks like `useFetch<T>` make data fetching reusable
- Generic components like `DataTable<T>` can render any data type
- Default type parameters (`<T = string>`) provide fallback types
- TypeScript infers generic types from usage - no need to specify them manually most of the time

## What's Next
Continue to [Typing Props and State](02-typing-react/01-typing-props-and-state.md) to learn how to properly type React component props, state, and handle discriminated unions for UI states.