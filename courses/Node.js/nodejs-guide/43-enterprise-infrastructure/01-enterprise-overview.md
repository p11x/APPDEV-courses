# Enterprise Infrastructure Patterns

## What You'll Learn

- Enterprise-grade infrastructure design
- Multi-cloud deployment strategies
- Disaster recovery planning
- Compliance and governance patterns
- Infrastructure as Code best practices

---

## Layer 1: Enterprise Architecture

### Multi-Cloud Strategy

An enterprise infrastructure spans multiple cloud providers to avoid vendor lock-in and improve resilience.

**Benefits:**
- Avoid vendor lock-in
- Leverage best-of-breed services
- Geographic redundancy
- Regulatory compliance

### Architecture Patterns

```
┌─────────────────────────────────────────────────────────────┐
│                        Load Balancer                         │
│                    (Global Traffic Manager)                  │
└─────────────────────────────────────────────────────────────┘
         │                    │                    │
         ▼                    ▼                    ▼
┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐
│   AWS Region    │  │  Azure Region   │  │   GCP Region    │
│  ┌───────────┐  │  │  ┌───────────┐  │  │  ┌───────────┐  │
│  │  Primary  │  │  │  │ Secondary │  │  │  │ Secondary │  │
│  │  Cluster  │  │  │  │  Cluster  │  │  │  │  Cluster  │  │
│  └───────────┘  │  │  └───────────┘  │  │  └───────────┘  │
└─────────────────┘  └─────────────────┘  └─────────────────┘
         │                    │                    │
         └────────────────────┼────────────────────┘
                              ▼
                   ┌─────────────────────┐
                   │  Cross-Cloud DB    │
                   │  (CockroachDB,    │
                   │   YugabyteDB)     │
                   └─────────────────────┘
```

---

## Layer 2: Multi-Cloud Deployment

### Terraform Multi-Provider Setup

```hcl
# multi-cloud/main.tf
terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
    google = {
      source  = "hashicorp/google"
      version = "~> 5.0"
    }
  }
}

# AWS provider
provider "aws" {
  alias  = "aws_primary"
  region = "us-east-1"
}

# Azure provider
provider "azurerm" {
  alias               = "azure_secondary"
  subscription_id    = var.azure_subscription_id
  tenant_id          = var.azure_tenant_id
  features {}
}

# GCP provider
provider "google" {
  alias  = "gcp_tertiary"
  region = "us-central1"
  project = var.gcp_project
}
```

### Kubernetes Multi-Cluster

```yaml
# kubernetes/federation.yaml
apiVersion: core.k8s.io/v1beta1
kind: FederatedNamespace
metadata:
  name: production
spec:
  placement:
    clusters:
      - name: aws-cluster
      - name: azure-cluster
      - name: gcp-cluster
```

---

## Layer 3: Disaster Recovery

### RTO/RPO Strategy

| Tier | RTO (Recovery Time Objective) | RPO (Recovery Point Objective) |
|------|-------------------------------|--------------------------------|
| Mission Critical | < 15 min | < 1 min |
| Business Critical | < 1 hour | < 15 min |
| Standard | < 4 hours | < 1 hour |
| Low Priority | < 24 hours | < 24 hours |

### Backup Strategy

```typescript
// infrastructure/backup/backup-service.ts
export class BackupService {
  private backupProviders: BackupProvider[];

  async performBackup(): Promise<BackupResult> {
    const results = await Promise.allSettled(
      this.backupProviders.map((provider) => provider.backup())
    );

    return {
      timestamp: new Date(),
      results: results.map((r) => ({
        provider: r.providerName,
        status: r.status,
        size: r.backupSize,
      })),
    };
  }

  async restore(backupId: string, target: string): Promise<void> {
    const backups = await this.findBackup(backupId);
    await this.restoreInParallel(backups, target);
  }
}
```

---

## Layer 4: Audit Logging

### Compliance Logging

```typescript
// infrastructure/audit/audit-service.ts
import { createWriteStream } from 'node:fs';

export interface AuditEvent {
  timestamp: Date;
  actor: {
    userId: string;
    ip: string;
    userAgent: string;
  };
  action: string;
  resource: string;
  result: 'success' | 'failure';
  metadata: Record<string, unknown>;
}

export class AuditService {
  private logger: AuditLogger;

  async log(event: AuditEvent): Promise<void> {
    // Validate audit event
    this.validate(event);

    // Write to multiple destinations
    await Promise.all([
      this.logger.write(event),
      this.auditLogShipper.send(event),
    ]);
  }

  async query(params: AuditQuery): Promise<AuditEvent[]> {
    return this.auditStore.query(params);
  }
}
```

---

## Layer 5: GDPR Compliance

### Data Handling

```typescript
// infrastructure/compliance/gdpr-service.ts
export class GDPRService {
  async handleDataDeletion(userId: string): Promise<DeletionResult> {
    // Find all user data across services
    const userData = await this.findUserData(userId);

    // Delete from each system
    const results = await Promise.allSettled(
      userData.map((system) => system.delete(userId))
    );

    // Record deletion for audit
    await this.audit.log({
      action: 'DATA_DELETION',
      userId,
      deletedSystems: results,
    });

    return new DeletionResult(results);
  }

  async handleDataExport(userId: string): Promise<DataPackage> {
    const userData = await this.findUserData(userId);
    return new DataPackage(userData);
  }

  async handleConsentChange(userId: string, consents: Consent[]): Promise<void> {
    await this.consentStore.update(userId, consents);
  }
}
```

---

## Next Steps

Continue to [Multi-Cloud Deployments](./02-multi-cloud-deployments.md)