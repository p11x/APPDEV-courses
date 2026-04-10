/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 06 Testing Frameworks Topic: 03_Mocha_Types Purpose: Mocha type definitions and test configuration Difficulty: intermediate UseCase: backend, web Version: TS 5.0+ Compatibility: Node.js 16+, Browsers Performance: fast Security: N/A */

declare function describe(blocks: string, description: string, fn: () => void): void;
declare function describe(description: string, fn: () => void): void;
declare function context(description: string, fn: () => void): void;
declare function it(description: string, action: (done: DoneCallback) => void): void;
declare function it(description: string, fn: () => void | Promise<void>): void;
declare function it(description: string, skip: boolean, fn: () => void | Promise<void>): void;
declare function specify(description: string, fn: () => void | Promise<void>): void;
declare function suite(description: string, fn: () => void): void;
declare function test(description: string, fn: () => void | Promise<void>): void;
declare function before(action: (done: DoneCallback) => void): void;
declare function before(action: () => void | Promise<string>): void;
declare function before(action: (done: DoneCallback) => void, description?: string): void;
declare function setup(action: (done: DoneCallback) => void): void;
declare function after(action: (done: DoneCallback) => void): void;
declare function after(action: () => void | Promise<void>): void;
declare function teardown(action: (done: DoneCallback) => void): void;
declare function beforeEach(action: (done: DoneCallback) => void): void;
declare function beforeEach(action: () => void | Promise<void>): void;
declare function suiteSetup(action: (done: DoneCallback) => void): void;
declare function afterEach(action: (done: DoneCallback) => void): void;
declare function suiteTeardown(action: (done: DoneCallback) => void): void;

type DoneCallback = (err?: Error) => void;

declare namespace Mocha {
  interface HookFunction {
    (fn: () => void | Promise<void>): Hook;
    (fn: (done: DoneCallback) => void): Hook;
  }

  interface Hook {
    (fn: () => void | Promise<string>): Hook;
    timeout(ms: number | string): Hook;
    enable(): Hook;
    disable(): Hook;
  }

  interface Test {
    callback: boolean;
    state: 'pending' | 'passed' | 'failed';
    async: boolean;
    fn: Function;
    aborted: boolean;
    duration: number;
    parent: Suite;
    timedOut: boolean;
    title: string;
    pending: boolean;

    reset(): Test;
    resetAsync(): Promise<Test>;
    run(): Promise<Test>;
    slow(ms: number): Test;
    timeout(ms: number | string): Test;
  }

  interface Suite {
    id: number;
    title: string;
    fn: Function;
    async: boolean;
    slow: number;
    timeout: string | number;
    parent: Suite | undefined;
    root: boolean;
    delayed: boolean;

    reset(): Suite;
    addSuite(suite: Suite): Suite;
    addTest(test: Test): Suite;
    pending: boolean;
    suites: Suite[];
    tests: Test[];

    beforeEach(action: HookFunction): this;
    afterEach(action: HookFunction): this;
    before(action: HookFunction): this;
    after(action: HookFunction): this;
    parentSuite(): Suite | undefined;
    getLog(): string;
    create(name: string, fn: () => void): Suite;
  }

  interface Reporter {
    (runner: Runner, options?: MochaOptions): void;
  }

  interface MochaOptions {
    bytes?: number;
    checkLeaks?: boolean;
    color?: boolean;
    compactor?(path: string): Promise<void>;
    delay?: boolean;
    dryRun?: boolean;
    exit?: boolean;
    extension?: string[];
    fgrep?: string;
    file?: string[];
    global?: string | string[];
    grep?: string | RegExp;
    invert?: boolean;
    jobs?: number;
    maxDiffSize?: number;
    parallel?: boolean;
    preserveSymlinks?: boolean;
    recursive?: boolean;
    reporter?: string | Reporter;
    reporterOption?: string | string[] | Record<string, unknown>;
    require?: string[];
    retry?: number | RetryFn;
    slow?: number;
    sort?: boolean;
    spec?: string[];
    timeout?: number | string;
    titlePattern?: string;
    trace?: boolean;
    ui?: string;
    usage?: boolean;
    version?: boolean;
    watch?: boolean;
    watchExtensions?: string[];
    watchIgnore?: string[] | Globals;
  }

  interface RetryFn {
    (suite: Suite, err: Error): number;
  }

  interface Globals {
    [key: string]: boolean | string;
  }

  interface Runnable {
    type: string;
    callback: boolean;
    pending: boolean;

    reset(fn?: Function): void;
    resetAsync(): Promise<void>;
    run(fn?: Function): void;
    slow(ms: number): void;
    timeout(ms: number | string): void;
  }

  interface Runner extends Runnable {
    suite: Suite;
    started: boolean;
    pending: boolean;
    failures: number;
    passes: number;
    parallelMode: 'runner' | 'lookup';

