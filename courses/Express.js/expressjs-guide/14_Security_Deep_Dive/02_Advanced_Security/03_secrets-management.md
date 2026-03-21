# Secrets Management

## 📌 What You'll Learn

- Never hardcode secrets
- Using dotenv for development
- AWS Secrets Manager / HashiCorp Vault

## 💻 Code Example

```js
// .env file (NEVER commit to git)
/*
DATABASE_URL=postgresql://...
API_KEY=sk-xxx
JWT_SECRET=xxx
*/

// Use dotenv in development
import dotenv from 'dotenv';
dotenv.config();

// In production, use environment variables directly
// Or use AWS Secrets Manager / HashiCorp Vault
```
