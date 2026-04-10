/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 03_DevOps
 * Concept: 01_CI_CD_Pipelines
 * Topic: 02_Jenkins_Types
 * Purpose: Define Jenkins pipeline types
 * Difficulty: intermediate
 * UseCase: DevOps
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Jenkins
 * Performance: Distributed builds, agent management
 * Security: Matrix authorization, credentials management
 */

namespace JenkinsTypes {
  export interface JenkinsPipeline {
    pipeline: PipelineDefinition;
  }

  export interface PipelineDefinition {
    agent?: AgentConfig;
    environment?: EnvironmentVariable[];
    options?: PipelineOptions;
    stages: Stage[];
    post?: PostBuild;
  }

  export interface AgentConfig {
    label?: string;
    docker?: DockerAgent;
    kubernetes?: KubernetesAgent;
    any?: boolean;
  }

  export interface DockerAgent {
    image: string;
    args?: string;
    registry?: string;
    label?: string;
  }

  export interface KubernetesAgent {
    label: string;
    containerTemplate?: ContainerTemplate;
    yaml?: string;
  }

  export interface ContainerTemplate {
    name: string;
    image: string;
    command?: string;
    args?: string;
    workingDir?: string;
  }

  export interface EnvironmentVariable {
    name: string;
    value: string;
  }

  export interface PipelineOptions {
    timeout?: number;
    retry?: number;
    buildDiscarder?: BuildDiscarder;
    timestamps?: boolean;
    disableConcurrentBuilds?: boolean;
  }

  export interface BuildDiscarder {
    artifactDeployer?: ArtifactManager;
    logRotator?: LogRotator;
  }

  export interface ArtifactManager {
    artifactNumToKeep: number;
    artifactDaysToKeep: number;
  }

  export interface LogRotator {
    daysToKeep: number;
    numToKeep: number;
  }

  export interface Stage {
    name: string;
    agent?: AgentConfig;
    when?: WhenCondition;
    environment?: EnvironmentVariable[];
    steps: Step[];
    post?: PostBuild;
  }

  export interface WhenCondition {
    expression?: string;
    branch?: BranchCondition;
    environment?: EnvironmentCondition;
    anyOf?: WhenCondition[];
    allOf?: WhenCondition[];
  }

  export interface BranchCondition {
    compare: 'matches' | 'equals';
    pattern: string;
  }

  export interface EnvironmentCondition {
    name: string;
    value: string;
  }

  export interface Step {
    script?: ScriptBlock;
    sh?: string;
    bat?: string;
    echo?: string;
    dir?: string;
    withEnv?: EnvironmentVariable[];
    withCredentials?: CredentialBinding[];
    archiveArtifacts?: ArchiveArtifacts;
    stash?: Stash;
    unstash?: string;
    timeout?: Timeout;
    retry?: number;
    parallel?: Step[];
  }

  export interface ScriptBlock {
    script: string;
  }

  export interface CredentialBinding {
    variable: string;
    credentialsId: string;
    secretType?: 'usernamePassword' | 'sshUserPrivateKey' | 'string' | 'file';
  }

  export interface ArchiveArtifacts {
    artifacts: string;
    fingerprint?: boolean;
    allowEmptyArchive?: boolean;
    defaultExcludes?: boolean;
    excludes?: string;
  }

  export interface Stash {
    name: string;
    includes?: string;
    excludes?: string;
    useDefaultExcludes?: boolean;
  }

  export interface Timeout {
    time: number;
    unit: 'MINUTES' | 'SECONDS' | 'HOURS';
  }

  export interface PostBuild {
    always?: Step[];
    success?: Step[];
    failure?: Step[];
    unstable?: Step[];
    cleanup?: Step[];
  }

  export interface Jenkinsfile {
    pipeline: PipelineDefinition;
  }

  export interface BuildResult {
    result: 'SUCCESS' | 'FAILURE' | 'UNSTABLE' | 'ABORTED' | 'NOT_BUILT';
    duration: number;
    number: number;
    displayName: string;
    url: string;
  }
}

// Cross-reference: 01_GitHub_Actions.ts, 03_Travis_CI.ts
console.log("\n=== Jenkins Types ===");
console.log("Related: 01_GitHub_Actions.ts, 03_Travis_CI.ts");