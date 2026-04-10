/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 09_Security_Scanning Topic: 05_Security_Linting Purpose: ESLint security rules for TypeScript Difficulty: intermediate UseCase: web, frontend, backend Version: TS 5.0+ Compatibility: Node.js 16+, ESLint Performance: fast Security: static-analysis */

declare namespace SecurityLinting {
  interface ESLintConfig {
    root?: boolean;
    env?: Record<string, boolean>;
    extends?: string[];
    plugins?: string[];
    rules?: Record<string, RuleConfig>;
    overrides?: ConfigOverride[];
  }

  interface RuleConfig {
    severity?: 'off' | 'warn' | 'error' | number;
    options?: unknown[];
  }

  interface ConfigOverride {
    files: string[];
    rules?: Record<string, RuleConfig>;
  }

  interface SecurityRule {
    meta: {
      type: 'problem' | 'suggestion' | 'layout';
      docs: {
        description: string;
        category: string;
        recommended: boolean | string;
        url?: string;
      };
      fixable: 'code' | 'whitespace' | 'layout';
      messages: Record<string, string>;
      schema?: unknown[];
    };
    create: (context: RuleContext) => RuleListener;
  }

  interface RuleContext {
    id: string;
    options: unknown[];
    settings: Record<string, unknown>;
    parser: { parse: (code: string) => AST; parseForESLint: (code: string) => ASTResult };
    parserServices: unknown;
    getAncestors(): ASTNode[];
    getDeclaredVariables(node: ASTNode): Variable[];
    getFilename(): string;
    getPhysicalFilename(): string;
    getSourceCode(): SourceCode;
    getScope(node?: ASTNode): Scope;
    getSourceLines(): string[];
    getCwd(): string;
    report(descriptor: ReportDescriptor): void;
  }

  interface RuleListener {
    [key: string]: (node: ASTNode) => void;
  }

  interface ReportDescriptor {
    node: ASTNode;
    message: string;
    data?: Record<string, unknown>;
    fix(fixer: Fixer): Fix | null;
  }

  interface AST {
    type: string;
    body: ASTNode[];
    comments: Token[];
    tokens: Token[];
    range?: [number, number];
    loc: {
      start: Position;
      end: Position;
    };
  }

  interface ASTNode extends Record<string, unknown> {
    type: string;
    range?: [number, number];
    loc: {
      start: Position;
      end: Position;
    };
  }

  interface Position {
    line: number;
    column: number;
  }

  interface Token {
    type: string;
    value: string;
    range: [number, number];
    loc: {
      start: Position;
      end: Position;
    };
  }

  interface Variable {
    name: string;
    references: Reference[];
  }

  interface Reference {
    identifier: ASTNode;
    from: Scope;
    variable: Variable;
  }

  interface Scope {
    type: string;
    upper?: Scope;
    childScopes: Scope[];
    variables: Variable[];
    set: Map<string, Variable>;
    references: Reference[];
  }

  interface SourceCode {
    text: string;
    lines: Line[];
    tokens: Token[];
    comments: Token[];
    ast: AST;
    getCommentsBefore(node: ASTNode): Token[];
    getCommentsAfter(node: ASTNode): Token[];
    getJSDocComment(node: ASTNode): Token | null;
    getTokenAfter(node: ASTNode, options?: FindOptions): Token | null;
    getTokenBefore(node: ASTNode, options?: FindOptions): Token | null;
    getTokens(node: ASTNode, options?: FindOptions): Token[];
  }

  interface Line {
    text: string;
    range: [number, number];
    loc: {
      start: Position;
      end: Position;
    };
  }

  interface FindOptions {
    includeComments?: boolean;
    count?: number;
  }

  interface Fix {
    range: [number, number];
    text: string;
  }

  interface Fixer {
    replaceTextRange(range: [number, number], text: string): Fix;
    insertTextAfter(node: ASTNode, text: string): Fix;
    insertTextBefore(node: ASTNode, text: string): Fix;
    remove(node: ASTNode): Fix;
    removeRange(range: [number, number]): Fix;
  }

  interface ASTResult extends AST {
    visitorKeys?: Record<string, string[]>;
    scopeManager?: ScopeManager;
  }

  interface ScopeManager {}
}

describe('Security Linting', () => {
  describe('ESLint Configuration', () => {
    it('should define config', () => {
      const config: SecurityLinting.ESLintConfig = {
        root: true,
        env: { es6: true, node: true },
        extends: ['plugin:security/recommended'],
        rules: {
          'no-eval': 'error',
          'no-implied-eval': 'error',
        },
      };
      expect(config).toBeDefined();
    });
  });

  describe('Security Rules', () => {
    it('should define rule', () => {
      const rule: SecurityLinting.SecurityRule = {
        meta: {
          type: 'problem',
          docs: { description: 'Disallow eval()', category: 'Security', recommended: true },
          fixable: 'code',
          messages: { noEval: 'eval() is not allowed' },
        },
        create: (context) => ({
          CallExpression(node) {
            expect(node).toBeDefined();
          },
        }),
      };
      expect(rule).toBeDefined();
    });
  });
});

console.log('\n=== Security Linting Complete ===');
console.log('Next: 06_Secret_Scanning.ts');