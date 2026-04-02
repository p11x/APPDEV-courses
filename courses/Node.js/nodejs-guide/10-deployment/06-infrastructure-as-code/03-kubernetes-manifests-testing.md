# Kubernetes Manifests, Helm & IaC Testing

## What You'll Learn

- Comprehensive Kubernetes manifests for Node.js applications
- Kustomize for environment-specific configurations
- Helm charts with complete Node.js application example
- IaC testing frameworks: Terratest, InSpec, OPA/Gatekeeper
- Kubernetes manifest validation tools
- IaC security scanning and CI/CD integration
- GitOps workflows

## Kubernetes Manifests

### Deployment

```yaml
# k8s/base/deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: node-app
  labels:
    app: node-app
    version: v1
spec:
  replicas: 3
  selector:
    matchLabels:
      app: node-app
  strategy:
    type: RollingUpdate
    rollingUpdate:
      maxSurge: 1
      maxUnavailable: 0
  template:
    metadata:
      labels:
        app: node-app
        version: v1
      annotations:
        prometheus.io/scrape: "true"
        prometheus.io/port: "3000"
        prometheus.io/path: "/metrics"
    spec:
      serviceAccountName: node-app-sa
      terminationGracePeriodSeconds: 30
      securityContext:
        runAsNonRoot: true
        runAsUser: 1000
        fsGroup: 1000
      containers:
        - name: app
          image: myregistry.azurecr.io/node-app:v1.0.0
          imagePullPolicy: IfNotPresent
          ports:
            - containerPort: 3000
              name: http
              protocol: TCP
          env:
            - name: NODE_ENV
              value: production
            - name: PORT
              value: "3000"
            - name: DB_HOST
              valueFrom:
                configMapKeyRef:
                  name: node-app-config
                  key: db_host
            - name: DB_PASSWORD
              valueFrom:
                secretKeyRef:
                  name: node-app-secrets
                  key: db_password
            - name: JWT_SECRET
              valueFrom:
                secretKeyRef:
                  name: node-app-secrets
                  key: jwt_secret
          resources:
            requests:
              cpu: 250m
              memory: 256Mi
            limits:
              cpu: 500m
              memory: 512Mi
          livenessProbe:
            httpGet:
              path: /health
              port: http
            initialDelaySeconds: 15
            periodSeconds: 20
            timeoutSeconds: 5
            failureThreshold: 3
          readinessProbe:
            httpGet:
              path: /health/ready
              port: http
            initialDelaySeconds: 5
            periodSeconds: 10
            timeoutSeconds: 3
            failureThreshold: 3
          startupProbe:
            httpGet:
              path: /health
              port: http
            initialDelaySeconds: 10
            periodSeconds: 5
            failureThreshold: 30
          lifecycle:
            preStop:
              exec:
                command: ["/bin/sh", "-c", "sleep 5"]
          volumeMounts:
            - name: config-volume
              mountPath: /app/config
              readOnly: true
      volumes:
        - name: config-volume
          configMap:
            name: node-app-files
      imagePullSecrets:
        - name: registry-credentials
```

### Service & Ingress

```yaml
# k8s/base/service.yaml
apiVersion: v1
kind: Service
metadata:
  name: node-app
  labels:
    app: node-app
spec:
  type: ClusterIP
  ports:
    - port: 80
      targetPort: http
      protocol: TCP
      name: http
  selector:
    app: node-app

---
# k8s/base/ingress.yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: node-app
  annotations:
    kubernetes.io/ingress.class: nginx
    cert-manager.io/cluster-issuer: letsencrypt-prod
    nginx.ingress.kubernetes.io/ssl-redirect: "true"
    nginx.ingress.kubernetes.io/proxy-body-size: "10m"
    nginx.ingress.kubernetes.io/rate-limit: "100"
    nginx.ingress.kubernetes.io/rate-limit-window: "1m"
spec:
  tls:
    - hosts:
        - app.example.com
      secretName: app-tls
  rules:
    - host: app.example.com
      http:
        paths:
          - path: /
            pathType: Prefix
            backend:
              service:
                name: node-app
                port:
                  name: http
          - path: /api
            pathType: Prefix
            backend:
              service:
                name: node-app-api
                port:
                  name: http
```

