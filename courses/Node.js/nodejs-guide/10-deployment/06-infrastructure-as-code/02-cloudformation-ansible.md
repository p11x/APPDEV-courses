# CloudFormation, AWS CDK & Ansible for Node.js Deployment

## What You'll Learn

- AWS CloudFormation templates for full-stack infrastructure
- Nested stacks, cross-stack references, custom resources
- AWS CDK with TypeScript for infrastructure
- Ansible playbooks for Node.js deployment automation
- Puppet and Chef overview with Node.js examples
- IaC best practices and security

## AWS CloudFormation

### VPC Stack

```yaml
# cloudformation/vpc.yaml
AWSTemplateFormatVersion: '2010-09-09'
Description: VPC with public/private subnets across 3 AZs

Parameters:
  ProjectName:
    Type: String
    Default: my-node-app
  Environment:
    Type: String
    AllowedValues: [development, staging, production]
  VpcCidr:
    Type: String
    Default: '10.0.0.0/16'

Resources:
  VPC:
    Type: AWS::EC2::VPC
    Properties:
      CidrBlock: !Ref VpcCidr
      EnableDnsHostnames: true
      EnableDnsSupport: true
      Tags:
        - Key: Name
          Value: !Sub '${ProjectName}-${Environment}-vpc'

  InternetGateway:
    Type: AWS::EC2::InternetGateway
    Properties:
      Tags:
        - Key: Name
          Value: !Sub '${ProjectName}-${Environment}-igw'

  GatewayAttachment:
    Type: AWS::EC2::VPCGatewayAttachment
    Properties:
      VpcId: !Ref VPC
      InternetGatewayId: !Ref InternetGateway

  PublicSubnet1:
    Type: AWS::EC2::Subnet
    Properties:
      VpcId: !Ref VPC
      CidrBlock: '10.0.1.0/24'
      AvailabilityZone: !Select [0, !GetAZs '']
      MapPublicIpOnLaunch: true
      Tags:
        - Key: Name
          Value: !Sub '${ProjectName}-public-1'

  PublicSubnet2:
    Type: AWS::EC2::Subnet
    Properties:
      VpcId: !Ref VPC
      CidrBlock: '10.0.2.0/24'
      AvailabilityZone: !Select [1, !GetAZs '']
      MapPublicIpOnLaunch: true
      Tags:
        - Key: Name
          Value: !Sub '${ProjectName}-public-2'

  PrivateSubnet1:
    Type: AWS::EC2::Subnet
    Properties:
      VpcId: !Ref VPC
      CidrBlock: '10.0.10.0/24'
      AvailabilityZone: !Select [0, !GetAZs '']
      Tags:
        - Key: Name
          Value: !Sub '${ProjectName}-private-1'

  PrivateSubnet2:
    Type: AWS::EC2::Subnet
    Properties:
      VpcId: !Ref VPC
      CidrBlock: '10.0.11.0/24'
      AvailabilityZone: !Select [1, !GetAZs '']
      Tags:
        - Key: Name
          Value: !Sub '${ProjectName}-private-2'

  NatGatewayEIP:
    Type: AWS::EC2::EIP
    DependsOn: GatewayAttachment
    Properties:
      Domain: vpc

  NatGateway:
    Type: AWS::EC2::NatGateway
    Properties:
      AllocationId: !GetAtt NatGatewayEIP.AllocationId
      SubnetId: !Ref PublicSubnet1

  PublicRouteTable:
    Type: AWS::EC2::RouteTable
    Properties:
      VpcId: !Ref VPC
      Tags:
        - Key: Name
          Value: !Sub '${ProjectName}-public-rt'

  PublicRoute:
    Type: AWS::EC2::Route
    DependsOn: GatewayAttachment
    Properties:
      RouteTableId: !Ref PublicRouteTable
      DestinationCidrBlock: '0.0.0.0/0'
      GatewayId: !Ref InternetGateway

  PrivateRouteTable:
    Type: AWS::EC2::RouteTable
    Properties:
      VpcId: !Ref VPC
      Tags:
        - Key: Name
          Value: !Sub '${ProjectName}-private-rt'

  PrivateRoute:
    Type: AWS::EC2::Route
    Properties:
      RouteTableId: !Ref PrivateRouteTable
      DestinationCidrBlock: '0.0.0.0/0'
      NatGatewayId: !Ref NatGateway

Outputs:
  VpcId:
    Value: !Ref VPC
    Export:
      Name: !Sub '${ProjectName}-${Environment}-VpcId'
  PublicSubnets:
    Value: !Join [',', [!Ref PublicSubnet1, !Ref PublicSubnet2]]
    Export:
      Name: !Sub '${ProjectName}-${Environment}-PublicSubnets'
  PrivateSubnets:
    Value: !Join [',', [!Ref PrivateSubnet1, !Ref PrivateSubnet2]]
    Export:
      Name: !Sub '${ProjectName}-${Environment}-PrivateSubnets'
```

### ECS Fargate Service with ALB

