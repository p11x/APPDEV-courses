---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: VPC Networking
Purpose: Understanding Amazon VPC for isolated cloud networking and network architecture
Difficulty: beginner
Prerequisites: 01_Basic_Cloud_Concepts.md
RelatedFiles: 02_Advanced_VPC.md, 03_Practical_VPC.md
UseCase: Creating isolated network environments in AWS
CertificationExam: AWS Certified Cloud Practitioner - Domain 2: Security
LastUpdated: 2025
---

## WHY

Amazon Virtual Private Cloud (VPC) is the fundamental networking layer in AWS, providing isolated network environments where you can launch AWS resources. Understanding VPC is crucial for security, compliance, and proper application architecture.

### Why VPC Matters

- **Security**: Isolates resources from internet by default
- **Compliance**: Meet network segmentation requirements
- **Control**: Configure routing, IP addressing, access
- **Connectivity**: Connect to on-premises, other VPCs, internet
- **Layer for All Services**: VPC underpins EC2, RDS, Lambda, and more

### Industry Statistics

- Every AWS resource runs inside a VPC
- 85%+ of security issues involve VPC misconfiguration
- Proper VPC design reduces attack surface 90%+

### When NOT to Use VPC

- Using fully-managed services without VPC (S3, DynamoDB)
- Simple static website hosting

## WHAT

### VPC Core Concepts

**VPC**: Isolated virtual network in AWS cloud. Each VPC is logically isolated from other VPCs and the internet unless explicitly connected.

**Subnet**: Subdivision of VPC IP range. Types:

- **Public Subnet**: Has direct route to internet via Internet Gateway
- **Private Subnet**: No direct route to internet, accesses internet via NAT Gateway or PrivateLink
- **VPN-only Subnet**: Only accessible via VPN

**Route Table**: Rules for directing network traffic within VPC.

**Internet Gateway (IGW)**: Enables VPC to connect to internet.

**NAT Gateway**: Allows private subnet instances to access internet (outbound only) while blocking inbound.

### Architecture Diagram

```
                    VPC NETWORKING ARCHITECTURE
                    ======================

    ┌─────────────────────────────────────────────────────────────┐
    │                    INTERNET                        │
    └─────────────────────────────────────────────────────────────┘
                           │
                           │
    ┌─────────────────────┼───────────────────────────┐
    │                   │                           │
    ▼                   ▼                           │
┌─────────┐      ┌──────────┐                  │
│  IGW   │      │  NAT GW  │                  │
└────┬────┘      └────┬─────┘                  │
     │                 │                          │
     │    ┌───────────┼───────────┐            │
     │    │           │           │            │
     ▼    ▼           ▼           ▼            │
│ Public │    │ Private 1 │ Private 2 │            │
│Subnet │    │  (AZ-a)  │  (AZ-b)  │            │
│ ┌───┐ │    │ ┌───┐   │ ┌───┐   │            │
│ │Web│ │    │ │App │   │ │App │   │            │
│ │Ser│ │    │ │Ser│   │ │Ser│   │            │
│ └──┘ │    │ └──┘   │ └──┘   │            │
└───────┘    └───────────────────────────┘            │
      │                                          │
      └──────────────────────────────────────────┘
                   │
              ┌──────┴──────┐
              │             │
              │  RDS DB    │
              │ (Multi-AZ) │
              │            │
              └───────────┘
```

### Key Components Matrix

| Component | Function | Traffic Direction |
|-----------|----------|----------------|
| IGW | Internet access | Outbound + Inbound |
| NAT Gateway | Outbound only | Outbound |
| VPC Endpoints | AWS service access | Outbound (AWS) |
| Security Groups | Instance firewall | Inbound + Outbound |
| NACLs | Subnet firewall | Stateless |
| Transit Gateway | VPC-to-VPC | Any |

## HOW

### Example 1: Create VPC with Public and Private Subnets

```bash
# Create VPC
VPC_ID=$(aws ec2 create-vpc \
    --cidr-block 10.0.0.0/16 \
    --query 'Vpc.VpcId' \
    --output text)
echo "VPC: $VPC_ID"

# Enable DNS hostnames
aws ec2 modify-vpc-attribute \
    --vpc-id $VPC_ID \
    --enable-dns-hostnames '{"Value": true}'

# Create public subnet in AZ-a
SUBNET_PUB_A=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.1.0/24 \
    --availability-zone us-east-1a \
    --query 'Subnet.SubnetId' \
    --output text)

# Create public subnet in AZ-b
SUBNET_PUB_B=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.2.0/24 \
    --availability-zone us-east-1b \
    --query 'Subnet.SubnetId' \
    --output text)

# Create private subnet in AZ-a
SUBNET_PRIV_A=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.10.0/24 \
    --availability-zone us-east-1a \
    --query 'Subnet.SubnetId' \
    --output text)

# Create private subnet in AZ-b
SUBNET_PRIV_B=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.20.0/24 \
    --availability-zone us-east-1b \
    --query 'Subnet.SubnetId' \
    --output text)

echo "Public Subnets: $SUBNET_PUB_A, $SUBNET_PUB_B"
echo "Private Subnets: $SUBNET_PRIV_A, $SUBNET_PRIV_B"
```

### Example 2: Configure Internet Access

