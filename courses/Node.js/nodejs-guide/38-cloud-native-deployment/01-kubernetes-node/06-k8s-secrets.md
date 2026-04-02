# Kubernetes Secrets

## What You'll Learn

- How to create and manage Kubernetes secrets
- How to inject secrets into pods
- How to implement secret encryption at rest
- How to use external secret management

---

## Layer 1: Academic Foundation

### Secrets Management

Secrets store sensitive information like passwords, OAuth tokens, and SSH keys. They can be mounted as files or exposed as environment variables.

---

## Layer 2: Multi-Paradigm Code Evolution

### Paradigm 1 — Generic Secret

```yaml
# secret.yaml
apiVersion: v1
kind: Secret
metadata:
  name: app-secrets
type: Opaque
stringData:
  database-password: "my-secret-password"
  api-key: "sk-xxxxxxxxxxxxx"
  jwt-secret: "jwt-signing-secret"
```

### Paradigm 2 — TLS Secret

```yaml
# secret-tls.yaml
apiVersion: v1
kind: Secret
metadata:
  name: tls-secret
type: kubernetes.io/tls
data:
  # Encoded cert and key (base64)
  tls.crt: LS0tLS1CRUdJTiBDRVJUSUZ...
  tls.key: LS0tLS1CRUdJTiBQUklWQVRF...
```

### Paradigm 3 — Secret in Pod

```yaml
# deployment-secret.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-api
spec:
  template:
    spec:
      containers:
        - name: node-api
          image: myregistry/node-api:latest
          env:
            - name: DB_PASSWORD
              valueFrom:
                secretKeyRef:
                  name: app-secrets
                  key: database-password
          volumeMounts:
            - name: secrets
              mountPath: /secrets
              readOnly: true
      volumes:
        - name: secrets
          secret:
            secretName: app-secrets
```

### Paradigm 4 — External Secrets (AWS Secrets Manager)

```yaml
# external-secret.yaml
apiVersion: external-secrets.io/v1beta1
kind: ExternalSecret
metadata:
  name: app-secrets
spec:
  refreshInterval: 1h
  secretStoreRef:
    name: aws-secrets-manager
    kind: SecretStore
  target:
    name: app-secrets
  data:
    - secretKey: database-password
      remoteRef:
        key: prod/database
        property: password
```

---

## Layer 3: Performance Engineering

### Encryption at Rest

```yaml
# encryption-config.yaml
apiVersion: apiserver.config.k8s.io/v1
kind: EncryptionConfiguration
resources:
  - resources:
      - secrets
    providers:
      - aescbc:
          keys:
            - name: key1
              secret: dGVzdC1rZXktc2VjcmV0LWZvci1lbmNyeXB0aW9u
      - identity: {}
```

---

## Layer 4: Security

### Secret Security Best Practices

- Enable encryption at rest for etcd
- Use RBAC to restrict secret access
- Rotate secrets regularly
- Use external secret management (Vault, AWS Secrets Manager)
- Never commit secrets to version control

---

## Layer 5: Testing

### Secret Validation

```typescript
// secret-test.ts
async function validateSecret(secretName: string, namespace: string) {
  const secret = await k8s.core.readNamespacedSecret(secretName, namespace);
  
  return {
    type: secret.type,
    dataKeys: Object.keys(secret.data || {}),
    hasOwnerReferences: !!secret.metadata?.ownerReferences
  };
}
```

---

## Next Steps

Continue to [Horizontal Pod Autoscaler](./07-k8s-hpa.md) for scaling configuration.