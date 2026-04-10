---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: VPC Networking
Purpose: Advanced VPC architecture including VPC endpoints, PrivateLink, and Transit Gateway
Difficulty: advanced
Prerequisites: 01_Basic_VPC.md
RelatedFiles: 01_Basic_VPC.md, 03_Practical_VPC.md
UseCase: Enterprise network architecture and hybrid connectivity
CertificationExam: AWS Solutions Architect Professional
LastUpdated: 2025
---

## WHY

Advanced VPC features enable enterprise architectures with secure private connectivity.

## WHAT

### VPC Endpoints

Access AWS services without internet traversal - via Gateway or Interface endpoints.

### AWS PrivateLink

Private connectivity to AWS services and third-party services via VPC endpoints.

### Transit Gateway

Central hub for connecting VPCs and on-premises networks.

## HOW

### Example: VPC Endpoints

```bash
# Create S3 Gateway endpoint
aws ec2 create-vpc-endpoint \
    --vpc-id vpc-123 \
    --vpc-endpoint-type Gateway \
    --service-name com.amazonaws.us-east-1.s3 \
    --route-table-ids rtb-123

# Create Interface endpoint (DynamoDB, Secrets Manager, etc.)
aws ec2 create-vpc-endpoint \
    --vpc-id vpc-123 \
    --vpc-endpoint-type Interface \
    --service-name com.amazonaws.us-east-1.secretsmanager \
    --subnet-ids subnet-a
```

### Example: Transit Gateway

```bash
# Create Transit Gateway
aws ec2 create-transit-gateway \
    --description "Central network hub" \
    --amazon-asn 64512

# Attach VPC
aws ec2 attach-transit-gateway-vpc-attachments \
    --transit-gateway-attachment-id tgw-attach-123 \
    --vpc-id vpc-123 \
    --subnet-ids subnet-a subnet-b
```

## CROSS-REFERENCES

### Related Services

- Direct Connect: Hybrid connectivity
- Transit Gateway: Multi-VPC