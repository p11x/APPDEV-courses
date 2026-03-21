# Types vs Interfaces in React

## Overview
One of the most common questions React developers have when learning TypeScript is: "Should I use `type` or `interface` for defining object shapes?" Both can define object types, but they have different use cases, capabilities, and trade-offs. Understanding when to use each will make your React code more maintainable and your types more powerful. This guide covers all the differences and provides clear recommendations for React applications.

## Prerequisites
- TypeScript basics from the previous guide
- Understanding of React component props
- Familiarity with JavaScript objects and arrays

## Core Concepts

### The Fundamental Difference
Both `type` and `interface` can describe object shapes, but they have different capabilities:

```typescript
// [File: src/examples/typesVsInterfaces.ts]

// INTERFACES - Best for object shapes that might be extended
// ==========================================================

// Define a User interface - perfect for objects with multiple properties
interface User {
  id: string;
  name: string;
  email: string;
}

// Interfaces can be EXTENDED - this is their super power
interface AdminUser extends User {
  role: 'admin';
  permissions: string[];
}

// Interfaces can also be DECLARED MERGING - multiple declarations combine
interface User {
  avatar?: string; // This gets added to the original User interface
}

// TYPES - Best for unions, primitives, and complex type transformations
// =====================================================================

// Type aliases can represent ANY type
type ID = string | number; // Union type - can't do this with interface

type Status = 'pending' | 'active' | 'completed'; // String literal union

type UserOrAdmin = User | AdminUser; // Union of object types

type UserKeys = keyof User; // "id" | "name" | "email"

// Primitives - types win here
type StringOrNumber = string | number;
type Nullish = null | undefined;
```

### When to Use Interface in React
Interfaces are ideal for defining component props, state, and API response shapes that might need extension:

```typescript
// [File: src/components/Button.tsx]

// INTERFACES are perfect for props - they're extendable and readable
interface ButtonProps {
  // Required props
  children: React.ReactNode; // What the button displays
  
  // Optional props - notice the ?
  variant?: 'primary' | 'secondary' | 'danger';
  size?: 'small' | 'medium' | 'large';
  disabled?: boolean;
  
  // Event handlers - type the event parameter!
  onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void;
  
  // Ref for imperative handling
  buttonRef?: React.Ref<HTMLButtonElement>;
  
  // HTML attributes you want to pass through
  type?: 'button' | 'submit' | 'reset';
}

// React.FC (Functional Component) with explicit props typing
// This is the traditional way - still valid but has some quirks
const Button: React.FC<ButtonProps> = ({ 
  children, 
  variant = 'primary',
  size = 'medium',
  disabled = false,
  onClick,
  buttonRef,
  type = 'button',
}) => {
  // Build className based on props
  const className = `btn btn-${variant} btn-${size}`;
  
  return (
    <button
      ref={buttonRef}
      type={type}
      className={className}
      disabled={disabled}
      onClick={onClick}
    >
      {children}
    </button>
  );
};

export default Button;
```

### When to Use Type in React
Use `type` for unions, discriminated unions, and when you need advanced type manipulation:

```typescript
// [File: src/components/AsyncData.tsx]

// TYPE ALIASES are perfect for state machines and discriminated unions
// ====================================================================

// This is a discriminated union - perfect for async data states
type AsyncData<T, E = Error> = 
  | { status: 'idle' }
  | { status: 'loading' }
  | { status: 'success'; data: T }
  | { status: 'error'; error: E };

// Use this pattern for component state
interface UserProfileProps {
  userId: string;
}

function UserProfile({ userId }: UserProfileProps) {
  // State with discriminated union - TypeScript can narrow the type!
  const [userState, setUserState] = React.useState<AsyncData<User>>({
    status: 'idle',
  });

  React.useEffect(() => {
    setUserState({ status: 'loading' });
    
    fetch(`/api/users/${userId}`)
      .then(res => res.json())
      .then(
        (data) => setUserState({ status: 'success', data }),
        (error) => setUserState({ status: 'error', error })
      );
  }, [userId]);

  // TypeScript KNOWS which properties exist based on status!
  // This is called "type narrowing"
  switch (userState.status) {
    case 'idle':
      return <div>Click to load user</div>;
    case 'loading':
      return <Spinner />;
    case 'success':
      // userState.data is guaranteed to exist here
      return <UserCard user={userState.data} />;
    case 'error':
      // userState.error is guaranteed to exist here
      return <ErrorMessage error={userState.error} />;
  }
}
```

