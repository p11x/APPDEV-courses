---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: VPC Networking
Purpose: Hands-on VPC deployment with subnets, routing, NAT Gateway, and security groups
Difficulty: intermediate
Prerequisites: 01_Basic_VPC.md, 02_Advanced_VPC.md
RelatedFiles: 01_Basic_VPC.md, 02_Advanced_VPC.md
UseCase: Production VPC infrastructure deployment
CertificationExam: AWS Solutions Architect Associate
LastUpdated: 2025
---

## 💡 WHY

Practical VPC implementation is essential for production cloud deployments. This hands-on guide provides step-by-step VPC creation with security best practices.

## 📖 WHAT

### Lab Architecture

```
Production VPC Architecture
============================

    Internet Gateway
           │
    ┌──────┴──────┐
    │ Public RT  │
    └──────┬──────┘
           │
    ┌──────┴──────┐
    │ Public Subnet AZ-A (10.0.1.0/24) │
    │ Public Subnet AZ-B (10.0.2.0/24) │
    └─────────────┘
           │
    ┌──────┴──────┐
    │ NAT Gateway │
    └──────┬──────┘
           │
    ┌──────┴──────┐
    │ Private RT │
    └──────┬──────┘
           │
    ┌──────┴──────┐
    │ Private Subnet AZ-A (10.0.10.0/24) │
    │ Private Subnet AZ-B (10.0.20.0/24) │
    └─────────────┘
```

## 🔧 HOW

### Module 1: VPC and Subnet Creation

```bash
#!/bin/bash
# VPC Setup Script

# Variables
VPC_CIDR="10.0.0.0/16"
REGION="us-east-1"

# Create VPC
VPC_ID=$(aws ec2 create-vpc \
    --cidr-block $VPC_CIDR \
    --query 'Vpc.VpcId' \
    --output text)
echo "VPC created: $VPC_ID"

# Enable DNS
aws ec2 modify-vpc-attribute \
    --vpc-id $VPC_ID \
    --enable-dns-hostnames "Value=true"
aws ec2 modify-vpc-attribute \
    --vpc-id $VPC_ID \
    --enable-dns-support "Value=true"

# Create subnets
SUBNET_PUB_A=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.1.0/24 \
    --availability-zone ${REGION}a \
    --query 'Subnet.SubnetId' \
    --output text)

SUBNET_PUB_B=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.2.0/24 \
    --availability-zone ${REGION}b \
    --query 'Subnet.SubnetId' \
    --output text)

SUBNET_PRIV_A=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.10.0/24 \
    --availability-zone ${REGION}a \
    --query 'Subnet.SubnetId' \
    --output text)

SUBNET_PRIV_B=$(aws ec2 create-subnet \
    --vpc-id $VPC_ID \
    --cidr-block 10.0.20.0/24 \
    --availability-zone ${REGION}b \
    --query 'Subnet.SubnetId' \
    --output text)

echo "Subnets created: $SUBNET_PUB_A, $SUBNET_PUB_B, $SUBNET_PRIV_A, $SUBNET_PRIV_B"
```

### Module 2: Internet Gateway and NAT Gateway

```bash
# Create Internet Gateway
IGW_ID=$(aws ec2 create-internet-gateway \
    --query 'InternetGateway.InternetGatewayId' \
    --output text)
aws ec2 attach-internet-gateway \
    --vpc-id $VPC_ID \
    --internet-gateway-id $IGW_ID

# Create Elastic IP for NAT
EIP_ALLOC=$(aws ec2 allocate-address \
    --domain vpc \
    --query 'AllocationId' \
    --output text)

# Create NAT Gateway
NAT_GW=$(aws ec2 create-nat-gateway \
    --subnet-id $SUBNET_PUB_A \
    --allocation-id $EIP_ALLOC \
    --query 'NatGateway.NatGatewayId' \
    --output text)

# Wait for NAT to be available
aws ec2 wait nat-gateway-available \
    --nat-gateway-ids $NAT_GW
echo "NAT Gateway: $NAT_GW"
```

### Module 3: Route Tables

```bash
# Public route table
RT_PUB=$(aws ec2 create-route-table \
    --vpc-id $VPC_ID \
    --query 'RouteTable.RouteTableId' \
    --output text)

aws ec2 create-route \
    --route-table-id $RT_PUB \
    --destination-cidr-block 0.0.0.0/0 \
    --gateway-id $IGW_ID

aws ec2 associate-route-table --route-table-id $RT_PUB --subnet-id $SUBNET_PUB_A
aws ec2 associate-route-table --route-table-id $RT_PUB --subnet-id $SUBNET_PUB_B

# Private route table
RT_PRIV=$(aws ec2 create-route-table \
    --vpc-id $VPC_ID \
    --query 'RouteTable.RouteTableId' \
    --output text)

aws ec2 create-route \
    --route-table-id $RT_PRIV \
    --destination-cidr-block 0.0.0.0/0 \
    --nat-gateway-id $NAT_GW

aws ec2 associate-route-table --route-table-id $RT_PRIV --subnet-id $SUBNET_PRIV_A
aws ec2 associate-route-table --route-table-id $RT_PRIV --subnet-id $SUBNET_PRIV_B

echo "Route tables created"
```

### Module 4: Security Groups

```bash
# Web server security group
SG_WEB=$(aws ec2 create-security-group \
    --group-name web-sg \
    --description "Web servers" \
    --vpc-id $VPC_ID \
    --query 'GroupId' \
    --output text)

aws ec2 authorize-security-group-ingress \
    --group-id $SG_WEB \
    --protocol tcp \
    --port 80 \
    --cidr 0.0.0.0/0

aws ec2 authorize-security-group-ingress \
    --group-id $SG_WEB \
    --protocol tcp \
    --port 443 \
    --cidr 0.0.0.0/0

aws ec2 authorize-security-group-ingress \
    --group-id $SG_WEB \
    --protocol tcp \
    --port 22 \
    --cidr 10.0.0.0/16

# Database security group
SG_DB=$(aws ec2 create-security-group \
    --group-name db-sg \
    --description "Database servers" \
    --vpc-id $VPC_ID \
    --query 'GroupId' \
    --output text)

aws ec2 authorize-security-group-ingress \
    --group-id $SG_DB \
    --protocol tcp \
    --port 3306 \
    --source-group $SG_WEB
```

## ⚠️ COMMON ISSUES

### 1. Subnet Not Associating

**Problem**: Route table association fails.

**Solution**: Ensure VPC exists and subnet IDs are valid.

### 2. NAT Gateway Not Routing

**Problem**: Private instances can't reach internet.

**Solution**: Check route table has 0.0.0.0/0 via NAT Gateway.

### 3. Security Group Not Working

**Problem**: Can't connect to instances.

**Solution**: Verify inbound rules and source addresses.

## 🔗 CROSS-REFERENCES

**Related**: EC2, RDS, Lambda VPC-enabled

**Next**: Add VPC endpoints, VPN connection