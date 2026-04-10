/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 06 Testing Frameworks Topic: 07_Snapshot_Testing Purpose: Snapshot testing type definitions and patterns Difficulty: intermediate UseCase: web, backend Version: TS 5.0+ Compatibility: Node.js 16+, Browsers Performance: medium Security: N/A */

declare namespace SnapshotTesting {
  interface SnapshotUpdateState {
    new: 'new';
    all: 'all';
    none: 'none';
  }

  interface CreateSnapshot {
    <T>(value: T, options?: SnapshotOptions): string;
    <T>(value: T, name: string, options?: SnapshotOptions): string;
  }

  interface SnapshotOptions {
    version?: number;
    path?: string;
    filename?: string;
    normalize?: (value: string) => string;
    separator?: string;
    forceInlineExpand?: boolean;
    lazy?: boolean | ((key: string) => unknown);
  }

  interface ToMatchSnapshot {
    (received: unknown): void;
    (received: unknown, propertyMatchers?: partial<object> | unknown[]): void;
    (received: unknown, options?: ToMatchSnapshotOptions): void;
    (received: unknown, propertyMatchers?: partial<object>, options?: ToMatchSnapshotOptions): void;
    name: string;
    message?: string;
    fn: (received: unknown) => void;
  }

  interface ToMatchSnapshotOptions {
    name?: string;
    edgeSpacing?: string;
    expand?: boolean;
    prettier?: boolean;
    indent?: string;
    indentationKind?: 'spaces' | 'tab';
    trailing comma?: 'all' | 'es5' | 'none';
  }

  interface ToHaveSnapshot {
    (received: unknown): void;
    (received: unknown, propertyMatchers?: Partial<Record<string, unknown>> | unknown[]): void;
    (received: unknown, options?: ToHaveSnapshotOptions): void;
    (received: unknown, propertyMatchers?: Partial<Record<string, unknown>>, options?: ToHaveSnapshotOptions): void;
    name: string;
  }

  interface ToHaveSnapshotOptions extends ToMatchSnapshotOptions {
    count?: number;
    updateState?: SnapshotUpdateState;
  }

  interface ToMatchInlineSnapshot {
    (propertyMatchers?: Partial<Record<string, unknown>> | unknown[]): void;
    (received: unknown, propertyMatchers?: Partial<Record<string, unknown>> | unknown[]): void;
    name: string;
    message?: string;
    fn: (received: unknown) => void;
  }

  interface ToThrowErrorMatchingSnapshot {
    (propertyMatchers?: Partial<Record<string, unknown>>): void;
    (received: unknown, propertyMatchers?: Partial<Record<string, unknown>>): void;
    name: string;
    message?: string;
  }

  interface ToThrowErrorMatchingInlineSnapshot {
    (expected?: string | string[] | null): void;
    (received: unknown, expected?: string | string[] | null): void;
    name: string;
  }

  interface SnapshotResolver {
    resolvePath(testPath: string, snapshotName?: string): string;
    resolveTestPath(testPath: string, snapshotsDirPath?: string): string;
  }

  interface SnapshotConfig {
    filepath?: string;
    snapshotFormat?: SerializedFormat;
    prettier?: PrettierConfig;
    resolveSnapshotPath?: (testPath: string, snapshotName: string) => string;
    resolveTestPath?: (snapshotPath: string, snapshotName: string) => string;
    snapshotDirectory?: string;
    expand?: boolean;
    forceInlineExpand?: boolean;
    prettierPath?: string;
    prettierConfig?: PrettierConfig;
  }

  interface SerializedFormat {
    type: 'json' | 'yaml' | 'markdown' | 'md';
    printWidth?: number;
  }

  interface PrettierConfig {
    semi?: boolean;
    singleQuote?: boolean;
    tabWidth?: number;
    trailingComma?: 'all' | 'es5' | 'none';
    useTabs?: boolean;
    printWidth?: number;
    bracketSpacing?: boolean;
    arrowParens?: 'always' | 'avoid';
    endOfLine?: 'lf' | 'crlf' | 'cr';
    parser?: string;
    plugins?: (string | PrettierPlugin)[];
    tabSize?: number;
  }

  interface PrettierPlugin {
    languages?: Language[];
    parsers?: Record<string, Parser>;
    printers?: Record<string, Printer>;
    options?: PrettierOptions[];
    defaultOptions?: PrettierOptions;
    preprocess?: (text: string, options: PrettierOptions) => string;
    print?: (doc: Doc, options: PrettierOptions, print: (doc: Doc) => string) => string;
    supportOptions?: PrettierOptions[];
    languages?: Partial<Language>[];
  }

  interface Parser {
    parse: (text: string, options: PrettierOptions) => AST;
    ast: ParserAST;
    preprocess?: (text: string, options: PrettierOptions) => string;
  }

  interface AST {
    type: string;
    loc?: Location;
    range?: [number, number];
    body?: AST[];
    comments?: Comment[];
  }

  interface Location {
    start: Position;
    end: Position;
  }

  interface Position {
    line: number;
    column: number;
    offset?: number;
  }

  interface Comment {
    type: 'Line' | 'Block';
    value: string;
    loc?: Location;
  }