    appendOnly(): Runner;
    appendOnlyMatcher(pattern: string | RegExp): Runner;
    asyncRunnable(val: boolean): Runner;
    bail(val: boolean): Runner;
    color(val: boolean | 'always'): Runner;
    delay(val: boolean): Runner;
    enableGlobalTracking(enable: boolean): Runner;
    fullTrace(): Runner;
    globals(list: string | string[] | false): Runner;
    ignoreLeaks(ignore: boolean): Runner;
    inlineDeps(inline: boolean): Runner;
    jobs(jobs: number): Runner;
    parallelMode(mode: 'runner' | 'lookup'): Runner;
    prevError(err: Error | null): Runner;
    reLength(list?: false): Runner | number;
    run(fn?: (failures: number) => void): Runner;
    selecturedList(): string[];
    setTimeout(ms: number | string): Runner;
    slow(ms: number): Runner;
    sortEntities(val: boolean): Runner;
    test(options: TestOptions): Runner;
    unhandledHandler(fn: Function): Runner;
  }

  interface TestOptions {
    allowUncaught?: boolean;
    bail?: boolean;
    computedStyle?: Window['getComputedStyle'];
    dom?: Window;
    editor?: any;
    enableGlobalTracking?: boolean;
    fullTrace?: boolean;
    inlineDeps?: boolean;
    recorder?: Recorder;
    reporter?: string | Reporter;
    slow?: number;
  }

  interface Recorder {
    create(): Recorder;
    append(rec: Record<string, unknown>): Recorder;
    get(): Record<string, unknown>[];
    reset(): Recorder;
  }

  interface EventEmitter {
    on(event: 'pre-require', listener: (context: MochaInterface, file: string, options: MochaOptions) => void): this;
    on(event: 'post-require', listener: (context: MochaInterface, file: string, options: MochaOptions) => void): this;
    on(event: 'test', listener: (test: Test) => void): this;
    on(event: 'test end', listener: (test: Test) => void): this;
    on(event: 'hook', listener: (hook: Hook) => void): this;
    on(event: 'hook end', listener: (hook: Hook) => void): this;
    on(event: 'pass', listener: (test: Test) => void): this;
    on(event: 'fail', listener: (test: Test, err: Error) => void): this;
    on(event: 'pending', listener: (test: Test) => void): this;
    on(event: 'suite', listener: (suite: Suite) => void): this;
    on(event: 'suite end', listener: (suite: Suite) => void): this;
    on(event: 'ready', listener: () => void): this;
    on(event: 'start', listener: () => void): this;
    on(event: 'Included', listener: (file: string) => void): this;
    on(event: 'r', listener: () => void): this;
    emit(event: string, ...args: unknown[]): boolean;
  }

  interface MochaInterface extends EventEmitter {
    (options?: MochaOptions): MochaInterface;
    create(options?: MochaOptions): MochaInterface;
    run(endProcess?: boolean, callback?: (failures: number, errors: number) => void): Runner;
    setUI(name: string): MochaInterface;
    setDefaultEndian(le: boolean): void;
    setupInterface(interfaceMode: 'bdd' | 'tdd' | 'qunit' | 'exports'): void;
    getOptions(key: string): unknown;
    listLeaks(): void;
    updateGlobalTracking(): void;
    isFunction(fn: unknown): boolean;
    stringify(obj: unknown): string;
    generateTest(runnable: Runnable): Test;
    markPending(runnable: Runnable): void;
    processawnables(suite: Suite): Runnable[];
    getRunnable(id: number): Runnable;
    setRunnable(id: number, runnable: Runnable): MochaInterface;
    handleNewRunnable(runnable: Runnable): void;
    addErrorHandler(handler: (err: Error) => void): void;
  }

  export interface Stats {
    suites: number;
    tests: number;
    passes: number;
    pending: number;
    failures: number;
    startDate: number;
    endDate: number;
    duration: number;
  }
}

declare const Mocha: Mocha.MochaInterface;

describe('Mocha Types', () => {
  describe('Test Structure', () => {
    it('should run basic test', () => {
      const result = 2 + 2;
      expect(result).toBe(4);
    });

    it('should handle async test', async () => {
      const data = await Promise.resolve({ id: 1, name: 'Test' });
      expect(data.name).toBe('Test');
    });

    it('should handle done callback', function(done) {
      setTimeout(() => {
        expect(true).toBe(true);
        done();
      }, 100);
    });

    it.skip('should skip this test', () => {
      expect(true).toBe(false);
    });

    it.only('should run only this test', () => {
      expect(true).toBe(true);
    });
  });

  describe('Async Testing', () => {
    it('should work with promises', async () => {
      const value = await Promise.resolve('async value');
      expect(value).toBe('async value');
    });

    it('should work with async/await', async () => {
      async function fetchData() {
        return { data: 'fetched' };
      }
      const result = await fetchData();
      expect(result.data).toBe('fetched');
    });
  });

  describe('Setup and Teardown', () => {
    before(() => {
      console.log('Before all tests');
    });

    after(() => {
      console.log('After all tests');
    });

    beforeEach(() => {
      console.log('Before each test');
    });

    afterEach(() => {
      console.log('After each test');
    });
  });

  describe('BDD Interface', () => {
    describe('Nested describe blocks', () => {
      it('should work with nested structure', () => {
        expect(true).toBe(true);
      });
    });

    context('context synonym for describe', () => {
      it('works like describe', () => {
        expect(true).toBe(true);
      });
    });

    specify('specify synonym for it', () => {
      expect(true).toBe(true);
    });
  });
});

console.log('\n=== Mocha Types Complete ===');
console.log('Next: 04_Vitest_Types.ts');