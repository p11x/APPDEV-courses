# TypeScript Best Practices

## Introduction

Following TypeScript best practices ensures type safety, maintainability, and readability of your code. These practices are especially important when working with Angular, where TypeScript is the primary language.

## Strong Typing Benefits

### Always Use Type Annotations

Avoid using `any` type as it defeats the purpose of TypeScript. Instead:

```typescript
// Bad
function processData(data: any): any {
    return data;
}

// Good
interface Data {
    id: number;
    name: string;
}

function processData(data: Data): Data {
    return data;
}
```

### Prefer Interfaces Over Type Aliases for Objects

Interfaces are more expressive and can be extended:

```typescript
// Preferred for objects
interface User {
    id: number;
    name: string;
}

// Use type for unions, primitives, tuples
type Status = 'pending' | 'active' | 'completed';
type Point = [number, number];
```

## Naming Conventions

### Use Consistent Naming

- **Interfaces**: PascalCase, descriptive (e.g., `UserService`, `ProductListComponent`)
- **Types**: PascalCase (e.g., `ApiResponse<T>`)
- **Enums**: PascalCase with PascalCase members (e.g., `UserRole.Admin`)
- **Constants**: UPPER_SNAKE_CASE for values, camelCase for variables

### Avoid Short or Ambiguous Names

```typescript
// Bad
const u = users.find(x => x.i === 1);

// Good
const activeUser = users.find(user => user.id === 1);
```

## Code Organization

### Separate Types into Files

Create a dedicated folder or file for types:

```
src/
  models/
    user.model.ts
    product.model.ts
  services/
    user.service.ts
  components/
    user-list/
```

### Use Barrel Exports

Create index files that re-export from a folder:

```typescript
// models/index.ts
export * from './user.model';
export * from './product.model';
```

## Angular-Specific Practices

### Component Property Typing

Always type component inputs and outputs:

```typescript
@Component({...})
export class UserComponent {
    @Input() user: User = {} as User;
    @Output() userSelected = new EventEmitter<User>();
}
```

### Service Method Return Types

Always specify return types for service methods:

```typescript
@Injectable({ providedIn: 'root' })
export class UserService {
    getUsers(): Promise<User[]> {
        return this.http.get<User[]>('/api/users');
    }
}
```

### Use Generics for Reusable Services

Create generic services for common operations:

```typescript
@Injectable()
export class ApiService<T> {
    constructor(private http: HttpClient) {}
    
    getAll(): Observable<T[]> {
        return this.http.get<T[]>(this.endpoint);
    }
}
```

## Avoiding Common Pitfalls

### Don't Use `any` to Avoid Type Errors

```typescript
// Bad
const data: any = getData();

// Good - use unknown then narrow
const data: unknown = getData();
if (isUserData(data)) {
    console.log(data.name);
}
```

### Use Strict Null Checks

Enable strict null checks in tsconfig.json and handle nulls properly:

```typescript
// With strict null checks enabled
function getName(user: User | null): string {
    return user?.name ?? 'Unknown';
}
```

### Prefer Union Types Over Optional Properties

```typescript
// Better for state management
type LoadingState = 
    | { status: 'idle' }
    | { status: 'loading' }
    | { status: 'success'; data: User[] }
    | { status: 'error'; error: Error };
```

## Configuration Recommendations

### tsconfig.json Best Practices

```json
{
    "compilerOptions": {
        "strict": true,
        "noImplicitAny": true,
        "strictNullChecks": true,
        "noUnusedLocals": true,
        "noUnusedParameters": true,
        "esModuleInterop": true,
        "skipLibCheck": true
    }
}
```

## Summary

- Always prefer strong typing over `any`
- Use interfaces for object shapes
- Follow consistent naming conventions
- Organize types into separate files
- Enable strict mode in TypeScript configuration
- Use generics for reusable code
- Type all function parameters and return values