  interface Printer {
    print: (
      path: FastPath,
      options: PrettierOptions,
      print: (path: FastPath) => string
    ) => string;
    embed?: (
      path: FastPath,
      print: (path: FastPath) => string,
      textToDoc: (text: string, options: PrettierOptions) => Doc,
      options: PrettierOptions
    ) => string;
    canAttachComment?: (node: AST) => boolean;
    getCommentChildren?: (node: FastPath) => AST[];
    handleComments?: {
      ownLine?: CommentHandler[];
      endOfLine?: CommentHandler[];
      remaining?: CommentHandler[];
    };
  }

  interface PrettierOptions extends Record<string, unknown> {
    filepath?: string;
    parser?: string;
    plugins?: (string | PrettierPlugin)[];
    printWidth?: number;
    tabWidth?: number;
    useTabs?: boolean;
    semi?: boolean;
    singleQuote?: boolean;
    trailingComma?: 'all' | 'es5' | 'none';
    bracketSpacing?: boolean;
    arrowParens?: 'always' | 'avoid';
  }

  interface Doc {
    type: string;
    [key: string]: unknown;
  }

  interface FastPath {
    getValue(): AST;
    getNode(): AST;
    getParentNode(): AST;
    getName(): string | number | undefined;
    getKey(): string | number;
    getLength(): number;
    inForStatementInit(): boolean;
    call<T>(callback: (path: FastPath) => T, ...attrs: unknown[]): T;
    callUnknown<T>(callback: (path: FastPath, ...attrs: unknown[]) => T, ...attrs: unknown[]): T;
    each(callback: (path: FastPath) => void, ...attrs: unknown[]): void;
    map<T>(callback: (path: FastPath) => T, ...attrs: unknown[]): T[];
    getMatches(): any[];
  }

  function matchSnapshot(received: unknown): ToMatchSnapshot;
  function matchInlineSnapshot(received: unknown): ToMatchInlineSnapshot;
  function snapshotAssertion(name: string): void;

  interface SnapshotTestOptions {
    mode?: 'classic' | 'concurrent';
    updateSnapshot?: SnapshotUpdateState;
    includePropertyRectangles?: boolean;
    pretty?: boolean;
  }

  interface MatcherHintOptions {
    comment?: string;
    isDirectly?: boolean;
    isNot?: boolean;
    secondArgument?: string;
    thirdArgument?: string;
  }

  interface MatcherFunction {
    (received: unknown, ...args: unknown[]): { pass: boolean; message: () => string };
    (received: unknown, ...args: unknown[]): { pass: boolean; message: () => string };
  }
}

interface Snapshot {
  state: string;
  name: string;
  fn: () => void;
}

interface Snapshots {
  name: string;
  state: {
    snapshotState: SnapshotState;
  };
  skip?: boolean;
  fn?: () => void;
  todo?: boolean;
}

interface SnapshotState {
  added: number;
  file: number;
  new: number;
  removed: number;
  total: number;
  unmatched: number;
  updated: number;
}

function toMatchSnapshot(this: any, received: unknown): void;
function toMatchSnapshot(
  this: any,
  propertyMatchers?: Record<string, unknown> | unknown[]
): void;
function toMatchInlineSnapshot(this: any, propertyMatchers?: Record<string, unknown> | unknown[]): void;
function toHaveSnapshot(this: any, key: string): void;

import { toMatchSnapshot, toMatchInlineSnapshot } from '@jest/snapshots';
import snapshot from 'jest-snapshot';

describe('Snapshot Testing', () => {
  describe('Basic Snapshots', () => {
    it('should match simple value snapshot', () => {
      const data = { name: 'John', age: 30 };
      expect(data).toMatchSnapshot();
    });

    it('should match snapshot with property matchers', () => {
      const data = { id: '123', created: '2024-01-01T00:00:00.000Z' };
      expect(data).toMatchSnapshot({
        id: expect.any(String),
        created: expect.any(String),
      });
    });

    it('should match snapshot with name', () => {
      const data = { items: [1, 2, 3] };
      expect(data).toMatchSnapshot('snapshot-name');
    });
  });

  describe('Inline Snapshots', () => {
    it('should match inline snapshot', () => {
      const result = { status: 'success', code: 200 };
      expect(result).toMatchInlineSnapshot({
        status: 'success',
        code: 200,
      });
    });
  });

  describe('Snapshot Updates', () => {
    it('should update snapshot', async () => {
      const data = { message: 'Hello' };
      await expect(data).toMatchSnapshot();
    });
  });

  describe('Object Snapshots', () => {
    it('should handle complex objects', () => {
      const user = {
        id: 1,
        name: 'Test User',
        email: 'test@example.com',
        profile: {
          avatar: 'https://example.com/avatar.png',
          bio: 'Test bio',
        },
        roles: ['admin', 'user'],
        settings: {
          theme: 'dark',
          notifications: true,
        },
      };
      expect(user).toMatchSnapshot();
    });

    it('should handle arrays', () => {
      const items = [
        { id: 1, name: 'Item 1' },
        { id: 2, name: 'Item 2' },
        { id: 3, name: 'Item 3' },
      ];
      expect(items).toMatchSnapshot();
    });
  });

  describe('Error Snapshots', () => {
    it('should capture error snapshot', () => {
      const error = new Error('Something went wrong');
      expect(error).toMatchSnapshot();
    });
  });

  describe('Async Snapshots', () => {
    it('should handle async snapshots', async () => {
      const data = await Promise.resolve({ async: true, timestamp: Date.now() });
      expect(data).toMatchSnapshot();
    });
  });
});

console.log('\n=== Snapshot Testing Complete ===');
console.log('Next: 08_Property_Based_Testing.ts');