### PropsWithChildren Utility
React provides helpful utility types for common patterns:

```typescript
// [File: src/components/Wrapper.tsx]

// Option 1: Manual children prop - explicit and clear
interface WrapperProps {
  children: React.ReactNode;
  className?: string;
  padding?: string;
}

// Option 2: Use PropsWithChildren - built-in utility
import { PropsWithChildren } from 'react';

interface CardProps extends PropsWithChildren {
  title?: string;
  footer?: React.ReactNode;
  elevated?: boolean;
}

// Option 3: ComponentProps pattern - derive from existing components
import { ButtonHTMLAttributes } from 'react';

// This makes IconButton have all Button's props plus custom ones
interface IconButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  icon: React.ReactNode;
  tooltip?: string;
}
```

### Real-World Component Comparison
Here's the same Button component written both ways to show the trade-offs:

```typescript
// [File: src/components/ButtonVariants.tsx]
import React from 'react';

// ======== USING INTERFACE ========
interface ButtonInterfaceProps {
  children: React.ReactNode;
  variant?: 'primary' | 'secondary';
  onClick?: () => void;
}

// Pros: Readable, can be extended with extends
// Cons: Can't easily create union types from it
const ButtonInterface: React.FC<ButtonInterfaceProps> = (props) => {
  return <button onClick={props.onClick}>{props.children}</button>;
};

// ======== USING TYPE ========
type ButtonTypeProps = {
  children: React.ReactNode;
  variant?: 'primary' | 'secondary';
  onClick?: () => void;
};

// Pros: More flexible, works with unions
// Cons: Slightly less readable for simple objects
const ButtonType = (props: ButtonTypeProps) => {
  return <button onClick={props.onClick}>{props.children}</button>;
};

// ======== USING TYPE WITH COMPONENTTYPE ========
// Alternative: Use ComponentType for more flexibility
import { ComponentType } from 'react';

type ButtonPropsV2 = {
  variant?: 'primary' | 'secondary';
};

// This allows passing any component
const withButton = <P extends ButtonPropsV2>(
  Component: ComponentType<P>
): ComponentType<P> => {
  return (props) => (
    <div className="button-wrapper">
      <Component {...props} />
    </div>
  );
};
```

## Common Mistakes

### Mistake 1: Using Interface for Union Types
```typescript
// ❌ WRONG - Interface can't represent unions properly
interface Response {
  status: 'success' | 'error';
  data?: string;
  error?: string;
}

// ✅ CORRECT - Use type for unions
type Response = 
  | { status: 'success'; data: string }
  | { status: 'error'; error: string };
```

### Mistake 2: Not Using Readonly for Props
```typescript
// ❌ WRONG - Props can be mutated (bad practice!)
interface Props {
  items: string[];
}

// ✅ CORRECT - Mark as readonly to prevent mutation
interface Props {
  items: readonly string[];
}

// Or in type:
type Props = {
  items: ReadonlyArray<string>;
};
```

### Mistake 3: Forgetting to Export Types for Reuse
```typescript
// ❌ WRONG - Types not exported, can't be reused
const UserCard = ({ user }: { user: { name: string } }) => {
  return <div>{user.name}</div>;
};

// ✅ CORRECT - Export types for reuse across components
interface User {
  name: string;
  email: string;
}

interface UserCardProps {
  user: User;
}

export const UserCard = ({ user }: UserCardProps) => {
  return <div>{user.name}</div>;
};

export type { User, UserCardProps };
```

### Mistake 4: Using React.FC Incorrectly
```typescript
// ❌ WRONG - React.FC has quirks with generic props
const MyComponent: React.FC<MyProps> = ({ prop }) => { ... }
// Problem: children is automatically included, type inference can be tricky

// ✅ CORRECT - Define props explicitly without React.FC
const MyComponent = ({ prop }: MyProps) => { ... }
// Simpler, more predictable, works better with generics
```

