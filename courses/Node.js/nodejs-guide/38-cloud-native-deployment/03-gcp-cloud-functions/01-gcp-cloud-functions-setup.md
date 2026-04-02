# GCP Cloud Functions Setup

## What You'll Learn

- How to set up GCP Cloud Functions development environment
- How to install gcloud CLI and Functions Framework
- How to create and test Cloud Functions locally
- How to configure GCP project for functions

---

## Layer 1: Environment Setup

### Installation

```bash
# Install gcloud CLI
brew install google-cloud-sdk

# Install Functions Framework
npm install -g @google-cloud/functions-framework

# Verify installation
gcloud version
functions-framework --version

# Authenticate
gcloud auth login
gcloud config set project my-project
```

---

## Layer 2: Project Structure

```
my-function/
├── package.json
├── src/
│   └── index.js
└── .gcloudignore
```

---

## Next Steps

Continue to [GCP Cloud Functions Node.js](./02-gcp-cloud-functions-node.md)