```yaml
# cloudformation/ecs-service.yaml
AWSTemplateFormatVersion: '2010-09-09'
Description: ECS Fargate service with ALB

Parameters:
  ProjectName:
    Type: String
  Environment:
    Type: String
  ImageUri:
    Type: String
  ContainerPort:
    Type: Number
    Default: 3000
  DesiredCount:
    Type: Number
    Default: 2
  Cpu:
    Type: String
    Default: '256'
  Memory:
    Type: String
    Default: '512'

Resources:
  ECSCluster:
    Type: AWS::ECS::Cluster
    Properties:
      ClusterName: !Sub '${ProjectName}-${Environment}'
      CapacityProviders: [FARGATE, FARGATE_SPOT]
      DefaultCapacityProviderStrategy:
        - CapacityProvider: FARGATE
          Weight: 1

  TaskExecutionRole:
    Type: AWS::IAM::Role
    Properties:
      AssumeRolePolicyDocument:
        Version: '2012-10-17'
        Statement:
          - Effect: Allow
            Principal:
              Service: ecs-tasks.amazonaws.com
            Action: sts:AssumeRole
      ManagedPolicyArns:
        - arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy

  TaskRole:
    Type: AWS::IAM::Role
    Properties:
      AssumeRolePolicyDocument:
        Version: '2012-10-17'
        Statement:
          - Effect: Allow
            Principal:
              Service: ecs-tasks.amazonaws.com
            Action: sts:AssumeRole
      Policies:
        - PolicyName: SecretsAccess
          PolicyDocument:
            Version: '2012-10-17'
            Statement:
              - Effect: Allow
                Action:
                  - secretsmanager:GetSecretValue
                Resource: !Sub 'arn:aws:secretsmanager:*:*:secret:${ProjectName}/${Environment}/*'

  TaskDefinition:
    Type: AWS::ECS::TaskDefinition
    Properties:
      Family: !Sub '${ProjectName}-app'
      Cpu: !Ref Cpu
      Memory: !Ref Memory
      NetworkMode: awsvpc
      RequiresCompatibilities: [FARGATE]
      ExecutionRoleArn: !GetAtt TaskExecutionRole.Arn
      TaskRoleArn: !GetAtt TaskRole.Arn
      ContainerDefinitions:
        - Name: app
          Image: !Ref ImageUri
          PortMappings:
            - ContainerPort: !Ref ContainerPort
          Environment:
            - Name: NODE_ENV
              Value: !Ref Environment
            - Name: PORT
              Value: !Sub '${ContainerPort}'
          Secrets:
            - Name: DATABASE_URL
              ValueFrom: !Sub 'arn:aws:secretsmanager:*:*:secret:${ProjectName}/${Environment}/db-url'
          LogConfiguration:
            LogDriver: awslogs
            Options:
              awslogs-group: !Ref LogGroup
              awslogs-region: !Ref 'AWS::Region'
              awslogs-stream-prefix: app
          HealthCheck:
            Command: ['CMD-SHELL', !Sub 'curl -f http://localhost:${ContainerPort}/health || exit 1']
            Interval: 30
            Timeout: 5
            Retries: 3
            StartPeriod: 60

  LogGroup:
    Type: AWS::Logs::LogGroup
    Properties:
      LogGroupName: !Sub '/ecs/${ProjectName}/${Environment}'
      RetentionInDays: 30

  ALB:
    Type: AWS::ElasticLoadBalancingV2::LoadBalancer
    Properties:
      Name: !Sub '${ProjectName}-${Environment}-alb'
      Scheme: internet-facing
      Type: application
      Subnets:
        - !ImportValue !Sub '${ProjectName}-${Environment}-PublicSubnet1'
        - !ImportValue !Sub '${ProjectName}-${Environment}-PublicSubnet2'
      SecurityGroups:
        - !Ref ALBSecurityGroup

  ALBSecurityGroup:
    Type: AWS::EC2::SecurityGroup
    Properties:
      GroupDescription: ALB security group
      VpcId: !ImportValue !Sub '${ProjectName}-${Environment}-VpcId'
      SecurityGroupIngress:
        - IpProtocol: tcp
          FromPort: 443
          ToPort: 443
          CidrIp: '0.0.0.0/0'
        - IpProtocol: tcp
          FromPort: 80
          ToPort: 80
          CidrIp: '0.0.0.0/0'

  TargetGroup:
    Type: AWS::ElasticLoadBalancingV2::TargetGroup
    Properties:
      Name: !Sub '${ProjectName}-${Environment}-tg'
      Port: !Ref ContainerPort
      Protocol: HTTP
      TargetType: ip
      VpcId: !ImportValue !Sub '${ProjectName}-${Environment}-VpcId'
      HealthCheckPath: /health
      HealthCheckIntervalSeconds: 30
      HealthyThresholdCount: 2
      UnhealthyThresholdCount: 3

  HTTPListener:
    Type: AWS::ElasticLoadBalancingV2::Listener
    Properties:
      LoadBalancerArn: !Ref ALB
      Port: 80
      Protocol: HTTP
      DefaultActions:
        - Type: forward
          TargetGroupArn: !Ref TargetGroup

  ECSService:
    Type: AWS::ECS::Service
    DependsOn: HTTPListener
    Properties:
      ServiceName: !Sub '${ProjectName}-app'
      Cluster: !Ref ECSCluster
      TaskDefinition: !Ref TaskDefinition
      DesiredCount: !Ref DesiredCount
      LaunchType: FARGATE
      NetworkConfiguration:
        AwsvpcConfiguration:
          Subnets:
            - !ImportValue !Sub '${ProjectName}-${Environment}-PrivateSubnet1'
            - !ImportValue !Sub '${ProjectName}-${Environment}-PrivateSubnet2'
          SecurityGroups:
            - !Ref ServiceSecurityGroup
      LoadBalancers:
        - ContainerName: app
          ContainerPort: !Ref ContainerPort
          TargetGroupArn: !Ref TargetGroup
      DeploymentConfiguration:
        MaximumPercent: 200
        MinimumHealthyPercent: 100

  ServiceSecurityGroup:
    Type: AWS::EC2::SecurityGroup
    Properties:
      GroupDescription: ECS service security group
      VpcId: !ImportValue !Sub '${ProjectName}-${Environment}-VpcId'
      SecurityGroupIngress:
        - IpProtocol: tcp
          FromPort: !Ref ContainerPort
          ToPort: !Ref ContainerPort
          SourceSecurityGroupId: !Ref ALBSecurityGroup

  AutoScalingTarget:
    Type: AWS::ApplicationAutoScaling::ScalableTarget
    Properties:
      MaxCapacity: 10
      MinCapacity: !Ref DesiredCount
      ResourceId: !Sub 'service/${ECSCluster}/${ECSService.Name}'
      ScalableDimension: ecs:service:DesiredCount
      ServiceNamespace: ecs

  AutoScalingPolicy:
    Type: AWS::ApplicationAutoScaling::ScalingPolicy
    Properties:
      PolicyName: cpu-scaling
      PolicyType: TargetTrackingScaling
      ScalableTargetId: !Ref AutoScalingTarget
      TargetTrackingScalingPolicyConfiguration:
        PredefinedMetricSpecification:
          PredefinedMetricType: ECSServiceAverageCPUUtilization
        TargetValue: 70

Outputs:
  ALBDnsName:
    Value: !GetAtt ALB.DNSName
  ServiceUrl:
    Value: !Sub 'http://${ALB.DNSName}'
```

