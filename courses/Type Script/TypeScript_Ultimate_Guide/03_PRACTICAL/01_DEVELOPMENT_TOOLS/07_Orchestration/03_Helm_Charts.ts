/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 07_Orchestration Topic: 03_Helm_Charts Purpose: Helm chart type definitions Difficulty: advanced UseCase: devops, kubernetes Version: TS 5.0+ Compatibility: Node.js 16+ Performance: N/A Security: RBAC */

declare namespace HelmCharts {
  interface Chart {
    apiVersion: string;
    name: string;
    version: string;
    kubeVersion?: string;
    description?: string;
    type?: string;
    keywords?: string[];
    maintainers?: Maintainer[];
    home?: string;
    sources?: string[];
    dependencies?: Dependency[];
    icon?: string;
    engine?: string;
    condition?: string;
    tags?: string;
    deprecated?: boolean;
    annotations?: Record<string, string>;
  }

  interface Maintainer {
    name: string;
    email?: string;
    url?: string;
  }

  interface Dependency {
    name: string;
    version?: string;
    repository?: string;
    condition?: string;
    tags?: string[];
    import-values?: ImportValues[];
    alias?: string;
  }

  interface ImportValues {
    child?: string;
    parent: string;
  }

  interface Values {
    [key: string]: unknown;
  }

  interface Release {
    name: string;
    namespace: string;
    revision: number;
    updated: string;
    status: string;
    chart: Chart;
    values: Values;
  }

  interface Template {
    name: string;
    data?: string;
  }
}

describe('Helm Charts', () => {
  describe('Chart Metadata', () => {
    it('should create chart', () => {
      const chart: HelmCharts.Chart = {
        apiVersion: 'v2',
        name: 'my-chart',
        version: '1.0.0',
        dependencies: [],
      };
      expect(chart).toBeDefined();
    });
  });
});

console.log('\n=== Helm Charts Complete ===');
console.log('Next: 04_Kustomize_Types.ts');