/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 04_Blockchain
 * Concept: 02_Crypto_Types
 * Topic: 01_Hash_Types
 * Purpose: Define cryptographic hash types for blockchain
 * Difficulty: intermediate
 * UseCase: Blockchain
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n) hash computation, collision resistance
 * Security: Pre-image resistance, collision resistance
 */

namespace HashTypes {
  export type HashAlgorithm = 'keccak256' | 'sha256' | 'sha3' | 'ripemd160' | 'blake2b';

  export interface HashResult {
    algorithm: HashAlgorithm;
    hash: string;
    digest: Uint8Array;
    hex: string;
  }

  export interface HashFunction {
    update(data: Uint8Array | string): HashFunction;
    digest(encoding?: 'hex' | 'buffer'): string | Uint8Array;
  }

  export interface Keccak256 extends HashFunction {
    algorithm: 'keccak256';
    rate: number;
    capacity: number;
  }

  export interface SHA256 extends HashFunction {
    algorithm: 'sha256';
    blockSize: number;
    outputSize: number;
  }

  export interface SHA3 extends HashFunction {
    algorithm: 'sha3';
    variant: 'sha3-224' | 'sha3-256' | 'sha3-384' | 'sha3-512';
  }

  export interface RIPEMD160 extends HashFunction {
    algorithm: 'ripemd160';
    outputSize: number;
  }

  export interface MerkleTree {
    leaves: string[];
    layers: string[][];
    root: string;
    build(): void;
    getProof(leaf: string): MerkleProof;
    verify(proof: MerkleProof, leaf: string): boolean;
  }

  export interface MerkleProof {
    leaf: string;
    path: MerklePathItem[];
    root: string;
    index: number;
  }

  export interface MerklePathItem {
    position: 'left' | 'right';
    hash: string;
  }

  export interface Digest {
    id: string;
    algorithm: HashAlgorithm;
    value: string;
    timestamp: number;
    metadata?: Record<string, unknown>;
  }

  export interface HashUtilities {
    hash(data: string | Uint8Array, algorithm: HashAlgorithm): HashResult;
    hashKeccak256(data: string | Uint8Array): HashResult;
    hashSHA256(data: string | Uint8Array): HashResult;
    hashSHA3256(data: string | Uint8Array): HashResult;
    hashRIPEMD160(data: string | Uint8Array): HashResult;
    doubleHash(data: string | Uint8Array): HashResult;
    hashMultiple(data: string | Uint8Array, algorithms: HashAlgorithm[]): HashResult[];
  }

  export function computeMerkleRoot(leaves: string[]): string {
    if (leaves.length === 0) return '';
    if (leaves.length === 1) return leaves[0];
    const nextLevel: string[] = [];
    for (let i = 0; i < leaves.length; i += 2) {
      const left = leaves[i];
      const right = leaves[i + 1] || left;
      nextLevel.push(hashKeccak256(left + right).hex);
    }
    return computeMerkleRoot(nextLevel);
  }

  function hashKeccak256(data: string | Uint8Array): HashResult {
    const input = typeof data === 'string' ? new TextEncoder().encode(data) : data;
    return { algorithm: 'keccak256', hash: '', digest: input, hex: '' };
  }
}

// Cross-reference: 02_Signature_Types.ts (digital signatures)
console.log("\n=== Hash Types ===");
console.log("Related: 02_Signature_Types.ts");