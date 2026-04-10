/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: Code_Generation
 * Purpose: API client generation from TypeScript
 * Difficulty: intermediate
 * UseCase: web, backend
 */

/**
 * API Client Generation - Comprehensive Guide
 * ========================================
 * 
 * 📚 WHAT: Generating type-safe API clients
 * 💡 WHERE: Auto-generated API clients
 * 🔧 HOW: Code generators, type definitions
 */

// ============================================================================
// SECTION 1: OPENAPI GENERATION
// ============================================================================

// Example 1.1: OpenAPI to TypeScript
// ---------------------------------

interface OpenAPISpec {
  openapi: string;
  info: { title: string; version: string };
  paths: Record<string, PathItem>;
  components: { schemas: Record<string, Schema> };
}

interface PathItem {
  get?: Operation;
  post?: Operation;
}

interface Operation {
  operationId: string;
  parameters?: Parameter[];
  responses: Record<string, Response>;
}

interface Parameter {
  name: string;
  in: string;
  schema: Schema;
}

interface Schema {
  type: string;
  properties?: Record<string, Schema>;
}

function generateClient(spec: OpenAPISpec): string {
  let output = 'import axios from "axios";\n\n';
  output += `export const api = axios.create({ baseURL: "" });\n\n`;
  
  for (const [path, item] of Object.entries(spec.paths)) {
    if (item.get) {
      output += generateMethod("get", path, item.get);
    }
    if (item.post) {
      output += generateMethod("post", path, item.post);
    }
  }
  
  return output;
}

function generateMethod(method: string, path: string, op: Operation): string {
  const name = op.operationId;
  return `export async function ${name}() {\n`;
}

// ============================================================================
// SECTION 2: TYPE GENERATION
// ============================================================================

// Example 2.1: Schema to TypeScript
// ---------------------------------

interface TypeGenerationOptions {
  explicitTypes: boolean;
  optionalProperties: boolean;
}

function generateTypes(spec: OpenAPISpec): string {
  let output = "";
  
  for (const [name, schema] of Object.entries(spec.components.schemas)) {
    output += `interface ${name} {\n`;
    if (schema.properties) {
      for (const [prop, propSchema] of Object.entries(schema.properties)) {
        output += `  ${prop}: ${mapSchemaType(propSchema)};\n`;
      }
    }
    output += "}\n\n";
  }
  
  return output;
}

function mapSchemaType(schema: Schema): string {
  switch (schema.type) {
    case "string": return "string";
    case "integer":
    case "number": return "number";
    case "boolean": return "boolean";
    case "array": return `${mapSchemaType({ type: "object" })}[]`;
    default: return "any";
  }
}

console.log("\n=== API Client Generation Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/03_Code_Generation/03_Type_Safety_Generation.ts");