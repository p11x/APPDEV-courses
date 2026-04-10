/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: Documentation_Tools
 * Purpose: TypeDoc configuration and setup
 * Difficulty: intermediate
 * UseCase: web, backend
 */

/**
 * TypeDoc Setup - Comprehensive Guide
 * ==================================
 * 
 * 📚 WHAT: Generating documentation from TypeScript
 * 💡 WHERE: API documentation
 * 🔧 HOW: TypeDoc configured
 */

// ============================================================================
// SECTION 1: TYPEDOC CONFIG
// ============================================================================

// Example 1.1: TypeDoc Configuration
// ---------------------------------

interface TypeDocOptions {
  entryPoints: string[];
  out: string;
  name: string;
  includeVersion: boolean;
  excludeProtected: boolean;
  excludePrivate: boolean;
  excludeInternal: boolean;
  plugin: string[];
  theme: string;
  readme: string;
}

const typedocOptions: TypeDocOptions = {
  entryPoints: ["src/index.ts"],
  out: "docs",
  name: "My Project API",
  includeVersion: true,
  excludeProtected: false,
  excludePrivate: true,
  excludeInternal: true,
  plugin: [],
  theme: "default",
  readme: "README.md"
};

// ============================================================================
// SECTION 2: MARKDOWN PLUGIN
// ============================================================================

// Example 2.1: Markdown Configuration
// ---------------------------------

interface MarkdownOptions {
  theme: string;
  pages: string;
  filename: string;
}

const markdownPlugin: TypeDocOptions = {
  ...typedocOptions,
  plugin: ["typedoc-plugin-markdown"]
};

// ============================================================================
// SECTION 3: GITBOOK THEME
// ============================================================================

// Example 3.1: GitBook Theme
// ---------------------------------

const gitbookTheme: TypeDocOptions = {
  ...typedocOptions,
  plugin: ["typedoc-plugin-gitbook"]
};

console.log("\n=== TypeDoc Setup Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/05_CI_CD_Integration/02_GitLab_CI.ts");