/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 03_DevOps
 * Concept: 02_Infrastructure_as_Code
 * Topic: 01_AWS_CDK_Types
 * Purpose: Define AWS Cloud Development Kit types
 * Difficulty: advanced
 * UseCase: DevOps
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, AWS CDK
 * Performance: Synthesize CloudFormation, parallel stacks
 * Security: IAM role validation, secret encryption
 */

namespace AWSCDKTypes {
  export interface App {
    outdir: string;
    version: string;
    synth(): SynthesizedStack[];
    synthStack(name: string): SynthesizedStack;
  }

  export interface Stack {
    stackId: string;
    stackName: string;
    template: CloudFormationTemplate;
    synthesis(): SynthesizedStack;
  }

  export interface CloudFormationTemplate {
    AWSTemplateFormatVersion?: string;
    Description?: string;
    Parameters?: Record<string, Parameter>;
    Mappings?: Record<string, Record<string, Record<string, string>>>;
    Conditions?: Record<string, FnIntrinsic>;
    Resources?: Record<string, Resource>;
    Outputs?: Record<string, Output>;
  }

  export interface Parameter {
    Type: 'String' | 'Number' | 'List<Number>' | 'CommaDelimitedList' | 'AWS::EC2::VPC::Id' | 'AWS::EC2::Subnet::Id' | 'AWS::SSM::Parameter::Type';
    Default?: unknown;
    Description?: string;
    AllowedValues?: string[];
    MinLength?: number;
    MaxLength?: number;
    MinValue?: number;
    MaxValue?: number;
    ConstraintDescription?: string;
    NoEcho?: boolean;
  }

  export interface Resource {
    Type: string;
    Properties?: Record<string, unknown>;
    DependsOn?: string | string[];
    Condition?: string;
    DeletionPolicy?: 'Delete' | 'Retain' | 'Snapshot';
    UpdateReplacePolicy?: 'Delete' | 'Retain' | 'Snapshot';
    Metadata?: Record<string, unknown>;
  }

  export interface Output {
    Description?: string;
    Value: FnIntrinsic;
    Export?: { Name: FnIntrinsic };
    Condition?: string;
  }

  export type FnIntrinsic = 
    | { 'Fn::Ref': string }
    | { 'Fn::GetAtt': [string, string] }
    | { 'Fn::Sub': string | Record<string, string> }
    | { 'Fn::Join': [string, string[]] }
    | { 'Fn::If': [string, FnIntrinsic, FnIntrinsic] }
    | { 'Fn::Equals': [FnIntrinsic, FnIntrinsic] }
    | { 'Fn::Not': [FnIntrinsic] }
    | { 'Fn::Or': FnIntrinsic[] }
    | { 'Fn::And': FnIntrinsic[] }
    | { 'Fn::Base64': FnIntrinsic }
    | { 'Fn::GetAZs': string }
    | { 'Fn::ImportValue': FnIntrinsic }
    | { 'Fn::Split': [string, FnIntrinsic] }
    | { 'Fn::Select': [number, FnIntrinsic] }
    | 'Ref' | 'Condition';

  export interface SynthesizedStack {
    name: string;
    template: CloudFormationTemplate;
    assets: Asset[];
    dependencies: string[];
  }

  export interface Asset {
    path: string;
    packaging: 'file' | 'zip';
    sourceHash: string;
  }

  export interface Construct {
    id: string;
    node: ConstructNode;
    applyAspect(aspect: IAspect): void;
  }

  export interface ConstructNode {
    id: string;
    path: string;
    children: Map<string, Construct>;
    scopes: Construct[];
    addDependency(scope: Construct): void;
  }

  export interface IAspect {
    visit(construct: Construct): void;
  }

  export interface VpcStack extends Stack {
    vpc: Vpc;
  }

  export interface Vpc {
    vpcId: string;
    cidr: string;
    publicSubnets: Subnet[];
    privateSubnets: Subnet[];
    isolatedSubnets: Subnet[];
    internetGatewayId: string;
  }

  export interface Subnet {
    subnetId: string;
    cidr: string;
    availabilityZone: string;
    routeTableId: string;
  }

  export interface Ec2Instance extends Construct {
    instanceId: string;
    instanceType: string;
    machineImage: IMachineImage;
    vpc: ISecurityGroup;
    keyName?: string;
  }

  export interface IMachineImage {
    getImage(scope: Construct): AmazonMachineImage;
  }

  export interface AmazonMachineImage {
    imageId: string;
    os: OperatingSystem;
  }

  export type OperatingSystem = 'LINUX' | 'WINDOWS';

  export interface ISecurityGroup {
    securityGroupId: string;
  }

  export interface Role extends Construct {
    roleName: string;
    roleArn: string;
    assumeRolePolicy: unknown;
    managedPolicies: string[];
  }

  export interface Bucket extends Construct {
    bucketName: string;
    bucketArn: string;
    region: string;
    encryption: BucketEncryption;
    versioning: boolean;
  }

  export type BucketEncryption = 'S3_MANAGED' | 'KMS' | 'UNENCRYPTED';

  export interface LambdaFunction extends Construct {
    functionName: string;
    functionArn: string;
    runtime: string;
    handler: string;
    code: CodeConfig;
    environment: Record<string, string>;
    timeout: number;
    memorySize: number;
  }

  export interface CodeConfig {
    image?: ContainerImage;
    inline?: string;
    s3?: S3Code;
    zip?: Asset;
  }

  export interface ContainerImage {
    imageUri: string;
  }

  export interface S3Code {
    bucket: string;
    key: string;
  }

  export interface Table extends Construct {
    tableName: string;
    tableArn: string;
    partitionKey: Attribute;
    sortKey?: Attribute;
    billingMode: 'PAY_PER_REQUEST' | 'PROVISIONED';
  }

  export interface Attribute {
    name: string;
    type: 'S' | 'N' | 'B';
  }
}

// Cross-reference: 02_Pulumi_Types.ts, 03_Serverless_Framework.ts
console.log("\n=== AWS CDK Types ===");
console.log("Related: 02_Pulumi_Types.ts, 03_Serverless_Framework.ts");