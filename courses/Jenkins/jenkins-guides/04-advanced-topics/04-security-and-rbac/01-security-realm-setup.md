# Security Realm Setup in Jenkins

## What this covers

This guide explains how to configure the Security Realm in Jenkins to control authentication. You'll learn about the different options: Jenkins own database, LDAP, Active Directory, and GitHub OAuth.

## Prerequisites

- Jenkins admin access
- Understanding of your authentication requirements

## What is Security Realm?

The Security Realm defines **how users authenticate** to Jenkins:

```
┌─────────────────────────────────────────────────────────────────────┐
│  Security Realm (Authentication)                                    │
│  ──────────────────────────                                         │
│                                                                     │
│  Options:                                                          │
│  ├── Jenkins' own user database                                    │
│  ├── LDAP                                                          │
│  ├── Active Directory                                              │
│  └── OAuth (GitHub, GitLab, Google, etc.)                         │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Configuring Security Realm

1. Go to **Manage Jenkins** → **Configure Global Security**
2. Find **Security Realm** section
3. Select authentication method

---

## Option 1: Jenkins Own Database (Default)

**Use for**: Small teams, development

```
┌─────────────────────────────────────────────────────────────────────┐
│  Security Realm                                                    │
│                                                                     │
│  ● Jenkins' own user database                                      │
│       Allows creating users in Jenkins                             │
│                                                                     │
│  ✓ Allow users to sign up                                         │
│       Users can register themselves                                │
└─────────────────────────────────────────────────────────────────────┘
```

### Creating Users Manually

1. Go to **Manage Jenkins** → **Manage Users**
2. Click **Create User**
3. Fill in details

---

## Option 2: GitHub OAuth (Common for Open Source)

### Step 1: Create GitHub OAuth App

1. Go to https://github.com/settings/developers
2. Click **New OAuth App**
3. Fill in:

```
Application name: Jenkins CI
Homepage URL: http://jenkins.example.com
Authorization callback URL: http://jenkins.example.com/securityRealm/finishLogin
```

4. Note the **Client ID** and **Client Secret**

### Step 2: Install GitHub Authentication Plugin

1. Install **GitHub Authentication** plugin

### Step 3: Configure in Jenkins

```
┌─────────────────────────────────────────────────────────────────────┐
│  Security Realm                                                    │
│                                                                     │
│  ● GitHub Authentication Plugin                                    │
│       GitHub API URL: https://api.github.com                      │
│       Client ID: (from GitHub)                                     │
│       Client Secret: (from GitHub)                                 │
│       OAuth scope: read:user                                       │
│                                                                     │
│  Admin users: (GitHub usernames who should have admin access)      │
│  └─ github-admin-user                                              │
│                                                                     │
│  ✓ Mark existing Jenkins users as anonymous                       │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Option 3: GitLab OAuth

### Step 1: Create GitLab OAuth App

1. In GitLab: **Applications** → **New Application**
2. Set redirect URI: `http://jenkins.example.com/securityRealm/finishLogin`
3. Select scopes: `api`, `read_user`

### Step 2: Install Plugin

Install **GitLab Authentication** plugin

### Step 3: Configure

```
┌─────────────────────────────────────────────────────────────────────┐
│  Security Realm                                                    │
│                                                                     │
│  ● GitLab Authentication Plugin                                    │
│       GitLab URL: https://gitlab.com                              │
│       Client ID: (from GitLab)                                     │
│       Client Secret: (from GitLab)                                 │
│       Scope: api                                                   │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Option 4: LDAP

**Use for**: Enterprise integration

```
┌─────────────────────────────────────────────────────────────────────┐
│  Security Realm                                                    │
│                                                                     │
│  ● LDAP                                                            │
│       Server: ldap://ldap.example.com:389                         │
│       Root DN: dc=example,dc=com                                  │
│       User search base: cn=users,dc=example,dc=com                │
│       User search filter: uid={0}                                 │
│       Group search base: cn=groups,dc=example,dc=com             │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Option 5: Active Directory

**Use for**: Windows environments with AD

```
┌─────────────────────────────────────────────────────────────────────┐
│  Security Realm                                                    │
│                                                                     │
│  ● Active Directory                                                │
│       Domain: example.com                                         │
│       Site: auto-detect                                           │
│       Bind DN: (service account or leave blank)                   │
│       Bind Password: (service account password)                   │
└─────────────────────────────────────────────────────────────────────┘
```

---

## Comparison

| Method | Complexity | Use Case |
|--------|------------|----------|
| Jenkins DB | Low | Small teams, testing |
| GitHub OAuth | Low | Open source, GitHub users |
| GitLab OAuth | Low | GitLab users |
| LDAP | Medium | Enterprise |
| AD | Medium | Windows domains |

---

## Best Practices

### 1. Disable Sign-Up in Production

```
┌─────────────────────────────────────────────────────────────────────┐
│  ✓ Allow users to sign up  ← DISABLE in production!               │
└─────────────────────────────────────────────────────────────────────┘
```

### 2. Use OAuth When Possible

- No need to manage Jenkins users
- Single Sign-On (SSO)
- Automatic deprovisioning

### 3. Protect Admin Access

```groovy
// In GitHub OAuth
Admin users: list your actual admins only
```

---

## Next Steps

- **[Role-Based Access Control](02-role-based-access-control.md)** - Set up authorization
- **[Script Approval](03-script-approval-and-sandbox.md)** - Pipeline security
- **[Security Best Practices](https://www.jenkins.io/doc/book/security/)** - Official docs
