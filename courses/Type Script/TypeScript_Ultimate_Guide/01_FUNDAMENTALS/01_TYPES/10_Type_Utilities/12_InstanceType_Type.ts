/**
 * Category: 01_FUNDAMENTALS
 * Subcategory: 01_TYPES
 * Concept: 10_Type_Utilities
 * Topic: 12_InstanceType_Type
 * Purpose: Extracts instance type from a constructor
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 2.8+
 * Compatibility: All ES2020+ environments
 * Performance: No runtime overhead - compile-time only
 * Security: Type-safe class handling
 */

/**
 * InstanceType<T> - Built-in Utility Type
 * =======================================
 * 
 * 📚 WHAT: Extracts the instance type of a constructor function T
 * 💡 WHY: Use when you need the type created by a constructor
 * 🔧 HOW: Extracts instance type from constructor type
 */

// ============================================================================
// SECTION 1: BASIC INSTANCETYPE USAGE
// ============================================================================

// Example 1.1: Using InstanceType utility type
class User {
  constructor(public name: string, public age: number) {}
  
  greet(): string {
    return `Hello, ${this.name}!`;
  }
}

type UserInstance = InstanceType<typeof User>;

// Result:
// type UserInstance = User

// Example 1.2: InstanceType with interface
interface UserConstructor {
  new (name: string, age: number): User;
}

type UserInstance2 = InstanceType<UserConstructor>;

// ============================================================================
// SECTION 2: PRACTICAL PATTERNS
// ============================================================================

// Example 2.1: Factory pattern
function createFactory<T extends new (...args: any[]) => any>(
  constructor: T
): InstanceType<T> {
  return new constructor();
}

// Example 2.2: Class registry
type ClassConstructor<T> = new (...args: any[]) => T;

function register<T>(name: string, constructor: ClassConstructor<T>): void {
  console.log(`Registered: ${name}`);
}

class Service {}
register("service", Service);
type ServiceInstance = InstanceType<typeof Service>;

// ============================================================================
// SECTION 3: COMPOSITION WITH OTHER UTILITIES
// ============================================================================

// Example 3.1: InstanceType with Partial
class Config {
  host: string = "localhost";
  port: number = 5432;
}

type PartialConfig = Partial<InstanceType<typeof Config>>;

// Example 3.2: InstanceType with ReturnType
function getConstructor<T>(fn: () => new () => T): InstanceType<ReturnType<typeof fn>> {
  const Ctor = fn();
  return new Ctor();
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

/**
 * Performance: InstanceType<T> is compile-time only. No runtime overhead.
 */

// ============================================================================
// COMPATIBILITY
// ============================================================================

/**
 * Compatibility: InstanceType<T> requires TypeScript 2.8+.
 */

// ============================================================================
// SECURITY
// ============================================================================

/**
 * Security: InstanceType helps maintain type safety for class instantiation.
 * Use for factory patterns and class registration.
 */

// ============================================================================
// TESTING
// ============================================================================

/**
 * Testing: Test with various class constructors. Verify correct instance type.
 */

// ============================================================================
// DEBUGGING
// ============================================================================

/**
 * Debugging: Hover over type to see result.
 */

// ============================================================================
// ALTERNATIVE PATTERNS
// ============================================================================

/**
 * Alternatives:
 * - typeof class: More explicit
 * - ReturnType: For function returns
 * - ConstructorParameters: For constructor params
 */

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

/**
 * Cross-Reference:
 * - 10_ReturnType_Type.ts: Extracts return type
 * - 11_Parameters_Type.ts: Extracts parameters
 * - 01_Required_Type.ts: Required properties
 */

console.log("=== InstanceType Type Complete ===");
