/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 02_Machine_Learning
 * Concept: 02_ML_Algorithms
 * Topic: 01_Supervised_Learning
 * Purpose: Define supervised learning algorithm types
 * Difficulty: advanced
 * UseCase: machine learning
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n*d*k) training, O(k) inference for k classes
 * Security: Input validation prevents model evasion
 */

namespace SupervisedLearningTypes {
  export type AlgorithmType = 'regression' | 'classification';
  export type ModelFamily = 'linear' | 'tree' | 'neural' | 'ensemble' | 'svm' | 'knn';

  export interface SupervisedModel<T = unknown> {
    type: AlgorithmType;
    family: ModelFamily;
    train(X: Matrix, y: Vector): Promise<Model<T>>;
    predict(X: Matrix): PredictionResult;
    evaluate(X: Matrix, y: Vector): EvaluationMetrics;
    save(path: string): Promise<void>;
    load(path: string): Promise<void>;
  }

  export interface Matrix {
    rows: number;
    cols: number;
    data: number[][];
  }

  export interface Vector {
    data: number[];
    shape: [number];
  }

  export interface PredictionResult {
    predictions: number[] | number[][];
    probabilities?: number[][];
    confidence: number[];
  }

  export interface EvaluationMetrics {
    accuracy?: number;
    precision?: number;
    recall?: number;
    f1Score?: number;
    mse?: number;
    rmse?: number;
    mae?: number;
    r2?: number;
    confusionMatrix?: number[][];
  }

  export interface LinearRegression extends SupervisedModel<'regression'> {
    family: 'linear';
    coefficients: Vector;
    intercept: number;
    regularization?: 'none' | 'l1' | 'l2';
    lambda?: number;
  }

  export interface LogisticRegression extends SupervisedModel<'classification'> {
    family: 'linear';
    weights: Matrix;
    bias: Vector;
    multiClass: boolean;
    solver: 'newton-cg' | 'lbfgs' | 'liblinear' | 'sag' | 'saga';
  }

  export interface DecisionTree extends SupervisedModel {
    family: 'tree';
    maxDepth: number;
    minSamplesSplit: number;
    minSamplesLeaf: number;
    criterion: 'gini' | 'entropy';
    featureImportance: number[];
  }

  export interface RandomForest extends SupervisedModel {
    family: 'ensemble';
    nEstimators: number;
    maxFeatures: number | 'sqrt' | 'log2';
    bootstrap: boolean;
    oobScore: boolean;
    trees: DecisionTree[];
  }

  export interface NeuralNetwork extends SupervisedModel {
    family: 'neural';
    architecture: Layer[];
    optimizer: OptimizerConfig;
    loss: LossFunction;
    epochs: number;
    batchSize: number;
    learningRate: number;
  }

  export interface Layer {
    type: 'dense' | 'conv2d' | 'maxpool' | 'dropout' | 'batchnorm';
    units?: number;
    activation?: ActivationFunction;
    kernelSize?: number[];
    strides?: number[];
    rate?: number;
  }

  export type ActivationFunction = 'relu' | 'sigmoid' | 'tanh' | 'softmax' | 'linear';

  export interface OptimizerConfig {
    type: 'sgd' | 'adam' | 'rmsprop' | 'adagrad';
    learningRate: number;
    momentum?: number;
    beta1?: number;
    beta2?: number;
  }

  export type LossFunction = 'mse' | 'mae' | 'binary_crossentropy' | 'categorical_crossentropy';

  export interface SVM extends SupervisedModel {
    family: 'svm';
    kernel: 'linear' | 'poly' | 'rbf' | 'sigmoid';
    C: number;
    gamma: number | 'scale' | 'auto';
    supportVectors: number[][];
    dualCoefficients: number[];
  }

  export interface KNN extends SupervisedModel {
    family: 'knn';
    k: number;
    metric: 'euclidean' | 'manhattan' | 'minkowski';
    weights: 'uniform' | 'distance';
  }
}

// Cross-reference: 02_Unsupervised_Learning.ts, 03_Reinforcement_Learning.ts
console.log("\n=== Supervised Learning Types ===");
console.log("Related: 02_Unsupervised_Learning.ts, 03_Reinforcement_Learning.ts");