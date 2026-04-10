/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 04_Blockchain
 * Concept: 03_Web3_Integration
 * Topic: 01_Ethers_Canister
 * Purpose: Define Ethers.js and Dfinity Canister integration types
 * Difficulty: advanced
 * UseCase: Blockchain
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(1) provider queries, efficient caching
 * Security: Type-safe contract calls, secure RPC
 */

namespace EthersCanisterTypes {
  export interface Provider {
    network: Network;
    getBlockNumber(): Promise<number>;
    getGasPrice(): Promise<bigint>;
    getBalance(address: string): Promise<bigint>;
    getTransactionCount(address: string): Promise<number>;
    getCode(address: string): Promise<string>;
    getStorageAt(address: string, position: string): Promise<string>;
    getBlock(blockHashOrNumber: number | string): Promise<Block>;
    getTransaction(txHash: string): Promise<Transaction>;
    getTransactionReceipt(txHash: string): Promise<Receipt>;
    call(tx: TransactionRequest): Promise<string>;
    sendTransaction(tx: TransactionRequest): Promise<TransactionResponse>;
    send(method: string, params?: unknown[]): Promise<unknown>;
    on(event: string, listener: Listener): Provider;
    once(event: string, listener: Listener): Provider;
    off(event: string, listener?: Listener): Provider;
    removeAllListeners(event?: string): Provider;
  }

  export interface Network {
    name: string;
    chainId: number;
    ensAddress?: string;
  }

  export interface Block {
    number: number;
    hash: string;
    parentHash: string;
    timestamp: number;
    nonce: string;
    difficulty: bigint;
    gasLimit: bigint;
    gasUsed: bigint;
    miner: string;
    transactions: string[];
  }

  export interface Transaction {
    hash: string;
    blockHash?: string;
    blockNumber?: number;
    from: string;
    to?: string;
    value: bigint;
    gasLimit: bigint;
    gasPrice?: bigint;
    maxFeePerGas?: bigint;
    maxPriorityFeePerGas?: bigint;
    data: string;
    nonce: number;
    chainId: number;
    type?: number;
  }

  export interface TransactionRequest {
    to?: string;
    from?: string;
    nonce?: number;
    gasLimit?: bigint;
    gasPrice?: bigint;
    maxFeePerGas?: bigint;
    maxPriorityFeePerGas?: bigint;
    data?: string;
    value?: bigint;
    chainId?: number;
  }

  export interface TransactionResponse extends Transaction {
    wait(confirmations?: number): Promise<Receipt>;
  }

  export interface Receipt {
    to: string;
    from: string;
    contractAddress?: string;
    blockNumber: number;
    blockHash: string;
    status: number;
    gasUsed: bigint;
    logs: Log[];
    logsBloom: string;
    cumulativeGasUsed: bigint;
    effectiveGasPrice: bigint;
  }

  export interface Log {
    address: string;
    topics: string[];
    data: string;
    blockNumber: number;
    transactionHash: string;
    logIndex: number;
  }

  export type Listener = (...args: unknown[]) => void;

  export interface CanisterConfig {
    canisterId: string;
    canisterUri: string;
    managementCanister?: string;
  }

  export interface CanisterActor {
    canisterId: string;
    interfaces: CanisterInterface[];
    call(method: string, args?: unknown[]): Promise<unknown>;
    query(method: string, args?: unknown[]): Promise<unknown>;
  }

  export interface CanisterInterface {
    method: string;
    args: CanisterType[];
    returnType: CanisterType;
    mode: 'query' | 'update';
  }

  export interface CanisterType {
    type: 'text' | 'nat' | 'int' | 'bool' | 'null' | 'vec' | 'opt' | 'record' | 'variant' | 'func' | 'principal' | 'empty';
    inner?: CanisterType;
    fields?: Record<string, CanisterType>;
    variants?: Record<string, CanisterType>;
    subtypes?: CanisterType[];
  }

  export interface Wallet extends Provider {
    getAddress(): Promise<string>;
    getSigner(): Signer;
    signMessage(message: string | Uint8Array): Promise<string>;
    signTransaction(tx: TransactionRequest): Promise<string>;
  }

  export interface Signer {
    getAddress(): Promise<string>;
    signMessage(message: string | Uint8Array): Promise<string>;
    signTransaction(tx: TransactionRequest): Promise<string>;
    sendTransaction(tx: TransactionRequest): Promise<TransactionResponse>;
    getChainId(): Promise<number>;
    getNetwork(): Promise<Network>;
  }
}

// Cross-reference: 02_Wallet_Types.ts (wallet integration)
console.log("\n=== Ethers Canister Types ===");
console.log("Related: 02_Wallet_Types.ts");