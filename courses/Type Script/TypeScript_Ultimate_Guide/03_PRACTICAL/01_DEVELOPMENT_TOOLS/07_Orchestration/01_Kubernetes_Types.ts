/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 07_Orchestration Topic: 01_Kubernetes_Types Purpose: Kubernetes resource type definitions Difficulty: advanced UseCase: devops, cloud Version: TS 5.0+ Compatibility: Node.js 16+, Kubernetes Performance: N/A Security: RBAC */

declare namespace KubernetesTypes {
  interface KubernetesObject {
    apiVersion: string;
    kind: string;
    metadata: ObjectMeta;
  }

  interface ObjectMeta {
    name: string;
    namespace?: string;
    labels?: Record<string, string>;
    annotations?: Record<string, string>;
    uid?: string;
    resourceVersion?: string;
    generation?: number;
    creationTimestamp?: string;
    deletionTimestamp?: string;
  }

  interface Pod extends KubernetesObject {
    kind: 'Pod';
    spec: PodSpec;
    status?: PodStatus;
  }

  interface PodSpec {
    containers: Container[];
    volumes?: Volume[];
    restartPolicy?: 'Always' | 'OnFailure' | 'Never';
    terminationGracePeriodSeconds?: number;
    activeDeadlineSeconds?: number;
    dnsPolicy?: 'ClusterFirst' | 'Default' | 'ClusterFirstWithHostNet' | 'None';
    nodeSelector?: Record<string, string>;
    serviceAccountName?: string;
    hostNetwork?: boolean;
    hostPID?: boolean;
    hostIPC?: boolean;
    securityContext?: PodSecurityContext;
  }

  interface Container {
    name: string;
    image: string;
    ports?: ContainerPort[];
    env?: EnvVar[];
    resources?: ResourceRequirements;
    volumeMounts?: VolumeMount[];
    command?: string[];
    args?: string[];
    imagePullPolicy?: 'Always' | 'IfNotPresent' | 'Never';
    lifecycle?: Lifecycle;
  }

  interface ContainerPort {
    name?: string;
    containerPort: number;
    protocol: 'TCP' | 'UDP' | 'SCTP';
    hostPort?: number;
  }

  interface EnvVar {
    name: string;
    value?: string;
    valueFrom?: EnvVarSource;
  }

  interface EnvVarSource {
    configMapKeyRef?: ConfigMapKeyRef;
    secretKeyRef?: SecretKeyRef;
    fieldRef?: ObjectFieldSelector;
  }

  interface ConfigMapKeyRef {
    name: string;
    key: string;
  }

  interface SecretKeyRef {
    name: string;
    key: string;
  }

  interface ObjectFieldSelector {
    fieldPath: string;
  }

  interface ResourceRequirements {
    limits?: Record<string, string>;
    requests?: Record<string, string>;
  }

  interface VolumeMount {
    name: string;
    mountPath: string;
    readOnly?: boolean;
  }

  interface Volume {
    name: string;
    configMap?: ConfigMapVolumeSource;
    secret?: SecretVolumeSource;
    emptyDir?: EmptyDirVolumeSource;
  }

  interface ConfigMapVolumeSource {
    name: string;
    items?: KeyToPath[];
  }

  interface SecretVolumeSource {
    secretName: string;
    items?: KeyToPath[];
  }

  interface EmptyDirVolumeSource {
    medium?: string;
    sizeLimit?: string;
  }

  interface KeyToPath {
    key: string;
    path: string;
    mode?: number;
  }

  interface PodSecurityContext {
    runAsNonRoot?: boolean;
    runAsUser?: number;
    fsGroup?: number;
    seLinuxOptions?: SELinuxOptions;
  }

  interface SELinuxOptions {
    level?: string;
    role?: string;
    type?: string;
    user?: string;
  }

  interface PodStatus {
    phase: 'Pending' | 'Running' | 'Succeeded' | 'Failed' | 'Unknown';
    conditions?: PodCondition[];
    message?: string;
  }

