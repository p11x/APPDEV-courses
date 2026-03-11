# TypeScript vs JavaScript

## Understanding the Differences

While TypeScript and JavaScript are closely related, understanding their differences is crucial for Angular development. TypeScript was designed to address JavaScript's limitations while maintaining compatibility.

## Comparison Table

| Feature | JavaScript | TypeScript |
|---------|------------|------------|
| **Type System** | Dynamic (types checked at runtime) | Static (types checked at compile time) |
| **Type Annotations** | Not supported | Optional type annotations |
| **Error Detection** | Runtime errors only | Compile-time and runtime errors |
| **Tooling/IDE Support** | Basic autocompletion | Advanced IntelliSense, refactoring |
| **Compilation** | Not required (interpreted) | Must be compiled to JavaScript |
| **Optional Types** | N/A | Types are optional |
| **Interfaces** | Not supported | Supported |
| **Generics** | Not supported | Supported |
| **Decorators** | Not supported | Supported |
| **Modules** | ES6 modules | ES6 modules + namespace support |

## Key Differences Explained

### Static vs Dynamic Typing

JavaScript uses dynamic typing, meaning variable types are determined at runtime:

```javascript
// JavaScript - This works fine
let message = "Hello";
message = 42; // No error, type changes dynamically
```

TypeScript allows static typing with compile-time checks:

```typescript
// TypeScript - Compile error
let message: string = "Hello";
message = 42; // Error: Type 'number' is not assignable to type 'string'
```

### Compile-Time Error Detection

TypeScript catches errors before your code runs:

```typescript
// TypeScript - Catches error at compile time
function add(a: number, b: number): number {
    return a + b;
}

add("hello", "world"); // Error: Argument of type 'string' is not assignable
```

### Better Tooling

TypeScript provides:
- Autocomplete for properties and methods
- Inline documentation
- Rename refactoring across files
- Find references
- Code navigation

## When to Use Each

**Use JavaScript for:**
- Simple scripts and quick prototypes
- Learning basic programming concepts
- Projects where type safety isn't critical

**Use TypeScript for:**
- Large-scale applications
- Angular development (required)
- Team projects
- Projects requiring maintainability

## Connection to Angular

Angular **requires** TypeScript - it's not optional. When you create an Angular application, you'll write TypeScript code that:

- Defines component classes with type annotations
- Creates interfaces for data models
- Uses generics for type-safe services
- Leverages decorators (a TypeScript feature)

Understanding these differences will help you appreciate why Angular chose TypeScript and how to write effective Angular applications.
