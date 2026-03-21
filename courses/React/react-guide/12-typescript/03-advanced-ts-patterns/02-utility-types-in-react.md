# Utility Types in React

## Overview
TypeScript provides powerful built-in utility types that can transform existing types into new ones. When building React applications, these utilities become essential for creating flexible components, handling form updates, deriving types from other types, and more. This guide covers the most useful utility types for React development: Partial, Required, Pick, Omit, Record, ReturnType, Parameters, and how to use them effectively in components, hooks, and props.

## Prerequisites
- TypeScript basics and generics
- Understanding of React component props
- Familiarity with TypeScript interfaces and types

## Core Concepts

### Partial<T> - Make All Properties Optional
Useful when updating partial data, like form updates:

```typescript
// [File: src/examples/utilityPartial.ts]

interface User {
  id: string;
  name: string;
  email: string;
  age: number;
  role: 'admin' | 'user';
}

// Partial<User> makes all properties optional
// Useful for update forms where you might update just some fields
type PartialUser = Partial<User>;
// Equivalent to:
// {
//   id?: string | undefined;
//   name?: string | undefined;
//   email?: string | undefined;
//   age?: number | undefined;
//   role?: 'admin' | 'user' | undefined;
// }

// Example: Form that updates partial user data
function UserUpdateForm({ 
  initialUser, 
  onUpdate 
}: { 
  initialUser: User;
  onUpdate: (updates: Partial<User>) => void;
}) {
  // Track which fields have changed
  const [updates, setUpdates] = React.useState<Partial<User>>({});

  const handleChange = (field: keyof User, value: string | number) => {
    setUpdates(prev => ({ ...prev, [field]: value }));
  };

  const handleSubmit = () => {
    // Only send changed fields to the API
    onUpdate(updates);
  };

  return (
    <form>
      {/* Show all fields with current or updated values */}
      <input
        value={updates.name ?? initialUser.name}
        onChange={(e) => handleChange('name', e.target.value)}
      />
      {/* ... more fields */}
    </form>
  );
}
```

### Required<T> - Make All Properties Required
The opposite of Partial - useful when you need to ensure all properties exist:

```typescript
// [File: src/examples/utilityRequired.ts]

interface Config {
  apiUrl?: string;
  timeout?: number;
  retryCount?: number;
}

// Required<Config> makes all properties required
type RequiredConfig = Required<Config>;
// Equivalent to:
// {
//   apiUrl: string;
//   timeout: number;
//   retryCount: number;
// }

// Example: Default config that must be fully populated
function createApiClient(config: Required<Config>) {
  // TypeScript guarantees all properties exist
  const url = config.apiUrl; // Guaranteed string
  const timeout = config.timeout; // Guaranteed number
  // ...
}

// Usage with default values
function getClientConfig(base?: Partial<Config>): Required<Config> {
  return {
    apiUrl: base?.apiUrl ?? 'https://api.example.com',
    timeout: base?.timeout ?? 5000,
    retryCount: base?.retryCount ?? 3,
  };
}
```

### Pick<T, K> - Select Specific Properties
Create a type with only certain properties from another type:

```typescript
// [File: src/examples/utilityPick.ts]

interface User {
  id: string;
  name: string;
  email: string;
  password: string;
  avatar: string;
  createdAt: Date;
  updatedAt: Date;
}

// Pick only the properties you need
type UserSummary = Pick<User, 'id' | 'name' | 'avatar'>;
// Equivalent to:
// {
//   id: string;
//   name: string;
//   avatar: string;
// }

// Example: Display a list of users with minimal info
interface UserListItemProps {
  users: Pick<User, 'id' | 'name' | 'avatar'>[];
  onSelect: (userId: string) => void;
}

function UserList({ users, onSelect }: UserListItemProps) {
  return (
    <ul>
      {users.map(user => (
        <li key={user.id} onClick={() => onSelect(user.id)}>
          <img src={user.avatar} alt={user.name} />
          <span>{user.name}</span>
        </li>
      ))}
    </ul>
  );
}

// Another use: API response types
type CreateUserRequest = Pick<User, 'name' | 'email' | 'password'>;
type UserResponse = Pick<User, 'id' | 'name' | 'email' | 'avatar' | 'createdAt'>;
```

### Omit<T, K> - Exclude Specific Properties
Create a type excluding certain properties - the inverse of Pick:

```typescript
// [File: src/examples/utilityOmit.ts]

interface User {
  id: string;
  name: string;
  email: string;
  password: string; // Sensitive - don't expose
  role: 'admin' | 'user';
  createdAt: Date;
}

// Omit removes specific properties
type SafeUser = Omit<User, 'password'>;
// Equivalent to:
// {
//   id: string;
//   name: string;
//   email: string;
//   role: 'admin' | 'user';
//   createdAt: Date;
// }

// Example: Component that displays user info without sensitive data
interface UserCardProps {
  user: Omit<User, 'password'>;
}

function UserCard({ user }: UserCardProps) {
  return (
    <div>
      <h3>{user.name}</h3>
      <p>{user.email}</p>
      {/* No password exposed! */}
    </div>
  );
}

// Common use: Removing internal/props from component
interface InternalProps {
  internalId: string;
  internalHandler: () => void;
  name: string;
  email: string;
}

// Expose only public props
type PublicProps = Omit<InternalProps, 'internalId' | 'internalHandler'>;
```

