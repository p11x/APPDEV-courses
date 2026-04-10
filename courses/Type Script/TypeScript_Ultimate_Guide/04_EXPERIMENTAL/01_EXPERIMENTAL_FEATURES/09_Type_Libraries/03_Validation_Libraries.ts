/**
 * Category: EXPERIMENTAL
 * Subcategory: EXPERIMENTAL_FEATURES
 * Concept: 09_Type_Libraries
 * Topic: Validation_Libraries
 * Purpose: TypeScript validation libraries and patterns
 * Difficulty: intermediate
 * UseCase: web, backend
 * Version: TypeScript 4.1+
 * Compatibility: Modern browsers, Node.js 12+
 * Performance: Runtime validation overhead
 * Security: Input validation and sanitization
 */

/**
 * Validation Libraries - Comprehensive Guide
 * ===========================================
 * 
 * 📚 WHAT: TypeScript validation libraries and patterns
 * 💡 WHERE: Form validation, API input validation, data parsing
 * 🔧 HOW: Schema-based validation, type guards, runtime types
 */

// ============================================================================
// SECTION 1: WHAT - Validation Libraries
// ============================================================================

/**
 * WHAT are validation libraries?
 * - Runtime type validation
 * - Schema-based data validation
 * - Type guards and assertions
 * - Error message generation
 */

// ============================================================================
// SECTION 2: WHY - Use Cases
// ============================================================================

/**
 * WHY use validation libraries?
 * - Validate user input
 * - Parse external data (API, files)
 * - Type narrowing at runtime
 * - Form validation
 */

// ============================================================================
// SECTION 3: HOW - Implementation
// ============================================================================

// Example 3.1: Zod Basic Usage
// --------------------------------

/*
import { z } from 'zod';

const UserSchema = z.object({
  name: z.string().min(2),
  email: z.string().email(),
  age: z.number().min(0).optional(),
});

type User = z.infer<typeof UserSchema>;
// { name: string; email: string; age?: number }

const validUser = UserSchema.parse({ name: 'John', email: 'john@example.com' });
const invalidUser = UserSchema.safeParse({ name: 'J', email: 'invalid' });
*/

// Example 3.2: Yup Schema Validation
// --------------------------------

/*
import * as yup from 'yup';

const schema = yup.object({
  name: yup.string().required().min(2),
  email: yup.string().required().email(),
  age: yup.number().positive().optional(),
});

type YupUser = yup.InferType<typeof schema>;
// { name: string; email: string; age?: number }

await schema.validate({ name: 'John', email: 'john@example.com' });
*/

// Example 3.3: io-ts Type Validation
// --------------------------------

/*
import * as t from 'io-ts';

const UserCodec = t.type({
  name: t.string,
  email: t.string,
  age: t.union([t.number, t.undefined]),
});

type IotsUser = t.TypeOf<typeof UserCodec>;
// { name: string; email: string; age?: number }

const result = UserCodec.decode({ name: 'John', email: 'john@example.com' });
*/

// Example 3.4: Custom Type Guard
// --------------------------------

function isString(value: unknown): value is string {
  return typeof value === 'string';
}

function isNumber(value: unknown): value is number {
  return typeof value === 'number' && !isNaN(value);
}

function isUser(value: unknown): value is { name: string; email: string } {
  return (
    typeof value === 'object' &&
    value !== null &&
    isString((value as any).name) &&
    isString((value as any).email)
  );
}

// Example 3.5: Valibot Schema
// --------------------------------

/*
import { object, string, number, optional, parse } from 'valibot';

const UserSchema = object({
  name: string([minLength(2)]),
  email: string([email()]),
  age: optional(number([minValue(0))]),
});

type ValibotUser = Input<typeof UserSchema>;
// { name: string; email: string; age?: number }

const user = parse(UserSchema, { name: 'John', email: 'john@example.com' });
*/

// Example 3.6: Runtime Type Validation
// --------------------------------

type RuntimeValidator<T> = {
  validate: (value: unknown) => value is T;
  parse: (value: unknown) => T;
  schema: unknown;
};

function createValidator<T>(validator: (v: unknown) => v is T): RuntimeValidator<T> {
  return {
    validate: validator,
    parse: (value: unknown) => {
      if (validator(value)) return value;
      throw new Error('Validation failed');
    },
    schema: null,
  };
}

const StringValidator = createValidator(isString);
const NumberValidator = createValidator(isNumber);

// Example 3.7: Union Type Validation
// --------------------------------

type DiscriminatedUnion<T extends string> = {
  type: T;
} & { [K in T]: any };

function validateUnion<T extends string>(
  value: unknown,
  discriminators: Record<T, (v: unknown) => boolean>
): value is DiscriminatedUnion<T> {
  if (typeof value !== 'object' || value === null) return false;
  const type = (value as any).type;
  return type in discriminators && discriminators[type as T](value);
}

// ============================================================================
// SECTION 4: PERFORMANCE
// ============================================================================

/**
 * Performance:
 * - Runtime validation adds execution overhead
 * - Schema validation slower than simple checks
 * - Caching can help with repeated validations
 */

// ============================================================================
// SECTION 5: COMPATIBILITY
// ============================================================================

/**
 * Compatibility:
 * - Works in all TypeScript environments
 * - Works in browsers and Node.js
 */

// ============================================================================
// SECTION 6: SECURITY
// ============================================================================

/**
 * Security:
 * - Sanitize before validation
 * - Validate all external input
 * - Use strict validation rules
 */

// ============================================================================
// SECTION 7: TESTING
// ============================================================================

/**
 * Testing:
 * - Test valid and invalid inputs
 * - Test edge cases and boundaries
 */

// ============================================================================
// SECTION 8: DEBUGGING
// ============================================================================

/**
 * Debugging:
 * - Use safeParse for detailed errors
 * - Check validation result.isSuccess
 */

// ============================================================================
// SECTION 9: ALTERNATIVE
// ============================================================================

/**
 * Alternatives:
 * - Manual validation functions
 * - Class-validator decorators
 * - JSON Schema validation
 */

// ============================================================================
// SECTION 10: CROSS-REFERENCE
// ============================================================================

console.log("\n=== Validation Libraries Complete ===");
console.log("Previous: 01_Typed_Query_Builders.ts, 02_Type_Safe_API_Clients.ts");
console.log("Related: 08_Advanced_Type_Utilities/04_Deep_Partial_Types.ts, 07_AI_Assisted_Development/03_Type_Generation_AI.ts");