/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: Build_Automation
 * Purpose: Webpack configuration for TypeScript projects
 * Difficulty: intermediate
 * UseCase: web, backend
 */

/**
 * Webpack Configuration - Comprehensive Guide
 * ===============================================
 * 
 * 📚 WHAT: Setting up Webpack with TypeScript for efficient builds
 * 💡 WHY: Provides code splitting, tree shaking, and optimized bundles
 * 🔧 HOW: Loaders, plugins, optimization configurations
 */

// ============================================================================
// SECTION 1: BASIC WEBPACK CONFIG
// ============================================================================

// Example 1.1: Basic Webpack Configuration
// ----------------------------------------

interface WebpackConfig {
  entry: string;
  output: {
    path: string;
    filename: string;
    publicPath: string;
  };
  resolve: {
    extensions: string[];
    alias: Record<string, string>;
  };
  module: {
    rules: WebpackRule[];
  };
  plugins: unknown[];
  devServer: {
    port: number;
    hot: boolean;
    open: boolean;
  };
}

interface WebpackRule {
  test: RegExp;
  exclude: RegExp;
  use: WebpackLoader[];
}

interface WebpackLoader {
  loader: string;
  options?: Record<string, unknown>;
}

const basicConfig: WebpackConfig = {
  entry: "./src/index.ts",
  output: {
    path: "./dist",
    filename: "bundle.js",
    publicPath: "/"
  },
  resolve: {
    extensions: [".ts", ".tsx", ".js", ".jsx"],
    alias: {
      "@": "./src",
      "@components": "./src/components"
    }
  },
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        exclude: /node_modules/,
        use: [
          {
            loader: "ts-loader",
            options: {
              transpileOnly: false
            }
          }
        ]
      }
    ]
  },
  plugins: [],
  devServer: {
    port: 3000,
    hot: true,
    open: true
  }
};

// ============================================================================
// SECTION 2: PRODUCTION CONFIG
// ============================================================================

// Example 2.1: Production Optimizations
// -----------------------------------

interface ProductionConfig extends WebpackConfig {
  mode: "production";
  optimization: {
    minimize: boolean;
    splitChunks: SplitChunksConfig;
    runtimeChunk: boolean | "single";
    moduleIds: string;
  };
  plugins: unknown[];
}

interface SplitChunksConfig {
  chunks: string;
  cacheGroups: Record<string, CacheGroup>;
}

interface CacheGroup {
  test: RegExp;
  name: string;
  priority: number;
}

const productionConfig: ProductionConfig = {
  mode: "production",
  entry: "./src/index.ts",
  output: {
    path: "./dist",
    filename: "[name].[contenthash].js",
    publicPath: "/"
  },
  resolve: {
    extensions: [".ts", ".tsx"]
  },
  module: { rules: [] },
  plugins: [],
  optimization: {
    minimize: true,
    splitChunks: {
      chunks: "all",
      cacheGroups: {
        vendor: {
          test: /[\\/]node_modules[\\/]/,
          name: "vendors",
          priority: 10
        }
      }
    },
    runtimeChunk: "single",
    moduleIds: "deterministic"
  },
  devServer: {
    port: 3000,
    hot: true,
    open: true
  }
};

// ============================================================================
// SECTION 3: CODE SPLITTING
// ============================================================================

// Example 3.1: Dynamic Imports
// -----------------------

// // In your code
// const module = await import("./heavy-module");

// Example 3.2: Chunk Groups
// -----------------------

// webpackChunkName comment for naming
// import(/* webpackChunkName: "module-name" */ "./module").then(...)

// ============================================================================
// SECTION 4: TS-LOADER OPTIONS
// ============================================================================

// Example 4.1: TS-Loader Configuration
// ---------------------------------

interface TSLoaderOptions {
  transpileOnly: boolean;
  compilerOptions: Record<string, unknown>;
  appendTsSuffixTo: RegExp;
  beforeCompile: (program: unknown) => void;
}

const tsLoaderOptions: TSLoaderOptions = {
  transpileOnly: true,
  compilerOptions: {
    module: "esnext",
    target: "esnext"
  },
  appendTsSuffixTo: [/\.vue$/],
  beforeCompile: (program) => console.log("Compiling...", program)
};

console.log("\n=== Webpack Configuration Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/01_Build_Automation/02_Vite_Setup.ts");