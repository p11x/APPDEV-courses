/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 04_Blockchain
 * Concept: 01_Smart_Contracts
 * Topic: 02_Solidity_Types
 * Purpose: Define Solidity language and compiler types
 * Difficulty: advanced
 * UseCase: Blockchain
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Solc compiler
 * Performance: Efficient compilation, optimization passes
 * Security: Safe math, reentrancy guards
 */

namespace SolidityTypes {
  export interface SoliditySource {
    fileName: string;
    content: string;
    language: 'Solidity';
    version: string;
  }

  export interface CompilationInput {
    language: string;
    sources: Record<string, SoliditySource>;
    settings: CompilerSettings;
  }

  export interface CompilerSettings {
    optimizer: OptimizerSettings;
    evmVersion: string;
    libraries: Record<string, string>;
    outputSelection: OutputSelection;
    metadata: MetadataSettings;
  }

  export interface OptimizerSettings {
    enabled: boolean;
    runs: number;
    details: OptimizerDetails;
  }

  export interface OptimizerDetails {
    peephole: boolean;
    inliner: boolean;
    jumpdestRemover: boolean;
    orderLiterals: boolean;
    cse: boolean;
    constantOptimizer: boolean;
    yul: boolean;
    yulDetails: YulDetails;
  }

  export interface YulDetails {
    stackAllocation: boolean;
    optimizerSteps: string;
  }

  export interface MetadataSettings {
    useLiteralContent: boolean;
    hashes: boolean;
    appendCBOR: boolean;
  }

  export interface OutputSelection {
    '*'?: Record<string, (OutputName | string)[]>;
    [file: string]: Record<string, (OutputName | string)[]>;
  }

  export type OutputName = 'evm.bytecode' | 'evm.deployedBytecode' | 'evm.assembly' | 'evm.gasEstimates' | 'evm.methodIdentifiers' | 'abi' | 'natspec' | 'devdoc' | 'userdoc' | 'metadata' | 'ir' | 'irOptimized';

  export interface CompilationOutput {
    contracts: Record<string, Record<string, ContractCompilation>>;
    sources: Record<string, SourceCompilation>;
    errors: CompilerError[];
    version: string;
  }

  export interface ContractCompilation {
    abi: ABI;
    metadata: string;
    bytecode: BytecodeObject;
    deployedBytecode: BytecodeObject;
    asm?: Assembly;
    functionHashes?: Record<string, string>;
    gasEstimates?: GasEstimates;
  }

  export interface ABI {
    name?: string;
    type: 'constructor' | 'function' | 'event' | 'error';
    inputs?: FunctionParameter[];
    outputs?: FunctionParameter[];
    stateMutability?: 'pure' | 'view' | 'nonpayable' | 'payable';
  }

  export interface FunctionParameter {
    name: string;
    type: string;
    internalType?: string;
    components?: FunctionParameter[];
  }

  export interface BytecodeObject {
    object: string;
    opcodes: string;
    sourceMap: string;
    linkReferences?: Record<string, Record<string, LinkReference[]>>;
  }

  export interface LinkReference {
    length: number;
    start: number;
  }

  export interface Assembly {
    '.code': AssemblyInstruction[];
    '.auxdata': string;
  }

  export interface AssemblyInstruction {
    name?: string;
    args?: string[];
    sourceIndex?: number;
  }

  export interface GasEstimates {
    creation: CreationGas;
    external: Record<string, string>;
    internal: Record<string, string>;
  }

  export interface CreationGas {
    codeDeploymentCost: string;
    codeDepositCost: string;
  }

  export interface SourceCompilation {
    id: number;
    ast: ASTNode;
    legacyAST?: ASTNode;
  }

  export interface ASTNode {
    nodeType: string;
    src: string;
    children?: ASTNode[];
    name?: string;
    attributes?: Record<string, unknown>;
    [key: string]: unknown;
  }

  export interface CompilerError {
    severity: 'error' | 'warning' | 'info';
    type: string;
    sourceLocation?: SourceLocation;
    message: string;
    errorCode?: string;
    formattedMessage: string;
  }

  export interface SourceLocation {
    file: string;
    start: number;
    end: number;
  }

  export interface SolidityType {
    name: string;
    kind: 'value' | 'reference' | 'mapping';
    base?: string;
    arrayLength?: number;
    keyType?: string;
    valueType?: SolidityType;
  }

  export interface ContractDefinition {
    name: string;
    kind: 'contract' | 'interface' | 'library';
    abstract: boolean;
    baseContracts: InheritanceSpecifier[];
    subNodes: ContractMember[];
  }

  export interface InheritanceSpecifier {
    baseName: string;
    arguments?: Expression[];
  }

  export interface ContractMember {
    type: 'function' | 'variable' | 'event' | 'modifier' | 'struct' | 'enum';
    name: string;
    visibility: 'public' | 'private' | 'internal' | 'external';
    stateMutability?: string;
  }

  export interface Expression {
    type: string;
    value?: unknown;
    arguments?: Expression[];
  }
}

// Cross-reference: 01_Ethereum_Types.ts (Ethereum integration)
console.log("\n=== Solidity Types ===");
console.log("Related: 01_Ethereum_Types.ts");