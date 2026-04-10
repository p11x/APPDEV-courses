/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 01_Metaprogramming
 * Concept: 01_Decorator_Metaprogramming
 * Topic: 03_Class_Decorators
 * Purpose: Explore class-level decorators and their applications
 * Difficulty: intermediate
 * UseCase: framework-development
 * Version: TS 5.0+
 * Compatibility: Node.js 12+, Browsers (ES2020+)
 * Performance: Class decoration adds ~0.1ms overhead
 * Security: Avoid modifying class prototypes in production
 */

/**
 * WHAT: Class decorators are functions that can modify or inspect a class constructor.
 * They are applied to the class declaration before the class is instantiated.
 */

// ============================================
// SECTION 1: BASIC CLASS DECORATORS
// ============================================

function sealed(constructor: Function) {
  Object.seal(constructor);
  Object.seal(constructor.prototype);
}

function logger<T extends Function>(constructor: T) {
  console.log(`[LOGGER] Class ${constructor.name} defined`);
  return class extends constructor {
    constructor(...args: any[]) {
      super(...args);
      console.log(`[LOGGER] Instance of ${constructor.name} created`);
    }
  };
}

@sealed
@logger
class User {
  constructor(public name: string) {}
}

// ============================================
// SECTION 2: CLASS DECORATOR FACTORIES
// ============================================

function createDecorator(prefix: string) {
  return function <T extends Function>(constructor: T): T {
    console.log(`[${prefix}] Decorating ${constructor.name}`);
    return class extends constructor {
      createdAt = new Date();
    };
  };
}

@createDecorator("TIMESTAMP")
@createDecorator("AUDIT")
class Product {
  constructor(public id: number, public name: string) {}
}

// ============================================
// SECTION 3: DECORATORS WITH PARAMETERS
// ============================================

function singleton<T extends Function>(constructor: T) {
  let instance: any;
  return function (...args: any[]) {
    if (!instance) {
      instance = new constructor(...args);
    }
    return instance;
  } as any;
}

function injectable(serviceName: string) {
  return function <T extends Function>(constructor: T): T {
    (constructor as any).serviceName = serviceName;
    return constructor;
  };
}

@singleton
@injectable("userService")
class UserService {
  users: string[] = [];
  addUser(name: string) { this.users.push(name); }
}

// ============================================
// SECTION 4: MODIFYING CLASS PROPERTIES
// ============================================

function addStaticProperties<T extends Function>(props: Record<string, any>) {
  return function (constructor: T): T {
    Object.assign(constructor, props);
    return constructor;
  };
}

@addStaticProperties({ 
  VERSION: "1.0.0",
  MAX_INSTANCES: 100
})
class Config {
  static readonly VERSION: string;
  static readonly MAX_INSTANCES: number;
}

// ============================================
// SECTION 5: CLASS DECORATOR COMPOSITION
// ============================================

function observable(target: any) {
  const observed = new Map();
  return class ObservableClass extends target {
    constructor(...args: any[]) {
      super(...args);
      observed.set(this, {});
    }
    get observed() { return observed.get(this); }
  };
}

function serializable(constructor: Function) {
  return class extends constructor {
    toJSON() {
      return Object.fromEntries(
        Object.entries(this).filter(([k]) => !k.startsWith('_'))
      );
    }
  };
}

@observable
@serializable
class Document {
  constructor(public title: string, public content: string) {}
}

/**
 * PERFORMANCE:
 * - Class decorators execute once at definition time
 * - Extending classes adds prototype chain overhead
 * - Use sparingly in hot code paths
 * 
 * COMPATIBILITY:
 * - Requires experimentalDecorators in TS < 5.0
 * - Full decorator support in TS 5.0+
 * 
 * SECURITY:
 * - Don't expose private members via decorators
 * - Avoid storing sensitive data in class metadata
 * 
 * TESTING:
 * - Test decorator effects in isolation
 * - Verify class still instantiates correctly
 * 
 * DEBUGGING:
 * - Check constructor.name for debugging
 * - Use console.log in decorator factories
 * 
 * ALTERNATIVE:
 * - Mixins for multiple inheritance
 * - Factory pattern for configuration
 * 
 * CROSS-REFERENCE:
 * - 01_Advanced_Decorators.ts - Basic patterns
 * - 02_Decorator_Composition.ts - Composition
 * - 04_Method_Decorators.ts - Method decorators
 */

console.log("\n=== Class Decorators Demo ===");
const user1 = new User("Alice");
const user2 = new User("Bob");

console.log("\n=== Decorator with Parameters ===");
const service1 = new UserService();
const service2 = new UserService();
console.log("Same instance:", service1 === service2);

console.log("\n=== Static Properties ===");
console.log("Version:", Config.VERSION);
console.log("Max:", Config.MAX_INSTANCES);

console.log("\n=== Serializable ===");
const doc = new Document("Test", "Content");
console.log(doc.toJSON());