### RDS with ElastiCache

```yaml
# cloudformation/data-stores.yaml
AWSTemplateFormatVersion: '2010-09-09'
Description: RDS PostgreSQL and ElastiCache Redis

Parameters:
  ProjectName:
    Type: String
  Environment:
    Type: String
  DBInstanceClass:
    Type: String
    Default: 'db.t3.medium'
  DBName:
    Type: String
    Default: myapp
  DBUsername:
    Type: String
    NoEcho: true
  DBPassword:
    Type: String
    NoEcho: true
    MinLength: 12

Resources:
  DBSubnetGroup:
    Type: AWS::RDS::DBSubnetGroup
    Properties:
      DBSubnetGroupDescription: !Sub '${ProjectName} DB subnet group'
      SubnetIds:
        - !ImportValue !Sub '${ProjectName}-${Environment}-PrivateSubnet1'
        - !ImportValue !Sub '${ProjectName}-${Environment}-PrivateSubnet2'

  DBSecurityGroup:
    Type: AWS::EC2::SecurityGroup
    Properties:
      GroupDescription: Database security group
      VpcId: !ImportValue !Sub '${ProjectName}-${Environment}-VpcId'
      SecurityGroupIngress:
        - IpProtocol: tcp
          FromPort: 5432
          ToPort: 5432
          SourceSecurityGroupId: !ImportValue !Sub '${ProjectName}-${Environment}-EcsServiceSG'

  RDSInstance:
    Type: AWS::RDS::DBInstance
    DeletionPolicy: Snapshot
    Properties:
      DBInstanceIdentifier: !Sub '${ProjectName}-${Environment}'
      Engine: postgres
      EngineVersion: '15'
      DBInstanceClass: !Ref DBInstanceClass
      DBName: !Ref DBName
      MasterUsername: !Ref DBUsername
      MasterUserPassword: !Ref DBPassword
      AllocatedStorage: 20
      MaxAllocatedStorage: 100
      StorageType: gp3
      StorageEncrypted: true
      MultiAZ: !If [IsProduction, true, false]
      DBSubnetGroupName: !Ref DBSubnetGroup
      VPCSecurityGroups:
        - !Ref DBSecurityGroup
      BackupRetentionPeriod: !If [IsProduction, 30, 7]
      DeletionProtection: !If [IsProduction, true, false]
      PerformanceInsightsEnabled: true
      MonitoringInterval: 60
      MonitoringRoleArn: !GetAtt RDSMonitoringRole.Arn

  RDSMonitoringRole:
    Type: AWS::IAM::Role
    Properties:
      AssumeRolePolicyDocument:
        Version: '2012-10-17'
        Statement:
          - Effect: Allow
            Principal:
              Service: monitoring.rds.amazonaws.com
            Action: sts:AssumeRole
      ManagedPolicyArns:
        - arn:aws:iam::aws:policy/service-role/AmazonRDSEnhancedMonitoringRole

  RedisSubnetGroup:
    Type: AWS::ElastiCache::SubnetGroup
    Properties:
      Description: !Sub '${ProjectName} Redis subnet group'
      SubnetIds:
        - !ImportValue !Sub '${ProjectName}-${Environment}-PrivateSubnet1'
        - !ImportValue !Sub '${ProjectName}-${Environment}-PrivateSubnet2'

  RedisSecurityGroup:
    Type: AWS::EC2::SecurityGroup
    Properties:
      GroupDescription: Redis security group
      VpcId: !ImportValue !Sub '${ProjectName}-${Environment}-VpcId'
      SecurityGroupIngress:
        - IpProtocol: tcp
          FromPort: 6379
          ToPort: 6379
          SourceSecurityGroupId: !ImportValue !Sub '${ProjectName}-${Environment}-EcsServiceSG'

  RedisReplicationGroup:
    Type: AWS::ElastiCache::ReplicationGroup
    Properties:
      ReplicationGroupDescription: !Sub '${ProjectName} Redis'
      Engine: redis
      EngineVersion: '7.0'
      CacheNodeType: cache.t3.medium
      NumCacheClusters: !If [IsProduction, 2, 1]
      AutomaticFailoverEnabled: !If [IsProduction, true, false]
      MultiAZEnabled: !If [IsProduction, true, false]
      CacheSubnetGroupName: !Ref RedisSubnetGroup
      SecurityGroupIds:
        - !Ref RedisSecurityGroup
      TransitEncryptionEnabled: true
      AtRestEncryptionEnabled: true
      SnapshotRetentionLimit: !If [IsProduction, 7, 1]

  DBSecret:
    Type: AWS::SecretsManager::Secret
    Properties:
      Name: !Sub '${ProjectName}/${Environment}/db-credentials'
      SecretString: !Sub |
        {
          "username": "${DBUsername}",
          "password": "${DBPassword}",
          "host": "${RDSInstance.Endpoint.Address}",
          "port": 5432,
          "dbname": "${DBName}"
        }

Conditions:
  IsProduction: !Equals [!Ref Environment, production]

Outputs:
  DBEndpoint:
    Value: !GetAtt RDSInstance.Endpoint.Address
  RedisEndpoint:
    Value: !GetAtt RedisReplicationGroup.PrimaryEndPoint.Address
  DBSecretArn:
    Value: !Ref DBSecret
```

