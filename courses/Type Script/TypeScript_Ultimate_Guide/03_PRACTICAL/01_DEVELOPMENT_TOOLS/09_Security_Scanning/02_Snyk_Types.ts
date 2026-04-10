/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 09_Security_Scanning Topic: 02_Snyk_Types Purpose: Snyk vulnerability scanning types Difficulty: intermediate UseCase: web, backend Version: TS 5.0+ Compatibility: Node.js 16+ Performance: medium Security: vulnerability-handling */

declare namespace SnykTypes {
  interface SnykProject {
    id: string;
    name: string;
    type: string;
    organization: string;
    remoteId?: string;
    branch?: string;
    status?: string;
  }

  interface Vulnerability {
    id: string;
    severity: 'critical' | 'high' | 'medium' | 'low';
    title: string;
    description?: string;
    from?: string[];
    upgradePath?: string[];
    patches?: Array<{ urls?: string[]; regions?: unknown[]; fallbackVersion?: string }>;
    exploitPath?: string;
    credits?: Array<{ name?: string; email?: string }>;
    descriptionHTML?: string;
    internal?: boolean;
    type?: string;
    package?: string;
    language?: string;
    packageManager?: string;
    vulnerableFunction?: string;
    note?: string;
    severityWithCritical?: string;
    isSubProject?: boolean;
  }

  interface Issue extends Vulnerability {
    issueId: string;
    priorityScore: number;
    cwe?: string;
    filePath?: string;
    code?: string;
    ranges?: Array<{ start: { line: number }; end: { line: number } }>;
    securityAdvisory?: Advisory;
    securityVulnerability?: Vulnerability;
  }

  interface Advisory {
    id: string;
    cve: string[];
    cwe: string[];
    title: string;
    description: string;
    severity: string;
    publicationDate?: string;
    disclosureDate?: string;
    credit?: Array<{ name?: string }>;
    permaId: string;
    ghsaId: string;
  }

  interface DepgraphResponse {
    depGraph: DepGraph;
    meta: Meta;
  }

  interface DepGraph {
    schemaVersion: string;
    pkgManager: PkgManager;
    pkgs: PackageInfo[];
    graph: GraphNode[];
  }

  interface PkgManager {
    name: string;
    repositories?: unknown[];
  }

  interface PackageInfo {
    id: string;
    info: PackageDetails;
  }

  interface PackageDetails {
    name: string;
    version?: string;
    type?: string;
  }

  interface GraphNode {
    nodeId: string;
    pkgs: string[];
    deps: Dependency[];
  }

  interface Dependency {
    nodeId: string;
    depIds: string[];
  }

  interface Meta {
    isPublic: boolean;
    name: string;
    version: string;
    packageManager: string;
    hasUnresolvedVulnerabilities: boolean;
    multiProjec?: boolean;
  }

  interface TestResult {
    ok: boolean;
    dependencyCount: number;
    issues: Issue[];
    issuesData?: {
      vulnerabilities: Record<string, Vulnerability>;
      ignoreSettings: unknown;
      advisories: Record<string, Advisory>;
    };
    from: string[][];
  }
}

import Snyk from '@snyk/protect';

describe('Snyk Types', () => {
  describe('Test Result', () => {
    it('should test project', async () => {
      const result = await Snyk.test('package.json');
      expect(result).toBeDefined();
    });
  });
});

console.log('\n=== Snyk Types Complete ===');
console.log('Next: 03_OWASP_Dependency_Check.ts');