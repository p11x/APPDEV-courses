/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 04_Blockchain
 * Concept: 03_Web3_Integration
 * Topic: 02_Wallet_Types
 * Purpose: Define Web3 wallet connection types
 * Difficulty: intermediate
 * UseCase: Blockchain
 * Version: TypeScript 5.0+
 * Compatibility: Modern Browsers (EIP-1193)
 * Performance: O(1) account switching, efficient event handling
 * Security: Secure request handling, connection validation
 */

namespace WalletTypes {
  export interface WalletProvider {
    isMetaMask?: boolean;
    isCoinbaseWallet?: boolean;
    isWalletConnect?: boolean;
    isPhantom?: boolean;
    chainId?: string;
    networkVersion?: string;
    selectedAddress?: string;
    isConnected(): boolean;
    request<T = unknown>(request: JsonRpcRequest): Promise<T>;
    on(event: string, listener: JsonRpcListener): void;
    removeListener(event: string, listener: JsonRpcListener): void;
    removeAllListeners(event?: string): void;
    isUnauthorized(): boolean;
    isLocked(): Promise<boolean>;
    getChainId(): Promise<string>;
    getNetwork(): Promise<string>;
    getBalance(): Promise<string>;
    getAccounts(): Promise<string[]>;
    getTransactionCount(): Promise<number>;
  }

  export interface JsonRpcRequest {
    method: string;
    params?: unknown[];
  }

  export interface JsonRpcResponse {
    id: number;
    jsonrpc: string;
    result?: unknown;
    error?: JsonRpcError;
  }

  export interface JsonRpcError {
    code: number;
    message: string;
    data?: unknown;
  }

  export type JsonRpcListener = (result: unknown) => void;

  export interface WalletConnection {
    connect(provider?: WalletProvider): Promise<WalletState>;
    disconnect(): void;
    onConnect(callback: (state: WalletState) => void): void;
    onDisconnect(callback: (error: Error) => void): void;
    onChainChange(callback: (chainId: string) => void): void;
    onAccountChange(callback: (accounts: string[]) => void): void;
  }

  export interface WalletState {
    connected: boolean;
    chainId: string;
    account: string;
    balance?: string;
    network?: string;
    error?: string;
  }

  export interface WalletBalance {
    address: string;
    balance: string;
    balanceFormatted: string;
    chainId: string;
  }

  export interface WalletPermissions {
    eth_accounts: boolean;
    eth_chainId: boolean;
    eth_requestAccounts: boolean;
    wallet_addEthereumChain?: boolean;
    wallet_switchEthereumChain?: boolean;
    wallet_watchAsset?: boolean;
  }

  export interface ChainInfo {
    chainId: string;
    chainName: string;
    nativeCurrency: NativeCurrency;
    rpcUrls: string[];
    blockExplorerUrls?: string[];
    iconUrls?: string[];
  }

  export interface NativeCurrency {
    name: string;
    symbol: string;
    decimals: number;
  }

  export interface TokenInfo {
    address: string;
    symbol: string;
    decimals: number;
    name?: string;
    logoURI?: string;
  }

  export interface WalletTransaction {
    from: string;
    to: string;
    value?: string;
    gasLimit?: string;
    gasPrice?: string;
    maxFeePerGas?: string;
    maxPriorityFeePerGas?: string;
    data?: string;
    nonce?: number;
    chainId?: number;
  }

  export interface TransactionApproval {
    hash: string;
    from: string;
    to: string;
    value: string;
    gasLimit: string;
    gasPrice?: string;
    maxFeePerGas?: string;
    maxPriorityFeePerGas?: string;
    data: string;
  }

  export interface SignMessageRequest {
    message: string | Uint8Array;
    type: 'message' | 'typedData';
  }

  export interface TypedDataMessage {
    domain: TypedDataDomain;
    message: Record<string, unknown>;
    primaryType: string;
    types: Record<string, TypedDataField[]>;
  }

  export interface TypedDataDomain {
    name?: string;
    version?: string;
    chainId?: string;
    verifyingContract?: string;
    salt?: string;
  }

  export interface TypedDataField {
    name: string;
    type: string;
  }

  export interface WalletEvents {
    connect: (info: { chainId: string }) => void;
    disconnect: (error: Error) => void;
    chainChanged: (chainId: string) => void;
    accountsChanged: (accounts: string[]) => void;
    message: (message: { type: string; data: unknown }) => void;
  }
}

// Cross-reference: 01_Ethers_Canister.ts (ethers integration)
console.log("\n=== Wallet Types ===");
console.log("Related: 01_Ethers_Canister.ts");