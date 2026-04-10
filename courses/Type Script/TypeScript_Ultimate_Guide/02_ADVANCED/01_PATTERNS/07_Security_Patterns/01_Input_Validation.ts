/**
 * Category: 02_ADVANCED
 * Subcategory: 01_PATTERNS
 * Concept: 07_Security_Patterns
 * Topic: 01_Input_Validation
 * Purpose: Deep dive into Input Validation Patterns with TypeScript examples
 * Difficulty: intermediate
 * UseCase: web, backend, enterprise
 * Version: TS 5.0+
 * Compatibility: Browsers, Node.js
 * Performance: Depends on validation complexity
 * Security: Critical - prevents injection attacks
 */

/**
 * Input Validation Patterns - Comprehensive Guide
 * ===============================================
 * 
 * WHAT: Patterns for validating and sanitizing user input to prevent malicious
 * data from entering the system.
 * 
 * WHY:
 * - Prevent injection attacks
 * - Ensure data integrity
 * - Fail fast principle
 * - Defense in depth
 * 
 * HOW:
 * - Validate on input boundary
 * - Use allowlists
 * - Sanitize dangerous characters
 * - Validate types and ranges
 */

// ============================================================================
// SECTION 1: VALIDATION UTILITIES
// ============================================================================

interface ValidationResult {
  valid: boolean;
  errors: string[];
}

type Validator<T> = (value: T) => ValidationResult;

const isRequired = (value: any): boolean => {
  if (value === null || value === undefined) return false;
  if (typeof value === "string") return value.trim().length > 0;
  return true;
};

const isEmail = (value: string): boolean => {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
};

const isUrl = (value: string): boolean => {
  try {
    new URL(value);
    return true;
  } catch {
    return false;
  }
};

const isInRange = (value: number, min: number, max: number): boolean => {
  return value >= min && value <= max;
};

const hasMinLength = (value: string, min: number): boolean => {
  return value.length >= min;
};

const hasMaxLength = (value: string, max: number): boolean => {
  return value.length <= max;
};

// ============================================================================
// SECTION 2: SCHEMA VALIDATION
// ============================================================================

interface FieldSchema {
  required?: boolean;
  type: "string" | "number" | "boolean" | "object" | "array";
  minLength?: number;
  maxLength?: number;
  min?: number;
  max?: number;
  pattern?: RegExp;
  custom?: Validator<any>;
}

interface ObjectSchema {
  [key: string]: FieldSchema;
}

function validateObject(data: any, schema: ObjectSchema): ValidationResult {
  const errors: string[] = [];
  
  for (const [field, fieldSchema] of Object.entries(schema)) {
    const value = data[field];
    
    if (fieldSchema.required && !isRequired(value)) {
      errors.push(`${field} is required`);
      continue;
    }
    
    if (value !== undefined && value !== null) {
      if (typeof value !== fieldSchema.type) {
        errors.push(`${field} must be of type ${fieldSchema.type}`);
        continue;
      }
      
      if (fieldSchema.type === "string") {
        if (fieldSchema.minLength && !hasMinLength(value, fieldSchema.minLength)) {
          errors.push(`${field} must be at least ${fieldSchema.minLength} characters`);
        }
        if (fieldSchema.maxLength && !hasMaxLength(value, fieldSchema.maxLength)) {
          errors.push(`${field} must be at most ${fieldSchema.maxLength} characters`);
        }
        if (fieldSchema.pattern && !fieldSchema.pattern.test(value)) {
          errors.push(`${field} has invalid format`);
        }
      }
      
      if (fieldSchema.type === "number") {
        if (fieldSchema.min !== undefined && value < fieldSchema.min) {
          errors.push(`${field} must be at least ${fieldSchema.min}`);
        }
        if (fieldSchema.max !== undefined && value > fieldSchema.max) {
          errors.push(`${field} must be at most ${fieldSchema.max}`);
        }
      }
      
      if (fieldSchema.custom && !fieldSchema.custom(value).valid) {
        errors.push(...fieldSchema.custom(value).errors.map(e => `${field}: ${e}`));
      }
    }
  }
  
  return { valid: errors.length === 0, errors };
}

// ============================================================================
// SECTION 3: SANITIZATION
// ============================================================================

const sanitizeHtml = (input: string): string => {
  return input
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;")
    .replace(/'/g, "&#039;");
};

const sanitizeSql = (input: string): string => {
  return input.replace(/['";]/g, "");
};

const sanitizeFilename = (input: string): string => {
  return input.replace(/[^a-zA-Z0-9._-]/g, "_");
};

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

// Input Validation Performance:
// - Validation has overhead
// - Early validation saves resources
// - Consider allowlists

// ============================================================================
// COMPATIBILITY
// ============================================================================

// Compatible with:
// - TypeScript 1.6+
// - All ES targets
// - Node.js and browsers

// ============================================================================
// SECURITY CONSIDERATIONS
// ============================================================================

// Security is critical:
// - Validate ALL input
// - Use allowlists
// - Sanitize before use

// ============================================================================
// TESTING STRATEGY
// ============================================================================

// Testing validation:
// 1. Test valid inputs
// 2. Test invalid inputs
// 3. Test edge cases

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related patterns:
// - Sanitization Patterns (02_Sanitization_Patterns.ts)
// - Authentication Patterns (03_Authentication_Patterns.ts)
// - Authorization Patterns (04_Authorization_Patterns.ts)

// Next: 02_Sanitization_Patterns.ts