### CloudFront Distribution

```yaml
# cloudformation/cloudfront.yaml
AWSTemplateFormatVersion: '2010-09-09'
Description: CloudFront CDN distribution

Parameters:
  ProjectName:
    Type: String
  ALBDnsName:
    Type: String
  DomainName:
    Type: String

Resources:
  CloudFrontOAI:
    Type: AWS::CloudFront::CloudFrontOriginAccessIdentity
    Properties:
      CloudFrontOriginAccessIdentityConfig:
        Comment: !Sub '${ProjectName} OAI'

  StaticAssetsBucket:
    Type: AWS::S3::Bucket
    Properties:
      BucketName: !Sub '${ProjectName}-static-assets'
      PublicAccessBlockConfiguration:
        BlockPublicAcls: true
        BlockPublicPolicy: true
        IgnorePublicAcls: true
        RestrictPublicBuckets: true

  Distribution:
    Type: AWS::CloudFront::Distribution
    Properties:
      DistributionConfig:
        Enabled: true
        DefaultRootObject: index.html
        HttpVersion: http2and3
        Origins:
          - Id: ALBOrigin
            DomainName: !Ref ALBDnsName
            CustomOriginConfig:
              HTTPPort: 80
              OriginProtocolPolicy: https-only
              OriginSSLProtocols: [TLSv1.2]
          - Id: S3Origin
            DomainName: !GetAtt StaticAssetsBucket.RegionalDomainName
            S3OriginConfig:
              OriginAccessIdentity: !Sub 'origin-access-identity/cloudfront/${CloudFrontOAI}'
        DefaultCacheBehavior:
          TargetOriginId: ALBOrigin
          ViewerProtocolPolicy: redirect-to-https
          AllowedMethods: [GET, HEAD, OPTIONS, PUT, POST, PATCH, DELETE]
          CachedMethods: [GET, HEAD]
          ForwardedValues:
            QueryString: true
            Headers: [Authorization, Accept, Content-Type]
            Cookies:
              Forward: whitelist
              WhitelistedNames: [session]
          DefaultTTL: 0
          MaxTTL: 0
          MinTTL: 0
        CacheBehaviors:
          - PathPattern: '/static/*'
            TargetOriginId: S3Origin
            ViewerProtocolPolicy: redirect-to-https
            AllowedMethods: [GET, HEAD]
            CachedMethods: [GET, HEAD]
            ForwardedValues:
              QueryString: false
              Cookies:
                Forward: none
            DefaultTTL: 86400
            MaxTTL: 31536000
            Compress: true
        PriceClass: PriceClass_100
        ViewerCertificate:
          AcmCertificateArn: !Sub 'arn:aws:acm:us-east-1:${AWS::AccountId}:certificate/${DomainName}'
          SslSupportMethod: sni-only
          MinimumProtocolVersion: TLSv1.2_2021

Outputs:
  DistributionId:
    Value: !Ref Distribution
  DistributionDomainName:
    Value: !GetAtt Distribution.DomainName
```

## Nested Stacks & Cross-Stack References

```yaml
# cloudformation/root-stack.yaml
AWSTemplateFormatVersion: '2010-09-09'
Description: Root stack composing all infrastructure

Parameters:
  Environment:
    Type: String
  ImageUri:
    Type: String
  DBPassword:
    Type: String
    NoEcho: true

Resources:
  VPCStack:
    Type: AWS::CloudFormation::Stack
    Properties:
      TemplateURL: https://s3.amazonaws.com/my-cfn-templates/vpc.yaml
      Parameters:
        ProjectName: my-node-app
        Environment: !Ref Environment

  DataStoresStack:
    Type: AWS::CloudFormation::Stack
    DependsOn: VPCStack
    Properties:
      TemplateURL: https://s3.amazonaws.com/my-cfn-templates/data-stores.yaml
      Parameters:
        ProjectName: my-node-app
        Environment: !Ref Environment
        DBPassword: !Ref DBPassword

  ECSStack:
    Type: AWS::CloudFormation::Stack
    DependsOn: DataStoresStack
    Properties:
      TemplateURL: https://s3.amazonaws.com/my-cfn-templates/ecs-service.yaml
      Parameters:
        ProjectName: my-node-app
        Environment: !Ref Environment
        ImageUri: !Ref ImageUri

  CloudFrontStack:
    Type: AWS::CloudFormation::Stack
    DependsOn: ECSStack
    Properties:
      TemplateURL: https://s3.amazonaws.com/my-cfn-templates/cloudfront.yaml
      Parameters:
        ProjectName: my-node-app
        ALBDnsName: !GetAtt ECSStack.Outputs.ALBDnsName
        DomainName: app.example.com
```

