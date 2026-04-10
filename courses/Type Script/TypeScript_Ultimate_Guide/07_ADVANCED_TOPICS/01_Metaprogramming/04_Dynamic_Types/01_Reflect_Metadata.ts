/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 01_Metaprogramming
 * Concept: 04_Dynamic_Types
 * Topic: 01_Reflect_Metadata
 * Purpose: Learn runtime type reflection with metadata
 * Difficulty: intermediate
 * UseCase: serialization
 * Version: TS 4.0+
 * Compatibility: Node.js 12+, Browsers (ES2015+)
 * Performance: Reflect operations ~1-5ms overhead
 * Security: Metadata can expose type information
 */

/**
 * WHAT: Reflect metadata allows storing and retrieving type information at runtime.
 * This enables serialization, dependency injection, and validation frameworks.
 */

import "reflect-metadata";

const METADATA_KEY = "design:type";
const PARAM_TYPES_KEY = "design:paramtypes";
const RETURN_TYPE_KEY = "design:returntype";

function metadata(key: string, value: any): PropertyDecorator {
  return function (target: Object, propertyKey: string | symbol) {
    Reflect.defineMetadata(key, value, target, propertyKey);
  };
}

function getMetadata<T>(key: string, target: Object, propertyKey: string | symbol): T | undefined {
  return Reflect.getMetadata(key, target, propertyKey);
}

class User {
  @metadata("validation", { required: true, minLength: 2 })
  name: string = "";
  
  @metadata("validation", { type: "number", min: 0, max: 150 })
  age: number = 0;
  
  @metadata("serialization", { format: "email" })
  email: string = "";
}

function getType(target: Object, propertyKey: string): string {
  return getMetadata<string>(METADATA_KEY, target, propertyKey) || "unknown";
}

function getValidationRules(target: Object, propertyKey: string): any {
  return getMetadata("validation", target, propertyKey);
}

console.log("\n=== Reflect Metadata ===");
console.log("Type of name:", getType(new User(), "name"));
console.log("Validation for age:", getValidationRules(new User(), "age"));

function inject(target: any, propertyKey: string, index: number): void {
  Reflect.defineMetadata("inject:parameter", { index, propertyKey }, target);
}

function getInjections(target: any): any[] {
  return Reflect.getMetadata("inject:parameter", target) || [];
}

class Container {
  private services = new Map<string, any>();
  
  register<T>(token: string, instance: T): void {
    this.services.set(token, instance);
  }
  
  resolve<T>(target: any): T {
    const injections = getInjections(target);
    return injections.map((inj: any) => this.services.get(inj.propertyKey));
  }
}

function injectable(token: string): MethodDecorator {
  return function (target, propertyKey, descriptor) {
    Reflect.defineMetadata("injectable:token", token, descriptor.value);
    return descriptor;
  };
}

function getInjectableToken(fn: Function): string | undefined {
  return Reflect.getMetadata("injectable:token", fn);
}

console.log("\n=== Dependency Injection ===");
const container = new Container();
container.register("userService", { getUser: () => "Alice" });
console.log("Container registered services:", Array.from((container as any).services.keys()));

/**
 * PERFORMANCE:
 * - Metadata storage uses WeakMap
 * - Access time ~O(1) amortized
 * - Use caching for repeated access
 * 
 * COMPATIBILITY:
 * - Requires reflect-metadata polyfill
 * - Works in all modern browsers
 * - Node.js 12+ native support
 * 
 * SECURITY:
 * - Don't expose sensitive type info
 * - Validate metadata values
 * 
 * TESTING:
 * - Mock Reflect operations
 * - Test metadata persistence
 * 
 * DEBUGGING:
 * - Console.log metadata keys
 * - Inspect Reflect metadata
 * 
 * ALTERNATIVE:
 * - Manual property metadata
 * - Symbol-based metadata
 * 
 * CROSS-REFERENCE:
 * - 02_Dynamic_Import.ts - Dynamic type loading
 * - 01_Advanced_Decorators.ts - Decorator metadata
 */