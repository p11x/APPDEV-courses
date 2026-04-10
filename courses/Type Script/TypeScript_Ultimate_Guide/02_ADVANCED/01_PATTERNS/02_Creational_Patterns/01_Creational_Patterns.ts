/**
 * Category: ADVANCED
 * Subcategory: PATTERNS
 * Concept: Creational_Patterns
 * Purpose: Factory, Builder, and Singleton patterns in TypeScript
 * Difficulty: intermediate
 * UseCase: web, backend, enterprise
 */

/**
 * Creational Patterns - Comprehensive Guide
 * =========================================
 * 
 * 📚 WHAT: Patterns for object creation mechanisms
 * 💡 WHY: Provides flexibility in object instantiation
 * 🔧 HOW: Factory, Builder, Singleton, Prototype patterns
 */

// ============================================================================
// SECTION 1: FACTORY PATTERN
// ============================================================================

// Example 1.1: Simple Factory
// -----------------------

interface Button {
  render(): void;
}

class WindowsButton implements Button {
  render(): void {
    console.log("Rendering Windows button");
  }
}

class MacButton implements Button {
  render(): void {
    console.log("Rendering Mac button");
  }
}

class ButtonFactory {
  static createButton(os: "windows" | "mac"): Button {
    switch (os) {
      case "windows": return new WindowsButton();
      case "mac": return new MacButton();
    }
  }
}

// Example 1.2: Generic Factory
// -----------------------

interface Factory<T> {
  create(): T;
}

class UserFactory implements Factory<User> {
  create(): User {
    return { name: "Default User", id: 0 };
  }
}

interface User {
  name: string;
  id: number;
}

// ============================================================================
// SECTION 2: BUILDER PATTERN
// ============================================================================

// Example 2.1: Builder Interface
// ---------------------------

interface Builder<T> {
  reset(): this;
  build(): T;
}

class UserBuilder implements Builder<User> {
  private user: Partial<User> = {};
  
  reset(): this {
    this.user = {};
    return this;
  }
  
  setName(name: string): this {
    this.user.name = name;
    return this;
  }
  
  setId(id: number): this {
    this.user.id = id;
    return this;
  }
  
  build(): User {
    return this.user as User;
  }
}

const user = new UserBuilder()
  .reset()
  .setName("John")
  .setId(1)
  .build();

// ============================================================================
// SECTION 3: SINGLETON PATTERN
// ============================================================================

// Example 3.1: Basic Singleton
// -----------------------

class Singleton {
  private static instance: Singleton;
  
  private constructor() {}
  
  static getInstance(): Singleton {
    if (!Singleton.instance) {
      Singleton.instance = new Singleton();
    }
    return Singleton.instance;
  }
  
  operation(): void {
    console.log("Singleton operation");
  }
}

// Example 3.2: Singleton with TypeScript
// ---------------------------------

const singletonInstance = (() => {
  let instance: DatabaseConnection | null = null;
  
  return {
    getInstance(): DatabaseConnection {
      if (!instance) {
        instance = {
          connect: () => console.log("Connected"),
          disconnect: () => console.log("Disconnected")
        };
      }
      return instance;
    }
  };
})();

interface DatabaseConnection {
  connect(): void;
  disconnect(): void;
}

// ============================================================================
// SECTION 4: PROTOTYPE PATTERN
// ============================================================================

// Example 4.1: Clone Method
// -----------------------

interface Cloneable<T> {
  clone(): T;
}

class Document implements Cloneable<Document> {
  constructor(
    public title: string,
    public content: string
  ) {}
  
  clone(): Document {
    return new Document(this.title, this.content);
  }
}

const original = new Document("Original", "Content");
const clone = original.clone();

console.log("\n=== Creational Patterns Complete ===");
console.log("Next: ADVANCED/PATTERNS/03_Structural_Patterns");