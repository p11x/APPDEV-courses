/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 02_Machine_Learning
 * Concept: 03_Model_Integration
 * Topic: 02_ONNX_Types
 * Purpose: Define ONNX model types for interoperability
 * Difficulty: advanced
 * UseCase: machine learning
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n) model loading, optimized inference
 * Security: Schema validation prevents malicious models
 */

namespace ONNXTypes {
  export type ONNXModelType = 'onnx' | 'onnx_proto';

  export interface ONNXModel {
    irVersion: number;
    opsetImports: OpsetImport[];
    producerName: string;
    producerVersion: string;
    domain: string;
    modelVersion: number;
    docString: string;
    graph: Graph;
    metadataProps: MetadataProp[];
  }

  export interface OpsetImport {
    version: number;
    domain?: string;
  }

  export interface Graph {
    name: string;
    inputs: ValueInfo[];
    outputs: ValueInfo[];
    initializers: Tensor[];
    nodes: Node[];
    valueInfo: ValueInfo[];
  }

  export interface ValueInfo {
    name: string;
    type: Type;
    docString?: string;
  }

  export interface Type {
    tensorType?: TensorType;
    sequenceType?: SequenceType;
    mapType?: MapType;
    optionalType?: OptionalType;
  }

  export interface TensorType {
    elemType: number;
    shape: TensorShape;
  }

  export interface TensorShape {
    dims: (number | string)[];
  }

  export interface SequenceType {
    elemType: Type;
  }

  export interface MapType {
    keyType: number;
    valueType: Type;
  }

  export interface OptionalType {
    elemType: Type;
  }

  export interface Tensor {
    name: string;
    dims: number[];
    dataType: number;
    rawData?: Uint8Array;
    floatData?: number[];
    int64Data?: number[];
  }

  export interface Node {
    input: string[];
    output: string[];
    name: string;
    opType: string;
    domain: string;
    attribute: Attribute[];
  }

  export interface Attribute {
    name: string;
    type: AttributeType;
    value: unknown;
    refAttrName?: string;
  }

  export type AttributeType = 'UNDEFINED' | 'FLOAT' | 'INT' | 'STRING' | 'TENSOR' | 'GRAPH' | 'FLOATS' | 'INTS' | 'STRINGS' | 'TENSORS' | 'GRAPHS';

  export interface MetadataProp {
    key: string;
    value: string;
  }

  export interface ONNXRuntime {
    session: InferenceSession;
    inputNames: string[];
    outputNames: string[];
    createSession(modelPath: string): Promise<InferenceSession>;
    run(inputs: Record<string, Tensor>): Promise<Record<string, Tensor>>;
  }

  export interface InferenceSession {
    inputNames: string[];
    outputNames: string[];
    run(inputs: Record<string, Tensor>): Promise<Record<string, Tensor>>;
  }

  export interface ONNXTensor {
    type: string;
    shape: number[];
    data: Float32Array | Int32Array | Uint8Array;
  }

  export interface ModelOptimizer {
    optimize(model: ONNXModel, options: OptimizeOptions): ONNXModel;
  }

  export interface OptimizeOptions {
    foldConstants: boolean;
    eliminateIdentity: boolean;
    eliminateNopPad: boolean;
    fuseConvBN: boolean;
    inferShapes: boolean;
  }

  export interface ONNXValidator {
    validate(model: ONNXModel): ValidationResult;
  }

  export interface ValidationResult {
    valid: boolean;
    errors: ValidationError[];
    warnings: ValidationWarning[];
  }

  export interface ValidationError {
    node?: string;
    message: string;
  }

  export interface ValidationWarning {
    node?: string;
    message: string;
  }
}

// Cross-reference: 01_TensorFlow_Integration.ts, 03_Model_Serving.ts
console.log("\n=== ONNX Types ===");
console.log("Related: 01_TensorFlow_Integration.ts, 03_Model_Serving.ts");