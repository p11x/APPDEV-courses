/**
 * Category: PRACTICAL
 * Subcategory: DEVELOPMENT_TOOLS
 * Concept: CI_CD_Integration
 * Purpose: GitHub Actions for TypeScript projects
 * Difficulty: intermediate
 * UseCase: web, backend
 */

/**
 * GitHub Actions - Comprehensive Guide
 * =================================
 * 
 * 📚 WHAT: CI/CD pipelines with GitHub Actions for TypeScript
 * 💡 WHERE: Automated testing and deployment
 * 🔧 HOW: Workflows, actions, secrets
 */

// ============================================================================
// SECTION 1: WORKFLOW STRUCTURE
// ============================================================================

// Example 1.1: Basic TypeScript Workflow
// ---------------------------------

interface GitHubWorkflow {
  name: string;
  on: string | Record<string, unknown>;
  jobs: Record<string, Job>;
}

interface Job {
  "runs-on": string;
  steps: Step[];
}

interface Step {
  name: string;
  uses: string;
  with?: Record<string, string>;
}

const workflow: GitHubWorkflow = {
  name: "TypeScript CI",
  on: "push",
  jobs: {
    build: {
      "runs-on": "ubuntu-latest",
      steps: [
        {
          name: "Checkout code",
          uses: "actions/checkout@v4"
        },
        {
          name: "Setup Node.js",
          uses: "actions/setup-node@v4",
          with: {
            "node-version": "20"
          }
        },
        {
          name: "Install dependencies",
          run: "npm ci"
        },
        {
          name: "Run type check",
          run: "npm run type-check"
        },
        {
          name: "Run tests",
          run: "npm test"
        },
        {
          name: "Build",
          run: "npm run build"
        }
      ]
    }
  }
};

// ============================================================================
// SECTION 2: MATRIX STRATEGY
// ============================================================================

// Example 2.1: Testing Multiple Versions
// ---------------------------------

interface MatrixWorkflow {
  jobs: {
    [key: string]: {
      "runs-on": string;
      strategy: {
        matrix: {
          "node-version": number[];
          os: string[];
        };
      };
      steps: Step[];
    };
  };
}

const matrixWorkflow: MatrixWorkflow = {
  jobs: {
    test: {
      "runs-on": "${{ matrix.os }}",
      strategy: {
        matrix: {
          "node-version": [16, 18, 20],
          os: ["ubuntu-latest", "windows-latest"]
        }
      },
      steps: []
    }
  }
};

console.log("\n=== GitHub Actions Complete ===");
console.log("Next: PRACTICAL/DATA_PROCESSING/03_Data_Validation/01_Validation_Types.ts");