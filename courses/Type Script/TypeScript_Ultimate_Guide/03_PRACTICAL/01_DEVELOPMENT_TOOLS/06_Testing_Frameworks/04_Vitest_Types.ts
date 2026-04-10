/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 06 Testing Frameworks Topic: 04_Vitest_Types Purpose: Vitest type definitions and configuration Difficulty: intermediate UseCase: web, backend Version: TS 5.0+ Compatibility: Node.js 16+, Browsers Performance: ultra-fast Security: N/A */

declare namespace Vi {
  type TestType = 'test' | 'benchmark';
  type TestTypeForExtension = TestType | 'vi';

  interface InternalTask extends Task {
    type: TestType;
    extension: TestTypeForExtension;
    result: TaskResult | undefined;
    errors: Error[];
  }

  type TaskResult = {
    state: 'passed' | 'failed' | 'skipped' | 'pending';
    start: number;
      end?: number;
      errors?: TestError[];
    };

    type TestError = {
      message: string;
      stack?: string;
      name: string;
    };

    interface Task {
      id: string;
      name: string;
      file?: File;
      meta?: Record<string, unknown>;
      queueSuite?: Suite;
      timeout?: number;
    }

    interface Suite {
      name: string;
      type: 'suite';
      mode: 'run' | 'skip' | 'only';
      fn: (() => void | Promise<void>) | undefined;
      children: Suite[] | Test[];
      task: InternalSuite;
      start?: number;
      end?: number;
      retry?: number;
    }

    interface InternalSuite extends InternalTask {
      type: 'suite';
      tasks: InternalTask[];
    }

    interface File {
      path: string;
      name: string;
    }

    interface Matchers<T> {
      toBe(expected: T): void;
      toEqual(expected: T): void;
      toBeNull(): void;
      toBeUndefined(): void;
      toBeDefined(): void;
      toBeTruthy(): void;
      toBeFalsy(): void;
      toContain(item: unknown): void;
      toHaveProperty(propertyPath: string | string[], value?: unknown): void;
      toMatch(regexp: RegExp): void;
      toMatchObject(object: Record<string, unknown>): void;
      toThrow(error?: string | Error | ErrorConstructor): void;
      toBeGreaterThan(expected: number): void;
      toBeGreaterThanOrEqual(expected: number): void;
      toBeLessThan(expected: number): void;
      toBeLessThanOrEqual(expected: number): void;
      toBeCloseTo(expected: number, precision?: number): void;
      toHaveLength(len: number): void;
      toMatchSnapshot(message?: string): void;
      toMatchInlineSnapshot(snapshot?: string): void;
      toThrowErrorMatchingSnapshot(): void;
      toThrowErrorMatchingInlineSnapshot(): void;
    }

    interface ViMatchers<T> extends Matchers<T> {
      not: ViMatchers<T>;
    }

    interface ChaiAssertion<T> {
      to: ChaiAssertion<T>;
      be: ChaiAssertion<T>;
      been: ChaiAssertion<T>;
      have: ChaiAssertion<T>;
      with: ChaiAssertion<T>;
      at: ChaiAssertion<T>;
      a: ChaiAssertion<T>;
      an: ChaiAssertion<T>;
      but: ChaiAssertion<T>;
      also: ChaiAssertion<T>;
      does: ChaiAssertion<T>;
      deep: ChaiAssertion<T>;
      nested: ChaiAssertion<T>;
      ordered: ChaiAssertion<T>;
      same: ChaiAssertion<T>;
      include: ChaiAssertion<T>;
      includes: ChaiAssertion<T>;
      contain: ChaiAssertion<T>;
      contains: ChaiAssertion<T>;
      equal: ChaiAssertion<T>;
      equals: ChaiAssertion<T>;
      eql: ChaiAssertion<T>;
      eqls: ChaiAssertion<T>;
      reflect: ChaiAssertion<T>;
      throws: ChaiAssertion<T>;
      satisfy: ChaiAssertion<T>;
      satisfies: ChaiAssertion<T>;

      then<T>(fn: () => T): void;
      then<T>(fn: () => T, expectations: string | string[]): void;
      fail(actual: unknown, expected: unknown, message?: string, operator?: string): void;

      true: void;
      false: void;
      null: void;
      NaN: void;
      undefined: void;
      Infinity: void;
      -Infinity: void;

      one: ChaiAssertion<number>;
      two: ChaiAssertion<number>;
      three: ChaiAssertion<number>;
      four: ChaiAssertion<number>;
      five: ChaiAssertion<number>;
    }

    type Expect = <T>(actual: T) => ViMatchers<T> & ChaiAssertion<T>;