### Stack Policies (Protect Critical Resources)

```json
{
  "Statement": [
    {
      "Effect": "Deny",
      "Action": "Update:*",
      "Principal": "*",
      "Resource": "LogicalResourceId/RDSInstance"
    },
    {
      "Effect": "Deny",
      "Action": "Update:Replace",
      "Principal": "*",
      "Resource": "LogicalResourceId/RedisReplicationGroup"
    },
    {
      "Effect": "Allow",
      "Action": "Update:*",
      "Principal": "*",
      "Resource": "*"
    }
  ]
}
```

### Change Sets & Drift Detection

```bash
# Review changes before applying
aws cloudformation create-change-set \
  --stack-name my-node-app-prod \
  --change-set-name review-changes \
  --template-body file://ecs-service.yaml \
  --parameters ParameterKey=ImageUri,ParameterValue=123456.dkr.ecr.us-east-1.amazonaws.com/myapp:v2.0

# View change set
aws cloudformation describe-change-set \
  --stack-name my-node-app-prod \
  --change-set-name review-changes

# Execute after review
aws cloudformation execute-change-set \
  --stack-name my-node-app-prod \
  --change-set-name review-changes

# Detect configuration drift
aws cloudformation detect-stack-drift \
  --stack-name my-node-app-prod

# View drift results
aws cloudformation describe-stack-resource-drifts \
  --stack-name my-node-app-prod
```

### Custom Resource with Lambda

```yaml
# cloudformation/custom-resource.yaml
Resources:
  CustomResourceFunction:
    Type: AWS::Lambda::Function
    Properties:
      FunctionName: !Sub '${ProjectName}-custom-resource'
      Runtime: nodejs20.x
      Handler: index.handler
      Role: !GetAtt LambdaRole.Arn
      Code:
        ZipFile: |
          const { SecretsManagerClient, GetSecretValueCommand } = require('@aws-sdk/client-secrets-manager');
          const cfnResponse = require('./cfn-response');

          exports.handler = async (event) => {
            try {
              const client = new SecretsManagerClient();
              if (event.RequestType === 'Create' || event.RequestType === 'Update') {
                const secret = await client.send(new GetSecretValueCommand({
                  SecretId: event.ResourceProperties.SecretArn
                }));
                const parsed = JSON.parse(secret.SecretString);
                await cfnResponse.send(event, 'SUCCESS', { Host: parsed.host, Port: parsed.port });
              } else {
                await cfnResponse.send(event, 'SUCCESS', {});
              }
            } catch (err) {
              await cfnResponse.send(event, 'FAILED', { Error: err.message });
            }
          };

  DBConnectionInfo:
    Type: Custom::DBConnection
    Properties:
      ServiceToken: !GetAtt CustomResourceFunction.Arn
      SecretArn: !Ref DBSecret
```

## AWS CDK with TypeScript

```typescript
// cdk/lib/infrastructure-stack.ts
import * as cdk from 'aws-cdk-lib';
import * as ec2 from 'aws-cdk-lib/aws-ec2';
import * as ecs from 'aws-cdk-lib/aws-ecs';
import * as ecs_patterns from 'aws-cdk-lib/aws-ecs-patterns';
import * as rds from 'aws-cdk-lib/aws-5678-rds';
import * as elasticache from 'aws-cdk-lib/aws-elasticache';
import * as cloudfront from 'aws-cdk-lib/aws-cloudfront';
import * as origins from 'aws-cdk-lib/aws-cloudfront-origins';
import { Construct } from 'constructs';

export class NodeAppStack extends cdk.Stack {
  constructor(scope: Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props);

    const vpc = new ec2.Vpc(this, 'AppVpc', {
      maxAzs: 3,
      natGateways: 1,
      subnetConfiguration: [
        { name: 'Public', subnetType: ec2.SubnetType.PUBLIC, cidrMask: 24 },
        { name: 'Private', subnetType: ec2.SubnetType.PRIVATE_WITH_EGRESS, cidrMask: 24 },
      ],
    });

    const cluster = new ecs.Cluster(this, 'AppCluster', {
      vpc,
      containerInsights: true,
    });

    const db = new rds.DatabaseInstance(this, 'AppDatabase', {
      engine: rds.DatabaseInstanceEngine.postgres({ version: rds.PostgresEngineVersion.VER_15 }),
      instanceType: ec2.InstanceType.of(ec2.InstanceClass.T3, ec2.InstanceSize.MEDIUM),
      vpc,
      vpcSubnets: { subnetType: ec2.SubnetType.PRIVATE_WITH_EGRESS },
      multiAz: true,
      allocatedStorage: 20,
      maxAllocatedStorage: 100,
      storageEncrypted: true,
      backupRetention: cdk.Duration.days(30),
      deletionProtection: true,
    });

    const service = new ecs_patterns.ApplicationLoadBalancedFargateService(this, 'AppService', {
      cluster,
      cpu: 256,
      memoryLimitMiB: 512,
      desiredCount: 2,
      taskImageOptions: {
        image: ecs.ContainerImage.fromAsset('.'),
        containerPort: 3000,
        environment: {
          NODE_ENV: 'production',
          DB_HOST: db.dbInstanceEndpointAddress,
        },
        secrets: {
          DB_PASSWORD: ecs.Secret.fromSsmParameter(
            cdk.aws_ssm.StringParameter.fromSecureStringParameterAttributes(this, 'DBPass', {
              parameterName: '/myapp/prod/db-password',
            })
          ),
        },
      },
      publicLoadBalancer: true,
    });

    service.targetGroup.configureHealthCheck({
      path: '/health',
      interval: cdk.Duration.seconds(30),
    });

    const distribution = new cloudfront.Distribution(this, 'CDN', {
      defaultBehavior: {
        origin: new origins.LoadBalancerV2Origin(service.loadBalancer, {
          protocolPolicy: cloudfront.OriginProtocolPolicy.HTTPS_ONLY,
        }),
        viewerProtocolPolicy: cloudfront.ViewerProtocolPolicy.REDIRECT_TO_HTTPS,
        cachePolicy: cloudfront.CachePolicy.CACHING_DISABLED,
      },
    });

    new cdk.CfnOutput(this, 'ServiceURL', {
      value: `https://${distribution.distributionDomainName}`,
    });
  }
}
```

## Ansible for Node.js Deployment

### Inventory

```ini
# ansible/inventory/production
[webservers]
web1 ansible_host=10.0.1.10
web2 ansible_host=10.0.1.11