### Record<K, V> - Create Object Types
Build object types with specific key and value types:

```typescript
// [File: src/examples/utilityRecord.ts]

// Create an object with string keys and User values
type UserMap = Record<string, User>;
// Equivalent to: { [key: string]: User; }

// Create an object with enum-like keys
type UserRoles = Record<'admin' | 'user' | 'guest', User[]>;
// Equivalent to:
// {
//   admin: User[];
//   user: User[];
//   guest: User[];
// }

// Example: Group users by role
function GroupUsersByRole(users: User[]): Record<'admin' | 'user' | 'guest', User[]> {
  return users.reduce((acc, user) => {
    const role = user.role as 'admin' | 'user' | 'guest';
    return {
      ...acc,
      [role]: [...(acc[role] || []), user],
    };
  }, { admin: [], user: [], guest: [] });
}

// Example: Form field validation rules
interface ValidationRules {
  required: boolean;
  minLength?: number;
  maxLength?: number;
  pattern?: RegExp;
}

type FormValidation = Record<string, ValidationRules>;

// Example: Theme colors
type ColorScheme = Record<'primary' | 'secondary' | 'accent', string>;

const lightColors: ColorScheme = {
  primary: '#007bff',
  secondary: '#6c757d',
  accent: '#ffc107',
};
```

### ReturnType<T> - Get Function Return Type
Extract the return type of a function:

```typescript
// [File: src/examples/utilityReturnType.ts]

// Define a function
function fetchUser(id: string): Promise<{ id: string; name: string }> {
  return fetch(`/api/users/${id}`).then(r => r.json());
}

// Get the return type
type UserResponse = ReturnType<typeof fetchUser>;
// Equivalent to: Promise<{ id: string; name: string }>

// Extract the resolved type
type UserData = Awaited<ReturnType<typeof fetchUser>>;
// Equivalent to: { id: string; name: string }

// Example: Create a hook that returns the same type as a function
function useAsyncFunction<T extends (...args: any[]) => Promise<any>>(
  fn: T
) {
  const [data, setData] = React.useState<Awaited<ReturnType<T>> | null>(null);
  const [loading, setLoading] = React.useState(false);
  const [error, setError] = React.useState<Error | null>(null);

  const execute = async (...args: Parameters<T>) => {
    setLoading(true);
    try {
      const result = await fn(...args);
      setData(result);
    } catch (e) {
      setError(e instanceof Error ? e : new Error('Unknown error'));
    } finally {
      setLoading(false);
    }
  };

  return { data, loading, error, execute };
}

// Usage - TypeScript infers the return type automatically!
const { data, loading, error, execute } = useAsyncFunction(fetchUser);
```

### Parameters<T> - Get Function Parameters
Extract parameter types as a tuple:

```typescript
// [File: src/examples/utilityParameters.ts]

// Define a function with multiple parameters
function createUser(name: string, email: string, role: 'admin' | 'user'): User {
  return { id: '1', name, email, role };
}

// Get parameter types as tuple
type CreateUserParams = Parameters<typeof createUser>;
// Equivalent to: [string, string, 'admin' | 'user']

// Extract individual types
type NameParam = Parameters<typeof createUser>[0]; // string
type RoleParam = Parameters<typeof createUser>[2]; // 'admin' | 'user'

// Example: Create a wrapper function
function createUserWrapper(...args: Parameters<typeof createUser>) {
  console.log('Creating user with:', args);
  return createUser(...args);
}

// Example: Handler that accepts same params as another function
function useHandler<T extends (...args: any[]) => any>(fn: T) {
  return (...args: Parameters<T>) => {
    console.log('Handler called with:', args);
    return fn(...args);
  };
}

const wrappedCreateUser = useHandler(createUser);
wrappedCreateUser('John', 'john@example.com', 'user'); // Fully typed!
```

## Real-World Example

Complete form builder using utility types:

```typescript
// [File: src/components/FormBuilder.tsx]
import React, { useState } from 'react';

// ======== Define Original Types ========

interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  age: number;
  bio: string;
  role: 'admin' | 'user' | 'guest';
  isActive: boolean;
  createdAt: Date;
}

// ======== Derive Form Types ========

// Form uses only editable fields (excluding id, createdAt)
type UserFormData = Omit<User, 'id' | 'createdAt'>;

// Form validation rules for each field
interface ValidationRules {
  required?: boolean;
  min?: number;
  max?: number;
  pattern?: RegExp;
  message?: string;
}

// Validation rules for all form fields
type FormValidation = Record<keyof UserFormData, ValidationRules>;

// Initial form values
const getInitialFormData = (): UserFormData => ({
  firstName: '',
  lastName: '',
  email: '',
  phone: '',
  age: 0,
  bio: '',
  role: 'user',
  isActive: true,
});

// Validation rules
const validationRules: FormValidation = {
  firstName: { required: true, min: 2, max: 50, message: 'First name is required' },
  lastName: { required: true, min: 2, max: 50, message: 'Last name is required' },
  email: { required: true, pattern: /^[^\s@]+@[^\s@]+\.[^\s@]+$/, message: 'Invalid email' },
  phone: { required: false, pattern: /^\d{10}$/, message: 'Phone must be 10 digits' },
  age: { required: true, min: 18, max: 150, message: 'Must be 18 or older' },
  bio: { required: false, max: 500, message: 'Bio too long' },
  role: { required: true },
  isActive: { required: false },
};

// ======== Form Component ========

interface FormFieldProps<T extends keyof UserFormData> {
  name: T;
  label: string;
  value: UserFormData[T];
  onChange: (name: T, value: UserFormData[T]) => void;
  validation?: ValidationRules;
  error?: string;
}

function FormField<T extends keyof UserFormData>({
  name,
  label,
  value,
  onChange,
  validation,
  error,
}: FormFieldProps<T>) {
  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    let newValue: string | number | boolean = e.target.value;
    
    if (e.target.type === 'number') {
      newValue = parseFloat(e.target.value) || 0;
    } else if (e.target.type === 'checkbox') {
      newValue = (e.target as HTMLInputElement).checked;
    }
    
    onChange(name, newValue as UserFormData[T]);
  };

  return (
    <div className={`form-field ${error ? 'has-error' : ''}`}>
      <label htmlFor={name}>
        {label}
        {validation?.required && <span className="required">*</span>}
      </label>
      <input
        id={name}
        name={name}
        type={name === 'age' ? 'number' : name === 'isActive' ? 'checkbox' : 'text'}
        value={value}
        onChange={handleChange}
        checked={name === 'isActive' ? (value as boolean) : undefined}
      />
      {error && <span className="error-message">{error}</span>}
    </div>
  );
}

// ======== Main Form ========

function UserForm({ 
  initialData, 
  onSubmit 
}: { 
  initialData?: Partial<User>;
  onSubmit: (data: UserFormData) => void;
}) {
  const [formData, setFormData] = useState<UserFormData>({
    ...getInitialFormData(),
    ...initialData,
  });

  const [errors, setErrors] = useState<Partial<Record<keyof UserFormData, string>>>({});

  // Generic field update handler using Pick
  const handleChange = <T extends keyof UserFormData>(
    name: T,
    value: UserFormData[T]
  ) => {
    setFormData(prev => ({ ...prev, [name]: value }));
    
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: undefined }));
    }
  };

  // Validate all fields
  const validate = (): boolean => {
    const newErrors: Partial<Record<keyof UserFormData, string>> = {};
    let isValid = true;

    (Object.keys(validationRules) as (keyof UserFormData)[]).forEach(field => {
      const rules = validationRules[field];
      const value = formData[field];
      const stringValue = String(value);

      if (rules.required && !stringValue) {
        newErrors[field] = rules.message || `${field} is required`;
        isValid = false;
      }

      if (rules.min !== undefined && typeof value === 'number' && value < rules.min) {
        newErrors[field] = rules.message || `Minimum is ${rules.min}`;
        isValid = false;
      }

      if (rules.max !== undefined && typeof value === 'number' && value > rules.max) {
        newErrors[field] = rules.message || `Maximum is ${rules.max}`;
        isValid = false;
      }

      if (rules.pattern && typeof value === 'string' && !rules.pattern.test(value)) {
        newErrors[field] = rules.message || `Invalid format`;
        isValid = false;
      }
    });

    setErrors(newErrors);
    return isValid;
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    if (validate()) {
      onSubmit(formData);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="user-form">
      {/* Use Pick to iterate over form fields */}
      {(['firstName', 'lastName', 'email', 'phone', 'age', 'bio', 'role', 'isActive'] as (keyof UserFormData)[]).map(field => (
        <FormField
          key={field}
          name={field}
          label={field.replace(/([A-Z])/g, ' $1').trim()}
          value={formData[field]}
          onChange={handleChange}
          validation={validationRules[field]}
          error={errors[field]}
        />
      ))}

      <button type="submit">Submit</button>
    </form>
  );
}

export default UserForm;
```

## Key Takeaways
- `Partial<T>` - Makes all properties optional (great for updates)
- `Required<T>` - Makes all properties required (opposite of Partial)
- `Pick<T, K>` - Select specific properties to include
- `Omit<T, K>` - Exclude specific properties (inverse of Pick)
- `Record<K, V>` - Create object types with specific keys and values
- `ReturnType<T>` - Extract function return type
- `Parameters<T>` - Extract function parameter types as tuple
- Combine these utilities to create flexible, reusable React components
- Use with React hooks for advanced type-safe patterns

## What's Next
Continue to [Satisfies Operator and Const Assertions](03-satisfies-operator-and-const-assertions.md) to learn how to validate types without widening and create immutable typed constants.