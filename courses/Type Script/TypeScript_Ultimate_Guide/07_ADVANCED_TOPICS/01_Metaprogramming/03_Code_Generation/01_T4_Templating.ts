/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 01_Metaprogramming
 * Concept: 03_Code_Generation
 * Topic: 01_T4_Templating
 * Purpose: Understand T4 template-based code generation
 * Difficulty: intermediate
 * UseCase: code-generation
 * Version: TS 4.0+
 * Compatibility: Visual Studio, dotnet CLI
 * Performance: Build-time generation
 * Security: Review generated code for vulnerabilities
 */

/**
 * WHAT: T4 (Text Template Transformation Toolkit) is a text generation tool
 * used to generate code, XML, HTML, or other text files from templates.
 */

interface T4Template {
  transformText(): string;
}

type TemplateOutput = string;

interface Model {
  name: string;
  properties: Array<{ name: string; type: string; required: boolean }>;
}

function generateModelClass(model: Model): string {
  const props = model.properties.map(p => {
    const modifier = p.required ? "" : "?";
    return `  ${p.name}${modifier}: ${p.type};`;
  }).join("\n");
  
  return `export class ${model.name} {\n${props}\n}`;
}

function generateInterface(model: Model): string {
  const props = model.properties.map(p => {
    const modifier = p.required ? "" : "?";
    return `  ${p.name}${modifier}: ${p.type};`;
  }).join("\n");
  
  return `export interface I${model.name} {\n${props}\n}`;
}

function generateConstructor(model: Model): string {
  const params = model.properties.map(p => `public ${p.name}: ${p.type}`).join(", ");
  const assignments = model.properties.map(p => `this.${p.name} = ${p.name};`).join("\n    ");
  
  return `constructor(${params}) {\n    ${assignments}\n  }`;
}

function generateValidators(model: Model): string {
  const validators = model.properties
    .filter(p => p.required)
    .map(p => `if (!this.${p.name}) throw new Error("${p.name} is required");`)
    .join("\n    ");
  
  return `validate(): void {\n    ${validators || "// No required fields"}\n  }`;
}

const userModel: Model = {
  name: "User",
  properties: [
    { name: "id", type: "number", required: true },
    { name: "email", type: "string", required: true },
    { name: "name", type: "string", required: false },
    { name: "age", type: "number", required: false }
  ]
};

const generatedClass = generateModelClass(userModel);
const generatedInterface = generateInterface(userModel);
const generatedConstructor = generateConstructor(userModel);
const generatedValidators = generateValidators(userModel);

console.log("\n=== T4-Style Template Generation ===");
console.log("Generated Class:");
console.log(generatedClass);

console.log("\nGenerated Interface:");
console.log(generatedInterface);

console.log("\nGenerated Constructor:");
console.log(generatedConstructor);

console.log("\nGenerated Validator:");
console.log(generatedValidators);

/**
 * COMPATIBILITY:
 * - Visual Studio has built-in T4 support
 * - .NET CLI uses dotnet new tool
 * - Can be run via t4 CLI in Node.js
 * 
 * SECURITY:
 * - Sanitize template inputs
 * - Review generated code
 * - Avoid eval() with generated code
 * 
 * CROSS-REFERENCE:
 * - 02_Macro_Systems.ts - Macro systems
 * - 03_AST_Transformations.ts - AST transformations
 */