[webservers:vars]
ansible_user=deploy
ansible_ssh_private_key_file=~/.ssh/deploy_key
app_port=3000
node_env=production

[loadbalancers]
lb1 ansible_host=10.0.2.10

[dbservers]
db1 ansible_host=10.0.3.10
```

### Variables

```yaml
# ansible/group_vars/webservers.yml
---
app_name: my-node-app
app_user: deploy
app_dir: /opt/{{ app_name }}
app_repo: git@github.com:org/my-node-app.git
app_branch: main
node_version: '20'
pm2_instances: max
pm2_max_memory: 512M

nginx_server_name: app.example.com
nginx_client_max_body_size: 10m

db_host: 10.0.3.10
db_name: myapp_production
redis_host: 10.0.3.11

# ansible/group_vars/all/vault.yml (encrypted with ansible-vault)
vault_db_password: "s3cure_db_p@ssw0rd"
vault_jwt_secret: "jwt-signing-secret-256bit"
vault_session_secret: "express-session-secret"
```

### Main Playbook

```yaml
# ansible/playbooks/deploy.yml
---
- name: Deploy Node.js application
  hosts: webservers
  become: true
  vars_files:
    - ../group_vars/all/vault.yml
    - ../group_vars/webservers.yml

  handlers:
    - name: restart nginx
      systemd:
        name: nginx
        state: restarted
        enabled: true

    - name: reload nginx
      systemd:
        name: nginx
        state: reloaded

    - name: restart pm2
      shell: |
        cd {{ app_dir }}/current
        su - {{ app_user }} -c "pm2 reload ecosystem.config.js --env {{ node_env }}"
      listen: "restart application"

  tasks:
    - name: Install system dependencies
      apt:
        name:
          - git
          - build-essential
          - nginx
          - certbot
          - python3-certbot-nginx
        state: present
        update_cache: true

    - name: Install Node.js via nvm
      become_user: "{{ app_user }}"
      shell: |
        export NVM_DIR="$HOME/.nvm"
        [ -s "$NVM_DIR/nvm.sh" ] && . "$NVM_DIR/nvm.sh"
        nvm install {{ node_version }}
        nvm alias default {{ node_version }}
      args:
        creates: "/home/{{ app_user }}/.nvm/versions/node"

    - name: Clone application repository
      become_user: "{{ app_user }}"
      git:
        repo: "{{ app_repo }}"
        dest: "{{ app_dir }}/releases/{{ ansible_date_time.epoch }}"
        version: "{{ app_branch }}"
        accept_hostkey: true
      register: git_result

    - name: Symlink current release
      file:
        src: "{{ app_dir }}/releases/{{ ansible_date_time.epoch }}"
        dest: "{{ app_dir }}/current"
        state: link
        force: true
      when: git_result.changed
      notify: "restart application"

    - name: Install npm dependencies
      become_user: "{{ app_user }}"
      shell: |
        export NVM_DIR="$HOME/.nvm"
        [ -s "$NVM_DIR/nvm.sh" ] && . "$NVM_DIR/nvm.sh"
        cd {{ app_dir }}/current
        npm ci --production
      when: git_result.changed

    - name: Create environment file
      template:
        src: templates/env.j2
        dest: "{{ app_dir }}/current/.env"
        owner: "{{ app_user }}"
        group: "{{ app_user }}"
        mode: '0600'
      notify: "restart application"

    - name: Run database migrations
      become_user: "{{ app_user }}"
      shell: |
        cd {{ app_dir }}/current
        npx prisma migrate deploy
      environment:
        DATABASE_URL: "postgresql://app:{{ vault_db_password }}@{{ db_host }}:5432/{{ db_name }}"
      when: git_result.changed
      run_once: true

    - name: Configure PM2 ecosystem
      become_user: "{{ app_user }}"
      template:
        src: templates/ecosystem.config.js.j2
        dest: "{{ app_dir }}/current/ecosystem.config.js"

    - name: Start/Reload application with PM2
      become_user: "{{ app_user }}"
      shell: |
        cd {{ app_dir }}/current
        pm2 start ecosystem.config.js --env {{ node_env }} || pm2 reload ecosystem.config.js --env {{ node_env }}
      when: git_result.changed

    - name: Save PM2 process list
      become_user: "{{ app_user }}"
      shell: pm2 save

    - name: Configure PM2 startup
      shell: env PATH=$PATH:$(dirname $(which pm2)) pm2 startup systemd -u {{ app_user }} --hp /home/{{ app_user }}
      args:
        creates: /etc/systemd/system/pm2-{{ app_user }}.service

    - name: Configure Nginx
      template:
        src: templates/nginx.conf.j2
        dest: /etc/nginx/sites-available/{{ app_name }}
        mode: '0644'
      notify: reload nginx

    - name: Enable Nginx site
      file:
        src: /etc/nginx/sites-available/{{ app_name }}
        dest: /etc/nginx/sites-enabled/{{ app_name }}
        state: link
      notify: reload nginx

    - name: Remove default Nginx site
      file:
        path: /etc/nginx/sites-enabled/default
        state: absent
      notify: reload nginx

    - name: Clean up old releases (keep 5)
      shell: |
        cd {{ app_dir }}/releases
        ls -dt */ | tail -n +6 | xargs rm -rf
      args:
        executable: /bin/bash
