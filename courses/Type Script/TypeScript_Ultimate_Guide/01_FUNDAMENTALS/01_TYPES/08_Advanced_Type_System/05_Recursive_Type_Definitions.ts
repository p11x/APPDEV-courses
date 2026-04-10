/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 08_Advanced_Type_System
 * Topic: 05_Recursive_Type_Definitions
 * Purpose: Self-referential types for tree structures and nested data
 * Difficulty: advanced
 * UseCase: web, backend, data_structures
 * Version: TypeScript 5.0+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe recursive structures
 */

/**
 * Recursive Type Definitions - Self-Referential Types
 * ====================================================
 * 
 * 📚 WHAT: Types that reference themselves for representing nested structures
 * 💡 WHY: Enables modeling of trees, linked lists, and nested objects
 * 🔧 HOW: Type alias references itself in its definition
 */

// ============================================================================
// SECTION 1: BASIC RECURSIVE TYPES
// ============================================================================

// Example 1.1: Linked list type
type LinkedListNode<T> = {
  value: T;
  next?: LinkedListNode<T>;
};

type NumberList = LinkedListNode<number>;
type StringList = LinkedListNode<string>;

const numberList: NumberList = {
  value: 1,
  next: {
    value: 2,
    next: {
      value: 3,
    },
  },
};

// Example 1.2: Tree structure
interface TreeNode<T> {
  value: T;
  children: TreeNode<T>[];
}

type FileTree = TreeNode<{ name: string; isDirectory: boolean }>;

const fileTree: FileTree = {
  value: { name: "root", isDirectory: true },
  children: [
    {
      value: { name: "src", isDirectory: true },
      children: [
        { value: { name: "index.ts", isDirectory: false }, children: [] },
      ],
    },
    { value: { name: "package.json", isDirectory: false }, children: [] },
  ],
};

// ============================================================================
// SECTION 2: RECURSIVE OBJECT TYPES
// ============================================================================

// Example 2.1: Nested object with arbitrary depth
interface JsonValue {
  [key: string]: JsonValue | string | number | boolean | null;
}

const config: JsonValue = {
  database: {
    host: "localhost",
    port: 5432,
    settings: {
      timeout: 5000,
      ssl: true,
    },
  },
  features: {
    auth: true,
    caching: false,
  },
};

// Example 2.2: Deep partial type
type DeepPartial<T> = {
  [P in keyof T]?: T[P] extends object ? DeepPartial<T[P]> : T[P];
};

interface User {
  name: string;
  address: {
    street: string;
    city: string;
    country: {
      code: string;
      name: string;
    };
  };
}

type PartialUser = DeepPartial<User>;

const partialUser: PartialUser = {
  name: "John",
  address: {
    city: "NYC",
  },
};

// ============================================================================
// SECTION 3: RECURSIVE UTILITY TYPES
// ============================================================================

// Example 3.1: Deep readonly
type DeepReadonly<T> = {
  readonly [P in keyof T]: T[P] extends object ? DeepReadonly<T[P]> : T[P];
};

interface Config {
  apiUrl: string;
  settings: {
    timeout: number;
    retries: number;
  };
}

const readonlyConfig: DeepReadonly<Config> = {
  apiUrl: "https://api.example.com",
  settings: {
    timeout: 5000,
    retries: 3,
  },
};

// Example 3.2: Deep required
type DeepRequired<T> = {
  [P in keyof T]-?: T[P] extends object ? DeepRequired<T[P]> : T[P];
};

// ============================================================================
// SECTION 4: CONDITIONAL RECURSIVE TYPES
// ============================================================================

// Example 4.1: Flatten nested arrays
type Flatten<T> = T extends (infer U)[] ? Flatten<U>[] : [T];

type NestedArray = number[][][];
type Flattened = Flatten<NestedArray>; // number[]

// Example 4.2: Deep omit type
type DeepOmit<T, K extends keyof T> = {
  [P in keyof T as P extends K ? never : P]: T[P] extends object 
    ? DeepOmit<T[P], K>
    : T[P];
};

