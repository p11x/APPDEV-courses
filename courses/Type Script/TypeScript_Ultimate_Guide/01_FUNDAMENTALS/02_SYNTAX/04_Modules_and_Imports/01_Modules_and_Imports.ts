/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: Modules_and_Imports
 * Purpose: Understanding modules and imports in TypeScript
 * Difficulty: beginner
 * UseCase: web, backend, mobile, enterprise
 */

/**
 * Modules and Imports - Comprehensive Guide
 * ==========================================
 * 
 * 📚 WHAT: Module system, exports, imports, and code organization
 * 💡 WHY: Essential for building maintainable applications
 * 🔧 HOW: ES modules, namespaces, re-exports, type-only imports
 */

// ============================================================================
// SECTION 1: EXPORT DECLARATIONS
// ============================================================================

// Example 1.1: Named Exports
// -----------------------

// Export variables
export const PI = 3.14159;
export const APP_NAME = "MyApp";

// Export functions
export function add(a: number, b: number): number {
  return a + b;
}

// Export classes
export class User {
  constructor(public name: string) {}
}

// Export interfaces
export interface Config {
  debug: boolean;
  version: string;
}

// Example 1.2: Default Exports
// -----------------------

// Default export (one per file)
export default class App {
  start(): void {
    console.log("App started");
  }
}

// ============================================================================
// SECTION 2: IMPORT DECLARATIONS
// ============================================================================

// Example 2.1: Named Imports
// -----------------------

// import { add, PI } from "./math";
// import { add as sum } from "./math";
// import * as MathUtils from "./math";

// Example 2.2: Default Imports
// -----------------------

// import App from "./App";
// import App as Application from "./App";

// Example 2.3: Type-Only Imports
// ---------------------------

// import type { Config } from "./config";
// import { type User, type Role } from "./user";

// ============================================================================
// SECTION 3: RE-EXPORTS
// ============================================================================

// Example 3.1: Re-export from Another Module
// -------------------------------------

// Re-export all
// export * from "./utils";

// Re-export specific
// export { add, subtract } from "./math";

// Re-export with renaming
// export { add as sum, subtract as minus } from "./math";

// Example 3.2: Export Aggregate
// -------------------------

// index.ts - barrel file
// export { add, subtract } from "./math";
// export { User, type Config } from "./models";

// ============================================================================
// SECTION 4: DYNAMIC IMPORTS
// ============================================================================

// Example 4.1: Dynamic Import
// -----------------------

async function loadModule() {
  const math = await import("./math");
  return math.add(1, 2);
}

// Example 4.2: Lazy Loading
// ---------------------

// const handleClick = async () => {
//   const { Modal } = await import("./components/Modal");
//   new Modal().show();
// };

// ============================================================================
// SECTION 5: COMMONJS COMPATIBILITY
// ============================================================================

// Example 5.1: ES Module with CommonJS
// --------------------------------

// tsconfig.json: "esModuleInterop": true

// import fs = require("fs");
// import express from "express";

// Example 5.2: Export Assignment
// -----------------------

// namespace ExportAssignment {
//   export const value = 42;
// }
// export = ExportAssignment;

// ============================================================================
// SECTION 6: MODULE STRUCTURE PATTERNS
// ============================================================================

// Example 6.1: Barrel Pattern (index.ts)
// ---------------------------------

// Export from internal modules
// export { User } from "./user.model";
// export { UserService } from "./user.service";
// export { UserController } from "./user.controller";

// Example 6.2: Facade Pattern
// -----------------------

// Provides simple interface over complex subsystem
// export class ApiClient {
//   private userApi = new UserApi();
//   private productApi = new ProductApi();
//   
//   users = this.userApi.users;
//   products = this.productApi.products;
// }

// ============================================================================
// SECTION 7: NAMESPACE ALTERNATIVE
// ============================================================================

// Example 7.1: Namespaces (Legacy - prefer ES Modules)
// ------------------------------------------------

// math.ts
// export namespace MathUtils {
//   export function add(a: number, b: number): number { return a + b; }
//   export function sub(a: number, b: number): number { return a - b; }
// }

// Usage
// import { MathUtils } from "./math";
// MathUtils.add(1, 2);

console.log("\n=== Modules and Imports Complete ===");
console.log("Next: FUNDAMENTALS/SYNTAX/05_Control_Flow");