    type Assert = {
      (actual: unknown, message?: string): void;
      fail(actual?: unknown, expected?: unknown, message?: string, operator?: string): void;
      isOk(value: unknown, message?: string): void;
      isNotOk(value: unknown, message?: string): void;
      equal(actual: unknown, expected: unknown, message?: string): void;
      notEqual(actual: unknown, expected: unknown, message?: string): void;
      strictEqual(actual: unknown, expected: unknown, message?: string): void;
      notStrictEqual(actual: unknown, expected: unknown, message?: string): void;
      deepEqual(actual: unknown, expected: unknown, message?: string): void;
      notDeepEqual(actual: unknown, expected: unknown, message?: string): void;
      isTrue(value: unknown, message?: string): void;
      isFalse(value: unknown, message?: string): void;
      isNull(value: unknown, message?: string): void;
      isNotNull(value: unknown, message?: string): void;
      isUndefined(value: unknown, message?: string): void;
      isDefined(value: unknown, message?: string): void;
      isFunction(value: unknown, message?: string): void;
      isNotFunction(value: unknown, message?: string): void;
      isObject(value: unknown, message?: string): void;
      isNotObject(value: unknown, message?: string): void;
      isArray(value: unknown, message?: string): void;
      isNotArray(value: unknown, message?: string): void;
      isString(value: unknown, message?: string): void;
      isNotString(value: unknown, message?: string): void;
      isNumber(value: unknown, message?: string): void;
      isNotNumber(value: unknown, message?: string): void;
      isFinite(value: unknown, message?: string): void;
      isNotFinite(value: unknown, message?: string): void;
      isEmpty(value: unknown, message?: string): void;
      isNotEmpty(value: unknown, message?: string): void;
      isNaN(value: unknown, message?: string): void;
      isNotNaN(value: unknown, message?: string): void;
      isTicket(value: unknown, message?: string): void;
      isBoolean(value: unknown, message?: string): void;
      typeof(instance: unknown, type: string, message?: string): void;
    };

    interface Mock<T extends (...args: unknown[]) => unknown> {
      (...args: Parameters<T>): ReturnType<T>;
      mock: MockContext<T>;
      mockName(name: string): Mock<T>;
      getMockName(): string;
      mockImplementation(fn: (...args: Parameters<T>) => ReturnType<T>): Mock<T>;
      mockImplementationOnce(fn: (...args: Parameters<T>) => ReturnType<T>): Mock<T>;
      mockReturnValue(value: ReturnType<T>): Mock<T>;
      mockReturnValueOnce(value: ReturnType<T>): Mock<T>;
      mockResolvedValue(value: Awaited<ReturnType<T>>): Mock<T>;
      mockResolvedValueOnce(value: Awaited<ReturnType<T>>): Mock<T>;
      mockRejectedValue(value: Error): Mock<T>;
      mockRejectedValueOnce(value: Error): Mock<T>;
      mockReset(): Mock<T>;
      mockClear(): Mock<T>;
      mockRestore(): Mock<T>;
    }

    interface MockContext<T extends (...args: unknown[]) => unknown> {
      calls: Arguments<Parameters<T>>;
      instances: T[];
      lastCall: Arguments<Parameters<T>> | undefined;
      results: Array<{ value: ReturnType<T> }>;
    }

    type Arguments<T extends unknown[]> = T;

    type Awaited<T> = T extends Promise<infer U> ? U : T;

    interface Spy<T extends (...args: unknown[]) => unknown> extends Mock<T> {
      getMockImplementation(): ((...args: Parameters<T>) => ReturnType<T>) | undefined;
      mock: MockContext<T>;
    }

    interface SpyInstance<T extends (...args: unknown[]) => unknown> extends Spy<T> {}

    interface Timeout extends Number {
      id: string;
    }
  }

  interface UserConfig {
    allowOnly?: boolean;
    alias?: Record<string, string | string[]>;
    deps?: {
      inline?: string[];
      external?: (string | RegExp)[];
      transformerTransform?: {
        inline?: string;
      };
    include?: string[];
    exclude?: string[];
    };
    environmentMatchGlobs?: [string, string][];
    globals?: string[];
    cache?: boolean;
    cacheDir?: string;
    clearMocks?: boolean;
    commit?: boolean;
    coverage?: boolean | string | string[];
    coverageOptions?: string;
    depsInline?: string[];
    env?: Record<string, string>;
    exclude?: string[];
    extendMocks?: boolean[];
    fakeTimers?: AdvancedFakeTimersConfig;
    filter?: string;
    globalSetup?: string;
    globalTeardown?: string;
    hookglobal?: boolean;
    include?: string[];
    isolate?: boolean;
    logHeapUsage?: boolean;
    mode?: 'runner' | 'spawn' | 'forks';
    open?: boolean;
    passWithNoTests?: boolean;
    pools?: {
      fork?: {
        numberOfPools?: number;
      };
      vmForks?: {
        numberOfPools?: number;
      };
    };
    project?: string[];
    reporters?: (string | [string, Record<string, unknown>])[];
    require?: string[];
    resolveSnapshotPath?: (path: { snapshotPath: string; testPath: string }) => string;
    root?: string;
    shards?: {
      total: number;
      index: number;
    };
   silent?: boolean;
    snapshotFormat?: string[];
    testTimeout?: number;
    typecheck?: boolean;
    typecheckOptions?: string;
    ui?: string;
    unhandledErrors?: boolean;
    update?: boolean;
    verbose?: boolean;
    viewport?: {
      width?: number;
      height?: number;
    };
    watch?: boolean;
    watchIgnore?: string[] | { paths: string[]; gitignore?: boolean };
  }

  interface AdvancedFakeTimersConfig {
    enableGlobally?: boolean;
    autoRun?: boolean;
    now?: number | Date;
    loopLimit?: number;
    maxNextTickLoops?: number;
    maxTimersPristine?: number;
    shouldAdvanceTime?: boolean;
    advanceTimersBy?: number;
    shouldAdvanceTimeDefault?: () => boolean;
  }
}

