/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 04_Blockchain
 * Concept: 01_Smart_Contracts
 * Topic: 01_Ethereum_Types
 * Purpose: Define Ethereum smart contract types
 * Difficulty: advanced
 * UseCase: Blockchain
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Ethers.js
 * Performance: O(n) contract interaction, gas optimization
 * Security: ABI encoding, address validation
 */

namespace EthereumTypes {
  export interface EthereumConfig {
    network: Network;
    provider: ProviderType;
    account?: Account;
    gasPrice?: bigint;
    gasLimit?: bigint;
  }

  export type Network = 'mainnet' | 'goerli' | 'sepolia' | 'localhost' | 'custom';
  export type ProviderType = 'infura' | 'alchemy' | 'etherscan' | 'websocket' | 'http';

  export interface Account {
    address: string;
    privateKey?: string;
    publicKey?: string;
    mnemonic?: string;
  }

  export interface Contract {
    address: string;
    abi: ABI;
    interface: Interface;
    runner?: ethers.ContractRunner;
  }

  export interface ABI extends Array<FunctionFragment | EventFragment | ErrorFragment> {}

  export interface FunctionFragment {
    type: 'function';
    name: string;
    inputs: Param[];
    outputs: Param[];
    stateMutability: 'pure' | 'view' | 'nonpayable' | 'payable';
    constant: boolean;
  }

  export interface EventFragment {
    type: 'event';
    name: string;
    inputs: EventParam[];
    anonymous: boolean;
  }

  export interface ErrorFragment {
    type: 'error';
    name: string;
    inputs: Param[];
  }

  export interface Param {
    name: string;
    type: string;
    indexed?: boolean;
    components?: Param[];
  }

  export interface EventParam {
    name: string;
    type: string;
    indexed: boolean;
  }

  export interface TransactionRequest {
    to?: string;
    from?: string;
    nonce?: number;
    gasLimit?: bigint;
    gasPrice?: bigint;
    maxPriorityFeePerGas?: bigint;
    maxFeePerGas?: bigint;
    data?: string;
    value?: bigint;
    chainId?: number;
    type?: number;
    accessList?: AccessList;
  }

  export interface AccessList {
    address: string;
    storageKeys: string[];
  }

  export interface TransactionResponse {
    hash: string;
    blockNumber?: number;
    blockHash?: string;
    timestamp?: number;
    from: string;
    to?: string;
    value: bigint;
    gasLimit: bigint;
    gasPrice: bigint;
    data: string;
    nonce: number;
    chainId: number;
    status?: number;
    logs: Log[];
    confirmations: number;
  }

  export interface Log {
    address: string;
    topics: string[];
    data: string;
    blockNumber: number;
    transactionHash: string;
    logIndex: number;
  }

  export interface Block {
    number: number;
    hash: string;
    parentHash: string;
    timestamp: number;
    nonce: string;
    difficulty: bigint;
    totalDifficulty: bigint;
    size: number;
    gasLimit: bigint;
    gasUsed: bigint;
    miner: string;
    transactions: string[] | TransactionResponse[];
    logsBloom: string;
  }

  export interface ContractTransaction extends TransactionResponse {
    wait(confirmations?: number): Promise<TransactionReceipt>;
  }

  export interface TransactionReceipt {
    to: string;
    from: string;
    contractAddress?: string;
    transactionIndex: number;
    blockHash: string;
    blockNumber: number;
    cumulativeGasUsed: bigint;
    gasUsed: bigint;
    logs: Log[];
    logsBloom: string;
    status: number;
    effectiveGasPrice: bigint;
  }

  export interface Filter {
    fromBlock?: number | string;
    toBlock?: number | string;
    address?: string;
    topics?: (string | string[] | null)[];
    blockHash?: string;
  }

  export interface EventFilter extends Filter {
    address?: string;
    topics?: (string | string[] | null)[];
  }

  export interface Wallet {
    address: string;
    connect(provider: ethers.Provider): Wallet;
    getBalance(): Promise<bigint>;
    getNonce(): Promise<number>;
    sendTransaction(tx: TransactionRequest): Promise<ContractTransaction>;
    signMessage(message: string | Uint8Array): Promise<string>;
    signTransaction(tx: TransactionRequest): Promise<string>;
  }

  export interface ContractFactory {
    deploy(bytecode: string, abi: ABI): Promise<Contract>;
    attach(address: string): Contract;
  }

  export interface CallStaticResult<T = unknown> {
    success: boolean;
    value: T;
    error?: string;
  }
}

// Cross-reference: 02_Solidity_Types.ts (contract language)
console.log("\n=== Ethereum Types ===");
console.log("Related: 02_Solidity_Types.ts");