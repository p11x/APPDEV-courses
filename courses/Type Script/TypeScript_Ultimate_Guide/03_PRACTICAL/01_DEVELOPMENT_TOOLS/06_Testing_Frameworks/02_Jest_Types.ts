/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 06 Testing Frameworks Topic: 02_Jest_Types Purpose: Jest type definitions and configuration types Difficulty: intermediate UseCase: web, backend Version: TS 5.0+ Compatibility: Node.js 18+, Browsers Performance: fast Security: N/A */

declare function describe(name: string, fn: () => void): void;
declare function it(name: string, fn: () => void, timeout?: number): void;
declare function test(name: string, fn: () => void, timeout?: number): void;
declare function beforeEach(fn: () => void): void;
declare function afterEach(fn: () => void): void;
declare function beforeAll(fn: () => void): void;
declare function afterAll(fn: () => void): void;

declare namespace jest {
  interface Matchers<R> {
    toBe(expected: unknown): R;
    toEqual(expected: unknown): R;
    toBeNull(): R;
    toBeUndefined(): R;
    toBeDefined(): R;
    toBeTruthy(): R;
    toBeFalsy(): R;
    toContain(item: unknown): R;
    toThrow(error?: string | Error): R;
    toHaveBeenCalled(): R;
    toHaveBeenCalledTimes(times: number): R;
    toHaveBeenCalledWith(...args: unknown[]): R;
    toHaveReturned(): R;
    toHaveReturnedWith(value: unknown): R;
    toHaveLength(len: number): R;
    toMatchSnapshot(message?: string): R;
    toMatchInlineSnapshot(snapshot?: string): R;
    toThrowErrorMatchingSnapshot(message?: string): R;
    toThrowErrorMatchingInlineSnapshot(snapshot?: string): R;
  }

  interface Expect {
    (actual: unknown): Matchers<any>;
    any(expected: any): any;
    arrayContaining(expected: unknown[]): any;
    objectContaining(expected: Record<string, unknown>): any;
    stringContaining(str: string): any;
    stringMatching(pattern: string | RegExp): any;
    not: any;
    resololvers: any;
  }

  interface JestMock<T extends (...args: any[]) => any> extends MockInstance<T> {
    (...args: Parameters<T>): ReturnType<T>;
    mock: MockContext<T>;
    getMockName(): string;
    mockName(name: string): this;
    mockImplementation(fn: (...args: Parameters<T>) => ReturnType<T>): this;
    mockImplementationOnce(fn: (...args: Parameters<T>) => ReturnType<T>): this;
    mockReturnValue(value: ReturnType<T>): this;
    mockReturnValueOnce(value: ReturnType<T>): this;
    mockResolvedValue(value: Awaited<ReturnType<T>>): this;
    mockResolvedValueOnce(value: Awaited<ReturnType<T>>): this;
    mockRejectedValue(error: Error): this;
    mockRejectedValueOnce(error: Error): this;
  }

  interface MockContext<T extends (...args: any[]) => any> {
    calls: Arguments<Parameters<T>>;
    instances: T[];
    lastCall: Arguments<Parameters<T>> | undefined;
    results: Array<{ value: Awaited<ReturnType<T>> }>;
  }

  interface MockInstance<T extends (...args: any[]) => any> {
    _isMockFunction: boolean;
    _proto__: MockInstance<T>;
    getMockImplementation(): ((...args: Parameters<T>) => ReturnType<T>) | undefined;
  }

  type Arguments<T extends unknown[]> = T;
  type Awaited<T> = T extends Promise<infer U> ? U : T;

  interface GlobalSetupFn {
    (): Promise<void>;
  }

  interface GlobalTeardownFn {
    (): Promise<void>;
  }

