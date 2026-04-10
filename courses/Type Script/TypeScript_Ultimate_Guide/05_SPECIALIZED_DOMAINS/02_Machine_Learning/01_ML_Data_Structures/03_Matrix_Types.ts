/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 02_Machine_Learning
 * Concept: 01_ML_Data_Structures
 * Topic: 03_Matrix_Types
 * Purpose: Define matrix types for linear algebra operations
 * Difficulty: intermediate
 * UseCase: machine learning
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n²) matrix operations, O(n³) multiplication
 * Security: Bounds checking prevents buffer overflows
 */

namespace MatrixTypes {
  export interface Matrix<T = number> {
    rows: number;
    cols: number;
    data: T[][];
    dtype: DataType;
  }

  export type DataType = 'float32' | 'float64' | 'int32' | 'int64';

  export interface Vector extends Matrix {
    rows: number;
    cols: 1;
  }

  export interface SparseMatrix {
    rows: number;
    cols: number;
    format: SparseFormat;
    indices: number[];
    values: number[];
  }

  export type SparseFormat = 'coo' | 'csr' | 'csc';

  export interface MatrixOperations {
    add(a: Matrix, b: Matrix): Matrix;
    subtract(a: Matrix, b: Matrix): Matrix;
    multiply(a: Matrix, b: Matrix): Matrix;
    hadamard(a: Matrix, b: Matrix): Matrix;
    transpose(a: Matrix): Matrix;
    inverse(a: Matrix): Matrix;
    determinant(a: Matrix): number;
    trace(a: Matrix): number;
  }

  export interface MatrixDecomposition {
    LU: { L: Matrix; U: Matrix };
    QR: { Q: Matrix; R: Matrix };
    SVD: { U: Matrix; S: Vector; V: Matrix };
    Eigen: { values: Vector; vectors: Matrix };
  }

  export interface MatrixFactory {
    zeros(rows: number, cols: number): Matrix;
    ones(rows: number, cols: number): Matrix;
    identity(n: number): Matrix;
    random(rows: number, cols: number, distribution?: Distribution): Matrix;
    fromArray(data: number[][], dtype?: DataType): Matrix;
  }

  export type Distribution = 'uniform' | 'normal' | 'xavier' | 'he';

  export interface MatrixView {
    matrix: Matrix;
    rowStart: number;
    rowEnd: number;
    colStart: number;
    colEnd: number;
  }

  export interface BandMatrix extends Matrix {
    lowerBandwidth: number;
    upperBandwidth: number;
  }

  export interface BlockMatrix {
    blocks: Matrix[][];
    blockSize: [number, number];
  }

  export function multiply(a: Matrix, b: Matrix): Matrix {
    if (a.cols !== b.rows) throw new Error('Matrix dimensions mismatch');
    const result: number[][] = [];
    for (let i = 0; i < a.rows; i++) {
      result[i] = [];
      for (let j = 0; j < b.cols; j++) {
        let sum = 0;
        for (let k = 0; k < a.cols; k++) {
          sum += a.data[i][k] * b.data[k][j];
        }
        result[i][j] = sum;
      }
    }
    return { rows: a.rows, cols: b.cols, data: result, dtype: a.dtype };
  }

  export function transpose(a: Matrix): Matrix {
    const result: number[][] = [];
    for (let i = 0; i < a.cols; i++) {
      result[i] = [];
      for (let j = 0; j < a.rows; j++) {
        result[i][j] = a.data[j][i];
      }
    }
    return { rows: a.cols, cols: a.rows, data: result, dtype: a.dtype };
  }
}

// Cross-reference: 01_Tensor_Types.ts (tensors), 02_Dataset_Types.ts (datasets)
console.log("\n=== Matrix Types ===");
console.log("Related: 01_Tensor_Types.ts, 02_Dataset_Types.ts");