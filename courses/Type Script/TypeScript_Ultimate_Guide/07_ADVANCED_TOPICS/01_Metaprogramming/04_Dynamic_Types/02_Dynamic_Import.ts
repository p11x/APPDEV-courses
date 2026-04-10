/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 01_Metaprogramming
 * Concept: 04_Dynamic_Types
 * Topic: 02_Dynamic_Import
 * Purpose: Master dynamic imports for type-safe code splitting
 * Difficulty: intermediate
 * UseCase: performance-optimization
 * Version: TS 2.4+
 * Compatibility: Node.js 12+, Browsers (ES2020+)
 * Performance: ~50-200ms per dynamic import
 * Security: Validate imported module paths
 */

/**
 * WHAT: Dynamic imports enable lazy loading of modules at runtime.
 * TypeScript provides type inference for dynamically imported modules.
 */

type ModuleMapper<T> = {
  [K in keyof T]: () => Promise<T[K]>;
};

const modules: ModuleMapper<{
  math: { add: (a: number, b: number) => number };
  string: { capitalize: (s: string) => string };
  array: { chunk: <T>(arr: T[], size: number) => T[][] };
}> = {
  math: () => import("./math"),
  string: () => import("./string"),
  array: () => import("./array"),
};

async function lazyLoadMath() {
  const { add } = await import("./math");
  return add(1, 2);
}

type AsyncReturnType<T> = T extends (...args: any[]) => Promise<infer R> ? R : never;

type LoadedModule = AsyncReturnType<typeof import("./math")>;

function createLazyLoader<T extends Record<string, () => Promise<any>>>(map: T) {
  return new Proxy({}, {
    get: (_, key: string) => {
      if (key in map) {
        return async () => {
          const module = await map[key]();
          return module;
        };
      }
      throw new Error(`Module ${key} not found`);
    }
  }) as { [K in keyof T]: () => Promise<any> };
}

const loader = createLazyLoader({
  utils: () => import("./utils"),
  helpers: () => import("./helpers"),
});

type DynamicImport<T> = T extends string ? Promise<any> : never;

type RouteConfig = {
  [path: string]: () => Promise<{ default: React.ComponentType<any> }>;
};

const routes: RouteConfig = {
  "/home": () => import("./pages/Home"),
  "/about": () => import("./pages/About"),
  "/contact": () => import("./pages/Contact"),
};

function createComponentLoader<T extends string>(paths: T[]): Record<T, () => Promise<any>> {
  const result = {} as Record<T, () => Promise<any>>;
  for (const path of paths) {
    result[path] = () => import(path);
  }
  return result;
}

async function loadOnDemand<T>(loader: () => Promise<T>, condition: boolean): Promise<T | null> {
  if (!condition) return null;
  return loader();
}

class DynamicModuleLoader {
  private cache = new Map<string, any>();
  
  async load<T>(path: string): Promise<T> {
    if (this.cache.has(path)) {
      return this.cache.get(path);
    }
    const module = await import(path);
    this.cache.set(path, module);
    return module;
  }
  
  preload(paths: string[]): Promise<void[]> {
    return Promise.all(paths.map(p => this.load(p)));
  }
}

console.log("\n=== Dynamic Import Examples ===");
console.log("Dynamic imports are powerful for code splitting");

/**
 * PERFORMANCE:
 * - Initial load: ~50-100ms
 * - Cached loads: ~1-5ms
 * - Parallel loads: use Promise.all
 * 
 * COMPATIBILITY:
 * - Dynamic import syntax: ES2020
 * - TypeScript 2.4+ for type inference
 * 
 * SECURITY:
 * - Validate module paths
 * - Avoid user input in paths
 * 
 * TESTING:
 * - Mock import() function
 * - Test loading failures
 * 
 * DEBUGGING:
 * - Check network tab for chunks
 * - Use source maps
 * 
 * ALTERNATIVE:
 * - require() for CommonJS
 * - System.import (deprecated)
 * 
 * CROSS-REFERENCE:
 * - 01_Reflect_Metadata.ts - Runtime type info
 * - 01_Type_Generation.ts - Type inference
 */