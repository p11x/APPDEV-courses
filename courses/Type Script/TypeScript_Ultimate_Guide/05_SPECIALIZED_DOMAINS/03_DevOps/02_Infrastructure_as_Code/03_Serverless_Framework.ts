/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 03_DevOps
 * Concept: 02_Infrastructure_as_Code
 * Topic: 03_Serverless_Framework
 * Purpose: Define Serverless Framework configuration types
 * Difficulty: intermediate
 * UseCase: DevOps
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Serverless Framework
 * Performance: Optimized cold starts, layer caching
 * Security: Environment encryption, VPC support
 */

namespace ServerlessFrameworkTypes {
  export interface ServerlessConfig {
    service: string;
    frameworkVersion: string;
    provider: Provider;
    functions?: Record<string, FunctionConfig>;
    layers?: Record<string, LayerConfig>;
    resources?: Resources;
    plugins?: string[];
    package?: PackageConfig;
    custom?: Record<string, unknown>;
  }

  export interface Provider {
    name: 'aws' | 'azure' | 'gcp' | 'kubernetes' | 'openwhisk' | 'fn' | 'spotinst' | 'alibaba';
    runtime?: string;
    stage?: string;
    region?: string;
    memorySize?: number;
    timeout?: number;
    logRetentionInDays?: number;
    deploymentBucket?: DeploymentBucket;
    vpc?: VpcConfig;
    iam?: IamConfig;
    environment?: Record<string, string>;
    tags?: Record<string, string>;
    tracing?: TracingConfig;
    endpointType?: 'EDGE' | 'REGIONAL' | 'PRIVATE';
  }

  export interface DeploymentBucket {
    name?: string;
    blockPublicAccess?: boolean;
    serverSideEncryption?: 'AES256' | 'KMS';
    versioning?: boolean;
    skipPolicySetup?: boolean;
  }

  export interface VpcConfig {
    vpcId: string;
    securityGroupIds?: string[];
    subnetIds?: string[];
  }

  export interface IamConfig {
    role?: IamRole;
    deploymentRole?: string;
    lambdaExecutionRole?: string;
  }

  export interface IamRole {
    name?: string;
    path?: string;
    managedPolicies?: string[];
    statements?: IamStatement[];
  }

  export interface IamStatement {
    Effect: 'Allow' | 'Deny';
    Action: string[];
    Resource: string | string[];
    Condition?: Record<string, Record<string, unknown>>;
  }

  export interface TracingConfig {
    lambda?: 'Active' | 'PassThrough';
    apiGateway?: 'ACTIVE' | 'PASS_THROUGH';
  }

  export interface FunctionConfig {
    handler: string;
    runtime?: string;
    memorySize?: number;
    timeout?: number;
    description?: string;
    reservedConcurrency?: number;
    provisionConcurrency?: number;
    events?: FunctionEvent[];
    layers?: string[];
    environment?: Record<string, string>;
    vpc?: VpcConfig;
    tags?: Record<string, string>;
    tracing?: 'Active' | 'PassThrough';
    onError?: string;
    deadLetterQueue?: DeadLetterQueue;
    destination?: DestinationConfig;
  }

  export interface FunctionEvent {
    http?: HttpEvent;
    httpApi?: HttpApiEvent;
    schedule?: ScheduleEvent;
    s3?: S3Event;
    sns?: SnsEvent;
    sqs?: SqsEvent;
    stream?: StreamEvent;
    websocket?: WebsocketEvent;
    alb?: AlbEvent;
    cloudwatch?: CloudwatchEvent;
    iot?: IotEvent;
    alexaSkill?: AlexaEvent;
    alexaSmartHome?: AlexaEvent;
  }

  export interface HttpEvent {
    path: string;
    method: string | string[];
    cors?: CorsConfig;
    authorizer?: AuthorizerConfig;
    integration?: 'lambda' | 'lambda-proxy' | 'http' | 'http-proxy';
    response?: HttpResponseConfig;
  }

  export interface CorsConfig {
    origin: string;
    headers?: string[];
    maxAge?: number;
    allowCredentials?: boolean;
  }

  export interface AuthorizerConfig {
    name: string;
    type: 'token' | 'request';
    identitySource?: string;
    resultTtlInSeconds?: number;
    authorizerUri?: string;
    authorizerCredentials?: string;
  }

  export interface HttpResponseConfig {
    statusCode?: string;
    headers?: Record<string, string>;
    template?: string;
  }

  export interface ScheduleEvent {
    rate: string;
    enabled?: boolean;
    input?: string | Record<string, unknown>;
    description?: string;
  }

  export interface S3Event {
    bucket: string;
    event: string;
    rules?: S3Rule[];
    existing?: boolean;
  }

  export interface S3Rule {
    prefix?: string;
    suffix?: string;
  }

  export interface SnsEvent {
    topic: string;
    filter?: Record<string, unknown>;
  }

  export interface SqsEvent {
    arn: string;
    batchSize?: number;
    maximumBatchingWindow?: number;
    maximumRecordAge?: number;
  }

  export interface StreamEvent {
    arn: string;
    batchSize?: number;
    startingPosition?: 'LATEST' | 'TRIM_HORIZON';
    filterPatterns?: unknown[];
  }

  export interface WebsocketEvent {
    route: string;
    authorizer?: AuthorizerConfig;
  }

  export interface AlbEvent {
    listener: string;
    priority: number;
    conditions: AlbCondition[];
    targetGroup: string;
  }

  export interface AlbCondition {
    path: string;
    header?: Record<string, string>;
    query?: Record<string, string>;
    method?: string[];
    ip?: string[];
  }

  export interface DeadLetterQueue {
    type: 'sqs' | 'sns';
    arn: string;
  }

  export interface DestinationConfig {
    onSuccess?: string;
    onFailure?: string;
  }

  export interface LayerConfig {
    path: string;
    name?: string;
    description?: string;
    compatibleRuntimes?: string[];
    licenseInfo?: string;
    retain?: boolean;
  }

  export interface Resources {
    Resources?: Record<string, ResourceTemplate>;
    Outputs?: Record<string, OutputTemplate>;
    Conditions?: Record<string, unknown>;
    Mappings?: Record<string, unknown>;
  }

  export interface ResourceTemplate {
    Type: string;
    Properties?: Record<string, unknown>;
    DependsOn?: string | string[];
  }

  export interface OutputTemplate {
    Description?: string;
    Value: unknown;
    Export?: { Name: string };
  }

  export interface PackageConfig {
    individually?: boolean;
    include?: string[];
    exclude?: string[];
    artifact?: string;
    compiledCloudFormationTemplate?: string;
  }
}

// Cross-reference: 01_AWS_CDK_Types.ts, 02_Pulumi_Types.ts
console.log("\n=== Serverless Framework Types ===");
console.log("Related: 01_AWS_CDK_Types.ts, 02_Pulumi_Types.ts");