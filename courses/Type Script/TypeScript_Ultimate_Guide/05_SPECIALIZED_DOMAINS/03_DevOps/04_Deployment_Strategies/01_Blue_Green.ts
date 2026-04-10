/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 03_DevOps
 * Concept: 04_Deployment_Strategies
 * Topic: 01_Blue_Green
 * Purpose: Define blue-green deployment types
 * Difficulty: intermediate
 * UseCase: DevOps
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Cloud Platforms
 * Performance: Zero-downtime switching, instant rollback
 * Security: Traffic isolation prevents unauthorized access
 */

namespace BlueGreenDeploymentTypes {
  export interface BlueGreenConfig {
    name: string;
    blueEnvironment: Environment;
    greenEnvironment: Environment;
    trafficSplit: TrafficConfig;
    rollback: RollbackConfig;
  }

  export interface Environment {
    name: string;
    color: 'blue' | 'green';
    endpoint: string;
    healthy: boolean;
    instances: Instance[];
    healthCheck: HealthCheckConfig;
    autoscaling?: AutoscalingConfig;
  }

  export interface Instance {
    id: string;
    ip: string;
    status: InstanceStatus;
    version: string;
    created: number;
  }

  export type InstanceStatus = 'healthy' | 'unhealthy' | 'starting' | 'stopping';

  export interface HealthCheckConfig {
    path: string;
    interval: number;
    timeout: number;
    healthyThreshold: number;
    unhealthyThreshold: number;
    method: 'GET' | 'POST' | 'HEAD';
    expectedStatus: number;
  }

  export interface AutoscalingConfig {
    minInstances: number;
    maxInstances: number;
    targetCPU: number;
    targetMemory: number;
  }

  export interface TrafficConfig {
    type: TrafficType;
    percentage: number;
    header?: HeaderRouting;
    weightBased?: WeightRouting;
  }

  export type TrafficType = 'instant' | 'gradual' | 'header' | 'weighted';

  export interface HeaderRouting {
    headerName: string;
    headerValue: string;
  }

  export interface WeightRouting {
    blue: number;
    green: number;
  }

  export interface RollbackConfig {
    automatic: boolean;
    triggerConditions: RollbackCondition[];
    timeout: number;
    notifications: NotificationConfig;
  }

  export interface RollbackCondition {
    type: 'error_rate' | 'latency' | 'health_check' | 'custom_metric';
    threshold: number;
    duration: number;
  }

  export interface NotificationConfig {
    slack?: string;
    email?: string;
    webhook?: string;
  }

  export interface DeploymentState {
    current: 'blue' | 'green';
    previous: 'blue' | 'green';
    active: string;
    backup: string;
    lastSwitch: number;
    healthCheckResults: HealthCheckResult[];
  }

  export interface HealthCheckResult {
    environment: string;
    timestamp: number;
    healthy: boolean;
    latency: number;
    errorRate: number;
  }

  export interface SwitchResult {
    success: boolean;
    previous: string;
    active: string;
    duration: number;
    trafficSwitched: boolean;
    error?: string;
  }

  export interface BlueGreenDeployer {
    deploy(newVersion: string): Promise<DeploymentState>;
    switchTraffic(target: 'blue' | 'green', config?: TrafficConfig): Promise<SwitchResult>;
    rollback(): Promise<SwitchResult>;
    validateEnvironment(env: string): Promise<HealthCheckResult>;
    monitor(): Promise<DeploymentState>;
  }
}

// Cross-reference: 02_Canary_Deployments.ts (alternative strategy)
console.log("\n=== Blue-Green Deployment Types ===");
console.log("Related: 02_Canary_Deployments.ts");