### ConfigMap & Secrets

```yaml
# k8s/base/configmap.yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: node-app-config
data:
  db_host: "postgres.database.svc.cluster.local"
  db_port: "5432"
  db_name: "myapp"
  redis_host: "redis.cache.svc.cluster.local"
  log_level: "info"
  cors_origins: "https://app.example.com"

---
# k8s/base/secret.yaml
apiVersion: v1
kind: Secret
metadata:
  name: node-app-secrets
type: Opaque
stringData:
  db_password: "CHANGEME_USE_EXTERNAL_SECRETS"
  jwt_secret: "CHANGEME_USE_EXTERNAL_SECRETS"
  session_secret: "CHANGEME_USE_EXTERNAL_SECRETS"

---
# k8s/base/external-secret.yaml (using External Secrets Operator)
apiVersion: external-secrets.io/v1beta1
kind: ExternalSecret
metadata:
  name: node-app-secrets
spec:
  refreshInterval: 1h
  secretStoreRef:
    name: aws-secrets-manager
    kind: ClusterSecretStore
  target:
    name: node-app-secrets
  data:
    - secretKey: db_password
      remoteRef:
        key: myapp/production/db-password
    - secretKey: jwt_secret
      remoteRef:
        key: myapp/production/jwt-secret
    - secretKey: session_secret
      remoteRef:
        key: myapp/production/session-secret
```

### Horizontal Pod Autoscaler

```yaml
# k8s/base/hpa.yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: node-app
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: node-app
  minReplicas: 2
  maxReplicas: 20
  behavior:
    scaleUp:
      stabilizationWindowSeconds: 60
      policies:
        - type: Pods
          value: 2
          periodSeconds: 60
    scaleDown:
      stabilizationWindowSeconds: 300
      policies:
        - type: Pods
          value: 1
          periodSeconds: 120
  metrics:
    - type: Resource
      resource:
        name: cpu
        target:
          type: Utilization
          averageUtilization: 70
    - type: Resource
      resource:
        name: memory
        target:
          type: Utilization
          averageUtilization: 80
    - type: Pods
      pods:
        metric:
          name: http_requests_per_second
        target:
          type: AverageValue
          averageValue: "1000"
```

### Pod Disruption Budget

```yaml
# k8s/base/pdb.yaml
apiVersion: policy/v1
kind: PodDisruptionBudget
metadata:
  name: node-app
spec:
  minAvailable: 1
  selector:
    matchLabels:
      app: node-app
```

### Network Policy

```yaml
# k8s/base/network-policy.yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: node-app
spec:
  podSelector:
    matchLabels:
      app: node-app
  policyTypes:
    - Ingress
    - Egress
  ingress:
    - from:
        - namespaceSelector:
            matchLabels:
              name: ingress-nginx
        - podSelector:
            matchLabels:
              app: node-app
      ports:
        - protocol: TCP
          port: 3000
  egress:
    - to:
        - namespaceSelector:
            matchLabels:
              name: database
      ports:
        - protocol: TCP
          port: 5432
    - to:
        - namespaceSelector:
            matchLabels:
              name: cache
      ports:
        - protocol: TCP
          port: 6379
    - to:
        - ipBlock:
            cidr: 0.0.0.0/0
            except:
              - 10.0.0.0/8
              - 172.16.0.0/12
              - 192.168.0.0/16
      ports:
        - protocol: TCP
          port: 443
```

## Kustomize

### Base Configuration

```yaml
# k8s/base/kustomization.yaml
apiVersion: kustomize.config.k8s.io/v1beta1
kind: Kustomization

commonLabels:
  app.kubernetes.io/managed-by: kustomize
  app.kubernetes.io/part-of: node-app

resources:
  - deployment.yaml
  - service.yaml
  - ingress.yaml
  - configmap.yaml
  - hpa.yaml
  - pdb.yaml
  - network-policy.yaml
  - serviceaccount.yaml

configMapGenerator:
  - name: node-app-files
    files:
      - config/default.json
      - config/logging.json

generatorOptions:
  disableNameSuffixHash: true
```

