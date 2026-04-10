/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 07_Orchestration Topic: 04_Kustomize_Types Purpose: Kustomize type definitions Difficulty: intermediate UseCase: devops, kubernetes Version: TS 5.0+ Compatibility: Node.js 16+ Performance: N/A Security: N/A */

declare namespace KustomizeTypes {
  interface Kustomization {
    apiVersion?: string;
    kind?: string;
    metadata?: {
      name: string;
      namespace?: string;
    };
    resources?: string[];
    generators?: string[];
    transformers?: string[];
    configMapGenerator?: ConfigMapGenerator[];
    secretGenerator?: SecretGenerator[];
    patchesStrategicMerge?: string[];
    patchesJson6902?: JSONPatch[];
    patches?: Patch[];
    vars?: Var[];
    images?: Record<string, string>;
    commonLabels?: Record<string, string>;
    commonAnnotations?: Record<string, string>;
    replacements?: Replacement[];
  }

  interface ConfigMapGenerator {
    name?: string;
    env?: string;
    literals?: string[];
    files?: string[];
    behavioralMerge?: boolean;
  }

  interface SecretGenerator {
    name?: string;
    literals?: string[];
    files?: string[];
    env?: string;
  }

  interface JSONPatch {
    path: string;
    patch:
      | {
          op: string;
          from?: string;
          value?: unknown;
        }[];
    target: {
      group?: string;
      version?: string;
      kind?: string;
      name?: string;
      namespace?: string;
    };
  }

  interface Patch {
    target: {
      group?: string;
      version?: string;
      kind?: string;
      name?: string;
      namespace?: string;
      labelSelector?: string;
      annotationSelector?: string;
    };
    patch: string;
  }

  interface Var {
    name: string;
    objref: ObjectReference;
  }

  interface ObjectReference {
    apiVersion?: string;
    kind?: string;
    name?: string;
  }

  interface Replacement {
    path: string;
    sources: Record<string, ReplacementSource>;
  }

  interface ReplacementSource {
    ref: ObjectReference;
    fieldPaths?: Array<{
      path: string;
      version?: string;
    }>;
  }
}

describe('Kustomize Types', () => {
  describe('Kustomization', () => {
    it('should define kustomization', () => {
      const k: KustomizeTypes.Kustomization = {
        resources: ['deployment.yaml', 'service.yaml'],
        commonLabels: { app: 'my-app' },
      };
      expect(k).toBeDefined();
    });
  });
});

console.log('\n=== Kustomize Types Complete ===');
console.log('Next: 05_Terraform_Types.ts');