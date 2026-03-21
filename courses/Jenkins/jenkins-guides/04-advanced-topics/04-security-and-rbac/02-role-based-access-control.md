# Role-Based Access Control (RBAC) in Jenkins

## What this covers

This guide explains how to configure Role-Based Access Control using the Role-based Authorization Strategy plugin. You'll learn how to create roles (Admin, Developer, Viewer), assign permissions, and use folder-level access control.

## Prerequisites

- Role-based Authorization Strategy plugin installed
- Security Realm configured
- Jenkins admin access

## What is RBAC?

RBAC controls **what users can do** after they authenticate:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Authorization (RBAC)                                               │
│  ───────────────────                                               │
│                                                                     │
│  Roles:                                                            │
│  ├── Admin: Full access to everything                             │
│  ├── Developer: Build, configure jobs, not admin settings         │
│  └── Viewer: Read-only access                                     │
│                                                                     │
│  Assignments:                                                      │
│  ├── alice → Admin                                                 │
│  ├── bob → Developer                                               │
│  └── charlie → Viewer                                             │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Installing the Plugin

1. Go to **Manage Jenkins** → **Plugin Manager**
2. Search for "Role"
3. Install **Role-based Authorization Strategy**

---

## Configuring Authorization Strategy

1. Go to **Manage Jenkins** → **Configure Global Security**
2. Find **Authorization**
3. Select **Role-Based Strategy**

```
┌─────────────────────────────────────────────────────────────────────┐
│  Authorization                                                    │
│                                                                     │
│  ● Role-Based Strategy                                             │
│       (Requires Role Strategy plugin)                              │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Managing Roles

### Step 1: Go to Role Management

Go to **Manage Jenkins** → **Manage and Assign Roles**

```
┌─────────────────────────────────────────────────────────────────────┐
│  Manage and Assign Roles                                          │
│                                                                     │
│  ├── Manage Roles                                                  │
│  └── Assign Roles                                                  │
└─────────────────────────────────────────────────────────────────────┘
```

### Step 2: Create Global Roles

Click **Manage Roles**:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Global Roles                                                      │
│                                                                     │
│  Role:  [ admin ]       Pattern: [ .* ]                           │
│  └─ Permissions:                                                   │
│      ☑ Administer                                                 │
│      ☑ Run Scripts                                               │
│      ☑ Configure Cloud                                           │
│                                                                     │
│  Role:  [ developer ]   Pattern: [ .* ]                           │
│  └─ Permissions:                                                   │
│      ☑ Overall: Read                                              │
│      ☑ Job: Configure                                             │
│      ☑ Job: Build                                                 │
│      ☑ Job: Workspace                                             │
│      ...                                                          │
│                                                                     │
│  Role:  [ viewer ]    Pattern: [ .* ]                           │
│  └─ Permissions:                                                   │
│      ☑ Overall: Read                                              │
│      ☑ Job: Discover                                              │
│      ☑ View: Read                                                 │
└─────────────────────────────────────────────────────────────────────┘
```

### Common Permissions

| Category | Permission | Description |
|----------|------------|-------------|
| **Overall** | Administer | Full admin access |
| **Overall** | Read | View Jenkins |
| **Credentials** | Create/Update/Delete | Manage credentials |
| **Job** | Build | Trigger builds |
| **Job** | Configure | Edit job config |
| **Job** | Delete | Delete jobs |
| **Job** | Discover | See jobs |
| **Job** | Read | View job |
| **Run** | Delete | Delete builds |
| **Run** | Update | Update builds |
| **View** | Configure | Edit views |

---

## Assigning Roles

### Step 1: Go to Assign Roles

Click **Assign Roles**:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Assign Roles                                                      │
│                                                                     │
│  Global Roles:                                                    │
│  ├─ admin                                                        │
│  │  ☑ alice                                                    │
│  │  ☑ bob                                                      │
│  ├─ developer                                                   │
│  │  ☑ charlie                                                  │
│  │  ☑ dave                                                     │
│  └─ viewer                                                      │
│       ☑ eve                                                      │
│                                                                     │
│  Project Roles:                                                   │
│  └─ (For folder-based roles)                                     │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Project/Folder Roles

For folder-level access control:

### Step 1: Create Project Role

In **Manage Roles** → Add:

```
Role:  [ app-developer ]
Pattern: [ my-app-.* ]

Permissions:
  ☑ Job: Build
  ☑ Job: Configure
  ☑ Job: Read
  ☑ Run: Update
```

### Step 2: Assign Project Role

In **Assign Roles** → Add:

```
Project Roles:
Role: app-developer
  ☑ team-member-1
  ☑ team-member-2
```

---

## Best Practices: Principle of Least Privilege

### Role Examples

| Role | Permissions | Use For |
|------|-------------|---------|
| **Admin** | Everything | Team leads |
| **Developer** | Build, Configure, Read | Developers |
| **CI-Manager** | Create jobs, Configure, Build | CI admins |
| **Viewer** | Read only | Stakeholders |
| **Deployer** | Build, Deploy | Deployment users |

### Example Configuration

```groovy
// Global Roles
admin         - Full access
developer     - Job: Build, Configure, Read, Run: Update
tester        - Job: Build, Read, Run: Update
viewer        - Overall: Read, Job: Discover, View: Read

// Project Roles  
backend-team  - Job: Build, Configure for backend-*
frontend-team - Job: Build, Configure for frontend-*
```

---

## Folder-Level Security

Combine with Jenkins Folders:

1. Create a folder for each team
2. Create project role with pattern matching folder:
   ```
   Pattern: team-folder/.*
   ```
3. Assign users to that role

---

## Common Mistakes

### Giving Everyone Admin

```groovy
// ❌ Too many admins!
Role: developer
  ☑ Administer

// ✅ Least privilege
Role: developer
  ☑ Job: Build
  ☑ Job: Configure
```

### Wrong Pattern Syntax

```groovy
// ❌ Won't match anything
Pattern: myapp

// ✅ Use regex
Pattern: myapp-.*
Pattern: .*myapp
Pattern: .*
```

---

## Next Steps

- **[Script Approval](03-script-approval-and-sandbox.md)** - Pipeline security
- **[Security Best Practices](https://www.jenkins.io/doc/book/security/)** - Official docs
