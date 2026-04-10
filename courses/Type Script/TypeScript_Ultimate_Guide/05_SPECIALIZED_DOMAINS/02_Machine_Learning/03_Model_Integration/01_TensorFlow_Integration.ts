/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 02_Machine_Learning
 * Concept: 03_Model_Integration
 * Topic: 01_TensorFlow_Integration
 * Purpose: Define TensorFlow.js integration types
 * Difficulty: advanced
 * UseCase: machine learning
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: GPU acceleration via WebGL, TPU support
 * Security: Model validation prevents malicious code
 */

namespace TensorFlowIntegrationTypes {
  export interface TensorFlowModel {
    name: string;
    version: string;
    format: 'savedmodel' | 'keras' | 'tfjs_layers' | 'tfjs_graph';
    metadata: ModelMetadata;
    load(path: string): Promise<TensorFlowModel>;
    predict(inputs: TF.Tensor): TF.Tensor | TF.Tensor[];
    train(config: TrainingConfig): Promise<History>;
  }

  export interface ModelMetadata {
    inputShapes: number[][];
    outputShapes: number[][];
    inputs: TensorInfo[];
    outputs: TensorInfo[];
    optimization?: OptimizationConfig;
  }

  export interface TensorInfo {
    name: string;
    dtype: DataType;
    shape: number[];
  }

  export type DataType = 'float32' | 'int32' | 'bool' | 'string';

  export interface TrainingConfig {
    epochs: number;
    batchSize: number;
    validationSplit: number;
    optimizer: Optimizer;
    loss: Loss | Loss[];
    metrics: Metric[];
    callbacks: Callback[];
    shuffle: boolean;
    classWeight?: number[];
    sampleWeight?: number[];
  }

  export interface Optimizer {
    type: 'sgd' | 'adam' | 'adamax' | 'rmsprop' | 'adadelta' | 'adagrad';
    learningRate: number;
    momentum?: number;
    beta1?: number;
    beta2?: number;
    epsilon?: number;
  }

  export type Loss = 'meanSquaredError' | 'meanAbsoluteError' | 'categoricalCrossentropy' | 'binaryCrossentropy' | 'sparseCategoricalCrossentropy';
  export type Metric = 'accuracy' | 'precision' | 'recall' | 'auc' | 'mse' | 'mae';

  export interface Callback {
    onEpochBegin?: (epoch: number) => void;
    onEpochEnd?: (epoch: number, logs: Logs) => void;
    onBatchBegin?: (batch: number) => void;
    onBatchEnd?: (batch: number, logs: Logs) => void;
    onTrainBegin?: () => void;
    onTrainEnd?: () => void;
  }

  export interface Logs {
    loss: number;
    val_loss?: number;
    accuracy?: number;
    val_accuracy?: number;
    [key: string]: number | undefined;
  }

  export interface History {
    epoch: number[];
    history: Record<string, number[]>;
  }

  export interface LayersModel extends TensorFlowModel {
    type: 'tfjs_layers';
    layers: Layer[];
    optimizer: Optimizer;
    compiled: boolean;
    fit(x: TF.Tensor | TF.Tensor[], y: TF.Tensor | TF.Tensor[], config?: TrainingConfig): Promise<History>;
    evaluate(x: TF.Tensor, y: TF.Tensor): Promise<Metrics>;
  }

  export interface Layer {
    name: string;
    type: string;
    inputShape?: number[];
    outputShape?: number[];
    params: number;
    config: LayerConfig;
  }

  export interface LayerConfig {
    units?: number;
    activation?: Activation;
    useBias?: boolean;
    kernelInitializer?: Initializer;
    biasInitializer?: Initializer;
    kernelRegularizer?: Regularizer;
    dropout?: number;
  }

  export type Activation = 'relu' | 'sigmoid' | 'softmax' | 'tanh' | 'linear';
  export type Initializer = 'glorotUniform' | 'heNormal' | 'randomNormal' | 'ones' | 'zeros';
  export type Regularizer = 'l1' | 'l2' | 'l1l2';

  export interface TensorFlowConverter {
    fromKeras(model: unknown): Promise<LayersModel>;
    fromSavedModel(path: string): Promise<TensorFlowModel>;
    toSavedModel(model: TensorFlowModel, path: string): Promise<void>;
  }

  export interface Metrics {
    loss: number;
    [key: string]: number;
  }
}

// Cross-reference: 02_ONNX_Types.ts, 03_Model_Serving.ts
console.log("\n=== TensorFlow Integration Types ===");
console.log("Related: 02_ONNX_Types.ts, 03_Model_Serving.ts");