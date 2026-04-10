/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 05_IoT
 * Concept: 03_Edge_Computing
 * Topic: 01_Edge_Device_Types
 * Purpose: Define edge computing device types for IoT
 * Difficulty: advanced
 * UseCase: IoT
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Edge Devices
 * Performance: Local processing, reduced latency, bandwidth optimization
 * Security: Secure boot, hardware encryption
 */

namespace EdgeDeviceTypes {
  export type EdgeTier = 'edge_gateway' | 'edge_node' | 'edge_controller' | 'edge_accelerator';

  export interface EdgeDevice {
    id: string;
    name: string;
    tier: EdgeTier;
    hardware: HardwareSpec;
    software: SoftwareSpec;
    network: NetworkConfig;
    capabilities: EdgeCapabilities;
    status: DeviceStatus;
  }

  export interface HardwareSpec {
    cpu: CPUInfo;
    memory: MemoryInfo;
    storage: StorageInfo;
    gpu?: GPUInfo;
    fpga?: FPGAInfo;
    tpu?: TPUInfo;
    connectivity: ConnectivityInfo;
  }

  export interface CPUInfo {
    architecture: 'x86' | 'arm' | 'risc-v';
    cores: number;
    threads: number;
    frequency: number;
    tdp: number;
    virtualization: boolean;
  }

  export interface MemoryInfo {
    type: 'ddr4' | 'ddr5' | 'lpddr4' | 'lpddr5';
    capacity: number;
    bandwidth: number;
    ecc: boolean;
  }

  export interface StorageInfo {
    type: 'ssd' | 'nvme' | 'emmc' | 'sd';
    capacity: number;
    readSpeed: number;
    writeSpeed: number;
  }

  export interface GPUInfo {
    model: string;
    vram: number;
    cores: number;
    tflops: number;
    computeCapability: string;
  }

  export interface FPGAInfo {
    model: string;
    logicCells: number;
    dspBlocks: number;
    blockRAM: number;
  }

  export interface TPUInfo {
    model: string;
    tops: number;
    memory: number;
    inference: boolean;
    training: boolean;
  }

  export interface ConnectivityInfo {
    ethernet: number;
    wifi: boolean;
    bluetooth: boolean;
    cellular: boolean;
    zigbee: boolean;
    z-wave: boolean;
    lora: boolean;
    nb_iot: boolean;
  }

  export interface SoftwareSpec {
    os: string;
    kernel: string;
    runtime: string;
    containerRuntime: string;
    mlRuntime?: string;
    edgeAgent: EdgeAgentVersion;
  }

  export interface EdgeAgentVersion {
    name: string;
    version: string;
    apiVersion: string;
  }

  export interface NetworkConfig {
    ipAddress: string;
    subnet: string;
    gateway: string;
    dns: string[];
    latency: number;
    bandwidth: number;
    failover: boolean;
  }

  export interface EdgeCapabilities {
    compute: ComputeCapabilities;
    storage: StorageCapabilities;
    ml: MLCapabilities;
    protocol: string[];
    localProcessing: boolean;
  }

  export interface ComputeCapabilities {
    containerOrchestration: boolean;
    kubernetes: boolean;
    serverless: boolean;
    realTime: boolean;
    parallelProcessing: boolean;
  }

  export interface StorageCapabilities {
    localDatabase: boolean;
    cache: boolean;
    timeSeries: boolean;
    blob: boolean;
    maxStorage: number;
  }

  export interface MLCapabilities {
    inference: boolean;
    training: boolean;
    onDeviceTraining: boolean;
    federatedLearning: boolean;
    modelFormats: string[];
    frameworks: string[];
  }

  export interface DeviceStatus {
    state: 'online' | 'offline' | 'degraded' | 'updating' | 'maintenance';
    uptime: number;
    cpuUsage: number;
    memoryUsage: number;
    diskUsage: number;
    temperature: number;
    powerConsumption: number;
    lastHeartbeat: number;
  }

  export interface EdgeNode extends EdgeDevice {
    tier: 'edge_node';
    clusters: ClusterInfo[];
    workloads: WorkloadInfo[];
    neighboringNodes: string[];
  }

  export interface ClusterInfo {
    clusterId: string;
    role: 'leader' | 'follower';
    members: string[];
    health: 'healthy' | 'degraded' | 'unhealthy';
  }

  export interface WorkloadInfo {
    workloadId: string;
    type: 'container' | 'function' | 'vm' | 'ml';
    status: 'running' | 'stopped' | 'pending' | 'error';
    resources: ResourceUsage;
  }

  export interface ResourceUsage {
    cpu: number;
    memory: number;
    gpu?: number;
    network: number;
  }

  export interface EdgeGateway extends EdgeDevice {
    tier: 'edge_gateway';
    upstreamConnections: UpstreamConnection[];
    downstreamConnections: DownstreamConnection[];
    protocolTranslation: string[];
  }

  export interface UpstreamConnection {
    endpoint: string;
    protocol: string;
    status: 'connected' | 'disconnected' | 'error';
    bandwidth: number;
    latency: number;
  }

  export interface DownstreamConnection {
    deviceId: string;
    protocol: string;
    status: 'connected' | 'disconnected';
    devices: number;
  }

  export interface EdgeController extends EdgeDevice {
    tier: 'edge_controller';
    managedDevices: string[];
    policies: Policy[];
    firmware: FirmwareConfig;
  }

  export interface Policy {
    id: string;
    name: string;
    type: 'security' | 'compute' | 'network' | 'data';
    rules: PolicyRule[];
  }

  export interface PolicyRule {
    condition: string;
    action: 'allow' | 'deny' | 'throttle' | 'redirect';
    parameters: Record<string, unknown>;
  }

  export interface FirmwareConfig {
    version: string;
    updateServer: string;
    autoUpdate: boolean;
    rollback: boolean;
  }

  export interface EdgeOrchestrator {
    deploy(deviceId: string, workload: WorkloadInfo): Promise<DeploymentResult>;
    scale(deviceId: string, replicas: number): Promise<void>;
    migrate(fromDevice: string, toDevice: string, workloadId: string): Promise<void>;
    monitor(): Promise<EdgeDevice[]>;
  }

  export interface DeploymentResult {
    success: boolean;
    workloadId: string;
    error?: string;
  }
}

// Cross-reference: 01_Sensor_Types.ts (data), 01_MQTT_Types.ts (communication)
console.log("\n=== Edge Device Types ===");
console.log("Related: 01_Sensor_Types.ts, 01_MQTT_Types.ts");