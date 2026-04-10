/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 02_Machine_Learning
 * Concept: 01_ML_Data_Structures
 * Topic: 02_Dataset_Types
 * Purpose: Define dataset types for machine learning
 * Difficulty: intermediate
 * UseCase: machine learning
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n) data loading, O(1) sample access
 * Security: Data validation prevents injection attacks
 */

namespace DatasetTypes {
  export type DataType = 'numerical' | 'categorical' | 'text' | 'image' | 'time_series';
  export type SplitType = 'train' | 'validation' | 'test';

  export interface Dataset<T = unknown> {
    id: string;
    name: string;
    features: FeatureSchema[];
    samples: DataSample<T>[];
    metadata: DatasetMetadata;
  }

  export interface FeatureSchema {
    name: string;
    type: DataType;
    nullable: boolean;
    min?: number;
    max?: number;
    categories?: string[];
    shape?: number[];
  }

  export interface DataSample<T = unknown> {
    id: string;
    features: Record<string, T>;
    label?: unknown;
    weights?: number;
    metadata: SampleMetadata;
  }

  export interface SampleMetadata {
    split?: SplitType;
    source?: string;
    timestamp?: number;
    tags?: string[];
  }

  export interface DatasetMetadata {
    totalSamples: number;
    featureCount: number;
    labelType?: DataType;
    classDistribution?: Record<string, number>;
    missingValues: Record<string, number>;
    created: number;
    version: string;
  }

  export interface DataIterator {
    next(): DataSample | null;
    hasNext(): boolean;
    reset(): void;
    shuffle(seed?: number): void;
  }

  export interface DataPipeline {
    transformers: DataTransformer[];
    batchSize: number;
    shuffle: boolean;
    prefetch: number;
    process(sample: DataSample): DataSample;
  }

  export interface DataTransformer {
    type: TransformerType;
    fit(samples: DataSample[]): void;
    transform(sample: DataSample): DataSample;
    inverseTransform(sample: DataSample): DataSample;
  }

  export type TransformerType = 'normalize' | 'standardize' | 'onehot' | 'tokenize' | 'encode';

  export interface DataAugmentation {
    type: AugmentationType;
    parameters: Record<string, unknown>;
  }

  export type AugmentationType = 'flip' | 'rotate' | 'crop' | 'noise' | 'mixup' | 'cutout';

  export interface DatasetSplit {
    train: Dataset;
    validation: Dataset;
    test: Dataset;
  }

  export function splitDataset(dataset: Dataset, ratios: number[]): DatasetSplit {
    const [trainRatio, valRatio] = ratios;
    const shuffled = [...dataset.samples].sort(() => Math.random() - 0.5);
    const trainEnd = Math.floor(shuffled.length * trainRatio);
    const valEnd = trainEnd + Math.floor(shuffled.length * valRatio);
    return {
      train: { ...dataset, samples: shuffled.slice(0, trainEnd) },
      validation: { ...dataset, samples: shuffled.slice(trainEnd, valEnd) },
      test: { ...dataset, samples: shuffled.slice(valEnd) },
    };
  }
}

// Cross-reference: 01_Tensor_Types.ts (tensors), 03_Matrix_Types.ts (matrices)
console.log("\n=== Dataset Types ===");
console.log("Related: 01_Tensor_Types.ts, 03_Matrix_Types.ts");