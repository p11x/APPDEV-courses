/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 03_DevOps
 * Concept: 01_CI_CD_Pipelines
 * Topic: 01_GitHub_Actions
 * Purpose: Define GitHub Actions workflow types
 * Difficulty: intermediate
 * UseCase: DevOps
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, GitHub Actions
 * Performance: Parallel job execution, caching
 * Security: Secret encryption, environment protection
 */

namespace GitHubActionsTypes {
  export interface Workflow {
    name: string;
    on: TriggerConfig;
    env?: EnvironmentVariables;
    jobs: Record<string, Job>;
  }

  export interface TriggerConfig {
    push?: PushTrigger;
    pull_request?: PullRequestTrigger;
    schedule?: ScheduleTrigger[];
    workflow_dispatch?: WorkflowDispatchTrigger;
    repository_dispatch?: RepositoryDispatchTrigger;
  }

  export interface PushTrigger {
    branches?: string[];
    branches_ignore?: string[];
    tags?: string[];
    tags_ignore?: string[];
    paths?: string[];
    paths_ignore?: string[];
  }

  export interface PullRequestTrigger {
    branches?: string[];
    branches_ignore?: string[];
    paths?: string[];
    paths_ignore?: string[];
    types?: ('opened' | 'synchronize' | 'reopened')[];
  }

  export interface ScheduleTrigger {
    cron: string;
  }

  export interface WorkflowDispatchTrigger {
    inputs?: WorkflowInput[];
  }

  export interface WorkflowInput {
    description: string;
    required: boolean;
    default?: string;
    type: 'choice' | 'boolean' | 'string';
    options?: string[];
  }

  export interface RepositoryDispatchTrigger {
    types: string[];
  }

  export interface Job {
    runs-on: string;
    needs?: string | string[];
    if?: string;
    environment?: string;
    outputs?: Record<string, JobOutput>;
    steps: Step[];
    strategy?: Strategy;
    container?: Container;
  }

  export interface JobOutput {
    description: string;
    value: string;
  }

  export interface Step {
    name: string;
    id?: string;
    if?: string;
    uses: string;
    run?: string;
    with?: Record<string, unknown>;
    env?: EnvironmentVariables;
    secrets?: Record<string, string>;
    working-directory?: string;
    continue-on-error?: boolean;
    timeout-minutes?: number;
  }

  export interface Strategy {
    matrix: MatrixStrategy;
    fail-fast?: boolean;
  }

  export interface MatrixStrategy {
    include?: Record<string, string>[];
    exclude?: Record<string, string>[];
    [key: string]: string[] | string | number | undefined;
  }

  export interface Container {
    image: string;
    options?: string;
    env?: EnvironmentVariables;
    volumes?: string[];
    credentials?: ContainerCredentials;
  }

  export interface ContainerCredentials {
    username: string;
    password: string;
  }

  export interface EnvironmentVariables {
    [key: string]: string;
  }

  export interface Action {
    name: string;
    description: string;
    inputs?: ActionInput[];
    outputs?: ActionOutput[];
    runs: ActionRuns;
  }

  export interface ActionInput {
    name: string;
    description: string;
    required: boolean;
    default?: string;
  }

  export interface ActionOutput {
    name: string;
    description: string;
  }

  export interface ActionRuns {
    using: 'composite' | 'docker' | 'node';
    image?: string;
    main?: string;
    env?: EnvironmentVariables;
  }

  export interface Cache {
    key: string;
    path: string[];
    restore-keys?: string[];
  }
}

// Cross-reference: 02_Jenkins_Types.ts, 03_Travis_CI.ts
console.log("\n=== GitHub Actions Types ===");
console.log("Related: 02_Jenkins_Types.ts, 03_Travis_CI.ts");