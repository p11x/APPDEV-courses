/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 03_DevOps
 * Concept: 03_Monitoring_Systems
 * Topic: 01_Prometheus_Types
 * Purpose: Define Prometheus monitoring types
 * Difficulty: intermediate
 * UseCase: DevOps
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Prometheus
 * Performance: O(n) metric collection, efficient storage
 * Security: TLS authentication, RBAC
 */

namespace PrometheusTypes {
  export interface PrometheusConfig {
    global: GlobalConfig;
    scrapeConfigs: ScrapeConfig[];
    alerting?: AlertingConfig;
    ruleFiles?: string[];
    remoteWrite?: RemoteWriteConfig[];
    remoteRead?: RemoteReadConfig[];
  }

  export interface GlobalConfig {
    scrapeInterval: string;
    scrapeTimeout: string;
    evaluationInterval: string;
    externalLabels?: Record<string, string>;
  }

  export interface ScrapeConfig {
    jobName: string;
    scrapeInterval?: string;
    scrapeTimeout?: string;
    metricsPath?: string;
    scheme?: 'http' | 'https';
    params?: Record<string, string[]>;
    staticConfigs?: StaticConfig[];
    fileSDConfigs?: FileSDConfig[];
    kubernetesSDConfigs?: KubernetesSDConfig[];
    ec2SDConfigs?: EC2SDConfig[];
    consulSDConfigs?: ConsulSDConfig[];
    relabelConfigs?: RelabelConfig[];
  }

  export interface StaticConfig {
    targets: string[];
    labels?: Record<string, string>;
  }

  export interface FileSDConfig {
    files: string[];
    refreshInterval?: string;
  }

  export interface KubernetesSDConfig {
    apiServer: string;
    role: 'node' | 'pod' | 'service' | 'endpoints' | 'ingress';
    namespaces?: string[];
    labels?: Record<string, string>;
  }

  export interface EC2SDConfig {
    region: string;
    accessKey?: string;
    secretKey?: string;
    profile?: string;
    filters?: Record<string, string[]>;
  }

  export interface ConsulSDConfig {
    server: string;
    datacenter?: string;
    scheme?: string;
    services?: string[];
    tags?: string[];
    nodeMeta?: Record<string, string>;
  }

  export interface RelabelConfig {
    sourceLabels?: string[];
    separator?: string;
    regex?: string;
    modulus?: number;
    targetLabel?: string;
    replacement?: string;
    action?: RelabelAction;
  }

  export type RelabelAction = 
    | 'replace' | 'keep' | 'drop' | 'labelmap' | 'labeldrop' | 'labelkeep' | 'hashmod' | 'encode' | 'label');

  export interface AlertingConfig {
    alertmanagers?: AlertmanagerConfig[];
  }

  export interface AlertmanagerConfig {
    staticConfigs?: StaticConfig[];
    fileSDConfigs?: FileSDConfig[];
    kubernetesSDConfigs?: KubernetesSDConfig[];
  }

  export interface RemoteWriteConfig {
    url: string;
    tlsConfig?: TLSConfig;
    basicAuth?: BasicAuth;
    queueConfig?: QueueConfig;
  }

  export interface RemoteReadConfig {
    url: string;
    tlsConfig?: TLSConfig;
    basicAuth?: BasicAuth;
  }

  export interface TLSConfig {
    certFile?: string;
    keyFile?: string;
    caFile?: string;
    serverName?: string;
    insecureSkipVerify?: boolean;
  }

  export interface BasicAuth {
    username: string;
    password: string;
  }

  export interface QueueConfig {
    capacity?: number;
    maxSamplesPerSend?: number;
    batchSendDeadline?: string;
    maxShards?: number;
    minShards?: number;
    maxRetries?: number;
    minRetryDelay?: string;
    maxRetryDelay?: string;
  }

  export interface Metric {
    name: string;
    help: string;
    type: MetricType;
    metric: string;
  }

  export type MetricType = 'counter' | 'gauge' | 'histogram' | 'summary' | 'untyped';

  export interface Sample {
    metric: Record<string, string>;
    value: number;
    timestamp: number;
  }

  export interface TimeSeries {
    metric: Record<string, string>;
    values: Sample[];
  }

  export interface QueryResult {
    resultType: 'vector' | 'matrix' | 'scalar' | 'string';
    result: (vector: VectorMatch) | (matrix: RangeVector)[];
  }

  export interface VectorMatch {
    metric: Record<string, string>;
    value: [number, string];
  }

  export interface RangeVector {
    metric: Record<string, string>;
    values: [number, string][];
  }

  export interface Alert {
    name: string;
    state: 'pending' | 'firing';
    labels: Record<string, string>;
    annotations: Record<string, string>;
    startsAt?: Date;
    endsAt?: Date;
    generatorURL?: string;
    labelsPath?: string;
  }

  export interface RecordingRule {
    name: string;
    expr: string;
    labels?: Record<string, string>;
  }
}

// Cross-reference: 02_Grafana_Dashboards.ts (visualization)
console.log("\n=== Prometheus Types ===");
console.log("Related: 02_Grafana_Dashboards.ts");