## Real-World Example

Here's a complete, production-ready example combining both approaches:

```typescript
// [File: src/types/user.ts]
// Centralized type definitions for a user feature

// Base user type - use type for primitives and unions
export type UserRole = 'guest' | 'user' | 'admin' | 'superadmin';

// User status for auth flow
export type UserStatus = 'pending' | 'active' | 'suspended' | 'banned';

// Interface for the core user entity - extensible
export interface User {
  id: string;
  email: string;
  name: string;
  role: UserRole;
  status: UserStatus;
  avatarUrl?: string;
  createdAt: Date;
  updatedAt: Date;
}

// Type for creating a new user - subset of User
export type CreateUserInput = Pick<
  User, 
  'email' | 'name' | 'role'
> & { password: string };

// Type for updating a user - partial with constraints
export type UpdateUserInput = Partial<Omit<User, 'id' | 'createdAt'>>;

// Discriminated union for user actions
export type UserAction =
  | { type: 'SET_USER'; payload: User }
  | { type: 'UPDATE_USER'; payload: Partial<User> }
  | { type: 'DELETE_USER' }
  | { type: 'SET_LOADING' }
  | { type: 'SET_ERROR'; payload: Error };

// ======== Component Props ========

// Props for user list component
export interface UserListProps {
  users: User[]; // Array of users
  onUserSelect?: (user: User) => void;
  onUserDelete?: (userId: string) => void;
  loading?: boolean;
  emptyMessage?: string;
}

// Props for user card - uses the User type
export interface UserCardProps {
  user: User;
  showActions?: boolean;
  onEdit?: (user: User) => void;
  onDelete?: (userId: string) => void;
}

// ======== API Response Types ========

// Generic API response wrapper
export interface ApiResponse<T> {
  data: T;
  message?: string;
  success: boolean;
}

// Paginated response
export interface PaginatedResponse<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
  hasMore: boolean;
}

// Use with discriminated unions
export type UserApiResponse = 
  | ApiResponse<User>
  | ApiResponse<User[]>
  | { error: string; code: number };
```

```typescript
// [File: src/components/UserCard.tsx]
// Implementation using the types we defined

import React from 'react';
import { User, UserCardProps } from '@/types/user';

// The component - clean props definition
export const UserCard: React.FC<UserCardProps> = ({
  user,
  showActions = false,
  onEdit,
  onDelete,
}) => {
  // Type narrowing - TypeScript knows user properties
  const isActive = user.status === 'active';
  const isAdmin = user.role === 'admin' || user.role === 'superadmin';

  return (
    <div className="user-card" data-status={user.status} data-role={user.role}>
      <div className="user-card-header">
        {user.avatarUrl ? (
          <img 
            src={user.avatarUrl} 
            alt={`${user.name}'s avatar`}
            className="user-avatar"
          />
        ) : (
          <div className="user-avatar-placeholder">
            {user.name.charAt(0).toUpperCase()}
          </div>
        )}
        
        <div className="user-info">
          <h3>{user.name}</h3>
          <p className="user-email">{user.email}</p>
          <span className={`status-badge status-${user.status}`}>
            {user.status}
          </span>
        </div>
      </div>

      {showActions && (
        <div className="user-card-actions">
          <button onClick={() => onEdit?.(user)}>
            Edit
          </button>
          <button 
            onClick={() => onDelete?.(user.id)}
            disabled={isAdmin} // Can't delete admins
          >
            Delete
          </button>
        </div>
      )}
    </div>
  );
};

export default UserCard;
```

## Key Takeaways
- Use `interface` for component props and object shapes that might be extended
- Use `type` for unions, primitives, and complex type transformations
- Interfaces support declaration merging; types do not
- Types are more flexible for creating new types from existing ones (Pick, Omit, Partial)
- Both can be used for React components - choose based on your needs
- Export types separately for better organization and reusability
- Use discriminated unions for state machines and async data states

## What's Next
Continue to [Generics in React](03-generics-in-react.md) to learn how to create flexible, reusable components and hooks that work with any data type.