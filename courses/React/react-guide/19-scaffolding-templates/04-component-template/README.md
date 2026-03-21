# Component Folder Template

## Overview
The component folder pattern is a best practice for organizing React components. Each component gets its own folder with all related files—implementation, tests, stories, and types—co-located for maintainability and scalability.

## Folder Structure

Every component follows this pattern:

```
ComponentName/
├── ComponentName.tsx        # Main implementation
├── ComponentName.test.tsx   # Vitest + RTL tests
├── ComponentName.stories.tsx # Storybook stories
├── ComponentName.module.css # CSS (if using CSS modules)
├── types.ts                 # TypeScript types (if needed)
├── hooks.ts                 # Component-local hooks (if needed)
└── index.ts                # Re-exports (public API)
```

## Complete Example: DataTable Component

Here's a fully implemented DataTable component following this pattern:

### types.ts

```typescript
// [File: DataTable/types.ts]
/**
 * Column definition for the DataTable.
 * Generic type T allows type-safe row data.
 */
export interface DataTableColumn<T> {
  /** Unique identifier for the column */
  key: keyof T;
  /** Display header text */
  header: string;
  /** Optional render function for custom cell content */
  render?: (row: T, index: number) => React.ReactNode;
  /** CSS className for the cell */
  className?: string;
  /** Whether this column is sortable */
  sortable?: boolean;
  /** Width of the column */
  width?: string;
}

/** Props for the DataTable component */
export interface DataTableProps<T> {
  /** Array of column definitions */
  columns: DataTableColumn<T>[];
  /** Array of row data */
  data: T[];
  /** Whether to show loading state */
  loading?: boolean;
  /** Empty state message */
  emptyMessage?: string;
  /** CSS className for the table */
  className?: string;
  /** Callback when row is clicked */
  onRowClick?: (row: T, index: number) => void;
  /** Unique key for each row */
  rowKey: keyof T;
}

/** Sort configuration */
export interface SortConfig<T> {
  key: keyof T;
  direction: 'asc' | 'desc';
}
```

### hooks.ts

```typescript
// [File: DataTable/hooks.ts]
import { useState, useMemo, useCallback } from 'react';
import type { SortConfig, DataTableColumn } from './types';

/**
 * Custom hook for DataTable sorting logic.
 * Separates sorting logic from presentation for testability.
 */
export function useDataTableSort<T>(data: T[], columns: DataTableColumn<T>[]) {
  const [sortConfig, setSortConfig] = useState<SortConfig<T> | null>(null);

  const sortedData = useMemo(() => {
    if (!sortConfig) return data;

    return [...data].sort((a, b) => {
      const aValue = a[sortConfig.key];
      const bValue = b[sortConfig.key];

      if (aValue === bValue) return 0;

      const direction = sortConfig.direction === 'asc' ? 1 : -1;
      return aValue > bValue ? direction : -direction;
    });
  }, [data, sortConfig]);

  const handleSort = useCallback((key: keyof T) => {
    setSortConfig((current) => {
      if (!current || current.key !== key) {
        return { key, direction: 'asc' };
      }
      if (current.direction === 'asc') {
        return { key, direction: 'desc' };
      }
      return null;
    });
  }, []);

  const getSortDirection = useCallback(
    (key: keyof T) => {
      if (!sortConfig || sortConfig.key !== key) return null;
      return sortConfig.direction;
    },
    [sortConfig]
  );

  return {
    sortedData,
    sortConfig,
    handleSort,
    getSortDirection,
  };
}
```

### ComponentName.tsx

