/**
 * Category: FUNDAMENTALS
 * Subcategory: SYNTAX
 * Concept: 08
 * Topic: Decorator_Basics
 * Purpose: Understanding TypeScript decorators
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TS 5.0+
 * Compatibility: ES2015+, Node 12+
 * Requires: experimentalDecorators in tsconfig
 */

/**
 * Decorator Basics - Comprehensive Guide
 * =======================================
 * 
 * 📚 WHAT: Decorators are expressions that evaluate to functions
 * 💡 WHY: Enable metaprogramming and declarative code composition
 * 🔧 HOW: @expression syntax, decorator composition
 */

// ============================================================================
// SECTION 1: DECORATOR SYNTAX
// ============================================================================

// Example 1.1: Basic Decorator
// ------------------------

function simpleDecorator(target: Function): void {
  console.log(`Decorating ${target.name}`);
}

@simpleDecorator
class MyClass { }

// ============================================================================
// SECTION 2: CLASS DECORATORS
// ============================================================================

// Example 2.1: Class Decorator Function
// ---------------------------------

function sealed(constructor: Function): void {
  Object.seal(constructor);
  Object.seal(constructor.prototype);
}

@sealed
class SealedClass { }

// ============================================================================
// SECTION 3: DECORATOR FACTORIES
// ============================================================================

// Example 3.1: Decorator Factory
// ---------------------------

function color(value: string) {
  return function(target: Function): void {
    target.prototype.color = value;
  };
}

@color("blue")
class ColoredClass { }

// ============================================================================
// SECTION 4: MULTIPLE DECORATORS
// ============================================================================

// Example 4.1: Multiple Decorators
// ---------------------------

function decorator1(target: Function): void {
  console.log("Decorator 1");
}

function decorator2(target: Function): void {
  console.log("Decorator 2");
}

@decorator1
@decorator2
class MultiDecorated { }

// ============================================================================
// SECTION 5: DECORATOR ORDER
// ============================================================================

// Example 5.1: Bottom-Up Execution
// ----------------------------

// Decorators execute bottom-up:
// @decorator2 runs first, then @decorator1

// ============================================================================
// PERFORMANCE
// ============================================================================

// Decorators add minimal overhead
// Executed at class definition time
// No per-instance overhead when cached

// ============================================================================
// COMPATIBILITY
// ============================================================================

// TS 5.0+ (decorator syntax)
// Requires experimentalDecorators
// Target ES2015+

// ============================================================================
// SECURITY
// ============================================================================

// Avoid storing sensitive data on prototypes
// Validate decorator inputs

// ============================================================================
// TESTING
// ============================================================================

// Test decorator execution
// Mock decorator functions

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related: 02_Class_Decorators, 03_Method_Decorators

console.log("\n=== Decorator Basics Complete ===");
console.log("Next: 08_Metaprogramming_Syntax/02_Class_Decorators");