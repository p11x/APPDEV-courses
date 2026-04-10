/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 09_Security_Scanning Topic: 01_NPM_Audit Purpose: NPM audit vulnerability scanning Difficulty: intermediate UseCase: web, frontend, backend Version: TS 5.0+ Compatibility: Node.js 16+ Performance: medium Security: vulnerability-handling */

declare namespace NPMAudit {
  interface AuditReport {
    audit_report_version: number;
    vulnerabilities: Record<string, Vulnerability>;
    metadata: Metadata;
  }

  interface Metadata {
    vulnerabilities: {
      critical: number;
      high: number;
      moderate: number;
      low: number;
      info: number;
      none: number;
    };
    dependencies: number;
    dev_dependencies: number;
    optional_dependencies: number;
    total_dependencies: number;
  }

  interface Vulnerability {
    module_name: string;
    severity: 'critical' | 'high' | 'moderate' | 'low' | 'info';
    vulnerable_versions: string;
    patched_versions?: string;
    dependency_of?: string[];
    finders?: Finder[];
    title: string;
    url?: string;
    severity_timeline?: TimelineEvent[];
    cwe?: string[];
    ecosystem: string;
    fragments?: unknown[];
    isTransitive?: boolean;
  }

  interface Finder {
    version: string;
    type: string;
    depth: number;
  }

  interface TimelineEvent {
    severity: string;
    event: string;
    date: number;
  }

  interface Advisory {
    id: number;
    created_at: string;
    updated_at: string;
    deleted_at?: string;
    title: string;
    found_by?: string;
    reported_by?: string;
    severity: string;
    cwe?: string;
    malware?: boolean;
    patched_versions?: string;
    publish_pending?: boolean;
    overview: string;
    official_advisory?: boolean;
    recommendations?: string;
    references?: string;
    vulnerability?: {
      name?: string;
      cwe?: string;
      osvdb?: string[];
      issue_url?: string;
      path?: string[];
    };
    affects?: {
      package?: { name: string };
      ranges?: Array<{
        type: string;
        events: Array<{ introduced: string; fixed?: string }>;
      }>;
    };
    info?: Array<{ type: string; value: string }>;
    patches?: Array<{ type: string; value: string; url?: string }>;
  }

  interface AuditOptions {
    auditLevel?: 'low' | 'moderate' | 'high' | 'critical';
    registry?: string;
    dev?: boolean;
    only?: string[];
    force?: boolean;
  }

  interface FixResult {
    updated: string;
    config?: string;
    current?: string;
    previous?: string;
    latest?: string;
    range?: string;
    resolved?: string;
  }
}

import { audit, fix } from '@npmcli/audit';

describe('NPM Audit', () => {
  describe('Audit Report', () => {
    it('should run audit', async () => {
      const result = await audit();
      expect(result).toBeDefined();
    });
  });
});

console.log('\n=== NPM Audit Complete ===');
console.log('Next: 02_Snyk_Types.ts');