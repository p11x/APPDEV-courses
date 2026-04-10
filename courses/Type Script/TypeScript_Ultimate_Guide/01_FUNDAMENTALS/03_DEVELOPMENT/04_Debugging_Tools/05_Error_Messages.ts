/**
 * Category: 01_FUNDAMENTALS Subcategory: 03_DEVELOPMENT Concept: 04 Topic: Error Messages Purpose: Understanding TypeScript error messages Difficulty: beginner UseCase: development,debugging Version: TS4.9+ Compatibility: All TypeScript versions Performance: N/A Security: N/A */

/**
 * Error Messages - Comprehensive Guide
 * =====================================
 * 
 * 📚 WHAT: Understanding TypeScript error messages
 * 💡 WHY: Learn to interpret and fix common TypeScript errors
 * 🔧 HOW: Error codes, message format, troubleshooting
 */

// ============================================================================
// SECTION 1: ERROR MESSAGE FORMAT
// ============================================================================

// Example 1.1: Basic Error Structure
// -------------------------------

// error TS2304: Cannot find name 'Student'.
//    at line 10 in src/index.ts

interface ErrorStructure {
  errorCode: string;
  message: string;
  file: string;
  line: number;
}

const errorStructure: ErrorStructure = {
  errorCode: "TS2304",
  message: "Cannot find name 'Student'",
  file: "src/index.ts",
  line: 10
};

// ============================================================================
// SECTION 2: COMMON TYPE ERRORS
// ============================================================================

// Example 2.1: Type Errors
// ---------------------

// Error: Type 'string' is not assignable to type 'number'
// const num: number = "hello";  // Error

// Solution:
const num: number = 123;

// Example 2.2: Implicit Any
// ---------------------

// Error: Parameter 'value' implicitly has an 'any' type
// function process(value) { return value; }  // Error

// Solution:
function process(value: string): string {
  return value;
}

// ============================================================================
// SECTION 3: NULL AND UNDEFINED ERRORS
// ============================================================================

// Example 3.1: Strict Null Checks Errors
// ----------------------------------

// Error: Object is possibly 'null'
// const name: string = null;  // Error with strictNullChecks

// Solution:
const name: string | null = null;

// Example 3.2: Undefined on Property
// -------------------------------

// Error: Property 'email' is possibly undefined
// const email = user.email.toLowerCase();  // Error

// Solution:
const email = user.email?.toLowerCase() ?? "";

// ============================================================================
// SECTION 4: IMPORT ERRORS
// ============================================================================

// Example 4.1: Module Not Found
// -----------------------

// Error: Cannot find module './utils' or its type declarations
// import { helper } from './utils';  // Error

// Solution: Check file path and extension

// Example 4.2: No Default Export
// --------------------------

// Error: Module has no default export
// import myModule from './module';  // Error

// Solution: Use named import
import { myModule } from "./module";

// ============================================================================
// SECTION 5: CLASS ERRORS
// ============================================================================

// Example 5.1: Property Not Initialized
// ---------------------------------

// Error: Property 'name' has no initializer
// class User { name: string; }  // Error

// Solution:
class User {
  name: string;
  constructor(name: string) {
    this.name = name;
  }
}

// Example 5.2: Abstract Class Instantiation
// -----------------------------------

// Error: Cannot create instance of abstract class
// const obj = new Animal();  // Error

// Solution: Extend and instantiate concrete class

// ============================================================================
// SECTION 6: GENERIC ERRORS
// ============================================================================

// Example 6.1: Generic Type Arguments
// ------------------------------

// Error: Type 'string' is not assignable to type 'T'
// function identity<T>(value: T): T { return value; }
// identity<string>(123);  // Error

// Solution:
identity<number>(123);

// Example 6.2: Generic Constraint
// -----------------------

// Error: Property 'length' does not exist on type 'T'
// function getLength<T>(arg: T): number { return arg.length; }  // Error

// Solution:
function getLength<T extends { length: number }>(arg: T): number {
  return arg.length;
}

// ============================================================================
// SECTION 7: ENUM ERRORS
// ============================================================================

// Example 7.1: Enum Member Issues
// -----------------------

// Error: Enum member must have initializer
// enum Status { Active, Pending }  // Error if not all initialized

// Solution:
enum Status {
  Active = "active",
  Pending = "pending"
}

// ============================================================================
// SECTION 8: DECORATOR ERRORS
// ============================================================================

// Example 8.1: Decorator Configuration
// -------------------------------

// Error: Decorators are not valid here
// @inject class Service { }  // Error

// Solution: Enable experimentalDecorators in tsconfig

interface DecoratorConfig {
  compilerOptions: {
    experimentalDecorators: boolean;
    emitDecoratorMetadata: boolean;
  };
}

const decoratorConfig: DecoratorConfig = {
  compilerOptions: {
    experimentalDecorators: true,
    emitDecoratorMetadata: true
  }
};

// ============================================================================
// SECTION 9: COMPILE ERRORS
// ============================================================================

// Example 9.1: Duplicate Identifier
// -----------------------------

// Error: Duplicate identifier 'name'
// const name = "John";
// const name = "Jane";  // Error

// Solution: Use unique variable names

// ============================================================================
// SECTION 10: MODULE ERRORS
// ============================================================================

// Example 10.1: Circular Dependency
// ----------------------------

// Error: Circular dependency detected
// import A from './A';  // A imports B
// import B from './B';  // B imports A

// Solution: Refactor to remove circular dependency

// ============================================================================
// SECTION 11: COMPATIBILITY
// ============================================================================

// Example 11.1: Version-Specific Errors
// ---------------------------------

interface ErrorVersionMapping {
  noImplicitAny: string;
  strictNullChecks: string;
  privateFields: string;
}

const errorVersionMapping: ErrorVersionMapping = {
  noImplicitAny: "TS2.0+",
  strictNullChecks: "TS2.0+",
  privateFields: "TS3.8+"
};

// ============================================================================
// SECTION 12: TROUBLESHOOTING
// ============================================================================

// Example 12.1: Error Resolution Steps
// -------------------------------

interface ResolutionSteps {
  read: string;
  locate: string;
  understand: string;
  fix: string;
  verify: string;
}

const resolutionSteps: ResolutionSteps = {
  read: "Read error message carefully",
  locate: "Locate file and line number",
  understand: "Understand what TypeScript expects",
  fix: "Fix the code appropriately",
  verify: "Verify with type check"
};

// ============================================================================
// SECTION 13: ERROR CODES
// ============================================================================

// Example 13.1: Common Error Codes
// -----------------------

interface CommonErrorCodes {
  TS2304: string;
  TS2339: string;
  TS2345: string;
  TS7006: string;
  TS7031: string;
}

const commonErrorCodes: CommonErrorCodes = {
  TS2304: "Cannot find name",
  TS2339: "Property does not exist",
  TS2345: "Argument type mismatch",
  TS7006: "Implicit any parameter",
  TS7031: "Let/const required"
};

// ============================================================================
// SECTION 14: ALTERNATIVES
// ============================================================================

// Alternative approaches:
// 1. TypeScript playground - Test types online
// 2. --noEmitOnError false - Emit despite errors
// 3. ESLint - Additional linting rules
// 4. Prettier - Code formatting

// ============================================================================
// SECTION 15: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 03_DEVELOPMENT/04_Debugging_Tools/03_Node_Debugging.ts
// - 03_DEVELOPMENT/04_Debugging_Tools/06_Type_Inspector.ts
// - 03_DEVELOPMENT/03_Compiler_Options/02_Strict_Mode.ts

console.log("\n=== Error Messages Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/04_Debugging_Tools/06_Type_Inspector");