### Production Overlay

```yaml
# k8s/overlays/production/kustomization.yaml
apiVersion: kustomize.config.k8s.io/v1beta1
kind: Kustomization

namespace: production

namePrefix: prod-

resources:
  - ../../base
  - external-secret.yaml

patches:
  - target:
      kind: Deployment
      name: node-app
    patch: |
      - op: replace
        path: /spec/replicas
        value: 3
      - op: replace
        path: /spec/template/spec/containers/0/image
        value: myregistry.azurecr.io/node-app:v1.0.0
      - op: replace
        path: /spec/template/spec/containers/0/resources/limits/cpu
        value: 1000m
      - op: replace
        path: /spec/template/spec/containers/0/resources/limits/memory
        value: 1Gi

  - target:
      kind: HPA
      name: node-app
    patch: |
      - op: replace
        path: /spec/minReplicas
        value: 3
      - op: replace
        path: /spec/maxReplicas
        value: 20

  - target:
      kind: Ingress
      name: node-app
    patch: |
      - op: replace
        path: /spec/rules/0/host
        value: app.example.com
      - op: replace
        path: /spec/tls/0/hosts/0
        value: app.example.com

images:
  - name: myregistry.azurecr.io/node-app
    newTag: v1.0.0

configMapGenerator:
  - name: node-app-config
    behavior: replace
    literals:
      - db_host=prod-postgres.database.svc.cluster.local
      - redis_host=prod-redis.cache.svc.cluster.local
      - log_level=warn
```

### Staging Overlay

```yaml
# k8s/overlays/staging/kustomization.yaml
apiVersion: kustomize.config.k8s.io/v1beta1
kind: Kustomization

namespace: staging

namePrefix: staging-

resources:
  - ../../base

patches:
  - target:
      kind: Deployment
      name: node-app
    patch: |
      - op: replace
        path: /spec/replicas
        value: 1
      - op: add
        path: /spec/template/spec/containers/0/env/-
        value:
          name: DEBUG
          value: "app:*"

images:
  - name: myregistry.azurecr.io/node-app
    newTag: latest
```

## Helm Charts

### Chart Structure

```
charts/node-app/
├── Chart.yaml
├── values.yaml
├── values-staging.yaml
├── values-production.yaml
├── charts/
├── templates/
│   ├── _helpers.tpl
│   ├── deployment.yaml
│   ├── service.yaml
│   ├── ingress.yaml
│   ├── configmap.yaml
│   ├── secret.yaml
│   ├── hpa.yaml
│   ├── pdb.yaml
│   ├── networkpolicy.yaml
│   ├── serviceaccount.yaml
│   ├── NOTES.txt
│   ├── tests/
│   │   ├── test-connection.yaml
│   │   └── test-health.yaml
│   └── hooks/
│       └── pre-install-job.yaml
└── .helmignore
```

### Chart.yaml

```yaml
# charts/node-app/Chart.yaml
apiVersion: v2
name: node-app
description: Helm chart for a Node.js application
type: application
version: 0.1.0
appVersion: "1.0.0"
maintainers:
  - name: Platform Team
    email: platform@example.com
keywords:
  - nodejs
  - express
  - api
home: https://github.com/org/node-app
sources:
  - https://github.com/org/node-app
dependencies:
  - name: postgresql
    version: "13.x.x"
    repository: https://charts.bitnami.com/bitnami
    condition: postgresql.enabled
  - name: redis
    version: "18.x.x"
    repository: https://charts.bitnami.com/bitnami
    condition: redis.enabled
```

### values.yaml

