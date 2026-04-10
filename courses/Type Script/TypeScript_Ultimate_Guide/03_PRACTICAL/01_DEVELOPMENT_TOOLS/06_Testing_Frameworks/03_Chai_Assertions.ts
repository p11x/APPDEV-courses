/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: Testing_Frameworks
 * Purpose: Chai assertions for TypeScript tests
 * Difficulty: beginner
 * UseCase: web, backend
 */

/**
 * Chai Assertions - Comprehensive Guide
 * ====================================
 * 
 * 📚 WHAT: Assertions library for testing
 * 💡 WHERE: BDD/TDD style assertions
 * 🔧 HOW: Expect, should, assert API
 */

// ============================================================================
// SECTION 1: EXPECT API
// ============================================================================

// Example 1.1: Basic Expectations
// ---------------------------------

import { expect } from "chai";

// Equality
expect(1 + 1).to.equal(2);
expect("hello").to.equal("hello");
expect({ a: 1 }).to.deep.equal({ a: 1 });

// Boolean
expect(true).to.be.true;
expect(false).to.not.be.true;

// Null
expect(null).to.be.null;
expect(undefined).to.be.undefined;

// Truthy/Falsy
expect("test").to.be truthy;
expect(0).to.be.falsy;

// Numeric
expect(10).to.be.greaterThan(5);
expect(10).to.be.lessThan(15);
expect(10).to.be.within(1, 20);

// ============================================================================
// SECTION 2: ASSERT API
// ============================================================================

// Example 2.1: Assert Methods
// ---------------------------------

import assert from "chai";

// Basic
assert.equal(1, 1);
assert.notEqual(1, 2);
assert.deepEqual({ a: 1 }, { a: 1 });

// Boolean
assert.isTrue(true);
assert.isFalse(false);

// Null
assert.isNull(null);
assert.isNotNull("test");

// Arrays
assert.isArray([]);
assert.isNotArray({});

// ============================================================================
// SECTION 3: CUSTOM ASSERTIONS
// ============================================================================

// Example 3.1: Custom Assertions
// ---------------------------------

import { Assertion } from "chai";

Assertion.addMethod("withinRange", function(min: number, max: number) {
  const actual = this._obj;
  this.assert(
    actual >= min && actual <= max,
    `expected #{this} to be within range ${min}-${max}`,
    `expected #{this} to not be within range ${min}-${max}`
  );
});

// Usage
expect(10).to.withinRange(5, 15);

console.log("\n=== Chai Assertions Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/06_Testing_Frameworks/04_Test_Utilities.ts");