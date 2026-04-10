/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: Build_Automation
 * Purpose: Vite configuration for TypeScript projects
 * Difficulty: beginner
 * UseCase: web
 */

/**
 * Vite Setup - Comprehensive Guide
 * ===================================
 * 
 * 📚 WHAT: Setting up Vite with TypeScript for fast builds
 * 💡 WHY: Native ESM, instant HMR, optimized builds
 * 🔧 HOW: Configuration, plugins, framework integration
 */

// ============================================================================
// SECTION 1: BASIC VITE CONFIG
// ============================================================================

// Example 1.1: Vite Configuration
// -----------------------

interface ViteConfig {
  root: string;
  base: string;
  publicDir: string;
  resolve: {
    alias: Record<string, string>;
  };
  build: {
    outDir: string;
    rollupOptions: RollupOptions;
    target: string;
    minify: string;
    sourcemap: boolean;
  };
  server: {
    port: number;
    open: boolean;
    proxy: Record<string, ProxyConfig>;
  };
  plugins: unknown[];
}

interface RollupOptions {
  input: string | Record<string, string>;
  output: Record<string, unknown>;
}

interface ProxyConfig {
  target: string;
  changeOrigin: boolean;
  rewrite?: (path: string) => string;
}

const viteConfig: ViteConfig = {
  root: ".",
  base: "/",
  publicDir: "public",
  resolve: {
    alias: {
      "@": "/src",
      "@components": "/src/components"
    }
  },
  build: {
    outDir: "dist",
    rollupOptions: {
      input: "./index.html"
    },
    target: "es2020",
    minify: "esbuild",
    sourcemap: false
  },
  server: {
    port: 3000,
    open: true,
    proxy: {
      "/api": {
        target: "http://localhost:8080",
        changeOrigin: true
      }
    }
  },
  plugins: []
};

// ============================================================================
// SECTION 2: VITE WITH REACT
// ============================================================================

// Example 2.1: React Plugin Configuration
// --------------------------------

interface ReactPluginOptions {
  fastRefresh: boolean;
  babel: {
    presets: string[];
    plugins: string[];
  };
}

const reactConfig = {
  plugins: [
    ["@vitejs/plugin-react", { fastRefresh: true }]
  ],
  esbuild: {
    jsxInject: "react/jsx-runtime"
  }
};

// ============================================================================
// SECTION 3: VITE WITH VUE
// ============================================================================

// Example 3.1: Vue Plugin Configuration
// --------------------------------

interface VuePluginOptions {
  script: {
    defineProps: Record<string, unknown>;
  };
}

const vueConfig = {
  plugins: [
    ["@vitejs/plugin-vue", { script: {} }]
  ]
};

// ============================================================================
// SECTION 4: ENVIRONMENT VARIABLES
// ============================================================================

// Example 4.1: .env Files
// ---------------------

// .env - loaded in all cases
// .env.local - local overrides, ignored by git
// .env.development - development
// .env.production - production

interface ImportMetaEnv {
  readonly VITE_APP_TITLE: string;
  readonly VITE_API_URL: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}

console.log("\n=== Vite Setup Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/01_Build_Automation/03_Rollup_Configuration.ts");