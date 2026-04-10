/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: CI_CD_Integration
 * Purpose: CircleCI configuration for TypeScript
 * Difficulty: intermediate
 * UseCase: web, backend
 */

/**
 * CircleCI - Comprehensive Guide
 * =========================
 * 
 * 📚 WHAT: CircleCI pipeline for TypeScript
 * 💡 WHERE: Cloud CI/CD
 * 🔧 HOW: config.yml configuration
 */

// ============================================================================
// SECTION 1: CIRCLECI CONFIG
// ============================================================================

// Example 1.1: CircleCI Configuration
// ---------------------------------

interface CircleCIConfig {
  version: number;
  orbs: Record<string, string>;
  jobs: Record<string, Job>;
  workflows: Record<string, WorkflowJob>;
}

interface Job {
  docker: DockerImage[];
  steps: string[];
}

interface DockerImage {
  image: string;
}

interface WorkflowJob {
  jobs: string[];
  filters: FilterConfig;
}

interface FilterConfig {
  branches: BranchFilter;
}

const circleCIConfig: CircleCIConfig = {
  version: 2.1,
  orbs: {
    node: "circleci/node@5.0.0"
  },
  jobs: {
    build: {
      docker: [{ image: "circleci/node:20" }],
      steps: [
        "checkout",
        "restore_cache",
        "run: npm ci",
        "run: npm run type-check",
        "run: npm test",
        "save_cache"
      ]
    }
  },
  workflows: {
    version: 2,
    build: {
      jobs: ["build"]
    }
  }
};

console.log("\n=== CircleCI Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/06_Testing_Frameworks/02_Mocha_Integration.ts");