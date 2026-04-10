---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: EC2 Compute
Purpose: Advanced EC2 concepts including placement groups, HPC, and performance optimization
Difficulty: advanced
Prerequisites: 01_Basic_EC2.md
RelatedFiles: 01_Basic_EC2.md, 03_Practical_EC2.md
UseCase: High-performance and specialized compute workloads
CertificationExam: AWS Solutions Architect
LastUpdated: 2025
---

## WHY

Advanced EC2 concepts enable high-performance computing, GPU workloads, and specialized architectures.

## WHAT

### Placement Groups

**Cluster**: Low latency, same rack
**Partition**: Distributed across partitions
**Spread**: Across distinct hardware

### High Performance Computing

- GPU instances (P4, G5)
- Inf1 (Machine Learning)
- HPC (High Performance Computing)

### Enhanced Networking

ENA, EFA for high throughput

## HOW

### Example 1: Cluster Placement Group

```bash
# Create placement group
aws ec2 create-placement-group \
    --group-name hpc-cluster \
    --strategy cluster

# Launch instance in placement group
aws ec2 run-instances \
    --image-id ami-0c55b159cbfafe1f0 \
    --instance-type c5n.18xlarge \
    --placement '{"GroupName": "hpc-cluster"}'
```

### Example 2: EFA-Enabled Instance

```bash
# Create security group for EFA
aws ec2 create-security-group \
    --group-name efa-sg \
    --description "EFA-enabled security group"

# Launch with EFA
aws ec2 run-instances \
    --image-id ami-0c55b159cbfafe1f0 \
    --instance-type c5n.18xlarge \
    --network-interfaces '[{
        "DeviceIndex": 0,
        "InterfaceType": "efa",
        "Groups": ["sg-123456789"]
    }]'
```

## ADVANCED FEATURES

### HPC on AWS

| Feature | Instance Type | Use Case |
|---------|--------------|----------|
| GPU | p4d, p5 | Deep learning |
| Inferentia | inf1 | ML inference |
| FPGA | f1 | Custom acceleration |

## CROSS-REFERENCES

### Related Services

- AWS Batch: Batch computing
- ParallelCluster: HPC clusters
- SageMaker: ML platform