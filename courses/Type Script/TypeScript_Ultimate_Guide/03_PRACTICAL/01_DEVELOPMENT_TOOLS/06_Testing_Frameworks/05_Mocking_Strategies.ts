/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: Testing_Frameworks
 * Purpose: Mocking strategies for TypeScript
 * Difficulty: intermediate
 * UseCase: web, backend
 */

/**
 * Mocking Strategies - Comprehensive Guide
 * =====================================
 * 
 * 📚 WHAT: Mocking techniques in TypeScript tests
 * 💡 WHERE: Unit testing with dependencies
 * 🔧 HOW: Jest mocks, sinon, manual mocks
 */

// ============================================================================
// SECTION 1: JEST MOCKS
// ============================================================================

// Example 1.1: Function Mock
// ---------------------------------

/*
// Mock implementation
const myMock = jest.fn();
myMock.mockReturnValue(42);
myMock.mockReturnValueOnce("first call");

// Mock with implementation
const mockFn = jest.fn((x) => x * 2);
*/

// Example 1.2: Module Mock
// ---------------------------------

/*
jest.mock("../api");
import { fetchUsers } from "../api";
*/

// ============================================================================
// SECTION 2: MANUAL MOCKS
// ============================================================================

// Example 2.1: Manual Mock
// ---------------------------------

/*
__mocks__/fs.js
export const readFile = jest.fn();
export const writeFile = jest.fn();
*/

// ============================================================================
// SECTION 3: SPY
// ============================================================================

// Example 3.1: Method Spy
// ---------------------------------

/*
const obj = { method: () => "real" };
const spy = jest.spyOn(obj, "method");
obj.method();
expect(spy).toHaveBeenCalled();
spy.mockRestore();
*/

console.log("\n=== Mocking Strategies Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/06_Testing_Frameworks/06_Integration_Testing.ts");