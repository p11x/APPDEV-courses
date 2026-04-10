---
Category: Google Cloud Platform
Subcategory: GCP Billing
Concept: Cost Optimization
Purpose: Hands-on exercises for GCP Cost Optimization implementation
Difficulty: advanced
Prerequisites: 01_Basic_Cost_Optimization.md, 02_Advanced_Cost_Optimization.md
RelatedFiles: 01_Basic_Cost_Optimization.md, 02_Advanced_Cost_Optimization.md
UseCase: Enterprise cost optimization, automated savings implementation
CertificationExam: GCP Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Cost Optimization is essential for implementing enterprise cost management and reducing GCP spending.

### Lab Goals

- Implement cost optimization
- Use Recommender
- Configure scheduled scaling

## 📖 WHAT

### Exercise Overview

1. **Cost Analysis**: Identify optimization opportunities
2. **Optimization**: Implement recommendations
3. **Automation**: Scheduled scaling

## 🔧 HOW

### Exercise 1: Analyze and Identify Optimization

```bash
#!/bin/bash
# Analyze costs and identify optimization

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Export billing to BigQuery (if not enabled)
bq mk --dataset ${PROJECT_ID}:billing

# Query cost by service
bq query --use_legacy_sql=false \
    "SELECT
        service.description as service,
        ROUND(SUM(cost), 2) as total_cost,
        ROUND(SUM(cost) / (SELECT SUM(cost) FROM \`$PROJECT_ID.billing.*\`) * 100, 1) as percentage
     FROM \`$PROJECT_ID.billing.*\`
     WHERE usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
     GROUP BY service.description
     ORDER BY total_cost DESC"

# Query idle resources
bq query --use_legacy_sql=false \
    "SELECT
        resource.name as resource,
        resource.type as type,
        ROUND(SUM(cost), 2) as idle_cost
     FROM \`$PROJECT_ID.billing.*\`
     WHERE usage_start_time >= DATE_SUB(CURRENT_DATE(), INTERVAL 30 DAY)
       AND usage.amount < 0.1
     GROUP BY resource.name, resource.type
     ORDER BY idle_cost DESC"

# Get Recommender insights
gcloud recommender insights list \
    --location=global \
    --insight-type=google.compute.insight.IdleResourceInsight

echo "Cost analysis complete!"
```

### Exercise 2: Implement Recommendations

```bash
#!/bin/bash
# Implement cost optimization recommendations

PROJECT_ID="my-project-id"

gcloud config set project $PROJECT_ID

# Get idle VM recommendations
gcloud recommender recommendations list \
    --recommender=google.compute.idleVmRecommender \
    --location=us-central1

# Apply idle VM recommendation (delete)
# gcloud compute instances delete idle-instance --zone=us-central1-a

# Get machine type recommendations
gcloud beta recommender recommendations list \
    --recommender=google.compute.machineTypeRightsizingRecommender \
    --location=us-central1

# Update instance to recommended type
# gcloud compute instances set-machine-type INSTANCE --machine-type=n2-standard-4

# Get snapshot recommendations
gcloud recommender recommendations list \
    --recommender=google.compute.snapshotUnusedRecommender \
    --location=global

# Delete old snapshots
# gcloud compute snapshots delete old-snapshot

# Get idle IP recommendations
gcloud recommender recommendations list \
    --recommender=google.compute.idleIpRecommender \
    --location=global

# Delete unused static IPs
# gcloud compute addresses delete unused-ip --region=us-central1

# Get idle load balancer recommendations
gcloud recommender recommendations list \
    --recommender=google.compute.idleLoadBalancerRecommender \
    --location=global

echo "Recommendations implemented!"
```

### Exercise 3: Implement Scheduled Scaling

```bash
#!/bin/bash
# Implement scheduled scaling for cost optimization

PROJECT_ID="my-project-id"
REGION="us-central1"
MIG_NAME="production-mig"

gcloud config set project $PROJECT_ID

# Create instance template
gcloud compute instance-templates create schedule-template \
    --machine-type=e2-medium \
    --image-family=debian-11 \
    --image-project=debian-cloud \
    --metadata=startup-script='#!/bin/bash echo "App running" > /var/www/html/index.html'

# Create regional MIG
gcloud compute instance-groups managed create $MIG_NAME \
    --template=schedule-template \
    --size=3 \
    --region=$REGION \
    --target-distribution-shape=balanced

# Scale up during business hours
gcloud scheduler jobs create http scale-up-job \
    --schedule="0 8 * * 1-5" \
    --uri="https://compute.googleapis.com/compute/v1/projects/$PROJECT_ID/regions/$REGION/instanceGroups/$MIG_NAME/resize" \
    --method=POST \
    --headers="Content-Type:application/json" \
    --body="{\"size\": 10}" \
    --location=$REGION

# Scale down after business hours
gcloud scheduler jobs create http scale-down-job \
    --schedule="0 19 * * 1-5" \
    --uri="https://compute.googleapis.com/compute/v1/projects/$PROJECT_ID/regions/$REGION/instanceGroups/$MIG_NAME/resize" \
    --method=POST \
    --headers="Content-Type=application/json" \
    --body="{\"size\": 2}" \
    --location=$REGION

# Weekend scaling
gcloud scheduler jobs create http weekend-scale-job \
    --schedule="0 0 * * 0,6" \
    --uri="https://compute.googleapis.com/compute/v1/projects/$PROJECT_ID/regions/$REGION/instanceGroups/$MIG_NAME/resize" \
    --method=POST \
    --headers="Content-Type=application/json" \
    --body="{\"size\": 1}" \
    --location=$REGION

# Verify schedules
gcloud scheduler jobs list --location=$REGION

# Calculate savings
echo "Estimated savings:"
echo "- Business hours (10 hrs x 8 instances): 80 instance-hours saved"
echo "- Weekend (24 hrs x 2 instances): 48 instance-hours saved"
echo "- Total weekly savings: ~128 instance-hours"

echo "Scheduled scaling configured!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| Recommendations not working | Check permissions |
| Scheduling issues | Verify cron syntax |
| Quota issues | Request quota increase |

### Validation

```bash
# Check recommendations
gcloud recommender recommendations list --location=global

# Verify scheduled jobs
gcloud scheduler jobs list --location=us-central1
```

## 🌐 COMPATIBILITY

### Integration

- Cloud Monitoring
- Cloud Scheduler
- Recommender

## 🔗 CROSS-REFERENCES

### Related Labs

- Sustained Use
- Committed Use
- Preemptible

### Next Steps

- Implement labels
- Set up budgets
- Configure alerts

## ✅ EXAM TIPS

- Use Recommender API
- Implement scheduled scaling
- Use labels for tracking
- Monitor quotas