```bash
# Create and attach Internet Gateway
IGW_ID=$(aws ec2 create-internet-gateway \
    --query 'InternetGateway.InternetGatewayId' \
    --output text)

aws ec2 attach-internet-gateway \
    --vpc-id $VPC_ID \
    --internet-gateway-id $IGW_ID

# Create public route table
RT_PUB=$(aws ec2 create-route-table \
    --vpc-id $VPC_ID \
    --query 'RouteTable.RouteTableId' \
    --output text)

# Add route to internet
aws ec2 create-route \
    --route-table-id $RT_PUB \
    --destination-cidr-block 0.0.0.0/0 \
    --gateway-id $IGW_ID

# Associate with public subnets
aws ec2 associate-route-table \
    --route-table-id $RT_PUB \
    --subnet-id $SUBNET_PUB_A

aws ec2 associate-route-table \
    --route-table-id $RT_PUB \
    --subnet-id $SUBNET_PUB_B

echo "Internet Gateway: $IGW_ID"
echo "Public Route Table: $RT_PUB"
```

### Example 3: Configure NAT Gateway

```bash
# Allocate Elastic IP for NAT Gateway
EIP_ALLOC=$(aws ec2 allocate-address \
    --domain vpc \
    --query 'AllocationId' \
    --output text)

# Create NAT Gateway in public subnet
NAT_GW=$(aws ec2 create-nat-gateway \
    --subnet-id $SUBNET_PUB_A \
    --allocation-id $EIP_ALLOC \
    --query 'NatGateway.NatGatewayId' \
    --output text)

# Wait for NAT Gateway to become available
aws ec2 wait nat-gateway-available \
    --nat-gateway-ids $NAT_GW

# Create private route table
RT_PRIV=$(aws ec2 create-route-table \
    --vpc-id $VPC_ID \
    --query 'RouteTable.RouteTableId' \
    --output text)

# Add route through NAT Gateway
aws ec2 create-route \
    --route-table-id $RT_PRIV \
    --destination-cidr-block 0.0.0.0/0 \
    --nat-gateway-id $NAT_GW

# Associate with private subnets
aws ec2 associate-route-table \
    --route-table-id $RT_PRIV \
    --subnet-id $SUBNET_PRIV_A

aws ec2 associate-route-table \
    --route-table-id $RT_PRIV \
    --subnet-id $SUBNET_PRIV_B

# Tag for identification
aws ec2 create-tags \
    --resources $RT_PUB \
    --tags Key=Name,Value=Public-Route-Table

echo "NAT Gateway: $NAT_GW"
```

### Example 4: Security Groups

```bash
# Create security group for web servers
SG_WEB=$(aws ec2 create-security-group \
    --group-name web-sg \
    --description "Web server security group" \
    --vpc-id $VPC_ID \
    --query 'GroupId' \
    --output text)

# Allow HTTP (80)
aws ec2 authorize-security-group-ingress \
    --group-id $SG_WEB \
    --protocol tcp \
    --port 80 \
    --cidr 0.0.0.0/0

# Allow HTTPS (443)
aws ec2 authorize-security-group-ingress \
    --group-id $SG_WEB \
    --protocol tcp \
    --port 443 \
    --cidr 0.0.0.0/0

# Allow SSH from specific IP
aws ec2 authorize-security-group-ingress \
    --group-id $SG_WEB \
    --protocol tcp \
    --port 22 \
    --cidr 203.0.113.0/32

# Allow all outbound
aws ec2 authorize-security-group-egress \
    --group-id $SG_WEB \
    --protocol -1 \
    --cidr 0.0.0.0/0

echo "Web Security Group: $SG_WEB"
```

## COMMON ISSUES

### 1. Cannot Connect to Instance

**Problem**: Connection timeout or refused.

**Solutions**:
- Check security group rules
- Check subnet routing
- Check instance has public IP
- Check NACL rules

### 2. Instances Cannot Reach Internet

**Problem**: Private instances have no outbound access.

**Solution**:
- Configure NAT Gateway
- Check route table has default route
- Verify security group allows outbound

### 3. Cross-VPC Communication Fails

**Problem**: Cannot communicate between VPCs.

**Solution**:
- Use Transit Gateway or VPC Peering
- Ensure non-overlapping CIDRs
- Configure security groups to reference peer

### 4. DNS Resolution Not Working

**Problem**: Cannot resolve internal hostnames.

**Solution**:
```bash
# Enable DNS support
aws ec2 modify-vpc-attribute \
    --vpc-id $VPC_ID \
    --enable-dns-support '{"Value": true}'

# Enable DNS hostnames (for public IPs)
aws ec2 modify-vpc-attribute \
    --vpc-id $VPC_ID \
    --enable-dns-hostnames '{"Value": true}'
```

## PERFORMANCE

### VPC Limits

| Resource | Default Limit |
|----------|--------------|
| VPCs per region | 5 |
| Subnets per VPC | 200 |
| Elastic IPs | 5 |
| Security Groups per VPC | 500 |
| Rules per SG | 60 (ingress), 60 (egress) |
| Route tables per VPC | 200 |
| Peering connections | 50 |

### Performance Considerations

- VPC peering: ~1-2ms latency
- Transit Gateway: ~1ms
- NAT Gateway: Adds ~2-5ms latency
- Direct Connect: <1ms to on-premises

## COMPATIBILITY

### Region Availability

- All commercial Regions
- Some resources limited

### Services Using VPC

- EC2, RDS, EKS, ECS
- Lambda (VPC-enabled)
- Redshift, ElastiCache

## CROSS-REFERENCES

### Related Services

- EC2: Runs in VPC
- RDS: Runs in VPC
- Direct Connect: Connects to VPC

### Prerequisites

- Cloud Concepts basics

### What to Study Next

1. Advanced VPC: Peering, Transit Gateway
2. Security Groups Deep Dive
3. Direct Connect