```

### Roles Structure

```
ansible/
├── roles/
│   ├── common/
│   │   ├── tasks/main.yml
│   │   ├── handlers/main.yml
│   │   └── templates/
│   ├── nodejs/
│   │   ├── tasks/main.yml
│   │   ├── vars/main.yml
│   │   └── templates/nvm.sh.j2
│   ├── pm2/
│   │   ├── tasks/main.yml
│   │   ├── templates/ecosystem.config.js.j2
│   │   └── handlers/main.yml
│   └── nginx/
│       ├── tasks/main.yml
│       ├── templates/nginx.conf.j2
│       ├── handlers/main.yml
│       └── files/security-headers.conf
├── playbooks/
│   ├── deploy.yml
│   ├── rollback.yml
│   └── setup.yml
└── inventory/
    ├── production
    └── staging
```

### PM2 Ecosystem Template

```javascript
// ansible/roles/pm2/templates/ecosystem.config.js.j2
module.exports = {
  apps: [{
    name: '{{ app_name }}',
    script: './dist/server.js',
    instances: '{{ pm2_instances }}',
    exec_mode: 'cluster',
    max_memory_restart: '{{ pm2_max_memory }}',
    env: {
      NODE_ENV: 'production',
      PORT: {{ app_port }},
      DATABASE_URL: 'postgresql://app:{{ vault_db_password }}@{{ db_host }}:5432/{{ db_name }}',
      REDIS_URL: 'redis://{{ redis_host }}:6379',
      JWT_SECRET: '{{ vault_jwt_secret }}',
    },
    error_file: '/var/log/pm2/{{ app_name }}-error.log',
    out_file: '/var/log/pm2/{{ app_name }}-out.log',
    merge_logs: true,
    time: true,
    autorestart: true,
    watch: false,
    max_restarts: 10,
    min_uptime: '10s',
    listen_timeout: 8000,
    kill_timeout: 5000,
  }]
};
```

### Nginx Template

```nginx
# ansible/roles/nginx/templates/nginx.conf.j2
upstream {{ app_name }} {
    ip_hash;
    server 127.0.0.1:{{ app_port }};
}

server {
    listen 80;
    server_name {{ nginx_server_name }};
    return 301 https://$host$request_uri;
}

server {
    listen 443 ssl http2;
    server_name {{ nginx_server_name }};

    ssl_certificate /etc/letsencrypt/live/{{ nginx_server_name }}/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/{{ nginx_server_name }}/privkey.pem;
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_prefer_server_ciphers on;

    client_max_body_size {{ nginx_client_max_body_size }};

    location / {
        proxy_pass http://{{ app_name }};
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
        proxy_connect_timeout 60s;
        proxy_read_timeout 60s;
    }

    location /static/ {
        alias {{ app_dir }}/current/public/;
        expires 30d;
        add_header Cache-Control "public, immutable";
    }

    location /health {
        proxy_pass http://{{ app_name }};
        access_log off;
    }
}
```

### Dynamic Inventory for AWS

```yaml
# ansible/inventory/aws_ec2.yml
plugin: amazon.aws.aws_ec2
regions:
  - us-east-1
  - us-west-2
filters:
  tag:Project: my-node-app
  tag:Environment: production
  instance-state-name: running
keyed_groups:
  - key: placement.availability_zone
    prefix: az
  - key: tags.Role
    prefix: role
  - key: instance_type
    prefix: type
hostnames:
  - tag:Name
  - private-ip-address
compose:
  ansible_host: private_ip_address
  ansible_user: "'deploy'"
```

### Ansible Vault for Secrets

```bash
# Create encrypted secrets file
ansible-vault create ansible/group_vars/all/vault.yml

# Edit existing vault
ansible-vault edit ansible/group_vars/all/vault.yml

# Run playbook with vault
ansible-playbook playbooks/deploy.yml --ask-vault-pass