```tsx
// [File: DataTable/DataTable.tsx]
import React from 'react';
import clsx from 'clsx';
import { useDataTableSort } from './hooks';
import type { DataTableProps, DataTableColumn } from './types';
import styles from './DataTable.module.css';

/**
 * A generic, reusable data table component.
 * Supports sorting, loading states, and custom cell rendering.
 * 
 * @example
 * ```tsx
 * const columns = [
 *   { key: 'name', header: 'Name', sortable: true },
 *   { key: 'email', header: 'Email' },
 * ] as const;
 * 
 * <DataTable columns={columns} data={users} rowKey="id" />
 * ```
 */
export function DataTable<T extends Record<string, unknown>>({
  columns,
  data,
  loading = false,
  emptyMessage = 'No data available',
  className,
  onRowClick,
  rowKey,
}: DataTableProps<T>) {
  const { sortedData, handleSort, getSortDirection } = useDataTableSort(data, columns);

  if (loading) {
    return (
      <div className={styles.loading}>
        <span>Loading...</span>
      </div>
    );
  }

  if (data.length === 0) {
    return (
      <div className={styles.empty}>
        {emptyMessage}
      </div>
    );
  }

  return (
    <div className={clsx(styles.container, className)}>
      <table className={styles.table}>
        <thead>
          <tr>
            {columns.map((column) => (
              <th
                key={String(column.key)}
                className={clsx(
                  styles.th,
                  column.sortable && styles.sortable,
                  getSortDirection(column.key) && styles.sorted
                )}
                style={{ width: column.width }}
                onClick={() => column.sortable && handleSort(column.key)}
              >
                <span className={styles.headerContent}>
                  {column.header}
                  {column.sortable && (
                    <span className={styles.sortIcon}>
                      {getSortDirection(column.key) === 'asc' && '↑'}
                      {getSortDirection(column.key) === 'desc' && '↓'}
                    </span>
                  )}
                </span>
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {sortedData.map((row, rowIndex) => (
            <tr
              key={String(row[rowKey])}
              className={clsx(
                styles.row,
                onRowClick && styles.clickable
              )}
              onClick={() => onRowClick?.(row, rowIndex)}
            >
              {columns.map((column) => (
                <td key={String(column.key)} className={clsx(styles.td, column.className)}>
                  {column.render
                    ? column.render(row, rowIndex)
                    : String(row[column.key])}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
```

### ComponentName.module.css

```css
/* [File: DataTable/DataTable.module.css] */
.container {
  width: 100%;
  overflow-x: auto;
}

.table {
  width: 100%;
  border-collapse: collapse;
}

.th {
  text-align: left;
  padding: 12px 16px;
  border-bottom: 2px solid #e5e7eb;
  font-weight: 600;
  color: #374151;
  background-color: #f9fafb;
}

.th.sortable {
  cursor: pointer;
  user-select: none;
}

.th.sortable:hover {
  background-color: #f3f4f6;
}

.headerContent {
  display: flex;
  align-items: center;
  gap: 4px;
}

.sortIcon {
  font-size: 12px;
}

.td {
  padding: 12px 16px;
  border-bottom: 1px solid #e5e7eb;
}

.row:hover {
  background-color: #f9fafb;
}

.row.clickable {
  cursor: pointer;
}

.loading,
.empty {
  padding: 48px;
  text-align: center;
  color: #6b7280;
}
```

### index.ts

```typescript
// [File: DataTable/index.ts]
/**
 * Public API for the DataTable component.
 * Only export what's intended for external use.
 */
export { DataTable } from './DataTable';
export type { 
  DataTableProps, 
  DataTableColumn,
  SortConfig 
} from './types';
```

### ComponentName.test.tsx

```tsx
// [File: DataTable/DataTable.test.tsx]
import { render, screen, fireEvent } from '@testing-library/react';
import { describe, it, expect, vi } from 'vitest';
import { DataTable } from './DataTable';

interface User {
  id: number;
  name: string;
  email: string;
}

const mockData: User[] = [
  { id: 1, name: 'John Doe', email: 'john@example.com' },
  { id: 2, name: 'Jane Smith', email: 'jane@example.com' },
];

const columns = [
  { key: 'name' as keyof User, header: 'Name', sortable: true },
  { key: 'email' as keyof User, header: 'Email' },
] as const;

describe('DataTable', () => {
  it('renders table with data', () => {
    render(
      <DataTable 
        columns={columns} 
        data={mockData} 
        rowKey="id" 
      />
    );

    expect(screen.getByText('John Doe')).toBeInTheDocument();
    expect(screen.getByText('jane@example.com')).toBeInTheDocument();
  });

  it('renders empty state when no data', () => {
    render(
      <DataTable 
        columns={columns} 
        data={[]} 
        rowKey="id"
        emptyMessage="No users found"
      />
    );

    expect(screen.getByText('No users found')).toBeInTheDocument();
  });

  it('renders loading state', () => {
    render(
      <DataTable 
        columns={columns} 
        data={[]} 
        rowKey="id"
        loading={true}
      />
    );

    expect(screen.getByText('Loading...')).toBeInTheDocument();
  });

  it('calls onRowClick when row is clicked', () => {
    const handleClick = vi.fn();
    
    render(
      <DataTable 
        columns={columns} 
        data={mockData} 
        rowKey="id"
        onRowClick={handleClick}
      />
    );

    fireEvent.click(screen.getByText('John Doe'));
    expect(handleClick).toHaveBeenCalledWith(mockData[0], 0);
  });

  it('sorts data when sortable column is clicked', () => {
    render(
      <DataTable 
        columns={columns} 
        data={mockData} 
        rowKey="id"
      />
    );

    // Click header to sort
    fireEvent.click(screen.getByText('Name'));
    
    // First row should be Jane (alphabetically first when ascending)
    const rows = screen.getAllByRole('row');
    expect(rows[1]).toHaveTextContent('Jane Smith');
  });
});
```

