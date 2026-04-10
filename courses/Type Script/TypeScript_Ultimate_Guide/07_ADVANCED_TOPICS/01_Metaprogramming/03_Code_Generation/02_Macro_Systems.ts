/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 01_Metaprogramming
 * Concept: 03_Code_Generation
 * Topic: 02_Macro_Systems
 * Purpose: Learn about macro systems in TypeScript
 * Difficulty: advanced
 * UseCase: code-generation
 * Version: TS 5.0+
 * Compatibility: Node.js 16+, bundler plugins
 * Performance: Build-time expansion
 * Security: Macro expansion must be sandboxed
 */

/**
 * WHAT: Macros are compile-time code transformations that expand shorthand
 * syntax into more complex code. TypeScript supports macros through bundler
 * plugins and custom transformers.
 */

interface MacroContext {
  path: string;
  line: number;
  column: number;
}

type MacroHandler = (args: string[], context: MacroContext) => string;

const macros: Record<string, MacroHandler> = {
  "LOG": (args) => `console.log(${args.map(a => `'${a}'`).join(", ")})`,
  "TIMED": (args) => `console.time('${args[0]}'); try { ${args.slice(1).join(" ")} } finally { console.timeEnd('${args[0]}'); }`,
  "DEBUG": (args) => `if (process.env.DEBUG) { console.debug(${args.map(a => `'${a}'`).join(", ")}); }`,
  "ASSERT": (args) => `console.assert(${args.join(", ")})`,
};

function expandMacro(source: string): string {
  const macroPattern = /@(\w+)\(([^)]*)\)/g;
  return source.replace(macroPattern, (match, name, args) => {
    const handler = macros[name];
    if (!handler) return match;
    const argsList = args.split(",").map(a => a.trim());
    return handler(argsList, { path: "", line: 0, column: 0 });
  });
}

const sampleCode = `
function test() {
  @LOG("Starting operation");
  @TIMED("operation", "doWork()");
  @DEBUG("Value:", value);
  @ASSERT(condition, "Condition must be true");
}
`;

const expandedCode = expandMacro(sampleCode);
console.log("\n=== Macro Expansion ===");
console.log("Original:");
console.log(sampleCode);
console.log("\nExpanded:");
console.log(expandedCode);

class MacroSystem {
  private handlers = new Map<string, MacroHandler>();
  
  register(name: string, handler: MacroHandler) {
    this.handlers.set(name, handler);
  }
  
  expand(source: string): string {
    const pattern = /@(\w+)\(([^)]*)\)/g;
    return source.replace(pattern, (match, name, args) => {
      const handler = this.handlers.get(name);
      return handler ? handler(args.split(","), { path: "", line: 0, column: 0 }) : match;
    });
  }
}

const system = new MacroSystem();
system.register("ENV", (args) => `process.env['${args[0]}'] || '${args[1]}'`);
system.register("CONST", (args) => `const ${args[0]} = ${args[1]}`);

const code = `@ENV("API_URL", "http://localhost")`;
console.log("\n=== Macro System ===");
console.log("Expanded:", system.expand(code));

/**
 * PERFORMANCE:
 * - Macro expansion adds build time
 * - Caching can speed up repeated expansions
 * - Parallel processing for large files
 * 
 * COMPATIBILITY:
 * - Babel plugins for JS/TS
 * - Custom TypeScript transformers
 * - esbuild/wrollup plugins
 * 
 * SECURITY:
 * - Validate macro inputs
 * - Prevent code injection
 * - Sandbox macro execution
 * 
 * TESTING:
 * - Test each macro handler
 * - Verify expansion output
 * 
 * DEBUGGING:
 * - Log macro expansions
 * - Show source location in errors
 * 
 * CROSS-REFERENCE:
 * - 01_T4_Templating.ts - Template-based generation
 * - 03_AST_Transformations.ts - AST-based transformations
 */