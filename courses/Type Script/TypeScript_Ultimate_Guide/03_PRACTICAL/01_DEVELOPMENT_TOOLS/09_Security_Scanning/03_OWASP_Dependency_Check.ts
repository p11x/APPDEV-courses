/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 09_Security_Scanning Topic: 03_OWASP_Dependency_Check Purpose: OWASP Dependency Check integration Difficulty: advanced UseCase: backend, enterprise Version: TS 5.0+ Compatibility: Node.js 16+, Java Performance: medium Security: vulnerability-handling */

declare namespace OWASPDependencyCheck {
  interface DependencyCheckReport {
    analysisStatus: string;
    scanDate: string;
    reportVersion: number;
    projectName: string;
    dependencies: Dependency[];
    vulnerabilities: Vulnerability[];
    suppressions: Suppression[];
  }

  interface Dependency {
    fileName: string;
    filePath: string;
    md5?: string;
    sha1?: string;
    sha256?: string;
    description?: string;
    license?: string;
    projectReference?: boolean;
    includedBy: IncludedBy[];
    dependencies?: Dependency[];
  }

  interface IncludedBy {
    fileName: string;
    filePath: string;
  }

  interface Vulnerability {
    name: string;
    vulnerabilityCode: string;
    severity: string;
    criticality?: string;
    description?: string;
    cwe?: string;
    cve?: string;
    cvssScore?: string;
    cvssVector?: string;
    source?: string;
    startExternalId?: string;
    references?: Reference[];
    vulnerableSoftware: VulnerableSoftware[];
    notes?: Note[];
    suppression: boolean;
    gotVersion?: string;
    publishedDate?: string;
    cweId?: string;
  }

  interface Reference {
    source: string;
    url: string;
    text?: string;
  }

  interface VulnerableSoftware {
    softwareId: string;
    name: string;
    publisher?: string;
    version: string;
    versionEnd?: string;
    versionStart?: string;
    versionStartIncluding?: string;
    versionEndIncluding?: string;
    environment?: string;
    externalId?: string;
    vulnerabilityId: string;
    used?: boolean;
  }

  interface Note {
    text: string;
    type: string;
    timestamp: string;
  }

  interface Suppression {
    suppressOn: boolean;
    expiryDate?: string;
    instance: string;
    justification: string;
    reference: string;
    untilDate?: string;
    name?: string;
    note?: string;
    package: Package;
    vulnerability: VulnerabilityReason;
  }

  interface Package {
    name: string;
    groupId?: string;
    artifactId?: string;
    version: string;
    extension?: string;
  }

  interface VulnerabilityReason {
    name: string;
    source?: string;
    type?: string;
  }
}

describe('OWASP Dependency Check', () => {
  describe('Report', () => {
    it('should generate report', () => {
      const report: OWASPDependencyCheck.DependencyCheckReport = {
        analysisStatus: 'COMPLETE',
        scanDate: new Date().toISOString(),
        reportVersion: 1,
        projectName: 'My Project',
        dependencies: [],
        vulnerabilities: [],
        suppressions: [],
      };
      expect(report).toBeDefined();
    });
  });
});

console.log('\n=== OWASP Dependency Check Complete ===');
console.log('Next: 04_SonarQube_Types.ts');