```yaml
# charts/node-app/values.yaml
replicaCount: 2

image:
  repository: myregistry.azurecr.io/node-app
  tag: latest
  pullPolicy: IfNotPresent

imagePullSecrets:
  - name: registry-credentials

nameOverride: ""
fullnameOverride: ""

serviceAccount:
  create: true
  annotations: {}
  name: ""

podAnnotations:
  prometheus.io/scrape: "true"
  prometheus.io/port: "3000"
  prometheus.io/path: "/metrics"

podSecurityContext:
  runAsNonRoot: true
  runAsUser: 1000
  fsGroup: 1000

securityContext:
  allowPrivilegeEscalation: false
  readOnlyRootFilesystem: true
  capabilities:
    drop: [ALL]

service:
  type: ClusterIP
  port: 80
  targetPort: 3000

ingress:
  enabled: true
  className: nginx
  annotations:
    cert-manager.io/cluster-issuer: letsencrypt-prod
    nginx.ingress.kubernetes.io/ssl-redirect: "true"
  hosts:
    - host: app.example.com
      paths:
        - path: /
          pathType: Prefix
  tls:
    - secretName: app-tls
      hosts:
        - app.example.com

resources:
  requests:
    cpu: 250m
    memory: 256Mi
  limits:
    cpu: 500m
    memory: 512Mi

autoscaling:
  enabled: true
  minReplicas: 2
  maxReplicas: 10
  targetCPUUtilizationPercentage: 70
  targetMemoryUtilizationPercentage: 80

pdb:
  enabled: true
  minAvailable: 1

networkPolicy:
  enabled: true
  ingress:
    - from:
        - namespaceSelector:
            matchLabels:
              name: ingress-nginx
      ports:
        - port: 3000
  egress:
    - to:
        - namespaceSelector:
            matchLabels:
              name: database
      ports:
        - port: 5432

env:
  NODE_ENV: production
  PORT: "3000"

config:
  db_host: postgres.database.svc.cluster.local
  db_port: "5432"
  db_name: myapp
  redis_host: redis.cache.svc.cluster.local

existingSecret: node-app-secrets

livenessProbe:
  httpGet:
    path: /health
    port: http
  initialDelaySeconds: 15
  periodSeconds: 20

readinessProbe:
  httpGet:
    path: /health/ready
    port: http
  initialDelaySeconds: 5
  periodSeconds: 10

startupProbe:
  httpGet:
    path: /health
    port: http
  initialDelaySeconds: 10
  periodSeconds: 5
  failureThreshold: 30

terminationGracePeriodSeconds: 30

lifecycle:
  preStop:
    exec:
      command: ["/bin/sh", "-c", "sleep 5"]

nodeSelector: {}
tolerations: []
affinity: {}

podDisruptionBudget:
  minAvailable: 1

strategy:
  type: RollingUpdate
  rollingUpdate:
    maxSurge: 1
    maxUnavailable: 0

postgresql:
  enabled: false
  auth:
    database: myapp

redis:
  enabled: false
```

### Helpers Template

```yaml
# charts/node-app/templates/_helpers.tpl
{{/*
Expand the name of the chart.
*/}}
{{- define "node-app.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
*/}}
{{- define "node-app.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "node-app.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "node-app.labels" -}}
helm.sh/chart: {{ include "node-app.chart" . }}
{{ include "node-app.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "node-app.selectorLabels" -}}
app.kubernetes.io/name: {{ include "node-app.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "node-app.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "node-app.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}
```

### Deployment Template

