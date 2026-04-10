/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 02_Machine_Learning
 * Concept: 02_ML_Algorithms
 * Topic: 02_Unsupervised_Learning
 * Purpose: Define unsupervised learning algorithm types
 * Difficulty: advanced
 * UseCase: machine learning
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n*k*i) clustering, O(n*d) dimensionality reduction
 * Security: Input validation prevents adversarial data
 */

namespace UnsupervisedLearningTypes {
  export type AlgorithmType = 'clustering' | 'dimensionality_reduction' | 'density_estimation';

  export interface UnsupervisedModel<T = unknown> {
    type: AlgorithmType;
    fit(X: Matrix): Promise<Model<T>>;
    transform(X: Matrix): TransformResult;
    fitTransform(X: Matrix): TransformResult;
  }

  export interface Matrix {
    rows: number;
    cols: number;
    data: number[][];
  }

  export interface TransformResult {
    labels?: number[];
    components?: number[][];
    probabilities?: number[][];
    scores?: number[];
  }

  export interface KMeans extends UnsupervisedModel<'clustering'> {
    type: 'clustering';
    nClusters: number;
    init: 'kmeans++' | 'random' | 'first';
    maxIter: number;
    tol: number;
    centroids: number[][];
    inertia: number;
    nInit: number;
  }

  export interface DBSCAN extends UnsupervisedModel<'clustering'> {
    type: 'clustering';
    eps: number;
    minSamples: number;
    metric: 'euclidean' | 'manhattan' | 'cosine';
    algorithm: 'brute' | 'ball_tree' | 'kd_tree';
    labels: number[];
    coreSampleIndices: number[];
  }

  export interface GaussianMixture extends UnsupervisedModel<'clustering'> {
    type: 'clustering';
    nComponents: number;
    covarianceType: 'full' | 'tied' | 'diag' | 'spherical';
    maxIter: number;
    initParams: 'kmeans' | 'random';
    weights: number[];
    means: number[][];
    covariances: number[][][];
  }

  export interface HierarchicalClustering extends UnsupervisedModel<'clustering'> {
    type: 'clustering';
    nClusters: number;
    metric: 'euclidean' | 'manhattan' | 'cosine' | 'ward';
    linkage: 'single' | 'complete' | 'average' | 'ward';
    dendrogram?: DendrogramNode;
  }

  export interface DendrogramNode {
    id: string;
    distance: number;
    children: DendrogramNode[];
    samples: number[];
  }

  export interface PCA extends UnsupervisedModel<'dimensionality_reduction'> {
    type: 'dimensionality_reduction';
    nComponents: number | 'mle';
    whiten: boolean;
    svdSolver: 'auto' | 'full' | 'arpack' | 'randomized';
    explainedVariance: number[];
    explainedVarianceRatio: number[];
    components: number[][];
    mean: number[];
  }

  export interface tSNE extends UnsupervisedModel<'dimensionality_reduction'> {
    type: 'dimensionality_reduction';
    nComponents: number;
    perplexity: number;
    learningRate: number;
    nIter: number;
    metric: 'euclidean' | 'precomputed';
    embedding: number[][];
    klDivergence: number;
  }

  export interface UMAP extends UnsupervisedModel<'dimensionality_reduction'> {
    type: 'dimensionality_reduction';
    nComponents: number;
    nNeighbors: number;
    minDist: number;
    metric: 'euclidean' | 'manhattan' | 'cosine';
    embedding: number[][];
  }

  export interface Autoencoder extends UnsupervisedModel<'dimensionality_reduction'> {
    type: 'dimensionality_reduction';
    architecture: EncoderDecoder;
    latentDim: number;
    loss: 'mse' | 'binary_crossentropy';
    encoder: Layer[];
    decoder: Layer[];
    encoded: number[][];
  }

  export interface EncoderDecoder {
    encoder: Layer[];
    decoder: Layer[];
  }

  export interface IsolationForest extends UnsupervisedModel<'density_estimation'> {
    type: 'density_estimation';
    nEstimators: number;
    contamination: number | 'auto';
    maxFeatures: number;
    maxSamples: number;
    randomState: number;
    anomalyScores: number[];
    threshold: number;
  }

  export interface ClusteringEvaluation {
    silhouetteScore: number;
    daviesBouldinIndex: number;
    calinskiHarabaszIndex: number;
    inertia: number;
  }
}

// Cross-reference: 01_Supervised_Learning.ts, 03_Reinforcement_Learning.ts
console.log("\n=== Unsupervised Learning Types ===");
console.log("Related: 01_Supervised_Learning.ts, 03_Reinforcement_Learning.ts");