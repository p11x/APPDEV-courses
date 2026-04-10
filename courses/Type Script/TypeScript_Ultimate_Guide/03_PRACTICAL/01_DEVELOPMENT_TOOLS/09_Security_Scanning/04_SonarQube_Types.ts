/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 09_Security_Scanning Topic: 04_SonarQube_Types Purpose: SonarQube code analysis types Difficulty: advanced UseCase: enterprise, devops Version: TS 5.0+ Compatibility: Node.js 16+ Performance: medium Security: N/A */

declare namespace SonarQubeTypes {
  interface Project {
    key: string;
    name: string;
    qualifier: string;
    visibility: 'public' | 'private';
    lastAnalysisDate?: string;
    revision?: string;
  }

  interface Component {
    key: string;
    name: string;
    qualifiers: string[];
    path?: string;
    language?: string;
    branch?: string;
    pullRequest?: string;
  }

  interface Measure {
    metric: string;
    value?: string;
    period?: {
      value: string;
    };
  }

  interface Issue {
    key: string;
    rule: string;
    severity: 'BLOCKER' | 'CRITICAL' | 'MAJOR' | 'MINOR' | 'INFO';
    type: 'BUG' | 'VULNERABILITY' | 'CODE_SMELL' | 'SECURITY_HOTSPOT';
    component: string;
    project: string;
    line?: number;
    textRange?: TextRange;
    flows: Flow[];
    context: IssueContext;
    status: 'OPEN' | 'CONFIRMED' | 'RESOLVED' | 'CLOSED' | 'REOPENED';
    message?: string;
    effort?: string;
    author?: string;
    creationDate: string;
    updateDate?: string;
    tags?: string[];
    comments?: Comment[];
  }

  interface TextRange {
    startLine: number;
    endLine: number;
    startOffset?: number;
    endOffset?: number;
  }

  interface Flow {
    type: 'EXCLUSIVE' | 'INCLUSIVE';
    description?: string;
    locations: FlowLocation[];
  }

  interface FlowLocation {
    component: string;
    textRange?: TextRange;
    msg?: string;
  }

  interface IssueContext {
    key: string;
    displayName: string;
  }

  interface Comment {
    key: string;
    login: string;
    author: string;
    htmlText: string;
    markdown: string;
    updatable: boolean;
    createdAt: string;
  }

  interface Rule {
    key: string;
    name: string;
    htmlDescription: string;
    markdownDescription?: string;
    severity: string;
    status: 'READY' | 'DEPRECATED' | 'REMOVED';
    type: string;
    tags?: string[];
    educationPrinciples?: string[];
  }

  interface QualityProfile {
    key: string;
    name: string;
    language: string;
    isDefault?: boolean;
    activeRuleCount?: number;
    activeRuleKeys?: string[];
  }

  interface QualityGate {
    key: string;
    name: string;
    isDefault?: boolean;
    conditions: QualityGateCondition[];
    onLeakPeriod?: boolean;
  }

  interface QualityGateCondition {
    metric: string;
    operator: 'EQ' | 'NE' | 'GT' | 'GTE' | 'LT';
    period?: number;
    error: string;
    isOver: boolean;
  }

  interface Analysis {
    key: string;
    date: string;
    buildNumber?: string;
    projectVersion?: string;
    revision?: string;
    changesets?: Changeset[];
  }

  interface Changeset {
    revision: string;
    author: string;
    date: string;
    message: string;
  }

  interface User {
    login: string;
    name?: string;
    email?: string;
    active?: boolean;
    token?: string;
  }

  interface Webhook {
    key: string;
    name: string;
    url: string;
    secret?: string;
    enabled?: boolean;
    events?: string[];
  }

  interface Metric {
    key: string;
    name: string;
    description?: string;
    type: 'INT' | 'FLOAT' | 'PERCENT' | 'MILLISEC' | 'SEC' | 'STRING' | 'BOOL' | 'DATA';
    domain?: string;
    direction?: number;
    qualitative?: boolean;
    hidden?: boolean;
  }
}

describe('SonarQube Types', () => {
  describe('Project', () => {
    it('should define project', () => {
      const project: SonarQubeTypes.Project = {
        key: 'my-project',
        name: 'My Project',
        qualifier: 'TRK',
        visibility: 'public',
      };
      expect(project).toBeDefined();
    });
  });

  describe('Issue', () => {
    it('should define issue', () => {
      const issue: SonarQubeTypes.Issue = {
        key: 'issue-1',
        rule: 'typescript:S1234',
        severity: 'MAJOR',
        type: 'BUG',
        component: 'src/main.ts',
        status: 'OPEN',
        creationDate: new Date().toISOString(),
      };
      expect(issue).toBeDefined();
    });
  });

  describe('Quality Gate', () => {
    it('should define quality gate', () => {
      const gate: SonarQubeTypes.QualityGate = {
        key: 'gate-1',
        name: 'Quality Gate',
        isDefault: true,
        conditions: [
          { metric: 'new_coverage', operator: 'LT', error: '80', isOver: false },
        ],
      };
      expect(gate).toBeDefined();
    });
  });
});

console.log('\n=== SonarQube Types Complete ===');
console.log('Next: 05_Security_Linting.ts');