```yaml
# charts/node-app/templates/deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: {{ include "node-app.fullname" . }}
  labels:
    {{- include "node-app.labels" . | nindent 4 }}
spec:
  {{- if not .Values.autoscaling.enabled }}
  replicas: {{ .Values.replicaCount }}
  {{- end }}
  selector:
    matchLabels:
      {{- include "node-app.selectorLabels" . | nindent 6 }}
  {{- with .Values.strategy }}
  strategy:
    {{- toYaml . | nindent 4 }}
  {{- end }}
  template:
    metadata:
      annotations:
        checksum/config: {{ include (print $.Template.BasePath "/configmap.yaml") . | sha256sum }}
        {{- with .Values.podAnnotations }}
        {{- toYaml . | nindent 8 }}
        {{- end }}
      labels:
        {{- include "node-app.selectorLabels" . | nindent 8 }}
    spec:
      serviceAccountName: {{ include "node-app.serviceAccountName" . }}
      {{- with .Values.imagePullSecrets }}
      imagePullSecrets:
        {{- toYaml . | nindent 8 }}
      {{- end }}
      terminationGracePeriodSeconds: {{ .Values.terminationGracePeriodSeconds }}
      {{- with .Values.podSecurityContext }}
      securityContext:
        {{- toYaml . | nindent 8 }}
      {{- end }}
      containers:
        - name: {{ .Chart.Name }}
          {{- with .Values.securityContext }}
          securityContext:
            {{- toYaml . | nindent 12 }}
          {{- end }}
          image: "{{ .Values.image.repository }}:{{ .Values.image.tag | default .Chart.AppVersion }}"
          imagePullPolicy: {{ .Values.image.pullPolicy }}
          ports:
            - name: http
              containerPort: {{ .Values.service.targetPort }}
              protocol: TCP
          env:
            {{- range $key, $value := .Values.env }}
            - name: {{ $key }}
              value: {{ $value | quote }}
            {{- end }}
            {{- if .Values.existingSecret }}
            - name: DB_PASSWORD
              valueFrom:
                secretKeyRef:
                  name: {{ .Values.existingSecret }}
                  key: db_password
            - name: JWT_SECRET
              valueFrom:
                secretKeyRef:
                  name: {{ .Values.existingSecret }}
                  key: jwt_secret
            {{- end }}
            {{- range $key, $value := .Values.config }}
            - name: {{ $key | upper }}
              valueFrom:
                configMapKeyRef:
                  name: {{ include "node-app.fullname" $ }}-config
                  key: {{ $key }}
            {{- end }}
          {{- with .Values.livenessProbe }}
          livenessProbe:
            {{- toYaml . | nindent 12 }}
          {{- end }}
          {{- with .Values.readinessProbe }}
          readinessProbe:
            {{- toYaml . | nindent 12 }}
          {{- end }}
          {{- with .Values.startupProbe }}
          startupProbe:
            {{- toYaml . | nindent 12 }}
          {{- end }}
          {{- with .Values.resources }}
          resources:
            {{- toYaml . | nindent 12 }}
          {{- end }}
          {{- with .Values.lifecycle }}
          lifecycle:
            {{- toYaml . | nindent 12 }}
          {{- end }}
      {{- with .Values.nodeSelector }}
      nodeSelector:
        {{- toYaml . | nindent 8 }}
      {{- end }}
      {{- with .Values.affinity }}
      affinity:
        {{- toYaml . | nindent 8 }}
      {{- end }}
      {{- with .Values.tolerations }}
      tolerations:
        {{- toYaml . | nindent 8 }}
      {{- end }}
```

### Helm Hooks

```yaml
# charts/node-app/templates/hooks/pre-install-job.yaml
apiVersion: batch/v1
kind: Job
metadata:
  name: {{ include "node-app.fullname" . }}-migrate
  labels:
    {{- include "node-app.labels" . | nindent 4 }}
  annotations:
    "helm.sh/hook": pre-install,pre-upgrade
    "helm.sh/hook-weight": "-5"
    "helm.sh/hook-delete-policy": before-hook-creation,hook-succeeded
spec:
  template:
    metadata:
      name: {{ include "node-app.fullname" . }}-migrate
    spec:
      restartPolicy: Never
      serviceAccountName: {{ include "node-app.serviceAccountName" . }}
      containers:
        - name: migrate
          image: "{{ .Values.image.repository }}:{{ .Values.image.tag | default .Chart.AppVersion }}"
          command: ["npx", "prisma", "migrate", "deploy"]
          env:
            - name: DATABASE_URL
              valueFrom:
                secretKeyRef:
                  name: {{ .Values.existingSecret }}
                  key: db_url
```

### Helm Tests

