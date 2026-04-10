/**
 * Category: PRACTICAL
 * Subcategory: DATA_PROCESSING
 * Concept: Data_Validation
 * Purpose: Data validation techniques in TypeScript
 * Difficulty: intermediate
 * UseCase: web, backend
 */

/**
 * Data Validation - Comprehensive Guide
 * ==================================
 * 
 * 📚 WHAT: Validating data with TypeScript
 * 💡 WHERE: API inputs, form data
 * 🔧 HOW: Schema validation, type guards
 */

// ============================================================================
// SECTION 1: MANUAL VALIDATION
// ============================================================================

// Example 1.1: Basic Validation Function
// ---------------------------------

interface ValidationResult {
  valid: boolean;
  errors: string[];
}

function validateUser(data: unknown): ValidationResult {
  const errors: string[] = [];
  
  if (typeof data !== "object" || data === null) {
    return { valid: false, errors: ["Data must be an object"] };
  }
  
  const obj = data as Record<string, unknown>;
  
  if (typeof obj.name !== "string" || obj.name.length < 2) {
    errors.push("Name must be at least 2 characters");
  }
  
  if (typeof obj.email !== "string" || !obj.email.includes("@")) {
    errors.push("Invalid email");
  }
  
  return { valid: errors.length === 0, errors };
}

// ============================================================================
// SECTION 2: SCHEMA VALIDATION
// ============================================================================

// Example 2.1: Zod Schema
// ---------------------------------

// npm install zod

/*
import { z } from "zod";

const userSchema = z.object({
  name: z.string().min(2),
  email: z.string().email(),
  age: z.number().min(0).optional()
});

type User = z.infer<typeof userSchema>;

function parseUser(data: unknown): User {
  return userSchema.parse(data);
}
*/

// ============================================================================
// SECTION 3: CONDITIONAL VALIDATION
// ============================================================================

// Example 3.1: Dependent Fields
// ---------------------------------

interface FormData {
  type: "personal" | "business";
  name: string;
  company?: string;
  companySize?: number;
}

function validateForm(data: FormData): ValidationResult {
  const errors: string[] = [];
  
  if (!data.name || data.name.length < 2) {
    errors.push("Name is required");
  }
  
  if (data.type === "business") {
    if (!data.company) {
      errors.push("Company name is required for business");
    }
    if (!data.companySize || data.companySize < 1) {
      errors.push("Company size must be at least 1");
    }
  }
  
  return { valid: errors.length === 0, errors };
}

console.log("\n=== Data Validation Complete ===");
console.log("Next: PRACTICAL/DATA_PROCESSING/04_Serialization/01_JSON_Types.ts");