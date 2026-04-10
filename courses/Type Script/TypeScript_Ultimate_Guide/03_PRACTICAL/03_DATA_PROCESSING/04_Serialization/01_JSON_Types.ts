/**
 * Category: PRACTICAL
 * Subcategory: DATA_PROCESSING
 * Concept: Serialization
 * Purpose: Data serialization in TypeScript
 * Difficulty: beginner
 * UseCase: web, backend
 */

/**
 * Serialization - Comprehensive Guide
 * ==================================
 * 
 * 📚 WHAT: Converting data between formats
 * 💡 WHERE: API communication, storage
 * 🔧 HOW: JSON, serialization libraries
 */

// ============================================================================
// SECTION 1: JSON SERIALIZATION
// ============================================================================

// Example 1.1: Basic JSON Operations
// ---------------------------------

interface User {
  id: number;
  name: string;
  email: string;
  createdAt: Date;
}

const user: User = {
  id: 1,
  name: "John",
  email: "john@example.com",
  createdAt: new Date()
};

// Serialize to JSON string
const jsonString = JSON.stringify(user);
console.log("JSON String:", jsonString);

// Parse JSON string
const parsed = JSON.parse(jsonString) as User;
console.log("Parsed:", parsed);

// ============================================================================
// SECTION 2: CUSTOM SERIALIZATION
// ============================================================================

// Example 2.1: Custom toJSON
// ---------------------------------

class SerializableUser {
  constructor(
    public id: number,
    public name: string,
    public email: string,
    public createdAt: Date
  ) {}
  
  toJSON(): Record<string, unknown> {
    return {
      id: this.id,
      name: this.name,
      email: this.email,
      createdAt: this.createdAt.toISOString()
    };
  }
  
  static fromJSON(data: Record<string, unknown>): SerializableUser {
    return new SerializableUser(
      data.id as number,
      data.name as string,
      data.email as string,
      new Date(data.createdAt as string)
    );
  }
}

// ============================================================================
// SECTION 3: SERIALIZATION LIBRARIES
// ============================================================================

// Example 3.1: Using class-transformer
// ---------------------------------

/*
import { plainToInstance, instanceToPlain } from "class-transformer";

class User {
  id: number;
  name: string;
}

const plain = { id: 1, name: "John" };
const instance = plainToInstance(User, plain);

const backToPlain = instanceToPlain(instance);
*/

console.log("\n=== Serialization Complete ===");
console.log("Next: PRACTICAL/UI_DEVELOPMENT/01_React_Integration/05_Redux_TypeScript.ts");