interface Company {
  name: string;
  address: {
    street: string;
    zip: string;
  };
}

type CompanyWithoutZip = DeepOmit<Company, "zip">;

// ============================================================================
// SECTION 5: RECURSIVE FUNCTION TYPES
// ============================================================================

// Example 5.1: Recursive function type
type RecursiveFn<T> = T extends object 
  ? (value: T) => void 
  : never;

type Handler = RecursiveFn<{ a: string }>;

// Example 5.2: Callback recursive type
type EventHandler<T> = T extends { type: string } 
  ? (event: T) => void 
  : EventHandler<{ type: string }>;

type ClickHandler = EventHandler<{ type: "click"; x: number; y: number }>;

// ============================================================================
// SECTION 6: PRACTICAL RECURSIVE PATTERNS
// ============================================================================

// Example 6.1: Nested navigation structure
interface NavItem {
  title: string;
  path: string;
  children?: NavItem[];
}

const navigation: NavItem[] = [
  {
    title: "Home",
    path: "/",
    children: [
      { title: "About", path: "/about" },
      { title: "Contact", path: "/contact" },
    ],
  },
  {
    title: "Products",
    path: "/products",
    children: [
      {
        title: "Software",
        path: "/products/software",
        children: [{ title: "Download", path: "/products/software/download" }],
      },
    ],
  },
];

// Example 6.2: Parse tree for expression
type Expression = 
  | { type: "number"; value: number }
  | { type: "binary"; operator: "+" | "-" | "*" | "/"; left: Expression; right: Expression };

const expr: Expression = {
  type: "binary",
  operator: "+",
  left: { type: "number", value: 1 },
  right: {
    type: "binary",
    operator: "*",
    left: { type: "number", value: 2 },
    right: { type: "number", value: 3 },
  },
};

function evaluate(expr: Expression): number {
  switch (expr.type) {
    case "number":
      return expr.value;
    case "binary":
      const left = evaluate(expr.left);
      const right = evaluate(expr.right);
      switch (expr.operator) {
        case "+": return left + right;
        case "-": return left - right;
        case "*": return left * right;
        case "/": return left / right;
      }
  }
}

// ============================================================================
// SECTION 7: INFINITE TYPES
// ============================================================================

// Example 7.1: Tuple recursion
type TupleToUnion<T extends unknown[]> = T[number];

type Result = TupleToUnion<[string, number, boolean]>; // string | number | boolean

// Example 7.2: Cyclic type (use carefully)
type JSONValue = 
  | string 
  | number 
  | boolean 
  | null 
  | JSONValue[] 
  | { [key: string]: JSONValue };

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: Deep recursive types may cause slower compilation. TypeScript
 * has a recursion depth limit (currently ~1000 levels). For deep structures,
 * consider iterative approaches or type-level computation limits.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: Recursive types require TypeScript 2.0+. Deep utility types
 * were significantly improved in TypeScript 4.1+ with recursive conditional
 * types support.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: Recursive types provide type safety for deeply nested data,
 * preventing runtime errors from malformed data. Use DeepReadonly to prevent
 * accidental mutations in security-sensitive data.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test with various depths of nesting. Verify that deep utility
 * types work correctly. Test that recursive functions handle base cases
 * correctly.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Use TypeScript's error messages to identify infinite recursion.
 * IDE tooltips show the expanded type. Break complex types into simpler
 * intermediate types for debugging.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - Flat structures: Simpler but less expressive
 * - Iterative approaches: More verbose but better performance
 * - Library solutions: ts-toolbelt provides optimized recursive types
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 10_Infer_Type_Patterns.ts: Using infer in recursive types
 * - 07_Conditional_Type_Chaining.ts: Conditional type recursion
 * - 09_Type_Utilities: Deep utility types
 */

console.log("=== Recursive Type Definitions Complete ===");