```yaml
# charts/node-app/templates/tests/test-connection.yaml
apiVersion: v1
kind: Pod
metadata:
  name: {{ include "node-app.fullname" . }}-test-connection
  labels:
    {{- include "node-app.labels" . | nindent 4 }}
  annotations:
    "helm.sh/hook": test
spec:
  restartPolicy: Never
  containers:
    - name: wget
      image: busybox
      command: ['wget']
      args: ['{{ include "node-app.fullname" . }}:{{ .Values.service.port }}/health']

---
# charts/node-app/templates/tests/test-health.yaml
apiVersion: v1
kind: Pod
metadata:
  name: {{ include "node-app.fullname" . }}-test-health
  annotations:
    "helm.sh/hook": test
spec:
  restartPolicy: Never
  containers:
    - name: health-test
      image: curlimages/curl
      command:
        - /bin/sh
        - -c
        - |
          response=$(curl -s -o /dev/null -w "%{http_code}" http://{{ include "node-app.fullname" . }}:{{ .Values.service.port }}/health)
          if [ "$response" != "200" ]; then
            echo "Health check failed with status $response"
            exit 1
          fi
          echo "Health check passed"
```

### Deploying with Helm

```bash
# Install
helm install node-app ./charts/node-app \
  --namespace production --create-namespace \
  -f charts/node-app/values-production.yaml \
  --set image.tag=v1.2.3

# Upgrade
helm upgrade node-app ./charts/node-app \
  -f charts/node-app/values-production.yaml \
  --set image.tag=v1.2.4 \
  --wait --timeout 5m

# Rollback
helm rollback node-app 2

# Dry run
helm install node-app ./charts/node-app --dry-run --debug

# Run tests
helm test node-app

# Template rendering
helm template node-app ./charts/node-app -f values-production.yaml
```

## IaC Testing & Validation

### Terratest (Go)

```go
// test/terraform_test.go
package test

import (
    "testing"
    "time"
    "github.com/gruntwork-io/terratest/modules/aws"
    "github.com/gruntwork-io/terratest/modules/terraform"
    "github.com/stretchr/testify/assert"
    "github.com/stretchr/testify/require"
)

func TestTerraformECS(t *testing.T) {
    t.Parallel()

    terraformOptions := terraform.WithDefaultRetryableErrors(t, &terraform.Options{
        TerraformDir: "../terraform",
        Vars: map[string]interface{}{
            "environment":  "test",
            "image_tag":    "test-latest",
            "db_username":  "testadmin",
            "db_password":  "TestP@ssw0rd!",
        },
        NoColor: true,
    })

    defer terraform.Destroy(t, terraformOptions)
    terraform.InitAndApply(t, terraformOptions)

    // Verify outputs
    appUrl := terraform.Output(t, terraformOptions, "app_url")
    assert.Contains(t, appUrl, "https://")

    dbEndpoint := terraform.Output(t, terraformOptions, "db_endpoint")
    assert.Contains(t, dbEndpoint, "rds.amazonaws.com")

    // Verify ECS service is running
    region := terraformOptions.Vars["aws_region"].(string)
    clusterName := terraform.Output(t, terraformOptions, "ecs_cluster_name")
    serviceName := terraform.Output(t, terraformOptions, "ecs_service_name")

    aws.WaitUntilEcsServiceRunning(t, region, clusterName, serviceName, 30, 5*time.Second)
}
```

### OPA/Gatekeeper Policies

```yaml
# opa/policies/no-latest-tag.yaml
apiVersion: templates.gatekeeper.sh/v1
kind: ConstraintTemplate
metadata:
  name: k8simagelatesttag
spec:
  crd:
    spec:
      names:
        kind: K8sImageLatestTag
      validation:
        openAPIV3Schema:
          type: object
          properties:
            message:
              type: string
  targets:
    - target: admission.k8s.gatekeeper.sh
      rego: |
        package k8simagelatesttag

        violation[{"msg": msg}] {
          container := input.review.object.spec.containers[_]
          endswith(container.image, ":latest")
          msg := sprintf("Container '%s' uses 'latest' tag. Use a specific version.", [container.name])
        }

        violation[{"msg": msg}] {
          container := input.review.object.spec.containers[_]
          not contains(container.image, ":")
          msg := sprintf("Container '%s' has no tag specified.", [container.name])
        }

---
apiVersion: constraints.gatekeeper.sh/v1beta1
kind: K8sImageLatestTag
metadata:
  name: no-latest-tag
spec:
  match:
    kinds:
      - apiGroups: ["apps"]
        kinds: ["Deployment", "StatefulSet", "DaemonSet"]
    namespaces: ["production", "staging"]
  parameters:
    message: "Production images must use specific version tags, not 'latest'."
```