  interface Config {
    automock?: boolean;
    bail?: boolean | number;
    cacheDirectory?: string;
    clearMocks?: boolean;
    coverageProvider?: 'v8' | 'babel';
    collectCoverageFrom?: string[];
    coveragePathIgnorePatterns?: string[];
    coverageThreshold?: Record<string, unknown>;
    errorOnDeprecated?: boolean;
    expand?: boolean;
    filter?: string;
    forceCoverageMatch?: string[];
    globals?: Record<string, unknown>;
    globalSetup?: string;
    globalTeardown?: string;
    haste?: {
      computeSha1?: boolean;
      enableSymlinks?: boolean;
      forceNodeFilesystemAPI?: boolean;
      ignore?: string[];
      mocks?: string;
      platforms?: string[];
      provider?: 'jest' | 'v8';
      rootDir?: string;
      roots?: string[];
      sha1?: string;
      defaultPlatform?: string | null;
      modules?: string[];
    };
    listFiles?: string;
    maxConcurrency?: number;
    moduleDirectories?: string[];
    moduleFileExtensions?: string[];
    moduleNameMapper?: Record<string, string | string[]>;
    modulePathIgnorePatterns?: string[];
    modulePaths?: string[];
    name?: string;
    preset?: string | null;
    projects?: string[] | string;
    reporters?: (string | [string, Record<string, unknown>])[];
    displayName?: string;
    testLocationInResults?: boolean;
    roots?: string[];
    setupFiles?: string[];
    setupFilesAfterEnv?: string[];
    skipPackageJson?: boolean;
    skipTsConfig?: boolean;
    snapshotResolver?: string;
    snapshotSerializers?: string[];
    testEnvironment?: string;
    testEnvironmentOptions?: Record<string, unknown>;
    testMatch?: (string | string[])[];
    testPathIgnorePatterns?: string[];
    testNamePattern?: string;
    testRunner?: string;
    testURL?: string;
    timers?: 'real' | 'fake' | 'modern' | 'legacy';
    transform?: Record<string, string | [string, Record<string, unknown>]>;
    transformIgnorePatterns?: string[];
    unmockedModulePathPatterns?: string[];
    useStderr?: boolean;
    verbose?: boolean;
    watchPlugin?: string;
    watchPathIgnorePatterns?: string[];
    workerIdleMemoryLimit?: string | number;
    workerThreads?: boolean;
  }
}

declare const expect: jest.Expect;
declare const jest: typeof jest;

interface JestGlobal {
  describe: typeof describe;
  it: typeof it;
  test: typeof test;
  beforeEach: typeof beforeEach;
  afterEach: typeof afterEach;
  beforeAll: typeof beforeAll;
  afterAll: typeof afterAll;
  expect: typeof expect;
  jest: typeof jest;
}

declare global {
  const describe: typeof describe;
  const it: typeof it;
  const test: typeof test;
  const beforeEach: typeof beforeEach;
  const afterEach: typeof afterEach;
  const beforeAll: typeof beforeAll;
  const afterAll: typeof afterAll;
  const expect: typeof expect;
  const jest: typeof jest;
}

export {};

describe('Jest Types', () => {
  describe('Basic Matchers', () => {
    it('should demonstrate toBe matcher', () => {
      const result = 1 + 1;
      expect(result).toBe(2);
    });

    it('should demonstrate toEqual matcher', () => {
      const obj = { name: 'test', value: 42 };
      expect(obj).toEqual({ name: 'test', value: 42 });
    });

    it('should demonstrate toHaveLength matcher', () => {
      const arr = [1, 2, 3];
      expect(arr).toHaveLength(3);
    });

    it('should demonstrate toMatchSnapshot matcher', () => {
      const data = { id: 1, name: 'Test User', email: 'test@example.com' };
      expect(data).toMatchSnapshot();
    });
  });

  describe('Async Matchers', () => {
    it('should demonstrate toResolveValue matcher', async () => {
      const promise = Promise.resolve('成功');
      await expect(promise).resolves.toBe('成功');
    });

    it('should demonstrate toRejectError matcher', async () => {
      const failingPromise = Promise.reject(new Error('Failed'));
      await expect(failingPromise).rejects.toThrow('Failed');
    });
  });

  describe('Mock Types', () => {
    it('should demonstrate jest.fn', () => {
      const mockFn = jest.fn((x: number) => x * 2);
      expect(mockFn(5)).toBe(10);
      expect(mockFn).toHaveBeenCalledTimes(1);
    });

    it('should demonstrate mockReturnValue', () => {
      const mock = jest.fn();
      mock.mockReturnValue(42);
      expect(mock()).toBe(42);
    });

    it('should demonstrate mockResolvedValue', async () => {
      const mock = jest.fn();
      mock.mockResolvedValue('async result');
      await expect(mock()).resolves.toBe('async result');
    });

    it('should demonstrate mockRejectedValue', async () => {
      const mock = jest.fn();
      mock.mockRejectedValue(new Error('Error occurred'));
      await expect(mock()).rejects.toThrow('Error occurred');
    });
  });

  describe('Spy Types', () => {
    const obj = {
      getValue: () => 'original',
      multiply: (a: number, b: number) => a * b,
    };

    it('should demonstrate jest.spyOn', () => {
      const spy = jest.spyOn(obj, 'getValue');
      obj.getValue();
      expect(spy).toHaveBeenCalled();
      spy.mockReturnValue('mocked');
      expect(obj.getValue()).toBe('mocked');
      spy.mockRestore();
    });
  });
});

console.log('\n=== Jest Types Complete ===');
console.log('Next: 03_Mocha_Types.ts');