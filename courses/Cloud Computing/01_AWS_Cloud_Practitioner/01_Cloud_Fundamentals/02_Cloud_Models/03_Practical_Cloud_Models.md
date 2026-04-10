---
Category: AWS Cloud Practitioner
Subcategory: Cloud Fundamentals
Concept: Cloud Models
Purpose: Cloud deployment model deployment and migration patterns
Difficulty: intermediate
Prerequisites: 01_Basic_Cloud_Models.md
RelatedFiles: 01_Basic_Cloud_Models.md, 02_Advanced_Cloud_Models.md
UseCase: Cloud migration planning and execution
CertificationExam: AWS Solutions Architect
LastUpdated: 2025
---

## WHY

Practical deployment models enable real-world cloud migrations with proven strategies.

## WHAT

### Migration Phases

1. Assess - Discover workloads
2. Mobilize - Prepare environment
3. Migrate - Move workloads
4. Optimize - Improve efficiency

## HOW

### Migration Implementation

```bash
# Use Migration Hub for tracking
aws mgh create-replication-template \
    --source-server-id server-123

# Database Migration Service
aws dms create-replication-task \
    --replication-task-id task-123 \
    --source-endpoint-arn source-arn \
    --target-endpoint-arn target-arn
```

## CROSS-REFERENCES

### Prerequisites

- Basic cloud models understanding