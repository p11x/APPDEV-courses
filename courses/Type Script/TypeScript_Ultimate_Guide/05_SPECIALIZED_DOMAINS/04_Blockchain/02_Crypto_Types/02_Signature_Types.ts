/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 04_Blockchain
 * Concept: 02_Crypto_Types
 * Topic: 02_Signature_Types
 * Purpose: Define digital signature types for blockchain
 * Difficulty: advanced
 * UseCase: Blockchain
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n) signature verification, O(1) signing
 * Security: ECDSA, schnorr signatures, message authentication
 */

namespace SignatureTypes {
  export type SignatureAlgorithm = 'ecdsa' | 'schnorr' | 'bip322' | 'eip191' | 'eip712';

  export interface Signature {
    algorithm: SignatureAlgorithm;
    r: string;
    s: string;
    v?: number;
    compact?: boolean;
  }

  export interface ECDSASignature extends Signature {
    algorithm: 'ecdsa';
    r: string;
    s: string;
    v: number;
    recoveryParam?: number;
  }

  export interface SchnorrSignature extends Signature {
    algorithm: 'schnorr';
    r: string;
    s: string;
    nonce?: string;
  }

  export interface SignedMessage {
    message: string;
    signature: Signature;
    signer: string;
    domain?: SignTypedDataDomain;
    types?: Record<string, TypedDataField[]>;
    primaryType?: string;
  }

  export interface SignTypedDataDomain {
    name?: string;
    version?: string;
    chainId?: number;
    verifyingContract?: string;
    salt?: string;
  }

  export interface TypedDataField {
    name: string;
    type: string;
  }

  export interface KeyPair {
    privateKey: string;
    publicKey: string;
    compressed?: boolean;
    address?: string;
  }

  export interface PublicKey {
    x: string;
    y: string;
    compressed?: string;
    uncompressed?: string;
  }

  export interface PrivateKey {
    hex: string;
    bytes: Uint8Array;
  }

  export interface WalletSignature {
    address: string;
    message: string;
    signature: string;
    version?: string;
  }

  export interface MessageSignature {
    message: string;
    signature: Signature;
    signer: string;
    timestamp?: number;
  }

  export interface TransactionSignature {
    transaction: string;
    signature: Signature;
    chainId: number;
    nonce: number;
    from: string;
  }

  export interface MultiSigSignature {
    threshold: number;
    signers: string[];
    signatures: Signature[];
    signed: string[];
  }

  export interface Signer {
    getAddress(): Promise<string>;
    signMessage(message: string | Uint8Array): Promise<Signature>;
    signTransaction(tx: unknown): Promise<Signature>;
    signTypedData(domain: SignTypedDataDomain, types: Record<string, TypedDataField[]>, message: Record<string, unknown>): Promise<Signature>;
  }

  export interface Verifier {
    verifySignature(signature: Signature, message: string, signer: string): Promise<boolean>;
    verifyTypedData(domain: SignTypedDataDomain, types: Record<string, TypedDataField[]>, message: Record<string, unknown>, signature: Signature, signer: string): Promise<boolean>;
    recoverAddress(signature: Signature, message: string): Promise<string>;
  }

  export interface SignatureUtils {
    parseSignature(signature: string, algorithm: SignatureAlgorithm): Signature;
    serializeSignature(signature: Signature, format?: 'der' | 'compact'): string;
    toEIP155(signature: Signature, chainId: number): Signature;
    fromEIP155(signature: Signature, chainId: number): Signature;
  }
}

// Cross-reference: 01_Hash_Types.ts (hash functions)
console.log("\n=== Signature Types ===");
console.log("Related: 01_Hash_Types.ts");