```yaml
# opa/policies/resource-limits-required.yaml
apiVersion: templates.gatekeeper.sh/v1
kind: ConstraintTemplate
metadata:
  name: k8sresourcelimitsrequired
spec:
  crd:
    spec:
      names:
        kind: K8sResourceLimitsRequired
  targets:
    - target: admission.k8s.gatekeeper.sh
      rego: |
        package k8sresourcelimitsrequired

        violation[{"msg": msg}] {
          container := input.review.object.spec.template.spec.containers[_]
          not container.resources.limits.cpu
          msg := sprintf("Container '%s' must have CPU limits set.", [container.name])
        }

        violation[{"msg": msg}] {
          container := input.review.object.spec.template.spec.containers[_]
          not container.resources.limits.memory
          msg := sprintf("Container '%s' must have memory limits set.", [container.name])
        }

---
apiVersion: constraints.gatekeeper.sh/v1beta1
kind: K8sResourceLimitsRequired
metadata:
  name: resource-limits-required
spec:
  match:
    kinds:
      - apiGroups: ["apps"]
        kinds: ["Deployment"]
    namespaces: ["production"]
```

### Conftest (OPA for IaC)

```rego
# policies/cloudformation.rego
package main

deny[msg] {
    resource := input.Resources[name]
    resource.Type == "AWS::RDS::DBInstance"
    not resource.Properties.StorageEncrypted
    msg := sprintf("RDS instance '%s' must have storage encryption enabled", [name])
}

deny[msg] {
    resource := input.Resources[name]
    resource.Type == "AWS::RDS::DBInstance"
    not resource.Properties.DeletionProtection
    input.Parameters.Environment.Default == "production"
    msg := sprintf("Production RDS instance '%s' must have deletion protection", [name])
}

deny[msg] {
    resource := input.Resources[name]
    resource.Type == "AWS::S3::Bucket"
    not resource.Properties.PublicAccessBlockConfiguration
    msg := sprintf("S3 bucket '%s' must have public access blocked", [name])
}
```

```bash
# Run Conftest against CloudFormation templates
conftest test cloudformation/data-stores.yaml --policy policies/

# Against Helm charts
helm template node-app ./charts/node-app | conftest test --policy policies/ -

# Against Kustomize
kustomize build k8s/overlays/production | conftest test --policy policies/ -
```

### Kubernetes Manifest Validation

```bash
# kubeval - validate against Kubernetes JSON schemas
kubeval k8s/base/deployment.yaml --strict --kubernetes-version 1.28.0

# kubeconform - faster alternative
kubeconform -summary -output json k8s/base/*.yaml
kubeconform -schema-location default -schema-location \
  'https://raw.githubusercontent.com/datreeio/CRDs-catalog/main/{{.Group}}/{{.ResourceKind}}_{{.ResourceAPIVersion}}.json' \
  k8s/base/external-secret.yaml

# kube-score - static analysis
kube-score score k8s/base/deployment.yaml k8s/base/service.yaml

# kustomize validation
kustomize build k8s/overlays/production | kubeconform -strict
kustomize build k8s/overlays/production | kube-score score -

# Helm validation
helm template node-app ./charts/node-app | kubeconform -strict
helm lint ./charts/node-app --strict
```

### Terraform Testing Tools

```bash
# terraform validate
terraform init -backend=false
terraform validate
terraform fmt -check -recursive

# tflint
tflint --init
tflint --format compact

# checkov
checkov -d terraform/ --framework terraform --output junitxml > results.xml

# tfsec
tfsec terraform/ --format json --out tfsec-results.json

# terrascan
terrascan scan -d terraform/ -t aws

# infracost (cost estimation)
infracost breakdown --path terraform/
```

## IaC Security Scanning

