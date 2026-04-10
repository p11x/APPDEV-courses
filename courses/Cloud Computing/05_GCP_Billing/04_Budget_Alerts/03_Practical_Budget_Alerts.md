---
Category: Google Cloud Platform
Subcategory: GCP Billing
Concept: Budget Alerts
Purpose: Hands-on exercises for GCP Budget Alerts configuration
Difficulty: advanced
Prerequisites: 01_Basic_Budget_Alerts.md, 02_Advanced_Budget_Alerts.md
RelatedFiles: 01_Basic_Budget_Alerts.md, 02_Advanced_Budget_Alerts.md
UseCase: Enterprise budget management, cost control implementation
CertificationExam: GCP Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

Hands-on experience with Budget Alerts is essential for implementing cost control and preventing unexpected GCP spending.

### Lab Goals

- Create budgets with filters
- Configure programmatic alerts
- Implement cost controls

## 📖 WHAT

### Exercise Overview

1. **Basic Budget**: Project-level budget
2. **Filtered Budget**: Service/project filters
3. **Automated Response**: Cloud Functions

## 🔧 HOW

### Exercise 1: Create Project Budgets

```bash
#!/bin/bash
# Create GCP Budgets

BILLING_ACCOUNT="XXXXXX-XXXXXX-XXXXXX"

gcloud config set project my-project

# Link project to billing
gcloud billing projects link my-project \
    --billing-account=$BILLING_ACCOUNT

# Create basic budget
gcloud billing budgets create basic-budget \
    --billing-account=$BILLING_ACCOUNT \
    --display-name="Monthly Budget" \
    --amount=5000USD \
    --threshold-rules="threshold=50, spendBasis=CURRENT_SPEND" \
    --threshold-rules="threshold=80, spendBasis=CURRENT_SPEND" \
    --threshold-rules="threshold=100, spendBasis=FORECASTED_SPEND"

# Create forecasted budget
gcloud billing budgets create forecast-budget \
    --billing-account=$BILLING_ACCOUNT \
    --display-name="Forecast Budget" \
    --amount=10000USD \
    --threshold-rules="threshold=80, spendBasis=FORECASTED_SPEND" \
    --threshold-rules="threshold=100, spendBasis=FORECASTED_SPEND"

# List budgets
gcloud billing budgets list --billing-account=$BILLING_ACCOUNT

# Describe specific budget
gcloud billing budgets describe basic-budget \
    --billing-account=$BILLING_ACCOUNT

echo "Budgets created successfully!"
```

### Exercise 2: Configure Filtered Budgets

```bash
#!/bin/bash
# Create filtered budgets

BILLING_ACCOUNT="XXXXXX-XXXXXX-XXXXXX"

# Budget for Compute Engine only
gcloud billing budgets create compute-budget \
    --billing-account=$BILLING_ACCOUNT \
    --display-name="Compute Budget" \
    --amount=3000USD \
    --filter-services="services/compute.googleapis.com" \
    --threshold-rules="threshold=80"

# Budget for specific project
gcloud billing budgets create dev-budget \
    --billing-account=$BILLING_ACCOUNT \
    --display-name="Dev Project Budget" \
    --amount=1000USD \
    --filter-projects="projects/123456789" \
    --threshold-rules="threshold=50" \
    --threshold-rules="threshold=80"

# Budget for labeled resources
gcloud billing budgets create team-budget \
    --billing-account=$BILLING_ACCOUNT \
    --display-name="Platform Team Budget" \
    --amount=5000USD \
    --filter-labels="labels.team=platform" \
    --threshold-rules="threshold=80"

# Budget excluding credits
gcloud billing budgets create net-spend-budget \
    --billing-account=$BILLING_ACCOUNT \
    --display-name="Net Spend Budget" \
    --amount=8000USD \
    --amountIncludesCredits=false \
    --threshold-rules="threshold=80"

# List all budgets with filters
gcloud billing budgets list --billing-account=$BILLING_ACCOUNT

echo "Filtered budgets configured!"
```

### Exercise 3: Programmatic Alerts

```bash
#!/bin/bash
# Configure programmatic budget alerts

PROJECT_ID="my-project-id"
BILLING_ACCOUNT="XXXXXX-XXXXXX-XXXXXX"

gcloud config set project $PROJECT_ID

# Enable Cloud Billing API
gcloud services enable cloudbilling.googleapis.com

# Create Pub/Sub topic
gcloud pubsub topics create budget-alerts

# Create budget with Pub/Sub notification
gcloud billing budgets create programmatic-budget \
    --billing-account=$BILLING_ACCOUNT \
    --display-name="Programmatic Budget" \
    --amount=5000USD \
    --threshold-rules="threshold=80" \
    --notification-methods="pubsubTopic=projects/$PROJECT_ID/topics/budget-alerts" \
    --notification-methods="schema=budget_notification_schema.json"

# Create Cloud Function to handle alerts
cat > index.js << 'EOF'
const {PubSub} = require('@google-cloud/pubsub');

exports.handleBudgetAlert = async (event, context) => {
  const message = Buffer.from(event.data, 'base64').toString();
  const budget = JSON.parse(message);
  
  console.log(`Budget Alert: ${budget.budgetDisplayName}`);
  console.log(`Cost: ${budget.costAmount}`);
  console.log(`Budget: ${budget.budgetAmount}`);
  console.log(`Alert Threshold: ${budget.alertThresholdBasis}`);
  
  // Implement custom logic (e.g., disable resources, notify team)
  if (budget.costAmount >= budget.budgetAmount * 0.9) {
    console.log('WARNING: Budget nearly exceeded!');
  }
};
EOF

cat > package.json << 'EOF'
{
  "name": "budget-alert-handler",
  "version": "1.0.0",
  "dependencies": {"@google-cloud/pubsub": "^4.0.0"}
}
EOF

gcloud functions deploy budgetAlertHandler \
    --runtime=nodejs18 \
    --region=us-central1 \
    --source=. \
    --entry-point=handleBudgetAlert \
    --trigger-topic=budget-alerts

# Test budget alert
gcloud pubsub topics publish budget-alerts \
    --message='{"budgetDisplayName":"Programmatic Budget","costAmount":4000,"budgetAmount":5000,"alertThresholdBasis":0.8}'

echo "Programmatic alerts configured!"
```

## ⚠️ COMMON ISSUES

### Troubleshooting

| Issue | Solution |
|-------|----------|
| No alerts | Check Pub/Sub, email |
| Wrong amounts | Verify filter scope |
| Too many alerts | Adjust thresholds |

### Validation

```bash
# List budgets
gcloud billing budgets list --billing-account=$BILLING_ACCOUNT

# Check Pub/Sub messages
gcloud pubsub subscriptions pull budget-sub
```

## 🌐 COMPATIBILITY

### Integration

- Cloud Monitoring
- Cloud Functions
- Pub/Sub

## 🔗 CROSS-REFERENCES

### Related Labs

- Cost Optimization
- Cloud Monitoring
- Cloud Functions

### Next Steps

- Set up Slack notifications
- Configure automated responses
- Implement cost controls

## ✅ EXAM TIPS

- Practice budget creation
- Use multiple thresholds
- Configure programmatic alerts
- Monitor forecasted spend
