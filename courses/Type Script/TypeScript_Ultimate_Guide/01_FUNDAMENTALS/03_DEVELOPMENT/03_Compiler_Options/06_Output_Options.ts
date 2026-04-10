/**
 * Category: 01_FUNDAMENTALS Subcategory: 03_DEVELOPMENT Concept: 03 Topic: Output Options Purpose: Configuring output paths and formats Difficulty: beginner UseCase: development,production,build Version: TS1.0+ Compatibility: Node.js, Browsers Performance: Build output Security: Output path validation */

/**
 * Output Options - Comprehensive Guide
 * ====================================
 * 
 * 📚 WHAT: Configuring output paths and formats
 * 💡 WHY: Control where compiled files are placed and how they're structured
 * 🔧 HOW: outDir, outFile, rootDir options in tsconfig.json
 */

// ============================================================================
// SECTION 1: WHAT ARE OUTPUT OPTIONS
// ============================================================================

// Example 1.1: Basic Concept
// -----------------------

// Output options control where compiled JavaScript goes and how it's structured

interface OutputOption {
  purpose: string;
  affects: string[];
}

const outputInfo: OutputOption = {
  purpose: "Configure compiled output location and format",
  affects: ["File location", "Directory structure", "Bundle format"]
};

// ============================================================================
// SECTION 2: OUTPUT DIRECTORY
// ============================================================================

// Example 2.1: Basic Output Directory
// -------------------------------

interface BasicOutputConfig {
  compilerOptions: {
    outDir: string;
  };
}

const basicOutputConfig: BasicOutputConfig = {
  compilerOptions: {
    outDir: "./dist"
  }
};

// Example 2.2: Nested Output Directory
// -------------------------------

interface NestedOutputConfig {
  compilerOptions: {
    rootDir: string;
    outDir: string;
  };
}

const nestedOutputConfig: NestedOutputConfig = {
  compilerOptions: {
    rootDir: "./src",
    outDir: "./build/output"
  }
};

// ============================================================================
// SECTION 3: ROOT DIRECTORY
// ============================================================================

// Example 3.1: Root Directory
// -----------------------

interface RootDirConfig {
  compilerOptions: {
    rootDir: string;
  };
}

const rootDirConfig: RootDirConfig = {
  compilerOptions: {
    rootDir: "./src"
  }
};

// Example 3.2: RootDir and OutDir Relationship
// -------------------------------------

interface RootOutDirExample {
  source: string;
  output: string;
}

const rootOutDirExample: RootOutDirExample = {
  source: "src/utils/helper.ts",
  output: "dist/utils/helper.js"
};

// ============================================================================
// SECTION 4: OUTFILE (AMD/SYSTEM)
// ============================================================================

// Example 4.1: OutFile for Bundling
// -------------------------------

interface OutFileConfig {
  compilerOptions: {
    outFile: string;
    module: string;
  };
}

const outFileConfig: OutFileConfig = {
  compilerOptions: {
    outFile: "./dist/bundle.js",
    module: "amd"
  }
};

// ============================================================================
// SECTION 5: DECLARATION OUTPUT
// ============================================================================

// Example 5.1: Declaration Output
// -----------------------------

interface DeclarationConfig {
  compilerOptions: {
    declaration: boolean;
    declarationDir: string;
  };
}

const declarationConfig: DeclarationConfig = {
  compilerOptions: {
    declaration: true,
    declarationDir: "./dist/types"
  }
};

// ============================================================================
// SECTION 6: SOURCE MAP OUTPUT
// ============================================================================

// Example 6.1: Source Map Configuration
// -------------------------------

interface SourceMapOutputConfig {
  compilerOptions: {
    sourceMap: boolean;
    sourceMapDir: string;
    mapRoot: string;
    sourceRoot: string;
  };
}

const sourceMapOutputConfig: SourceMapOutputConfig = {
  compilerOptions: {
    sourceMap: true,
    sourceMapDir: "./dist/maps",
    mapRoot: "",
    sourceRoot: ""
  }
};

// ============================================================================
// SECTION 7: REMOVE COMMENTS
// ============================================================================

// Example 7.1: Comment Removal
// -----------------------

interface RemoveCommentsConfig {
  compilerOptions: {
    removeComments: boolean;
  };
}

const removeCommentsConfig: RemoveCommentsConfig = {
  compilerOptions: {
    removeComments: true
  }
};

// ============================================================================
// SECTION 8: NO EMIT ON ERROR
// ============================================================================

// Example 8.1: No Emit on Error
// -----------------------

interface NoEmitOnErrorConfig {
  compilerOptions: {
    noEmitOnError: boolean;
  };
}

const noEmitOnErrorConfig: NoEmitOnErrorConfig = {
  compilerOptions: {
    noEmitOnError: true
  }
};

// ============================================================================
// SECTION 9: EMIT DECLARATION ONLY
// ============================================================================

// Example 9.1: Emit Declaration Only
// -----------------------------

interface EmitDeclOnlyConfig {
  compilerOptions: {
    emitDeclarationOnly: boolean;
  };
}

const emitDeclOnlyConfig: EmitDeclOnlyConfig = {
  compilerOptions: {
    emitDeclarationOnly: true
  }
};

// ============================================================================
// SECTION 10: PRESERVE CONST ENUM
// ============================================================================

// Example 10.1: Preserve Const Enum
// -----------------------------

interface PreserveConstEnumConfig {
  compilerOptions: {
    preserveConstEnums: boolean;
  };
}

const preserveConstEnumConfig: PreserveConstEnumConfig = {
  compilerOptions: {
    preserveConstEnums: false
  }
};

// ============================================================================
// SECTION 11: STRIP INTERNAL
// ============================================================================

// Example 11.1: Strip Internal
// -----------------------

interface StripInternalConfig {
  compilerOptions: {
    stripInternal: boolean;
  };
}

const stripInternalConfig: StripInternalConfig = {
  compilerOptions: {
    stripInternal: true
  }
};

// ============================================================================
// SECTION 12: COMPATIBILITY
// ============================================================================

// Example 12.1: Output Compatibility
// -------------------------------

interface OutputCompatibility {
  node: string;
  browser: string;
}

const outputCompat: OutputCompatibility = {
  node: "CommonJS output, .js and .d.ts",
  browser: "ESM or bundled output"
};

// ============================================================================
// SECTION 13: PERFORMANCE
// ============================================================================

// Example 13.1: Output Performance
// -----------------------

interface OutputPerformance {
  declarations: string;
  sourceMaps: string;
}

const outputPerf: OutputPerformance = {
  declarations: "Generate .d.ts for type checking",
  sourceMaps: "Generate .map for debugging"
};

// ============================================================================
// SECTION 14: SECURITY
// ============================================================================

// Example 14.1: Output Security
// -----------------------

interface OutputSecurity {
  noEmit: string;
  validation: string;
}

const outputSecurity: OutputSecurity = {
  noEmit: "Use --noEmit for type checking only",
  validation: "Validate output paths are within project"
};

// ============================================================================
// SECTION 15: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 03_DEVELOPMENT/03_Compiler_Options/01_Compiler_Flags.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/08_Declaration_Maps.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/09_Source_Maps.ts
// - 03_DEVELOPMENT/02_TypeScript_Workspace/10_Declaration_Files.ts

console.log("\n=== Output Options Complete ===");
console.log("Next: FUNDAMENTALS/DEVELOPMENT/03_Compiler_Options/07_Watch_Options");