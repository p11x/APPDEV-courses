# Runtime Hardening & Network Security

[← Back to Container Security Overview](./01-container-security-overview.md) | [Next: Vulnerability & Compliance Monitoring →](./03-vulnerability-compliance-monitoring.md)

## Table of Contents

- [Container Runtime Security](#container-runtime-security)
- [Runtime Security with Falco](#runtime-security-with-falco)
- [Container Network Security](#container-network-security)
- [Service Mesh Security](#service-mesh-security)
- [Container Image Signing](#container-image-signing)
- [OPA Gatekeeper](#opa-gatekeeper)
- [Kyverno Policies](#kyverno-policies)
- [Pod Security Standards](#pod-security-standards)
- [Docker Security Hardening](#docker-security-hardening)
- [Runtime Threat Detection](#runtime-threat-detection)

---

## Container Runtime Security

### Seccomp Profiles

Seccomp (Secure Computing Mode) restricts the system calls available to a container process, reducing the kernel attack surface.

**Default Docker seccomp profile (custom):**

```json
{
  "defaultAction": "SCMP_ACT_ERRNO",
  "defaultErrnoRet": 1,
  "archMap": [
    {
      "architecture": "SCMP_ARCH_X86_64",
      "subArches": ["SCMP_ARCH_X86", "SCMP_ARCH_X32"]
    }
  ],
  "syscalls": [
    {
      "names": [
        "accept", "accept4", "access", "adjtimex", "alarm", "bind", "brk",
        "clock_getres", "clock_gettime", "clock_nanosleep", "close",
        "connect", "copy_file_range", "creat", "dup", "dup2", "dup3",
        "epoll_create", "epoll_create1", "epoll_ctl", "epoll_wait",
        "epoll_pwait", "eventfd", "eventfd2", "execve", "execveat", "exit",
        "exit_group", "faccessat", "fadvise64", "fallocate", "fcntl",
        "flock", "fork", "fstat", "ftruncate", "futex", "getcwd", "getdents",
        "getdents64", "getegid", "geteuid", "getgid", "getgroups",
        "getpeername", "getpgrp", "getpid", "getppid", "getpriority",
        "getrandom", "getresgid", "getresuid", "getrlimit", "getrusage",
        "getsid", "getsockname", "getsockopt", "get_thread_area", "gettid",
        "gettimeofday", "getuid", "getxattr", "inotify_add_watch",
        "inotify_init", "inotify_init1", "inotify_rm_watch", "ioctl",
        "kill", "lseek", "lstat", "madvise", "memfd_create", "mincore",
        "mkdir", "mkdirat", "mmap", "mprotect", "mremap", "munmap",
        "nanosleep", "newfstatat", "open", "openat", "pause", "pipe",
        "pipe2", "poll", "ppoll", "prctl", "pread64", "pwrite64", "read",
        "readlink", "readlinkat", "readv", "recvfrom", "recvmsg",
        "remap_file_pages", "rename", "renameat", "restart_syscall",
        "rmdir", "rt_sigaction", "rt_sigpending", "rt_sigprocmask",
        "rt_sigqueueinfo", "rt_sigreturn", "rt_sigsuspend", "rt_sigtimedwait",
        "sched_getaffinity", "sched_yield", "select", "sendmsg", "sendto",
        "set_robust_list", "setgid", "setgroups", "setpgid", "setsid",
        "setsockopt", "set_tid_address", "setuid", "shutdown", "sigaltstack",
        "socket", "socketpair", "stat", "statfs", "symlink", "symlinkat",
        "tgkill", "time", "tkill", "truncate", "umask", "uname",
        "unlink", "unlinkat", "utime", "utimensat", "utimes", "vfork",
        "vmsplice", "wait4", "waitid", "write", "writev"
      ],
      "action": "SCMP_ACT_ALLOW"
    }
  ]
}
```

**Applying seccomp profile in Kubernetes:**

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: nodejs-secure-pod
spec:
  securityContext:
    seccompProfile:
      type: Localhost
      localhostProfile: profiles/nodejs-strict.json
  containers:
    - name: app
      image: nodejs-app:latest
      securityContext:
        seccompProfile:
          type: RuntimeDefault
```

**Docker Compose seccomp:**

```yaml
services:
  nodejs-app:
    image: nodejs-app:latest
    security_opt:
      - seccomp:./seccomp-profile.json
```

### AppArmor Configuration

AppArmor provides mandatory access control by confining programs to a limited set of resources.

**AppArmor profile for Node.js containers:**

```
#include <tunables/global>

profile nodejs-container flags=(attach_disconnected,mediate_deleted) {
  #include <abstractions/base>

  # Deny mount operations
  deny mount,

  # Deny access to sensitive paths
  deny /etc/shadow r,
  deny /etc/passwd w,
  deny /etc/sudoers* r,
  deny /proc/sys/** wklx,
  deny /sys/** wklx,

  # Allow network operations
  network inet tcp,
  network inet udp,
  network inet6 tcp,
  network inet6 udp,
  deny network raw,
  deny network packet,

  # Allow file operations in app directory
  /app/** rw,
  /tmp/** rw,

  # Deny ptrace
  deny ptrace (trace,read,traceby) peer=unconfined,

  # Allow execution of node
  /usr/bin/node rmix,
  /usr/local/bin/node rmix,

  # Deny execution of shells
  deny /bin/sh x,
  deny /bin/bash x,
  deny /usr/bin/dash x,
}
```

**Apply AppArmor profile in Kubernetes:**

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: nodejs-apparmor-pod
  annotations:
    container.apparmor.security.beta.kubernetes.io/app: localhost/nodejs-container
spec:
  containers:
    - name: app
      image: nodejs-app:latest
```

### SELinux Configuration

**SELinux context for containers:**

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: nodejs-selinux-pod
spec:
  securityContext:
    seLinuxOptions:
      level: "s0:c123,c456"
      type: "container_t"
      user: "system_u"
  containers:
    - name: app
      image: nodejs-app:latest
      securityContext:
        seLinuxOptions:
          type: "svirt_lxc_net_t"
```

---

## Runtime Security with Falco

### Installation

**Install Falco via Helm:**

```bash
helm repo add falcosecurity https://falcosecurity.github.io/charts
helm repo update

helm install falco falcosecurity/falco \
  --namespace falco \
  --create-namespace \
  --set falcosidekick.enabled=true \
  --set falcosidekick.config.slack.webhookurl="https://hooks.slack.com/services/xxx" \
  --set falcosidekick.config.slack.channel="#security-alerts" \
  --set driver.kind=ebpf \
  --set falco.httpOutput.enabled=true \
  --set falco.httpOutput.url="http://falco-exporter:2801"
```

### Custom Falco Rules for Node.js

**falco-rules-nodejs.yaml:**

```yaml
- list: nodejs_binaries
  items: [node, npm, npx, yarn, pnpm]

- rule: Node.js Shell Spawned
  desc: Detect shell spawned from Node.js process (possible RCE)
  condition: >
    spawned_process and container and
    proc.pname in (nodejs_binaries) and
    proc.name in (bash, sh, dash, zsh)
  output: >
    Shell spawned from Node.js process (user=%user.name command=%proc.cmdline
    container=%container.id image=%container.image.repository)
  priority: WARNING
  tags: [nodejs, shell, process]

- rule: Node.js Unexpected Network Connection
  desc: Detect unexpected outbound connections from Node.js containers
  condition: >
    outbound and container and
    fd.type=ipv4 and
    not fd.sip in (10.0.0.0/8, 172.16.0.0/12, 192.168.0.0/16) and
    proc.name in (nodejs_binaries) and
    not fd.sport in (80, 443, 8080, 3000, 5432, 6379, 27017)
  output: >
    Unexpected outbound connection from Node.js container
    (user=%user.name command=%proc.cmdline container=%container.id
    dest=%fd.sip:%fd.sport image=%container.image.repository)
  priority: WARNING
  tags: [nodejs, network]

- rule: Node.js Sensitive File Access
  desc: Detect access to sensitive files from Node.js
  condition: >
    open_read and container and
    proc.name in (nodejs_binaries) and
    (fd.name startswith /etc/shadow or
     fd.name startswith /etc/passwd or
     fd.name startswith /root/.ssh or
     fd.name startswith /root/.env or
     fd.name startswith /proc/self/environ)
  output: >
    Sensitive file accessed from Node.js process (user=%user.name
    file=%fd.name container=%container.id image=%container.image.repository)
  priority: CRITICAL
  tags: [nodejs, filesystem, secrets]

- rule: Node.js Crypto Mining Detected
  desc: Detect potential crypto mining activity in Node.js containers
  condition: >
    spawned_process and container and
    proc.pname in (nodejs_binaries) and
    (proc.cmdline contains "stratum" or
     proc.cmdline contains "mining" or
     proc.cmdline contains "xmrig" or
     proc.cmdline contains "minerd" or
     proc.cmdline contains "cryptonight")
  output: >
    Potential crypto mining detected in Node.js container
    (command=%proc.cmdline container=%container.id image=%container.image.repository)
  priority: CRITICAL
  tags: [nodejs, cryptomining]

- rule: Node.js Container Privilege Escalation
  desc: Detect privilege escalation attempts in Node.js containers
  condition: >
    spawned_process and container and
    (proc.name=sudo or proc.name=su or
     proc.name=chmod or proc.name=chown) and
    proc.pname in (nodejs_binaries)
  output: >
    Privilege escalation attempt in Node.js container
    (command=%proc.cmdline container=%container.id image=%container.image.repository)
  priority: CRITICAL
  tags: [nodejs, privilege-escalation]
```

### Falco Alerting Configuration

**falcosidekick-values.yaml:**

```yaml
config:
  slack:
    webhookurl: "https://hooks.slack.com/services/xxx"
    channel: "#security-alerts"
    username: "Falco"
    minimumpriority: "warning"

  pagerduty:
    apikey: "your-api-key"
    service: "falco-security"
    minimumpriority: "critical"

  elasticsearch:
    hostPort: "https://elasticsearch:9200"
    index: "falco"
    type: "_doc"
    minimumpriority: "warning"

  s3:
    accesskeyid: "${AWS_ACCESS_KEY_ID}"
    secretaccesskey: "${AWS_SECRET_ACCESS_KEY}"
    bucket: "falco-alerts"
    region: "us-east-1"
    minimumpriority: "warning"

  webhook:
    address: "http://security-webhook:8080/alert"
    minimumpriority: "critical"

  customfields:
    cluster: "production"
    region: "us-east-1"
```

---

## Container Network Security

### Kubernetes NetworkPolicies

**Default deny all traffic:**

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: default-deny-all
  namespace: nodejs-production
spec:
  podSelector: {}
  policyTypes:
    - Ingress
    - Egress
```

**Allow specific ingress for Node.js API:**

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: allow-api-ingress
  namespace: nodejs-production
spec:
  podSelector:
    matchLabels:
      app: nodejs-api
  policyTypes:
    - Ingress
  ingress:
    - from:
        - namespaceSelector:
            matchLabels:
              name: ingress-nginx
        - podSelector:
            matchLabels:
              app: api-gateway
      ports:
        - protocol: TCP
          port: 3000
```

**Restrict egress to databases only:**

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: api-egress-to-db
  namespace: nodejs-production
spec:
  podSelector:
    matchLabels:
      app: nodejs-api
  policyTypes:
    - Egress
  egress:
    - to:
        - podSelector:
            matchLabels:
              app: postgresql
      ports:
        - protocol: TCP
          port: 5432
    - to:
        - podSelector:
            matchLabels:
              app: redis
      ports:
        - protocol: TCP
          port: 6379
    - to:
        - namespaceSelector: {}
          podSelector:
            matchLabels:
              k8s-app: kube-dns
      ports:
        - protocol: UDP
          port: 53
```

**Micro-segmentation between services:**

```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: microservice-isolation
  namespace: nodejs-production
spec:
  podSelector:
    matchLabels:
      tier: backend
  policyTypes:
    - Ingress
    - Egress
  ingress:
    - from:
        - podSelector:
            matchLabels:
              tier: frontend
        - podSelector:
            matchLabels:
              tier: api-gateway
      ports:
        - protocol: TCP
          port: 3000
  egress:
    - to:
        - podSelector:
            matchLabels:
              tier: database
      ports:
        - protocol: TCP
          port: 5432
    - to:
        - podSelector:
            matchLabels:
              tier: cache
      ports:
        - protocol: TCP
          port: 6379
    - to:
        - namespaceSelector: {}
          podSelector:
            matchLabels:
              k8s-app: kube-dns
      ports:
        - protocol: UDP
          port: 53
        - protocol: TCP
          port: 53
```

### Cilium Network Policies

**Cilium L7 HTTP policy for Node.js API:**

```yaml
apiVersion: cilium.io/v2
kind: CiliumNetworkPolicy
metadata:
  name: nodejs-api-l7-policy
  namespace: nodejs-production
spec:
  endpointSelector:
    matchLabels:
      app: nodejs-api
  ingress:
    - fromEndpoints:
        - matchLabels:
            app: api-gateway
      toPorts:
        - ports:
            - port: "3000"
              protocol: TCP
          rules:
            http:
              - method: "GET"
                path: "/api/v1/.*"
              - method: "POST"
                path: "/api/v1/.*"
              - method: "PUT"
                path: "/api/v1/.*"
                headers:
                  - 'Content-Type: application/json'
              - method: "DELETE"
                path: "/api/v1/.*"
                headers:
                  - 'X-Admin-Token: .*'
  egress:
    - toEndpoints:
        - matchLabels:
            k8s:io.kubernetes.pod.namespace: kube-system
            k8s-app: kube-dns
      toPorts:
        - ports:
            - port: "53"
              protocol: UDP
          rules:
            dns:
              - matchPattern: "*.svc.cluster.local"
              - matchPattern: "*.amazonaws.com"
    - toEndpoints:
        - matchLabels:
            app: postgresql
      toPorts:
        - ports:
            - port: "5432"
              protocol: TCP
```

**Cilium DNS-based egress restriction:**

```yaml
apiVersion: cilium.io/v2
kind: CiliumNetworkPolicy
metadata:
  name: nodejs-egress-dns-restriction
spec:
  endpointSelector:
    matchLabels:
      app: nodejs-worker
  egress:
    - toEndpoints:
        - matchLabels:
            k8s-app: kube-dns
      toPorts:
        - ports:
            - port: "53"
              protocol: UDP
          rules:
            dns:
              - matchPattern: "api.stripe.com"
              - matchPattern: "*.slack.com"
              - matchPattern: "api.github.com"
    - toFQDNs:
        - matchName: "api.stripe.com"
      toPorts:
        - ports:
            - port: "443"
              protocol: TCP
    - toFQDNs:
        - matchName: "api.github.com"
      toPorts:
        - ports:
            - port: "443"
              protocol: TCP
```

---

## Service Mesh Security

### mTLS with Istio

**Istio installation with strict mTLS:**

```bash
istioctl install --set profile=default \
  --set meshConfig.enableAutoMtls=true \
  --set meshConfig.defaultConfig.holdApplicationUntilProxyStarts=true
```

**Strict mTLS mesh-wide policy:**

```yaml
apiVersion: security.istio.io/v1beta1
kind: PeerAuthentication
metadata:
  name: default
  namespace: istio-system
spec:
  mtls:
    mode: STRICT
```

**Per-namespace mTLS configuration:**

```yaml
apiVersion: security.istio.io/v1beta1
kind: PeerAuthentication
metadata:
  name: nodejs-mtls
  namespace: nodejs-production
spec:
  selector:
    matchLabels:
      app: nodejs-api
  mtls:
    mode: STRICT
  portLevelMtls:
    3000:
      mode: STRICT
    8080:
      mode: PERMISSIVE
```

### Istio Authorization Policies

**Allow only specific services:**

```yaml
apiVersion: security.istio.io/v1beta1
kind: AuthorizationPolicy
metadata:
  name: nodejs-api-authz
  namespace: nodejs-production
spec:
  selector:
    matchLabels:
      app: nodejs-api
  action: ALLOW
  rules:
    - from:
        - source:
            principals:
              - "cluster.local/ns/nodejs-production/sa/api-gateway"
              - "cluster.local/ns/nodejs-production/sa/frontend"
      to:
        - operation:
            methods: ["GET", "POST", "PUT", "DELETE"]
            paths: ["/api/v1/*"]
    - from:
        - source:
            principals:
              - "cluster.local/ns/monitoring/sa/prometheus"
      to:
        - operation:
            methods: ["GET"]
            paths: ["/metrics"]
```

**Deny-all with explicit allow:**

```yaml
apiVersion: security.istio.io/v1beta1
kind: AuthorizationPolicy
metadata:
  name: deny-all-default
  namespace: nodejs-production
spec:
  {}

---
apiVersion: security.istio.io/v1beta1
kind: AuthorizationPolicy
metadata:
  name: allow-specific
  namespace: nodejs-production
spec:
  selector:
    matchLabels:
      app: nodejs-api
  action: ALLOW
  rules:
    - from:
        - source:
            namespaces: ["ingress", "nodejs-production"]
      to:
        - operation:
            ports: ["3000"]
```

**Request authentication with JWT:**

```yaml
apiVersion: security.istio.io/v1beta1
kind: RequestAuthentication
metadata:
  name: jwt-auth
  namespace: nodejs-production
spec:
  selector:
    matchLabels:
      app: nodejs-api
  jwtRules:
    - issuer: "https://auth.example.com"
      jwksUri: "https://auth.example.com/.well-known/jwks.json"
      audiences:
        - "nodejs-api"
      forwardOriginalToken: true
      outputPayloadToHeader: "x-jwt-payload"

---
apiVersion: security.istio.io/v1beta1
kind: AuthorizationPolicy
metadata:
  name: require-jwt
  namespace: nodejs-production
spec:
  selector:
    matchLabels:
      app: nodejs-api
  action: DENY
  rules:
    - from:
        - source:
            notRequestPrincipals: ["*"]
      to:
        - operation:
            notPaths: ["/health", "/ready"]
```

---

## Container Image Signing

### Cosign

**Sign a container image:**

```bash
# Generate a key pair
cosign generate-key-pair

# Sign an image
cosign sign --key cosign.key ghcr.io/myorg/nodejs-app:v1.0.0

# Sign with keyless (OIDC) using Sigstore
cosign sign --yes ghcr.io/myorg/nodejs-app:v1.0.0
```

**Verify image signature:**

```bash
cosign verify --key cosign.pub ghcr.io/myorg/nodejs-app:v1.0.0
```

**SBOM attestation with Cosign:**

```bash
# Generate SBOM
syft ghcr.io/myorg/nodejs-app:v1.0.0 -o spdx-json > sbom.json

# Attach SBOM attestation
cosign attest --key cosign.key --predicate sbom.json --type spdxjson ghcr.io/myorg/nodejs-app:v1.0.0

# Verify attestation
cosign verify-attestation --key cosign.pub --type spdxjson ghcr.io/myorg/nodejs-app:v1.0.0
```

### Cosign Verification in Kubernetes

**Cosign admission controller policy:**

```yaml
apiVersion: admissionregistration.k8s.io/v1
kind: ValidatingAdmissionWebhook
metadata:
  name: cosign-webhook
webhooks:
  - name: cosign.sigstore.dev
    clientConfig:
      service:
        name: cosign-webhook
        namespace: cosign-system
        path: /validate
    rules:
      - operations: ["CREATE", "UPDATE"]
        apiGroups: [""]
        apiVersions: ["v1"]
        resources: ["pods"]
    admissionReviewVersions: ["v1"]
    sideEffects: None
```

### Notary (Notation)

**Sign with Notation:**

```bash
# Create signing key
notation cert generate-test --default "nodejs-signer"

# Sign image
notation sign ghcr.io/myorg/nodejs-app:v1.0.0

# List signatures
notation list ghcr.io/myorg/nodejs-app:v1.0.0
```

**Notation verification policy:**

```json
{
  "version": "1.0",
  "trustPolicies": [
    {
      "name": "nodejs-images",
      "registryScopes": ["ghcr.io/myorg/nodejs-app"],
      "signatureVerification": {
        "level": "strict"
      },
      "trustStores": ["ca:nodejs-signers"],
      "trustedIdentities": [
        "x509.subject: O=MyOrg,CN=nodejs-signer"
      ]
    }
  ]
}
```

### Image Verification with Kyverno

```yaml
apiVersion: kyverno.io/v1
kind: ClusterPolicy
metadata:
  name: verify-image-signatures
spec:
  validationFailureAction: Enforce
  background: false
  rules:
    - name: verify-cosign-signature
      match:
        any:
          - resources:
              kinds:
                - Pod
      verifyImages:
        - imageReferences:
            - "ghcr.io/myorg/*"
          attestors:
            - entries:
                - keys:
                    publicKeys: |-
                      -----BEGIN PUBLIC KEY-----
                      MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE...
                      -----END PUBLIC KEY-----
          mutateDigest: true
          verifyDigest: true
          required: true
```

---

## OPA Gatekeeper

### Installation

```bash
helm repo add gatekeeper https://open-policy-agent.github.io/gatekeeper/charts
helm install gatekeeper gatekeeper/gatekeeper \
  --namespace gatekeeper-system \
  --create-namespace
```

### Constraint Templates

**Require resource limits:**

```yaml
apiVersion: templates.gatekeeper.sh/v1
kind: ConstraintTemplate
metadata:
  name: k8srequiredresources
spec:
  crd:
    spec:
      names:
        kind: K8sRequiredResources
      validation:
        openAPIV3Schema:
          type: object
          properties:
            cpuRequired:
              type: boolean
            memoryRequired:
              type: boolean
  targets:
    - target: admission.k8s.gatekeeper.sh
      rego: |
        package k8srequiredresources

        violation[{"msg": msg}] {
          container := input.review.object.spec.containers[_]
          not container.resources.limits.cpu
          input.parameters.cpuRequired
          msg := sprintf("Container %v must have CPU limits", [container.name])
        }

        violation[{"msg": msg}] {
          container := input.review.object.spec.containers[_]
          not container.resources.limits.memory
          input.parameters.memoryRequired
          msg := sprintf("Container %v must have memory limits", [container.name])
        }

        violation[{"msg": msg}] {
          container := input.review.object.spec.containers[_]
          not container.resources.requests.cpu
          input.parameters.cpuRequired
          msg := sprintf("Container %v must have CPU requests", [container.name])
        }

        violation[{"msg": msg}] {
          container := input.review.object.spec.containers[_]
          not container.resources.requests.memory
          input.parameters.memoryRequired
          msg := sprintf("Container %v must have memory requests", [container.name])
        }
```

**Disallowed image registries:**

```yaml
apiVersion: templates.gatekeeper.sh/v1
kind: ConstraintTemplate
metadata:
  name: k8sallowedregistries
spec:
  crd:
    spec:
      names:
        kind: K8sAllowedRegistries
      validation:
        openAPIV3Schema:
          type: object
          properties:
            registries:
              type: array
              items:
                type: string
  targets:
    - target: admission.k8s.gatekeeper.sh
      rego: |
        package k8sallowedregistries

        violation[{"msg": msg}] {
          container := input.review.object.spec.containers[_]
          not startswith_any(container.image, input.parameters.registries)
          msg := sprintf("Container image %v is not from an allowed registry. Allowed: %v",
            [container.image, input.parameters.registries])
        }

        violation[{"msg": msg}] {
          container := input.review.object.spec.initContainers[_]
          not startswith_any(container.image, input.parameters.registries)
          msg := sprintf("Init container image %v is not from an allowed registry",
            [container.image])
        }

        startswith_any(str, prefixes) {
          prefix := prefixes[_]
          startswith(str, prefix)
        }
```

**Require non-root user:**

```yaml
apiVersion: templates.gatekeeper.sh/v1
kind: ConstraintTemplate
metadata:
  name: k8snonroot
spec:
  crd:
    spec:
      names:
        kind: K8sNonRoot
  targets:
    - target: admission.k8s.gatekeeper.sh
      rego: |
        package k8snonroot

        violation[{"msg": msg}] {
          container := input.review.object.spec.containers[_]
          not container.securityContext.runAsNonRoot
          msg := sprintf("Container %v must set securityContext.runAsNonRoot to true",
            [container.name])
        }

        violation[{"msg": msg}] {
          container := input.review.object.spec.containers[_]
          container.securityContext.runAsUser == 0
          msg := sprintf("Container %v must not run as root (UID 0)",
            [container.name])
        }
```

**No latest tag:**

```yaml
apiVersion: templates.gatekeeper.sh/v1
kind: ConstraintTemplate
metadata:
  name: k8snolatesttag
spec:
  crd:
    spec:
      names:
        kind: K8sNoLatestTag
  targets:
    - target: admission.k8s.gatekeeper.sh
      rego: |
        package k8snolatesttag

        violation[{"msg": msg}] {
          container := input.review.object.spec.containers[_]
          endswith(container.image, ":latest")
          msg := sprintf("Container %v must not use :latest tag", [container.name])
        }

        violation[{"msg": msg}] {
          container := input.review.object.spec.containers[_]
          not contains(container.image, ":")
          msg := sprintf("Container %v must specify an explicit image tag", [container.name])
        }
```

### Constraints

```yaml
apiVersion: constraints.gatekeeper.sh/v1beta1
kind: K8sRequiredResources
metadata:
  name: require-resource-limits
spec:
  enforcementAction: deny
  match:
    kinds:
      - apiGroups: [""]
        kinds: ["Pod"]
    namespaces: ["nodejs-production"]
  parameters:
    cpuRequired: true
    memoryRequired: true

---
apiVersion: constraints.gatekeeper.sh/v1beta1
kind: K8sAllowedRegistries
metadata:
  name: allowed-registries
spec:
  enforcementAction: deny
  match:
    kinds:
      - apiGroups: [""]
        kinds: ["Pod"]
  parameters:
    registries:
      - "ghcr.io/myorg/"
      - "gcr.io/myorg/"
      - "docker.io/library/"

---
apiVersion: constraints.gatekeeper.sh/v1beta1
kind: K8sNonRoot
metadata:
  name: require-non-root
spec:
  enforcementAction: deny
  match:
    kinds:
      - apiGroups: [""]
        kinds: ["Pod"]
    excludedNamespaces: ["kube-system"]

---
apiVersion: constraints.gatekeeper.sh/v1beta1
kind: K8sNoLatestTag
metadata:
  name: no-latest-tag
spec:
  enforcementAction: deny
  match:
    kinds:
      - apiGroups: [""]
        kinds: ["Pod"]
```

---

## Kyverno Policies

### Installation

```bash
helm repo add kyverno https://kyverno.github.io/kyverno/
helm install kyverno kyverno/kyverno \
  --namespace kyverno \
  --create-namespace
```

### Security Enforcement Policies

**Require labels:**

```yaml
apiVersion: kyverno.io/v1
kind: ClusterPolicy
metadata:
  name: require-labels
spec:
  validationFailureAction: Enforce
  background: true
  rules:
    - name: require-app-label
      match:
        any:
          - resources:
              kinds:
                - Pod
      validate:
        message: "Label 'app' is required"
        pattern:
          metadata:
            labels:
              app: "?*"
    - name: require-version-label
      match:
        any:
          - resources:
              kinds:
                - Deployment
      validate:
        message: "Label 'version' is required on Deployments"
        pattern:
          metadata:
            labels:
              version: "?*"
```

**Block privilege escalation:**

```yaml
apiVersion: kyverno.io/v1
kind: ClusterPolicy
metadata:
  name: restrict-privilege-escalation
spec:
  validationFailureAction: Enforce
  background: true
  rules:
    - name: deny-privileged
      match:
        any:
          - resources:
              kinds:
                - Pod
      validate:
        message: "Privileged containers are not allowed"
        pattern:
          spec:
            containers:
              - securityContext:
                  privileged: "false"
    - name: deny-cap-sys-admin
      match:
        any:
          - resources:
              kinds:
                - Pod
      validate:
        message: "SYS_ADMIN capability is not allowed"
        deny:
          conditions:
            any:
              - key: "{{ request.object.spec.containers[].securityContext.capabilities.add[] }}"
                operator: Equals
                value: "SYS_ADMIN"
    - name: deny-host-pid
      match:
        any:
          - resources:
              kinds:
                - Pod
      validate:
        message: "Host PID sharing is not allowed"
        pattern:
          spec:
            hostPID: "false"
    - name: deny-host-network
      match:
        any:
          - resources:
              kinds:
                - Pod
      validate:
        message: "Host network is not allowed"
        pattern:
          spec:
            hostNetwork: "false"
```

**Auto-inject security context:**

```yaml
apiVersion: kyverno.io/v1
kind: ClusterPolicy
metadata:
  name: inject-security-context
spec:
  background: false
  rules:
    - name: add-security-context
      match:
        any:
          - resources:
              kinds:
                - Pod
      mutate:
        patchStrategicMerge:
          spec:
            containers:
              - (name): "*"
                securityContext:
                  runAsNonRoot: true
                  runAsUser: 1000
                  runAsGroup: 1000
                  readOnlyRootFilesystem: true
                  allowPrivilegeEscalation: false
                  capabilities:
                    drop:
                      - ALL
                  seccompProfile:
                    type: RuntimeDefault
```

**Require resource quotas:**

```yaml
apiVersion: kyverno.io/v1
kind: ClusterPolicy
metadata:
  name: require-resource-limits
spec:
  validationFailureAction: Enforce
  background: true
  rules:
    - name: check-limits
      match:
        any:
          - resources:
              kinds:
                - Pod
      validate:
        message: "CPU and memory limits are required"
        pattern:
          spec:
            containers:
              - resources:
                  limits:
                    memory: "?*"
                    cpu: "?*"
                  requests:
                    memory: "?*"
                    cpu: "?*"
```

**Restrict volume types:**

```yaml
apiVersion: kyverno.io/v1
kind: ClusterPolicy
metadata:
  name: restrict-volume-types
spec:
  validationFailureAction: Enforce
  background: true
  rules:
    - name: deny-hostpath
      match:
        any:
          - resources:
              kinds:
                - Pod
      validate:
        message: "hostPath volumes are not allowed"
        deny:
          conditions:
            any:
              - key: "{{ request.object.spec.volumes[?hostPath] | length(@) }}"
                operator: GreaterThan
                value: 0
    - name: allow-only-safe-volumes
      match:
        any:
          - resources:
              kinds:
                - Pod
      validate:
        message: "Only configMap, secret, emptyDir, projected, and persistentVolumeClaim volumes are allowed"
        deny:
          conditions:
            all:
              - key: "{{ request.object.spec.volumes[].keys(@)[] | to_string(@) }}"
                operator: AnyNotIn
                value: >-
                  ["name","configMap","secret","emptyDir",
                  "projected","persistentVolumeClaim","downwardAPI",
                  "csi","ephemeral"]
```

---

## Pod Security Standards

### Pod Security Admission (PSA)

**Namespace-level enforcement:**

```yaml
apiVersion: v1
kind: Namespace
metadata:
  name: nodejs-production
  labels:
    pod-security.kubernetes.io/enforce: restricted
    pod-security.kubernetes.io/enforce-version: latest
    pod-security.kubernetes.io/audit: restricted
    pod-security.kubernetes.io/audit-version: latest
    pod-security.kubernetes.io/warn: restricted
    pod-security.kubernetes.io/warn-version: latest
```

**Namespace for workloads needing privileged access:**

```yaml
apiVersion: v1
kind: Namespace
metadata:
  name: system-components
  labels:
    pod-security.kubernetes.io/enforce: privileged
    pod-security.kubernetes.io/audit: privileged
    pod-security.kubernetes.io/warn: baseline
```

### Pod Security Standards - Restricted Compliant Pod

```yaml
apiVersion: v1
kind: Pod
metadata:
  name: nodejs-restricted-pod
  labels:
    app: nodejs-api
spec:
  automountServiceAccountToken: false
  securityContext:
    runAsNonRoot: true
    runAsUser: 1000
    runAsGroup: 1000
    fsGroup: 1000
    seccompProfile:
      type: RuntimeDefault
  containers:
    - name: app
      image: ghcr.io/myorg/nodejs-app:v1.0.0@sha256:abc123...
      securityContext:
        runAsNonRoot: true
        runAsUser: 1000
        readOnlyRootFilesystem: true
        allowPrivilegeEscalation: false
        capabilities:
          drop:
            - ALL
        seccompProfile:
          type: RuntimeDefault
      resources:
        requests:
          cpu: 100m
          memory: 128Mi
        limits:
          cpu: 500m
          memory: 512Mi
      ports:
        - containerPort: 3000
          protocol: TCP
      livenessProbe:
        httpGet:
          path: /health
          port: 3000
        initialDelaySeconds: 10
        periodSeconds: 15
      readinessProbe:
        httpGet:
          path: /ready
          port: 3000
        initialDelaySeconds: 5
        periodSeconds: 10
      volumeMounts:
        - name: tmp
          mountPath: /tmp
        - name: app-cache
          mountPath: /app/.cache
  volumes:
    - name: tmp
      emptyDir:
        sizeLimit: 100Mi
    - name: app-cache
      emptyDir:
        sizeLimit: 200Mi
  dnsPolicy: ClusterFirst
  enableServiceLinks: false
```

---

## Docker Security Hardening

### User Namespaces

**daemon.json configuration:**

```json
{
  "userns-remap": "default",
  "no-new-privileges": true,
  "live-restore": true,
  "userland-proxy": false,
  "log-driver": "json-file",
  "log-opts": {
    "max-size": "10m",
    "max-file": "3"
  }
}
```

### Read-Only Containers

**Dockerfile:**

```dockerfile
FROM node:20-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production
COPY . .
RUN npm run build

FROM node:20-alpine
RUN addgroup -g 1001 -S nodejs && \
    adduser -S nextjs -u 1001 && \
    mkdir -p /tmp/app && \
    chown -R nextjs:nodejs /tmp/app

USER nextjs
WORKDIR /app
COPY --from=builder --chown=nextjs:nodejs /app/dist ./dist
COPY --from=builder --chown=nextjs:nodejs /app/node_modules ./node_modules
COPY --from=builder --chown=nextjs:nodejs /app/package.json ./

ENV NODE_ENV=production
ENV PORT=3000

EXPOSE 3000

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:3000/health || exit 1

ENTRYPOINT ["node", "dist/server.js"]
```

**Docker run with security options:**

```bash
docker run -d \
  --name nodejs-secure \
  --read-only \
  --tmpfs /tmp:rw,noexec,nosuid,size=100m \
  --tmpfs /app/.cache:rw,noexec,nosuid,size=200m \
  --cap-drop ALL \
  --cap-add NET_BIND_SERVICE \
  --security-opt no-new-privileges:true \
  --security-opt seccomp=./seccomp-profile.json \
  --security-opt apparmor=nodejs-container \
  --user 1000:1000 \
  --pids-limit 100 \
  --memory 512m \
  --cpus 1.0 \
  --health-cmd "wget --spider -q http://localhost:3000/health || exit 1" \
  --health-interval 30s \
  --restart unless-stopped \
  nodejs-app:latest
```

### Docker Compose Hardened

```yaml
version: "3.8"

services:
  nodejs-app:
    image: nodejs-app:latest
    read_only: true
    tmpfs:
      - /tmp:size=100M,noexec,nosuid
      - /app/.cache:size=200M,noexec,nosuid
    cap_drop:
      - ALL
    cap_add:
      - NET_BIND_SERVICE
    security_opt:
      - no-new-privileges:true
      - seccomp:./seccomp-profile.json
    user: "1000:1000"
    pids_limit: 100
    deploy:
      resources:
        limits:
          cpus: "1.0"
          memory: 512M
        reservations:
          cpus: "0.25"
          memory: 128M
    healthcheck:
      test: ["CMD", "wget", "--spider", "-q", "http://localhost:3000/health"]
      interval: 30s
      timeout: 3s
      retries: 3
      start_period: 10s
    logging:
      driver: json-file
      options:
        max-size: "10m"
        max-file: "3"
    restart: unless-stopped
```

---

## Runtime Threat Detection

### Falco + Prometheus Alerting Pipeline

**PrometheusRule for Falco alerts:**

```yaml
apiVersion: monitoring.coreos.com/v1
kind: PrometheusRule
metadata:
  name: falco-security-rules
  namespace: monitoring
spec:
  groups:
    - name: falco-security
      rules:
        - alert: FalcoCriticalSecurityEvent
          expr: falco_events{priority="Critical"} > 0
          for: 0m
          labels:
            severity: critical
            team: security
          annotations:
            summary: "Critical Falco security event detected"
            description: "{{ $labels.rule }}: {{ $labels.output }}"

        - alert: FalcoShellInContainer
          expr: count(falco_events{rule="Terminal shell in container"}) by (container) > 3
          for: 2m
          labels:
            severity: warning
          annotations:
            summary: "Multiple shell sessions detected in container {{ $labels.container }}"

        - alert: FalcoUnexpectedOutboundConnection
          expr: falco_events{rule="Unexpected outbound connection"} > 0
          for: 0m
          labels:
            severity: warning
          annotations:
            summary: "Unexpected outbound connection from {{ $labels.container }}"
```

### Runtime Security Audit Script

```bash
#!/bin/bash
# runtime-security-audit.sh

echo "=== Container Runtime Security Audit ==="
echo "Date: $(date -u +"%Y-%m-%dT%H:%M:%SZ")"
echo ""

echo "--- Privileged Containers ---"
kubectl get pods --all-namespaces -o json | \
  jq -r '.items[] | select(.spec.containers[].securityContext.privileged==true) |
  "\(.metadata.namespace)/\(.metadata.name)"'

echo ""
echo "--- Containers Running as Root ---"
kubectl get pods --all-namespaces -o json | \
  jq -r '.items[] | select(.spec.containers[].securityContext.runAsUser==0) |
  "\(.metadata.namespace)/\(.metadata.name)"'

echo ""
echo "--- Containers Without Resource Limits ---"
kubectl get pods --all-namespaces -o json | \
  jq -r '.items[] | select(.spec.containers[].resources.limits==null) |
  "\(.metadata.namespace)/\(.metadata.name)"'

echo ""
echo "--- Containers Using hostNetwork ---"
kubectl get pods --all-namespaces -o json | \
  jq -r '.items[] | select(.spec.hostNetwork==true) |
  "\(.metadata.namespace)/\(.metadata.name)"'

echo ""
echo "--- Containers Using hostPID ---"
kubectl get pods --all-namespaces -o json | \
  jq -r '.items[] | select(.spec.hostPID==true) |
  "\(.metadata.namespace)/\(.metadata.name)"'

echo ""
echo "--- Pods Without Network Policies ---"
for ns in $(kubectl get namespaces -o jsonpath='{.items[*].metadata.name}'); do
  for pod in $(kubectl get pods -n $ns -o jsonpath='{.items[*].metadata.name}'); do
    np_count=$(kubectl get networkpolicies -n $ns -o json | \
      jq --arg pod "$pod" '[.items[] | select(.spec.podSelector.matchLabels.app==$pod)] | length')
    if [ "$np_count" -eq 0 ]; then
      echo "$ns/$pod"
    fi
  done
done

echo ""
echo "--- Images Using Latest Tag ---"
kubectl get pods --all-namespaces -o json | \
  jq -r '.items[].spec.containers[] | select(.image | endswith(":latest") or (contains(":")|not)) |
  .image' | sort -u

echo ""
echo "--- Containers Without Seccomp Profile ---"
kubectl get pods --all-namespaces -o json | \
  jq -r '.items[] | select(.spec.securityContext.seccompProfile==null and
  .spec.containers[].securityContext.seccompProfile==null) |
  "\(.metadata.namespace)/\(.metadata.name)"'
```

---

## Cross-References

- [Container Security Overview](./01-container-security-overview.md)
- [Vulnerability & Compliance Monitoring](./03-vulnerability-compliance-monitoring.md)
- [Docker Fundamentals](../06-databases-performance/docker-basics.md)
- [Kubernetes Deployment](../06-databases-performance/kubernetes-deployment.md)
- [Authentication & Authorization](../08-authentication/)
- [CI/CD Security](../09-testing/)
