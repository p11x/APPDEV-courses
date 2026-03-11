/**
 * Modules and Exports in TypeScript
 * 
 * Modules organize code into reusable units. Export/import statements
 * control what is shared between modules.
 * 
 * Angular Connection: Angular uses modules for:
 * - Feature modules
 * - Shared modules
 * - Component imports
 * - Service imports
 * - Lazy loading
 */

// Make this a module to avoid global scope conflicts
export {};

console.log("========== MODULES AND EXPORTS ==========\n");

// ============================================
// CONCEPTS (No actual imports/exports - just demo)
// ============================================

// TypeScript files are modules when they have imports or exports
// Each file with export/import is its own module

// Named exports - export specific items
// export const PI = 3.14159;
// export function add(a: number, b: number): number { return a + b; }
// export interface User { name: string; email: string; }

// Default export - one per file
// export default class MyClass { }

// Importing
// import { PI, add } from './math';           // Named import
// import MyClass from './my-class';            // Default import
// import { add as sum } from './math';        // Alias
// import * as MathUtils from './math';        // Namespace

// ============================================
// EXPORT EXAMPLES (DEFINED LOCALLY FOR DEMO)
// ============================================

// Define items that would be in separate files
const PI_VALUE = 3.14159;
const MAX_VALUE = 1000;

function multiply(a: number, b: number): number {
    return a * b;
}

interface UserData {
    name: string;
    email: string;
}

// Using locally (simulating import)
console.log("1. Named exports (simulated locally):");
console.log("   PI_VALUE:", PI_VALUE);
console.log("   MAX_VALUE:", MAX_VALUE);
console.log("   multiply(3, 4):", multiply(3, 4));

// ============================================
// IMPORT PATTERNS
// ============================================

// Simulate what imports look like (can't actually import from this file)
// import { multiply } from './math';
// import { UserData } from './user';

// Alias example
const sum = multiply;  // Just alias for demo

console.log("\n2. Alias usage:");
console.log("   sum(5, 3):", sum(5, 3));

// ============================================
// RE-EXPORTS (BARREL PATTERN)
// ============================================

// In a real index.ts file:
// export { multiply } from './math';
// export { UserData } from './user';
// This creates a "barrel" - single entry point for a folder

console.log("\n3. Barrel pattern:");
console.log("   (index.ts re-exports from module files)");

// ============================================
// ANGULAR MODULE EXAMPLE
// ============================================

console.log("\n========== ANGULAR EXAMPLES ==========\n");

// Angular modules are classes decorated with @NgModule
// This is a representation

// Feature module pattern
interface FeatureModuleConfig {
    declarations: string[];
    imports: string[];
    exports: string[];
    providers: string[];
}

const userModuleConfig: FeatureModuleConfig = {
    declarations: ['UserListComponent', 'UserDetailComponent', 'UserFormComponent'],
    imports: ['CommonModule', 'FormsModule', 'RouterModule'],
    exports: ['UserListComponent'],
    providers: ['UserService', 'UserGuard']
};

console.log("Feature Module:");
console.log("   declarations:", userModuleConfig.declarations);
console.log("   imports:", userModuleConfig.imports);
console.log("   providers:", userModuleConfig.providers);

// Shared module pattern
const sharedModuleConfig: FeatureModuleConfig = {
    declarations: ['ButtonComponent', 'CardComponent', 'InputComponent'],
    imports: ['CommonModule'],
    exports: ['ButtonComponent', 'CardComponent', 'InputComponent', 'CommonModule'],
    providers: []
};

console.log("\nShared Module:");
console.log("   declarations:", sharedModuleConfig.declarations);
console.log("   exports:", sharedModuleConfig.exports);

// Lazy loading routes
interface RouteConfig {
    path: string;
    component?: string;
    loadChildren?: string;
}

const routes: RouteConfig[] = [
    { path: 'home', component: 'HomeComponent' },
    { path: 'users', loadChildren: './users/users.module#UsersModule' },
    { path: 'products', loadChildren: './products/products.module#ProductsModule' }
];

console.log("\nRoutes (lazy loading):");
routes.forEach(route => {
    const type = route.component ? 'Eager' : 'Lazy';
    console.log(`   ${route.path}: ${type} loaded`);
});

// ============================================
// SERVICE ORGANIZATION
// ============================================

// Services are provided in root or feature modules
interface ServiceProvider {
    token: string;
    className: string;
}

const coreServices: ServiceProvider[] = [
    { token: 'HttpClient', className: 'HttpClient' },
    { token: 'Router', className: 'Router' }
];

const userServices: ServiceProvider[] = [
    { token: 'UserService', className: 'UserService' },
    { token: 'AuthService', className: 'AuthService' }
];

console.log("\nService Providers:");
console.log("   Core services:", coreServices.length);
console.log("   User services:", userServices.length);

console.log("\n========== SUMMARY ==========");
console.log("Modules:");
console.log("- Every .ts file with import/export is a module");
console.log("- Use 'export' to make items available");
console.log("- Use 'import' to use exported items");
console.log("\nExport Types:");
console.log("- Named: export const value = ... ");
console.log("- Default: export default ... ");
console.log("- Re-export: export { ... } from ... ");
console.log("\nImport Types:");
console.log("- Named: import { name } from ... ");
console.log("- Default: import name from ... ");
console.log("- Alias: import { name as alias } from ... ");
console.log("\nAngular Usage:");
console.log("- Feature modules organize features");
console.log("- Shared modules for reusable code");
console.log("- Lazy loading for performance");
console.log("- Services in providers arrays");
console.log("================================\n");
