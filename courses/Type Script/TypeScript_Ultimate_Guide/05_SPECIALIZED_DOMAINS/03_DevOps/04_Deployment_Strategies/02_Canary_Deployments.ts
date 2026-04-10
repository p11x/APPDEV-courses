/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 03_DevOps
 * Concept: 04_Deployment_Strategies
 * Topic: 02_Canary_Deployments
 * Purpose: Define canary deployment types
 * Difficulty: advanced
 * UseCase: DevOps
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Cloud Platforms
 * Performance: Progressive traffic shifting, metrics-driven
 * Security: Isolated canary instances, controlled exposure
 */

namespace CanaryDeploymentTypes {
  export interface CanaryConfig {
    name: string;
    canary: CanaryEnvironment;
    stable: StableEnvironment;
    analysis: AnalysisConfig;
    rollout: RolloutConfig;
  }

  export interface CanaryEnvironment {
    name: string;
    endpoint: string;
    version: string;
    replicas: number;
    resources: ResourceRequirements;
    pods: PodInfo[];
  }

  export interface StableEnvironment {
    name: string;
    endpoint: string;
    version: string;
    replicas: number;
    resources: ResourceRequirements;
    pods: PodInfo[];
  }

  export interface ResourceRequirements {
    requests: ResourceSpec;
    limits: ResourceSpec;
  }

  export interface ResourceSpec {
    cpu: string;
    memory: string;
  }

  export interface PodInfo {
    name: string;
    ip: string;
    ready: string;
    status: string;
    restarts: number;
    age: string;
  }

  export interface AnalysisConfig {
    interval: number;
    threshold: AnalysisThreshold;
    metrics: MetricSpec[];
    queries: PromQLQuery[];
    consecutiveFailureLimit: number;
    iterations: number;
  }

  export interface AnalysisThreshold {
    errorRate: number;
    latencyP50: number;
    latencyP95: number;
    latencyP99: number;
    successRate: number;
    cpuUsage: number;
    memoryUsage: number;
  }

  export interface MetricSpec {
    name: string;
    type: 'request' | 'resource' | 'custom';
    threshold: number;
    operator: 'gt' | 'lt' | 'eq' | 'gte' | 'lte';
  }

  export interface PromQLQuery {
    name: string;
    query: string;
    threshold: number;
  }

  export interface RolloutConfig {
    strategy: RolloutStrategy;
    steps: RolloutStep[];
    abort: AbortConfig;
  }

  export type RolloutStrategy = 'linear' | 'exponential' | 'manual';

  export interface RolloutStep {
    weight: number;
    duration: number;
    analysis: AnalysisConfig;
  }

  export interface AbortConfig {
    enabled: boolean;
    autoAbort: boolean;
    maxReplicas?: number;
    timeout?: number;
    conditions: AbortCondition[];
  }

  export interface AbortCondition {
    type: 'error_rate' | 'latency' | 'metrics';
    threshold: number;
    duration: number;
  }

  export interface CanaryStatus {
    phase: CanaryPhase;
    currentStep: number;
    currentWeight: number;
    analysis: AnalysisResult;
    canaryReplicas: number;
    stableReplicas: number;
    trafficPercentage: number;
  }

  export type CanaryPhase = 'Initializing' | 'CanaryAnalysis' | 'Promoting' | 'Successful' | 'Failed' | 'Paused';

  export interface AnalysisResult {
    passed: boolean;
    iterations: number;
    failures: number;
    metrics: MetricResult[];
    recommendation: 'promote' | 'abort' | 'pause';
  }

  export interface MetricResult {
    name: string;
    value: number;
    threshold: number;
    passed: boolean;
  }

  export interface StepAnalysis {
    step: number;
    weight: number;
    passed: boolean;
    analysisResults: AnalysisResult[];
    duration: number;
  }

  export interface CanaryDeployer {
    initialize(canaryVersion: string): Promise<CanaryEnvironment>;
    deploy(config: CanaryConfig): Promise<CanaryStatus>;
    promote(): Promise<CanaryStatus>;
    abort(): Promise<CanaryStatus>;
    pause(): Promise<CanaryStatus>;
    resume(): Promise<CanaryStatus>;
    getStatus(): Promise<CanaryStatus>;
    getAnalysis(): Promise<AnalysisResult>;
  }

  export interface TrafficManagement {
    setWeight(canaryWeight: number): Promise<void>;
    incrementWeight(step: number): Promise<void>;
    finalizeCanary(): Promise<void>;
    abortAndRollback(): Promise<void>;
  }
}

// Cross-reference: 01_Blue_Green.ts (alternative strategy)
console.log("\n=== Canary Deployment Types ===");
console.log("Related: 01_Blue_Green.ts");