  interface PodCondition {
    type: string;
    status: 'True' | 'False' | 'Unknown';
    lastTransitionTime?: string;
    reason?: string;
    message?: string;
  }

  interface Service extends KubernetesObject {
    kind: 'Service';
    spec: ServiceSpec;
  }

  interface ServiceSpec {
    type?: 'ClusterIP' | 'NodePort' | 'LoadBalancer' | 'ExternalName';
    selector?: Record<string, string>;
    ports: ServicePort[];
    clusterIP?: string;
  }

  interface ServicePort {
    name?: string;
    port: number;
    targetPort?: number | string;
    protocol?: 'TCP' | 'UDP' | 'SCTP';
  }

  interface Deployment extends KubernetesObject {
    kind: 'Deployment';
    spec: DeploymentSpec;
  }

  interface DeploymentSpec {
    replicas?: number;
    selector: LabelSelector;
    template: PodTemplateSpec;
    strategy?: DeploymentStrategy;
  }

  interface LabelSelector {
    matchLabels?: Record<string, string>;
    matchExpressions?: LabelSelectorRequirement[];
  }

  interface LabelSelectorRequirement {
    key: string;
    operator: 'In' | 'NotIn' | 'Exists' | 'DoesNotExist';
    values?: string[];
  }

  interface PodTemplateSpec {
    metadata: ObjectMeta;
    spec: PodSpec;
  }

  interface DeploymentStrategy {
    type: 'RollingUpdate' | 'Recreate';
    rollingUpdate?: RollingUpdateDeploymentStrategy;
  }

  interface RollingUpdateDeploymentStrategy {
    maxSurge?: number | string;
    maxUnavailable?: number | string;
  }

  interface ConfigMap extends KubernetesObject {
    kind: 'ConfigMap';
    data?: Record<string, string>;
    binaryData?: Record<string, string>;
  }

  interface Secret extends KubernetesObject {
    kind: 'Secret';
    type: 'Opaque' | 'kubernetes.io/service-account-token' | 'kubernetes.io/dockercfg' | 'kubernetes.io/dockerconfigjson';
    data?: Record<string, string>;
    stringData?: Record<string, string>;
  }

  interface Ingress extends KubernetesObject {
    kind: 'Ingress';
    spec: IngressSpec;
  }

  interface IngressSpec {
    ingressClassName?: string;
    tls?: IngressTLS[];
    rules?: IngressRule[];
  }

  interface IngressTLS {
    hosts?: string[];
    secretName?: string;
  }

  interface IngressRule {
    host?: string;
    http?: HTTPIngressRuleValue;
  }

  interface HTTPIngressRuleValue {
    paths: HTTPIngressPath[];
  }

  interface HTTPIngressPath {
    path?: string;
    pathType?: 'Exact' | 'Prefix' | 'ImplementationSpecific';
    backend: IngressBackend;
  }

  interface IngressBackend {
    service?: string;
    resource?: KubernetesObject;
  }

  interface Namespace extends KubernetesObject {
    kind: 'Namespace';
    status?: NamespaceStatus;
  }

  interface NamespaceStatus {
    phase: 'Active' | 'Terminating';
  }

  interface ServiceAccount extends KubernetesObject {
    kind: 'ServiceAccount';
    secrets?: ObjectReference[];
    imagePullSecrets?: ObjectReference[];
  }

  interface ObjectReference {
    name: string;
    uid?: string;
    apiVersion?: string;
    resourceVersion?: string;
  }

  interface PersistentVolume extends KubernetesObject {
    kind: 'PersistentVolume';
    spec: PersistentVolumeSpec;
    status?: PersistentVolumeStatus;
  }

  interface PersistentVolumeSpec {
    capacity?: Record<string, string>;
    accessModes?: string[];
    persistentVolumeReclaimPolicy?: 'Retain' | 'Delete' | 'Recycle';
    storageClassName?: string;
    mountOptions?: string[];
    claimRef?: ObjectReference;
  }

  interface PersistentVolumeStatus {
    phase: 'Available' | 'Bound' | 'Released' | 'Failed';
  }

