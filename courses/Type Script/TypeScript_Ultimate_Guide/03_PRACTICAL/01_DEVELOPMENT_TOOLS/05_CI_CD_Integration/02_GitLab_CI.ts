/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: CI_CD_Integration
 * Purpose: GitLab CI configuration for TypeScript
 * Difficulty: intermediate
 * UseCase: web, backend
 */

/**
 * GitLab CI - Comprehensive Guide
 * =============================
 * 
 * 📚 WHAT: GitLab CI/CD pipeline for TypeScript
 * 💡 WHERE: Automated testing and deployment
 * 🔧 HOW: .gitlab-ci.yml configuration
 */

// ============================================================================
// SECTION 1: GITLAB CI CONFIG
// ============================================================================

// Example 1.1: Basic GitLab CI Configuration
// ---------------------------------

interface GitLabCI {
  stages: string[];
  variables: Record<string, string>;
  image: string;
}

interface Job {
  stage: string;
  script: string[];
  artifacts: ArtifactsConfig;
  only: string[];
}

interface ArtifactsConfig {
  paths: string[];
  expire_in: string;
}

const gitlabCI: GitLabCI = {
  stages: ["test", "build", "deploy"],
  variables: {
    NODE_VERSION: "20"
  },
  image: "node:20"
};

const testJob: Job = {
  stage: "test",
  script: [
    "npm ci",
    "npm run type-check",
    "npm test"
  ],
  artifacts: {
    paths: ["coverage/"],
    expire_in: "1 week"
  },
  only: ["main", "develop"]
};

// ============================================================================
// SECTION 2: MATRIX TESTING
// ============================================================================

// Example 2.1: Matrix Jobs
// ---------------------------------

interface MatrixJob {
  extends: string;
  parallel: {
    matrix: Record<string, string[]>;
  };
}

const matrixTestJob: MatrixJob = {
  extends: "test",
  parallel: {
    matrix: [
      { NODE: "16" },
      { NODE: "18" },
      { NODE: "20" }
    ]
  }
};

console.log("\n=== GitLab CI Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/05_CI_CD_Integration/03_CircleCI_TypeScript.ts");