### ComponentName.stories.tsx

```tsx
// [File: DataTable/DataTable.stories.tsx]
import type { Meta, StoryObj } from '@storybook/react';
import { DataTable } from './DataTable';
import type { DataTableColumn } from './types';

interface User {
  id: number;
  name: string;
  email: string;
  role: string;
  status: 'active' | 'inactive';
}

const meta: Meta<typeof DataTable<User>> = {
  title: 'Components/DataTable',
  component: DataTable,
  parameters: {
    layout: 'padded',
  },
};

export default meta;
type Story = StoryObj<typeof DataTable<User>>;

const columns: DataTableColumn<User>[] = [
  { key: 'name', header: 'Name', sortable: true },
  { key: 'email', header: 'Email', sortable: true },
  { 
    key: 'role', 
    header: 'Role',
    render: (row) => (
      <span className="badge">{row.role}</span>
    )
  },
  {
    key: 'status',
    header: 'Status',
    render: (row) => (
      <span style={{ color: row.status === 'active' ? 'green' : 'red' }}>
        {row.status}
      </span>
    ),
  },
];

const data: User[] = [
  { id: 1, name: 'John Doe', email: 'john@example.com', role: 'Admin', status: 'active' },
  { id: 2, name: 'Jane Smith', email: 'jane@example.com', role: 'User', status: 'inactive' },
  { id: 3, name: 'Bob Wilson', email: 'bob@example.com', role: 'User', status: 'active' },
];

export const Default: Story = {
  args: {
    columns,
    data,
    rowKey: 'id',
  },
};

export const Loading: Story = {
  args: {
    columns,
    data: [],
    rowKey: 'id',
    loading: true,
  },
};

export const Empty: Story = {
  args: {
    columns,
    data: [],
    rowKey: 'id',
    emptyMessage: 'No users found',
  },
};

export const WithRowClick: Story = {
  args: {
    columns,
    data,
    rowKey: 'id',
    onRowClick: (row) => console.log('Clicked:', row),
  },
};
```

## VS Code Snippet

Create this snippet in VS Code to scaffold components quickly:

```json
{
  "Component Folder": {
    "prefix": "compfolder",
    "body": [
      "import React from 'react';",
      "import styles from './${1:ComponentName}.module.css';",
      "",
      "interface ${1:ComponentName}Props {",
      "  $2",
      "}",
      "",
      "export function ${1:ComponentName}({ $3 }: ${1:ComponentName}Props) {",
      "  return (",
      "    <div className={styles.$4}>",
      "      $5",
      "    </div>",
      "  );",
      "}",
      ""
    ],
    "description": "Scaffold a component folder"
  }
}
```

## Why This Scales

1. **Colocation** — All related files stay together
2. **Testability** — Tests are next to implementation
3. **Discoverability** — Easy to find component files
4. **Clear API** — index.ts defines public exports
5. **Isolation** — Component-specific hooks and types don't leak

## Next Steps

1. Create your first component folder using this pattern
2. Add Storybook stories for visual testing
3. Write tests alongside implementation

For more details, see:
- [Storybook Setup](../../18-ecosystem/01-development-tools/01-storybook-setup.md)
- [Testing Components with RTL](../../10-testing/01-unit-testing/02-testing-components-with-rtl.md)
