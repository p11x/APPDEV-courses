/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 08
 * Topic: Decorator_Factories
 * Purpose: Creating configurable decorator factories
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TS 5.0+
 * Compatibility: ES2015+, Node 12+
 */

/**
 * Decorator Factories - Comprehensive Guide
 * ==========================================
 * 
 * 📚 WHAT: Functions that return decorators
 * 💡 WHY: Enable configurable and reusable decorators
 * 🔧 HOW: Factory functions with options
 */

// ============================================================================
// SECTION 1: VALUE FACTORIES
// ============================================================================

// Example 1.1: Configurable Decorator
// ---------------------------------

function configure(options: { verbose?: boolean; format?: string }) {
  return function<T extends Function>(target: T): T {
    return class extends target {
      constructor(...args: unknown[]) {
        super(...args);
        if (options.verbose) {
          console.log(`Created: ${target.name}`);
        }
      }
    } as T;
  };
}

@configure({ verbose: true })
class ConfiguredClass { }

// ============================================================================
// SECTION 2: FACTORY COMPOSITION
// ============================================================================

// Example 2.1: Compose Multiple Options
// ---------------------------------

function compose(options: { prefix?: string; suffix?: string }) {
  return function<T extends Function>(target: T): T {
    return class extends target {
      getName() {
        return `${options.prefix}${super.toString()}${options.suffix}`;
      }
    } as T;
  };
}

// ============================================================================
// SECTION 3: CONDITIONAL FACTORIES
// ============================================================================

// Example 3.1: Conditional Decorator
// -----------------------------

function debugMode(enabled: boolean) {
  return function<T extends Function>(target: T): T {
    if (!enabled) return target;
    return class extends target {
      debug() {
        console.log("Debug mode enabled");
      }
    } as T;
  };
}

// ============================================================================
// SECTION 4: MULTI-OPTION FACTORIES
// ============================================================================

// Example 4.1: Rich Configuration
// ---------------------------

function createDecorator(options: {
  prefix?: string;
  suffix?: string;
  log?: boolean;
  cache?: boolean;
}) {
  const defaults = { prefix: "[", suffix: "]", log: false, cache: false };
  const config = { ...defaults, ...options };
  
  return function<T extends Function>(target: T): T {
    return class extends target {
      static get prefix() { return config.prefix; }
      static get suffix() { return config.suffix; }
    } as T;
  };
}

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 5.0+
// All decoration types

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 01_Decorator_Basics, 02_Class_Decorators

console.log("\n=== Decorator Factories Complete ===");
console.log("Next: 08_Metaprogramming_Syntax/06_Metadata_Reflection");