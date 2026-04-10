/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 02_Machine_Learning
 * Concept: 03_Model_Integration
 * Topic: 03_Model_Serving
 * Purpose: Define model serving and deployment types
 * Difficulty: advanced
 * UseCase: machine learning
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n) parallel inference, batch processing
 * Security: Authentication prevents unauthorized access
 */

namespace ModelServingTypes {
  export type ServingFramework = 'tensorflow_serving' | 'torchserve' | 'triton' | 'onnxruntime' | 'custom';

  export interface ModelServer {
    framework: ServingFramework;
    models: Map<string, ServedModel>;
    start(config: ServerConfig): Promise<void>;
    stop(): Promise<void>;
    loadModel(model: ModelArtifact): Promise<void>;
    unloadModel(modelId: string): Promise<void>;
  }

  export interface ServerConfig {
    host: string;
    port: number;
    grpcPort?: number;
    maxWorkers: number;
    timeout: number;
    maxBatchSize: number;
    maxBatchDelay: number;
  }

  export interface ServedModel {
    name: string;
    version: string;
    artifact: ModelArtifact;
    status: ModelStatus;
    ready: boolean;
    inferenceCount: number;
    lastInferenceTime: number;
  }

  export type ModelStatus = 'LOADING' | 'AVAILABLE' | 'UNLOADING' | 'ERROR';

  export interface ModelArtifact {
    uri: string;
    format: ModelFormat;
    size: number;
    checksum: string;
    metadata: ModelMetadata;
  }

  export type ModelFormat = 'tensorflow_savedmodel' | 'pytorch_script' | 'onnx' | 'keras' | 'tflite';

  export interface ModelMetadata {
    inputs: ModelInput[];
    outputs: ModelOutput[];
    version: string;
    created: number;
    description?: string;
    framework?: string;
    metrics?: Record<string, number>;
  }

  export interface ModelInput {
    name: string;
    dtype: string;
    shape: number[];
    metadata?: Record<string, unknown>;
  }

  export interface ModelOutput {
    name: string;
    dtype: string;
    shape: number[];
    metadata?: Record<string, unknown>;
  }

  export interface InferenceRequest {
    modelName: string;
    modelVersion?: string;
    inputs: Record<string, Tensor>;
    parameters?: RequestParameters;
  }

  export interface Tensor {
    dtype: string;
    shape: number[];
    data: number[] | Float32Array;
  }

  export interface RequestParameters {
    timeout?: number;
    batchSize?: number;
    customParameters?: Record<string, string>;
  }

  export interface InferenceResponse {
    outputs: Record<string, Tensor>;
    modelName: string;
    modelVersion: string;
    inferenceTime: number;
    parameters?: ResponseParameters;
  }

  export interface ResponseParameters {
    logRequests?: boolean;
    customOutputs?: Record<string, unknown>;
  }

  export interface ModelManager {
    registerModel(artifact: ModelArtifact): Promise<string>;
    listModels(): ServedModel[];
    getModel(name: string, version?: string): ServedModel | undefined;
    predict(request: InferenceRequest): Promise<InferenceResponse>;
    batchPredict(requests: InferenceRequest[]): Promise<InferenceResponse[]>;
  }

  export interface BatchingConfig {
    enabled: boolean;
    maxBatchSize: number;
    maxBatchDelay: number;
    timeout: number;
  }

  export interface ModelMonitor {
    metrics: ModelMetrics;
    startMonitoring(): void;
    stopMonitoring(): void;
    getMetrics(): Promise<ModelMetrics>;
  }

  export interface ModelMetrics {
    requestCount: number;
    errorCount: number;
    avgLatency: number;
    p50Latency: number;
    p90Latency: number;
    p99Latency: number;
    avgThroughput: number;
  }

  export interface HealthCheck {
    healthy: boolean;
    status: string;
    modelsLoaded: number;
    uptime: number;
  }
}

// Cross-reference: 01_TensorFlow_Integration.ts, 02_ONNX_Types.ts
console.log("\n=== Model Serving Types ===");
console.log("Related: 01_TensorFlow_Integration.ts, 02_ONNX_Types.ts");