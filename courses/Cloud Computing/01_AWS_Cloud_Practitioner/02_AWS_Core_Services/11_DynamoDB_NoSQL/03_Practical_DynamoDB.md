---
Category: AWS Cloud Practitioner
Subcategory: AWS Core Services
Concept: DynamoDB NoSQL
Purpose: Hands-on DynamoDB implementation including table design, operations, and real-time applications
Difficulty: intermediate
Prerequisites: 01_Basic_DynamoDB.md, 02_Advanced_DynamoDB.md
RelatedFiles: 01_Basic_DynamoDB.md, 02_Advanced_DynamoDB.md
UseCase: Production NoSQL database deployment
CertificationExam: AWS Database Specialty
LastUpdated: 2025
---

## 💡 WHY

Hands-on DynamoDB implementation provides practical experience building production NoSQL applications.

## 📖 WHAT

### Lab: E-Commerce Product Catalog

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│  Web App    │───►│ DynamoDB    │───►│  Lambda    │
└─────────────┘    │  Products   │    │  Triggers  │
                    └─────────────┘    └─────────────┘
                         │
                    ┌────┴────┐
                    │ GSI     │
                    └─────────┘
```

## 🔧 HOW

### Module 1: Create Product Table

```bash
#!/bin/bash
# DynamoDB Product Catalog Setup

# Create table with GSI
aws dynamodb create-table \
    --table-name products \
    --attribute-definitions '[
        {"AttributeName": "productId", "AttributeType": "S"},
        {"AttributeName": "category", "AttributeType": "S"},
        {"AttributeName": "price", "AttributeType": "N"}
    ]' \
    --key-schema '[
        {"AttributeName": "productId", "KeyType": "HASH"}
    ]' \
    --global-secondary-indexes '[
        {
            "IndexName": "category-price-index",
            "KeySchema": [
                {"AttributeName": "category", "KeyType": "HASH"},
                {"AttributeName": "price", "KeyType": "RANGE"}
            ],
            "Projection": {"ProjectionType": "ALL"}
        }
    ]' \
    --billing-mode PAY_PER_REQUEST \
    --tags 'Key=Environment,Value=Production'
```

### Module 2: Load Sample Data

```bash
# Add products
aws dynamodb put-item \
    --table-name products \
    --item '{
        "productId": {"S": "PROD-001"},
        "name": {"S": "Wireless Headphones"},
        "category": {"S": "Electronics"},
        "price": {"N": "79.99"},
        "inStock": {"BOOL": true},
        "tags": {"SS": ["audio", "wireless", "bluetooth"]}
    }'

aws dynamodb put-item \
    --table-name products \
    --item '{
        "productId": {"S": "PROD-002"},
        "name": {"S": "Running Shoes"},
        "category": {"S": "Sports"},
        "price": {"N": "129.99"},
        "inStock": {"BOOL": true},
        "tags": {"SS": ["sports", "footwear", "running"]}
    }'
```

### Module 3: Query Operations

```bash
# Query by product ID
aws dynamodb get-item \
    --table-name products \
    --key '{"productId": {"S": "PROD-001"}}'

# Query by category using GSI
aws dynamodb query \
    --table-name products \
    --index-name category-price-index \
    --key-condition-expression "category = :cat" \
    --expression-attribute-values '{
        ":cat": {"S": "Electronics"}
    }' \
    --projection-expression "productId,name,price"

# Scan for price range
aws dynamodb scan \
    --table-name products \
    --filter-expression "price BETWEEN :min AND :max" \
    --expression-attribute-values '{
        ":min": {"N": "50"},
        ":max": {"N": "150"}
    }'
```

## VERIFICATION

```bash
# Describe table
aws dynamodb describe-table --table-name products

# Get item count
aws dynamodb describe-table \
    --table-name products \
    --select COUNT

# Check GSI status
aws dynamodb describe-table --table-name products \
    --query 'Table.GlobalSecondaryIndexes'
```

## CLEANUP

```bash
# Delete table
aws dynamodb delete-table --table-name products
```

## 🔗 CROSS-REFERENCES

**Related**: Lambda, S3, CloudWatch