declare function beforeEach(fn: (global: any) => void | Promise<void>): void;
declare function afterEach(fn: (global: any) => void | Promise<void>): void;
declare function describe(name: string | Record<string, unknown>, fn: () => void): void;
declare function describe.each(table: (string | number)[][] | Record<string, unknown>[]): (
  title: string,
  fn: (title: string, ...args: unknown[]) => void
) => void;
declare function describeSkip(title: string, fn: () => void): void;
declare function describeOnly(title: string, fn: () => void): void;
declare function test(name: string, fn: () => void | Promise<void>): void;
declare function test(name: Record<string, unknown>, fn: () => void | Promise<void>): void;
declare function test.each(t) (table: (string | number)[][] | Record<string, unknown>[]): (
  title: string,
  fn: (title: string, ...args: unknown[]) => void
) => void;
declare function testSkip(title: string, fn: () => void | Promise<void>): void;
declare function testOnly(title: string, fn: () => void | Promise<void>): void;
declare function it(name: string, fn: () => void | Promise<void>): void;
declare function it(name: Record<string, unknown>, fn: () => void | Promise<void>): void;
declare function it.each(t) (table: (string | number)[][] | Record<string, unknown>[]): (
  title: string,
  fn: (title: string, ...args: unknown[]) => void
) => void;
declare function itSkip(title: string, fn: () => void | Promise<void>): void;
declare function itOnly(title: string, fn: () => void | Promise<void>): void;

declare const expect: Vi.Expect;
declare const assert: Vi.Assert;
declare const vi: typeof Vi;

describe('Vitest Types', () => {
  describe('Basic Expect', () => {
    expect(1 + 1).toBe(2);
    expect({ name: 'test' }).toEqual({ name: 'test' });
    expect(null).toBeNull();
    expect(undefined).toBeUndefined();
    expect(true).toBeTruthy();
    expect(false).toBeFalsy();

    expect([1, 2, 3]).toContain(2);
    expect([1, 2, 3]).toHaveLength(3);
    expect('hello').toMatch(/hello/);
  });

  describe('Async Expect', () => {
    it('should handle async expect', async () => {
      await expect(Promise.resolve(42)).resolves.toBe(42);
    });

    it('should handle rejected promise', async () => {
      await expect(Promise.reject(new Error('fail'))).rejects.toThrow('fail');
    });
  });

  describe('Vi Mock Functions', () => {
    it('should create mock function', () => {
      const mockFn = vi.fn((x: number) => x * 2);
      expect(mockFn(5)).toBe(10);
      expect(mockFn).toHaveBeenCalledOnce();
    });

    it('should mock return values', () => {
      const mock = vi.fn();
      mock.mockReturnValueOnce(1).mockReturnValueOnce(2);
      expect(mock()).toBe(1);
      expect(mock()).toBe(2);
    });

    it('should mock implementations', () => {
      const mock = vi.fn();
      mock.mockImplementationOnce((x: number) => x + 1).mockImplementation((x: number) => x * 2);
      expect(mock(5)).toBe(6);
      expect(mock(5)).toBe(10);
    });
  });

  describe('Vi Spies', () => {
    const obj = {
      method: () => 'original',
    };

    it('should create spy', () => {
      const spy = vi.spyOn(obj, 'method');
      obj.method();
      expect(spy).toHaveBeenCalled();
      spy.mockReturnValue('mocked');
      expect(obj.method()).toBe('mocked');
      spy.mockRestore();
    });
  });

  describe('Vi Timers', () => {
    it('should use fake timers', () => {
      vi.useFakeTimers();
      vi.advanceTimersByTime(1000);
      expect(vi.getMockedSystemFn()).toBeDefined();
    });
  });
});

console.log('\n=== Vitest Types Complete ===');
console.log('Next: 05_Testing_Library.ts');