  interface PersistentVolumeClaim extends KubernetesObject {
    kind: 'PersistentVolumeClaim';
    spec: PersistentVolumeClaimSpec;
    status?: PersistentVolumeClaimStatus;
  }

  interface PersistentVolumeClaimSpec {
    accessModes?: string[];
    resources?: ResourceRequirements;
    volumeName?: string;
    storageClassName?: string;
    volumeMode?: 'Filesystem' | 'Block';
  }

  interface PersistentVolumeClaimStatus {
    phase: 'Bound' | 'Lost';
  }

  interface ReplicaSet extends KubernetesObject {
    kind: 'ReplicaSet';
    spec: ReplicaSetSpec;
  }

  interface ReplicaSetSpec {
    replicas?: number;
    selector: LabelSelector;
    template?: PodTemplateSpec;
  }

  interface DaemonSet extends KubernetesObject {
    kind: 'DaemonSet';
    spec: DaemonSetSpec;
  }

  interface DaemonSetSpec {
    selector: LabelSelector;
    template: PodTemplateSpec;
    updateStrategy?: DaemonSetUpdateStrategy;
  }

  interface DaemonSetUpdateStrategy {
    type: 'RollingUpdate' | 'OnDelete';
    rollingUpdate?: RollingUpdate;
  }

  interface RollingUpdate {
    maxUnavailable?: number | string;
  }

  interface Job extends KubernetesObject {
    kind: 'Job';
    spec: JobSpec;
  }

  interface JobSpec {
    backoffLimit?: number;
    completions?: number;
    parallelism?: number;
    activeDeadlineSeconds?: number;
    ttlSecondsAfterFinished?: number;
    template: PodTemplateSpec;
  }

  interface CronJob extends KubernetesObject {
    kind: 'CronJob';
    spec: CronJobSpec;
  }

  interface CronJobSpec {
    schedule: string;
    concurrencyPolicy?: 'Allow' | 'Forbid' | 'Replace';
    suspend?: boolean;
    successfulJobsHistoryLimit?: number;
    failedJobsHistoryLimit?: number;
    jobTemplate: JobTemplate;
  }

  interface JobTemplate {
    spec?: JobSpec;
  }
}

import { KubeConfig, CoreV1Api, AppsV1Api } from '@kubernetes/client-node';

const kc = new KubeConfig();
kc.loadFromDefault();
const k8sCoreApi = kc.makeApiClient(CoreV1Api);
const k8sAppsApi = kc.makeApiClient(AppsV1Api);

const pod: KubernetesTypes.Pod = {
  apiVersion: 'v1',
  kind: 'Pod',
  metadata: { name: 'my-pod' },
  spec: {
    containers: [{
      name: 'main',
      image: 'nginx:latest',
      ports: [{ containerPort: 80 }],
    }],
  },
};

describe('Kubernetes Types', () => {
  describe('Pod', () => {
    it('should create pod', () => {
      expect(pod).toBeDefined();
    });
  });

  describe('Service', () => {
    it('should define service', () => {
      const service: KubernetesTypes.Service = {
        apiVersion: 'v1',
        kind: 'Service',
        metadata: { name: 'my-service' },
        spec: {
          selector: { app: 'my-app' },
          ports: [{ port: 80 }],
        },
      };
      expect(service).toBeDefined();
    });
  });

  describe('Deployment', () => {
    it('should define deployment', () => {
      const deployment: KubernetesTypes.Deployment = {
        apiVersion: 'apps/v1',
        kind: 'Deployment',
        metadata: { name: 'my-deployment' },
        spec: {
          replicas: 3,
          selector: { matchLabels: { app: 'my-app' } },
          template: {
            metadata: { labels: { app: 'my-app' } },
            spec: {
              containers: [{
                name: 'main',
                image: 'nginx:latest',
              }],
            },
          },
        },
      };
      expect(deployment).toBeDefined();
    });
  });
});

console.log('\n=== Kubernetes Types Complete ===');
console.log('Next: 02_Docker_Compose_Types.ts');