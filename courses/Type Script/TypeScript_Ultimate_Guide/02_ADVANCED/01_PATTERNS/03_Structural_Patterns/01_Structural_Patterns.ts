/**
 * Category: ADVANCED
 * Subcategory: PATTERNS
 * Concept: Structural_Patterns
 * Purpose: Adapter, Decorator, and Facade patterns in TypeScript
 * Difficulty: intermediate
 * UseCase: web, backend, enterprise
 */

/**
 * Structural Patterns - Comprehensive Guide
 * ==========================================
 * 
 * 📚 WHAT: Patterns for object composition and structure
 * 💡 WHY: Creates flexible and efficient structures
 * 🔧 HOW: Adapter, Decorator, Facade, Proxy, Composite
 */

// ============================================================================
// SECTION 1: ADAPTER PATTERN
// ============================================================================

// Example 1.1: Class Adapter
// -----------------------

interface Target {
  request(): string;
}

class Adaptee {
  specificRequest(): string {
    return "Specific behavior";
  }
}

class Adapter implements Target {
  constructor(private adaptee: Adaptee) {}
  
  request(): string {
    return `Adapter: ${this.adaptee.specificRequest()}`;
  }
}

// Example 1.2: Interface Adapter
// ---------------------------

interface ILogger {
  log(message: string): void;
  error(message: string): void;
  warn(message: string): void;
}

class Logger implements ILogger {
  log(message: string): void { console.log(`LOG: ${message}`); }
  error(message: string): void { console.error(`ERROR: ${message}`); }
  warn(message: string): void { console.warn(`WARN: ${message}`); }
}

class LoggerAdapter implements ILogger {
  constructor(private logger: Logger) {}
  
  log(message: string): void { this.logger.log(message); }
  error(message: string): void { this.logger.error(message); }
  warn(message: string): void { this.logger.warn(message); }
}

// ============================================================================
// SECTION 2: DECORATOR PATTERN
// ============================================================================

// Example 2.1: Basic Decorator
// -----------------------

interface Coffee {
  getCost(): number;
  getDescription(): string;
}

class SimpleCoffee implements Coffee {
  getCost(): number { return 10; }
  getDescription(): string { return "Coffee"; }
}

class MilkDecorator implements Coffee {
  constructor(private coffee: Coffee) {}
  
  getCost(): number { return this.coffee.getCost() + 2; }
  getDescription(): string { return this.coffee.getDescription() + ", Milk"; }
}

class SugarDecorator implements Coffee {
  constructor(private coffee: Coffee) {}
  
  getCost(): number { return this.coffee.getCost() + 1; }
  getDescription(): string { return this.coffee.getDescription() + ", Sugar"; }
}

// Usage
let coffee: Coffee = new SimpleCoffee();
coffee = new MilkDecorator(coffee);
coffee = new SugarDecorator(coffee);

// ============================================================================
// SECTION 3: FACADE PATTERN
// ============================================================================

// Example 3.1: Facade Implementation
// ---------------------------------

class CPU {
  freeze(): void { console.log("Freezing"); }
  jump(position: number): void { console.log(`Jumping to ${position}`); }
  execute(): void { console.log("Executing"); }
}

class Memory {
  load(position: number, data: string): void {
    console.log(`Loading ${data} at ${position}`);
  }
}

class HardDrive {
  read(sector: number, size: number): string {
    return "data";
  }
}

class ComputerFacade {
  private cpu = new CPU();
  private memory = new Memory();
  private hardDrive = new HardDrive();
  
  start(): void {
    this.cpu.freeze();
    this.memory.load(0, this.hardDrive.read(0, 1024));
    this.cpu.jump(0);
    this.cpu.execute();
  }
}

// ============================================================================
// SECTION 4: PROXY PATTERN
// ============================================================================

// Example 4.1: Virtual Proxy
// -----------------------

interface Subject {
  request(): void;
}

class RealSubject implements Subject {
  request(): void { console.log("Real request"); }
}

class ProxySubject implements Subject {
  private realSubject?: RealSubject;
  
  request(): void {
    if (!this.realSubject) {
      this.realSubject = new RealSubject();
    }
    this.realSubject.request();
  }
}

console.log("\n=== Structural Patterns Complete ===");
console.log("Next: ADVANCED/PATTERNS/04_Behavioral_Patterns");