# Run with vault password file
ansible-playbook playbooks/deploy.yml --vault-password-file ~/.vault_pass

# Encrypt existing file
ansible-vault encrypt ansible/group_vars/all/secrets.yml

# View encrypted file
ansible-vault view ansible/group_vars/all/vault.yml
```

## Puppet & Chef Overview

### Puppet Manifest for Node.js

```puppet
# puppet/modules/nodejs/manifests/init.pp
class { 'nodejs':
  version => '20',
}

package { 'pm2':
  ensure   => present,
  provider => 'npm',
  require  => Class['nodejs'],
}

file { '/opt/my-node-app':
  ensure => directory,
  owner  => 'deploy',
  group  => 'deploy',
}

vcsrepo { '/opt/my-node-app/current':
  ensure   => present,
  provider => 'git',
  source   => 'git@github.com:org/my-node-app.git',
  revision => 'main',
  user     => 'deploy',
}

exec { 'npm-install':
  command     => 'npm ci --production',
  cwd         => '/opt/my-node-app/current',
  path        => ['/usr/bin', '/usr/local/bin'],
  environment => ['HOME=/home/deploy'],
  user        => 'deploy',
  require     => Vcsrepo['/opt/my-node-app/current'],
  subscribe   => Vcsrepo['/opt/my-node-app/current'],
}

file { '/opt/my-node-app/current/.env':
  ensure  => file,
  owner   => 'deploy',
  mode    => '0600',
  content => epp('nodejs/env.epp'),
}

service { 'my-node-app':
  ensure    => running,
  enable    => true,
  subscribe => Exec['npm-install'],
}
```

### Chef Recipe for Node.js

```ruby
# chef/cookbooks/nodejs/recipes/default.rb
include_recipe 'nodejs::npm'

directory '/opt/my-node-app' do
  owner 'deploy'
  group 'deploy'
  mode '0755'
  recursive true
end

git '/opt/my-node-app/current' do
  repository 'git@github.com:org/my-node-app.git'
  revision 'main'
  user 'deploy'
  action :sync
  notifies :run, 'execute[npm-install]', :immediately
end

execute 'npm-install' do
  command 'npm ci --production'
  cwd '/opt/my-node-app/current'
  user 'deploy'
  environment 'HOME' => '/home/deploy'
  action :nothing
end

template '/opt/my-node-app/current/.env' do
  source 'env.erb'
  owner 'deploy'
  mode '0600'
  variables(
    db_password: data_bag_item('secrets', 'db')['password'],
    jwt_secret: data_bag_item('secrets', 'jwt')['secret']
  )
  notifies :restart, 'service[my-node-app]'
end

template '/etc/systemd/system/my-node-app.service' do
  source 'systemd.service.erb'
  owner 'root'
  mode '0644'
  notifies :run, 'execute[systemctl-daemon-reload]', :immediately
  notifies :restart, 'service[my-node-app]'
end

execute 'systemctl-daemon-reload' do
  command 'systemctl daemon-reload'
  action :nothing
end

service 'my-node-app' do
  supports status: true, restart: true
  action [:enable, :start]
end
```

## IaC Best Practices

### Version Control & Code Review

```
infrastructure/
├── cloudformation/
│   ├── templates/
│   │   ├── vpc.yaml
│   │   ├── ecs-service.yaml
│   │   ├── data-stores.yaml
│   │   └── cloudfront.yaml
│   ├── parameters/
│   │   ├── dev.json
│   │   ├── staging.json
│   │   └── prod.json
│   └── policies/
│       └── stack-policy.json
├── cdk/
│   ├── lib/
│   │   ├── vpc-stack.ts
│   │   ├── ecs-stack.ts
│   │   └── database-stack.ts
│   ├── bin/app.ts
│   └── test/
├── ansible/
│   ├── playbooks/
│   ├── roles/
│   ├── inventory/
│   └── ansible.cfg
├── scripts/
│   ├── deploy.sh
│   ├── rollback.sh
│   └── validate.sh
└── docs/
    ├── architecture.md
    └── runbooks/
```

### IaC Security

```yaml
# .github/workflows/iac-security.yml
name: IaC Security Scan
on:
  pull_request:
    paths:
      - 'cloudformation/**'
      - 'ansible/**'
      - 'cdk/**'

jobs:
  cfn-guard:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: CloudFormation Guard
        uses: aws-cloudformation/cloudformation-guard@v2
        with:
          rules: .cfn-guard/rules.guard
          data: cloudformation/templates/

  cfn-nag:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: CFN Nag scan
        uses: stelligent/cfn_nag@master
        with:
          input_path: cloudformation/templates/

  ansible-lint:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-python@v5
        with:
          python-version: '3.11'
      - run: pip install ansible-lint
      - run: ansible-lint ansible/
```

## Cross-References

- See [Terraform](./01-terraform.md) for Terraform-specific IaC
- See [Kubernetes Manifests](./03-kubernetes-manifests-testing.md) for K8s and Helm
- See [CI/CD Pipelines](../05-ci-cd-pipelines/01-github-actions.md) for automation
- See [Security Scanning](../09-deployment-security/01-security-scanning.md) for hardening
- See [Container Orchestration](../03-container-orchestration/01-kubernetes-patterns.md) for K8s patterns
- See [Architecture](../01-deployment-architecture/01-architecture-patterns.md) for design patterns

## Next Steps

Continue to [Kubernetes Manifests & Testing](./03-kubernetes-manifests-testing.md).
