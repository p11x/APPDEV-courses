---
Category: Google Cloud Platform
Subcategory: GCP Fundamentals
Concept: GCP Resource Hierarchy
Purpose: Understanding GCP resource organization, projects, folders, and organization
Difficulty: beginner
Prerequisites: 01_Basic_GCP_Infrastructure.md
RelatedFiles: 02_Advanced_GCP_Resource_Hierarchy.md, 03_Practical_GCP_Resource_Hierarchy.md
UseCase: Organizing GCP resources, access control, policy management
CertificationExam: GCP Associate Cloud Engineer
LastUpdated: 2025
---

## 💡 WHY

GCP resource hierarchy is the foundation of GCP organization. Understanding how resources are organized helps with access control, billing, and policy management.

## 📖 WHAT

### Resource Hierarchy

**Organization** (top level):
- Root of GCP resource tree
- Linked to domain
- Controls all projects and folders

**Folder**:
- Can contain projects or other folders
- Department/team organization
- Inherits policies from organization

**Project**:
- Basic unit for resource management
- Contains all GCP resources
- Has billing account attached

### Resource Relationships

- Organization → Folders → Projects → Resources
- Policies flow down (inheritance)
- IAM policies at any level

## 🔧 HOW

### Example: List Resources

```bash
# List organizations (if you have access)
gcloud organizations list

# List folders
gcloud resource-manager folders list --organization=ORG_ID

# List projects
gcloud projects list

# List resources in project
gcloud compute instances list
gcloud storage buckets list
```

## ✅ EXAM TIPS

- Organization at top
- Folder can contain folders
- Project is billing unit
- Policies inherit downward
