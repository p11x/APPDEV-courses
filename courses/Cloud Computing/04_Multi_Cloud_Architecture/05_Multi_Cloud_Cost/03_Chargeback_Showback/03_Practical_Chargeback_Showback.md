---
Category: Multi-Cloud Architecture
Subcategory: Multi-Cloud Cost
Concept: Chargeback and Showback - Practical
Purpose: Real-world implementation patterns, migration strategies, and operational best practices
Difficulty: intermediate
Prerequisites: [01_Basic_Chargeback_Showback.md]
RelatedFiles: [01_Basic_Chargeback_Showback.md], [02_Advanced_Chargeback_Showback.md]
UseCase: Cost allocation, IT billing
CertificationExam: Multi-Cloud Pro
LastUpdated: 2025
---

## 💡 WHY

Practical chargeback and showback implementation addresses real organizational challenges including starting from spreadsheets, migrating from legacy systems, and proving ROI. Organizations often struggle with getting buy-in from finance teams and establishing trust in the data. This practical guidance helps security and IT teams build credible cost allocation systems that finance will trust and use for decision-making.

# WHAT

Practical scenarios:

1. **Starting from Zero**: Building cost allocation from scratch
2. **Finance Integration**: Working with existing finance processes
3. **Monthly Reporting**: Automating monthly reports
4. **Cloud Migration Tracking**: Tracking costs during cloud migration
5. **Business Case Development**: Building business cases with cost data
6. **Showback to Chargeback**: Moving from showback to chargeback
7. **Multi-Cloud Migration**: Implementing cost allocation during migration
8. **Executive Dashboards**: Creating leadership-visible reports

# HOW

## Scenario 1: Build Cost Allocation from Zero

```bash
# Step 1: Audit existing cloud spending
# GCP
gcloud beta billing link list
gcloud beta billing get-project-info

# AWS  
aws organizations list-accounts
aws ce get-cost-and-usage --time-period Start=2025-01-01,End=2025-01-31 --granularity=MONTHLY --metrics UnblendedCost

# Azure
az account list --query "[].{name:name, id:id}"

# Step 2: Define cost center taxonomy
# Create Google Sheet with mapping
# Team | Cost Center | Cloud Project Mapping | Account Mapping

# Step 3: Apply labels to all resources
# Use Terraform for consistent labeling
resource "google_compute_instance" "app" {
  name         = "app-server"
  machine_type = "e2-medium"
  zone        = "us-central1-a"
  
  labels = {
    team        = "engineering"
    cost-center = "ENG001"
    environment = "prod"
  }
}
```

## Scenario 2: Monthly Showback Report Automation

```python
from google.cloud import bigquery
import pandas as pd

def generate_monthly_showback():
    # Query GCP costs from BigQuery billing export
    query = """
    SELECT 
        labels.value as team,
        cost,
        usage_amount,
        service.id as service
    FROM `myproject.billing_export.gcp_billing_export_v1_XXXX`
    WHERE usage_start_time >= '2025-01-01' 
      AND usage_end_time < '2025-02-01'
    """
    
    # Execute query
    client = bigquery.Client()
    df = client.query(query).to_dataframe()
    
    # Group by team
    team_costs = df.groupby('team')['cost'].sum()
    
    # Generate report
    report = f"""
    Monthly Cloud Cost Showback Report
    =================================
    Period: January 2025
    
    Team Costs:
    """
    for team, cost in team_costs.items():
        report += f"\n{team}: ${cost:,.2f}"
    
    report += f"\n\nTotal: ${team_costs.sum():,.2f}"
    
    return report
```

## Scenario 3: Cross-Cloud Dashboard with CloudHealth

```yaml
# CloudHealth Perspective Configuration
name: "Engineering Team Dashboard"
type: "cost"
group_by:
  - "service"
  - "usage_type"
filters:
  - "tags.Team": "Engineering"
time:
  preset: "current_month"
compare:
  - "previous_month"
  - "year_over_year"
visualization:
  - "chart/trend"
  - "table/top_services"
alerts:
  - name: "Budget Threshold"
    threshold: 10000
    action: "email"
    recipient: "engineering-lead@company.com"
```

## Scenario 4: Terraform Guardrails for Tag Enforcement

```bash
# Terraform Sentinel policy for tags
# policy.sentinel
import "tfplan/v2" as tfplan

param required_tags default = ["Team", "CostCenter", "Environment"]

main = rule {
    all tfplan.resource_changes as _, rc {
        all required_tags as tag {
            rc.change.after.keys contains tag
        }
    }
}

# GCP Organization Policy
gcloud resource-manager org-policies delete compute.require-shared-uxrtingLabels \
    --organization=ORG_ID

# Enforce at organization level
gcloud resource-manager org-policies set-uxrting \
    --organization=ORG_ID \
    --policy-file=policy.yaml
```

# COMMON ISSUES

1. **Trust Issues**: Finance teams skeptical of cloud cost data

2. **Manual Processes**: Monthly manual reports take too long

3. **Tag Drift**: New resources created without tags

4. **Incomplete Data**: Historical costs lack tags

5. **Currency Handling**: Multi-currency reconciliation

# PERFORMANCE

Production considerations:
- Use scheduled reports for recurring costs
- Pre-compute frequently accessed aggregations
- Use data warehousing for complex analysis

# COMPATIBILITY

Production tools:
- Looker for visualization
- CloudHealth/Zero for multi-cloud
- Excel/Sheets for manual reports
- Slack/Teams for notification delivery
- ERP integration via APIs

# CROSS-REFERENCES

- **01_Basic.yaml**: Fundamentals and concepts
- **02_Advanced.yaml**: Advanced configuration
- **FinOps**: Industry best practices
- **CloudHealth**: Multi-cloud tools
- **Terraform**: Infrastructure as Code

# EXAM TIPS

1. Start with showback before chargeback
2. Build trust with accurate data first
3. Automate reports early
4. Require tags in Terraform
5. Use standard cost center codes
6. Track untagged resources separately
7. Create executive dashboards
8. Review allocations monthly