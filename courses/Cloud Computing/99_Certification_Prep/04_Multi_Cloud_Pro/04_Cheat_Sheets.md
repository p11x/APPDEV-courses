---
Category: Certification Prep
Subcategory: Multi-Cloud Pro
Concept: Cheat Sheets
Purpose: Quick reference cheat sheets for multi-cloud environments
Difficulty: advanced
Prerequisites: AWS, Azure, GCP fundamentals
RelatedFiles: 01_Architecture_Patterns.md, 02_Design_Principles.md, 03_Case_Studies.md
UseCase: Multi-cloud architecture design and implementation
CertificationExam: Multi-Cloud Professional Certification
LastUpdated: 2025
---

# Multi-Cloud Cheat Sheet

## Service Comparison

### Compute Services

| Feature | AWS EC2 | Azure VMs | GCP Compute |
|---------|---------|-----------|-------------|
| Virtual Servers | EC2 | Virtual Machines | Compute Engine |
| Serverless | Lambda | Functions | Cloud Functions |
| Containers | ECS/EKS | ACI/AKS | GKE/Cloud Run |
| PaaS | Elastic Beanstalk | App Engine | App Engine |
| Batch | Batch | Batch | Batch |

### Storage Services

| Feature | AWS | Azure | GCP |
|---------|-----|-------|-----|
| Object Storage | S3 | Blob | Cloud Storage |
| Block Storage | EBS | Managed Disks | Persistent Disk |
| File Storage | EFS, FSx | Azure Files | Filestore |
| Archive | Glacier | Archive Storage | Archive Storage |
| Data Lake | S3 + Lake Formation | Data Lake Storage | Cloud Storage + Lake |

### Database Services

| Feature | AWS | Azure | GCP |
|---------|-----|-------|-----|
| Relational | RDS, Aurora | SQL Database, SQL MI | Cloud SQL, Spanner |
| NoSQL | DynamoDB | Cosmos DB | Firestore, Datastore |
| Cache | ElastiCache | Cache for Redis | Memorystore |
| Warehouse | Redshift | Synapse Analytics | BigQuery |

### Networking

| Feature | AWS | Azure | GCP |
|---------|-----|-------|-----|
| Virtual Network | VPC | Virtual Network | VPC |
| DNS | Route 53 | Azure DNS | Cloud DNS |
| CDN | CloudFront | Azure CDN | Cloud CDN |
| Load Balancer | ELB, ALB | Load Balancer | Load Balancer |
| VPN | VPN Gateway | VPN Gateway | Cloud VPN |
| Direct Connect | Direct Connect | ExpressRoute | Cloud Interconnect |

### Identity & Access

| Feature | AWS | Azure | GCP |
|---------|-----|-------|-----|
| IAM | IAM | Azure AD | Cloud IAM |
| Federation | IAM | Azure AD | Identity Platform |
| MFA | MFA | MFA | 2SV |
| Secrets | Secrets Manager | Key Vault | Secret Manager |

---

## CLI Quick Reference

### AWS CLI

```bash
# Configure
aws configure

# List resources
aws ec2 describe-instances
aws s3 ls
aws iam list-users

# Create resources
aws ec2 run-instances --image-id ami-xxx --instance-type t2.micro
aws s3 mb s3://bucket-name

# IAM
aws iam create-user --user-name john
aws iam attach-user-policy --user-name john --policy-arn arn:aws:iam::aws:policy/ReadOnlyAccess
```

### Azure CLI

```bash
# Login
az login

# List resources
az vm list
az storage account list
az group list

# Create resources
az vm create --name vm-name --resource-group rg-name --image UbuntuLRS
az storage account create --name account-name --resource-group rg-name

# VMs
az vm start --name vm-name --resource-group rg-name
az vm stop --name vm-name --resource-group rg-name
```

### GCP CLI

```bash
# Configure
gcloud init
gcloud auth login

# List resources
gcloud compute instances list
gcloud projects list
gsutil ls

# Create resources
gcloud compute instances create instance-name --zone us-central1-a
gsutil mb -l us-central1 gs://bucket-name

# IAM
gcloud projects add-iam-policy-binding project-id --member=user:email --role=roles/viewer
```

### Terraform

```hcl
# Provider configuration
provider "aws" {
  region = "us-east-1"
}

provider "azurerm" {
  features {}
}

provider "google" {
  project = "my-project"
  region  = "us-central1"
}

# Create resources
resource "aws_instance" "example" {
  ami           = "ami-xxx"
  instance_type = "t2.micro"
}

resource "azurerm_virtual_machine" "example" {
  name                = "vm-name"
  resource_group_name = "rg-name"
}

resource "google_compute_instance" "example" {
  name         = "instance-name"
  machine_type = "e2-medium"
}
```

---

## Kubernetes Comparison

### Managed K8s Services