```yaml
# .github/workflows/iac-security.yml
name: IaC Security Pipeline
on:
  pull_request:
    paths:
      - 'terraform/**'
      - 'cloudformation/**'
      - 'k8s/**'
      - 'charts/**'

jobs:
  terraform-security:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Terraform Validate
        run: |
          cd terraform
          terraform init -backend=false
          terraform validate
          terraform fmt -check

      - name: Checkov Scan
        uses: bridgecrewio/checkov-action@v12
        with:
          directory: terraform/
          framework: terraform
          output_format: cli,sarif
          output_file_path: console,checkov.sarif

      - name: tfsec
        uses: aquasecurity/tfsec-action@v1.0.3
        with:
          working_directory: terraform/
          soft_fail: false

  kubernetes-security:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Install tools
        run: |
          brew install kubeconform kube-score

      - name: Validate manifests
        run: |
          kustomize build k8s/overlays/production | kubeconform -strict -summary
          kustomize build k8s/overlays/production | kube-score score -

      - name: Checkov K8s scan
        uses: bridgecrewio/checkov-action@v12
        with:
          directory: k8s/
          framework: kubernetes

      - name: KICS Scan
        uses: checkmarx/kics-github-action@v1
        with:
          path: k8s/
          output_path: kics-results
          enable_comments: true

  helm-security:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Helm Lint
        run: helm lint charts/node-app/ --strict

      - name: Helm Template Validate
        run: |
          helm template node-app charts/node-app/ -f charts/node-app/values-production.yaml \
            | kubeconform -strict -summary

      - name: Checkov Helm scan
        uses: bridgecrewio/checkov-action@v12
        with:
          directory: charts/node-app/
          framework: helm
```

## IaC CI/CD Integration

```yaml
# .github/workflows/iac-deploy.yml
name: IaC Deploy
on:
  push:
    branches: [main]
    paths:
      - 'terraform/**'
      - 'k8s/**'
      - 'charts/**'

jobs:
  plan:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Terraform Plan
        run: |
          cd terraform
          terraform init
          terraform plan -out=tfplan

      - name: Upload Plan
        uses: actions/upload-artifact@v4
        with:
          name: terraform-plan
          path: terraform/tfplan

  deploy-staging:
    needs: plan
    if: github.ref == 'refs/heads/main'
    runs-on: ubuntu-latest
    environment: staging
    steps:
      - uses: actions/checkout@v4

      - name: Deploy K8s Staging
        run: |
          kustomize build k8s/overlays/staging | kubectl apply -f -
          kubectl rollout status deployment/prod-node-app -n staging --timeout=300s

  deploy-production:
    needs: deploy-staging
    runs-on: ubuntu-latest
    environment: production
    steps:
      - uses: actions/checkout@v4

      - name: Download Plan
        uses: actions/download-artifact@v4
        with:
          name: terraform-plan
          path: terraform/

      - name: Terraform Apply
        run: |
          cd terraform
          terraform apply -auto-approve tfplan

      - name: Deploy Helm Production
        run: |
          helm upgrade --install node-app ./charts/node-app \
            -f charts/node-app/values-production.yaml \
            --namespace production \
            --wait --timeout 5m
```

## GitOps with IaC

```yaml
# argocd/application.yaml
apiVersion: argoproj.io/v1alpha1
kind: Application
metadata:
  name: node-app
  namespace: argocd
spec:
  project: default
  source:
    repoURL: https://github.com/org/infrastructure.git
    targetRevision: main
    path: k8s/overlays/production
  destination:
    server: https://kubernetes.default.svc
    namespace: production
  syncPolicy:
    automated:
      prune: true
      selfHeal: true
    syncOptions:
      - CreateNamespace=true
    retry:
      limit: 5
      backoff:
        duration: 5s
        factor: 2
        maxDuration: 3m
```

## Cross-References

- See [Terraform](./01-terraform.md) for Terraform-specific IaC patterns
- See [CloudFormation & Ansible](./02-cloudformation-ansible.md) for AWS-native IaC
- See [CI/CD Pipelines](../05-ci-cd-pipelines/01-github-actions.md) for deployment automation
- See [Container Security](../07-container-security/01-image-scanning.md) for image hardening
- See [Security Scanning](../09-deployment-security/01-security-scanning.md) for vulnerability management
- See [Architecture](../01-deployment-architecture/01-architecture-patterns.md) for design patterns

## Next Steps

Continue to [Container Security](../07-container-security/01-image-scanning.md).