| Feature | AWS EKS | Azure AKS | GCP GKE |
|---------|---------|-----------|---------|
| Managed Control Plane | Yes | Yes | Yes |
| Node Pools | Yes | Yes | Yes |
| Auto-scaling | Cluster Autoscaler | Cluster Autoscaler | Cluster Autoscaler |
| Serverless Nodes | Fargate | Virtual Nodes | Autopilot |
| Private Endpoints | Yes | Yes | Yes |

### kubectl Commands

```bash
# Context management
kubectl config get-contexts
kubectl config use-context context-name

# Deployments
kubectl get pods
kubectl get svc
kubectl apply -f deployment.yaml
kubectl rollout status deployment/app

# Troubleshooting
kubectl logs -f pod-name
kubectl describe pod pod-name
kubectl exec -it pod-name -- /bin/bash
```

---

## Cost Optimization

### Pricing Models Comparison

| Model | AWS | Azure | GCP |
|-------|-----|-------|-----|
| On-Demand | Yes | Yes | Yes |
| Reserved (1-3yr) | RI | Reserved VM | Committed Use |
| Savings Plans | Yes | Azure Savings Plan | Committed Use |
| Spot/Preemptible | Spot | Spot | Preemptible |
| Free Tier | 12 months | 12 months | Always |

### Quick Tips

| Optimization | Action |
|--------------|--------|
| Right-size | Monitor utilization, downsize |
| Reserved | Commit to 1-3 years for steady workloads |
| Spot | Use for fault-tolerant batch jobs |
| Lifecycle | Move old data to cheaper tiers |
| Auto-scale | Scale down during off-hours |
| CDN | Cache at edge for static content |

---

## Monitoring Tools

| Layer | AWS | Azure | GCP |
|-------|-----|-------|-----|
| Metrics | CloudWatch | Monitor | Monitoring |
| Logging | CloudWatch Logs | Log Analytics | Logging |
| APM | X-Ray | Application Insights | Trace |
| Alerting | CloudWatch Alerts | Alerts | Alerting |
| Dashboard | CloudWatch Dashboards | Azure Dashboards | Monitoring |

### Open Source Alternatives

- **Metrics**: Prometheus
- **Logs**: ELK Stack (Elasticsearch, Logstash, Kibana)
- **Tracing**: Jaeger, Zipkin
- **Dashboard**: Grafana

---

## Networking Ports & Protocols

### Required Ports

| Service | Port | Protocol |
|---------|------|----------|
| SSH | 22 | TCP |
| RDP | 3389 | TCP |
| HTTP | 80 | TCP |
| HTTPS | 443 | TCP |
| DNS | 53 | UDP/TCP |
| SMTP | 25 | TCP |

### Firewall Comparison

| Feature | AWS SG | Azure NSG | GCP FW |
|---------|--------|-----------|--------|
| State | Stateful | Stateful | Stateful |
| Rules | Allow only | Allow/Deny | Allow/Deny |
| Scope | Instance | Subnet/VM | Network |
| Priority | N/A (evaluated together) | Numeric | Numeric |

---

## Disaster Recovery

### RTO/RPO Targets

| Tier | RTO | RPO | Architecture |
|------|-----|-----|--------------|
| Critical | < 1 min | < 1 min | Active-Active |
| High | < 1 hour | < 5 min | Hot Standby |
| Medium | < 4 hours | < 1 hour | Warm Standby |
| Low | < 24 hours | < 24 hours | Cold Backup |

### DR Patterns

| Pattern | Description | Cost |
|---------|-------------|------|
| Backup & Restore | Recover from backups | Low |
| Pilot Light | Minimal running services | Medium |
| Warm Standby | Scaled-down environment | Medium-High |
| Active-Active | Full environment running | High |

---

## Common Issues & Solutions

| Issue | Cloud | Solution |
|-------|-------|----------|
| Instance won't start | AWS | Check IAM role, security group |
| Cannot connect to VM | Azure | Check NSG rules, public IP |
| Permission denied | GCP | Check IAM, service account |
| High costs | All | Use reserved, right-size, spot |
| Slow website | All | Enable CDN, cache, optimize |

---

## Security Checklist

- [ ] Enable MFA on all accounts
- [ ] Use least-privilege IAM roles
- [ ] Enable VPC flow logs
- [ ] Encrypt all storage
- [ ] Enable audit logging
- [ ] Regular security patches
- [ ] Security scanning enabled
- [ ] Incident response plan

---

## Quick Reference Links

| Provider | Documentation |
|----------|----------------|
| AWS | https://docs.aws.amazon.com |
| Azure | https://docs.microsoft.com/azure |
| GCP | https://cloud.google.com/docs |
| Terraform | https://www.terraform.io/docs |
| Kubernetes | https://kubernetes.io/docs |

---

**End of Cheat Sheet**
