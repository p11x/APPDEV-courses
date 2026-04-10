/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 06 Testing Frameworks Topic: 06_Spy_Mock_Types Purpose: Spy and mock type definitions for testing Difficulty: intermediate UseCase: web, backend Version: TS 5.0+ Compatibility: Node.js 16+, Browsers Performance: fast Security: N/A */

declare namespace SpyMockTypes {
  interface SpyCall<T extends (...args: unknown[]) => unknown> {
    called: boolean;
    calledOnce: boolean;
    calledTwice: boolean;
    calledThrice: boolean;
    callCount: number;
    calledWith(...args: Parameters<T>): boolean;
    calledOnceWith(...args: Parameters<T>): boolean;
    returned(value: ReturnType<T>): boolean;
    threw(error?: Error | string): boolean;
    thisArgs: ThisParameterType<T>[];
    args: Arguments<Parameters<T>>[];
    lastCall: CallRecord<T>;
    firstCall: CallRecord<T>;
    calls: CallRecord<T>[];
  }

  type CallRecord<T extends (...args: unknown[]) => unknown> = {
    thisArg: ThisParameterType<T>;
    args: Arguments<Parameters<T>>;
    returnValue: ReturnType<T>;
  };

  type Arguments<T extends unknown[]> = T;

  type ThisParameterType<T> = T extends (this: infer U, ...args: unknown[]) => unknown ? U : unknown;

  interface Spy<T extends (...args: unknown[]) => unknown> extends Function {
    (this: ThisParameterType<T>, ...args: Parameters<T>): ReturnType<T>;
    called: boolean;
    calledOnce: boolean;
    calledTwice: boolean;
    calledThrice: boolean;
    callCount: number;
    calledWith(...args: Parameters<T>): boolean;
    calledOnceWith(...args: Parameters<T>): boolean;
    returned(value: ReturnType<T>): boolean;
    threw(error?: Error | string): boolean;
    thisArgs: ThisParameterType<T>[];
    args: Arguments<Parameters<T>>[];
    lastCall: CallRecord<T>;
    firstCall: CallRecord<T>;
    calls: CallRecord<T>[];
    getMockName(): string;
    getLastCall(): CallRecord<T> | undefined;
    getCalls(): CallRecord<T>[];
    resetAll(): void;
    restore(): void;
    mockImplementation(fn: (...args: Parameters<T>) => ReturnType<T>): Spy<T>;
    mockImplementationOnce(fn: (...args: Parameters<T>) => ReturnType<T>): Spy<T>;
    mockReturnValue(value: ReturnType<T>): Spy<T>;
    mockReturnValueOnce(value: ReturnType<T>): Spy<T>;
    mockResolvedValue(value: Awaited<ReturnType<T>>): Spy<T>;
    mockResolvedValueOnce(value: Awaited<ReturnType<T>>): Spy<T>;
    mockRejectedValue(error: Error): Spy<T>;
    mockRejectedValueOnce(error: Error): Spy<T>;
    mockName(name: string): Spy<T>;
    withValues(...args: Parameters<T>): Spy<T>;
    reset(): void;
    clear(): void;
  }

  interface Mock<T extends Record<string, unknown>> {
    mockName(name?: string): Mock<T>;
    mockImplementation(fn: (...args: unknown[]) => unknown): Mock<T>;
    mockImplementationOnce(fn: (...args: unknown[]) => unknown): Mock<T>;
    mockReturnValue(value: unknown): Mock<T>;
    mockReturnValueOnce(value: unknown): Mock<T>;
    mockResolvedValue(value: unknown): Mock<T>;
    mockResolvedValueOnce(value: unknown): Mock<T>;
    mockRejectedValue(error: Error): Mock<T>;
    mockRejectedValueOnce(error: Error): Mock<T>;
    mockReset(): Mock<T>;
    mockClear(): Mock<T>;
    mockRestore(): Mock<T>;
    spy: unknown;
  }

  type Awaited<T> = T extends Promise<infer U> ? U : T;

  type Stub<T extends (...args: unknown[]) => unknown> = Spy<T> & {
    then: (onfulfilled: (value: ReturnType<T>) => void) => Stub<T>;
    catch: (onrejected: (reason: Error) => void) => Stub<T>;
    finally: (onfinally: () => void) => Stub<T>;
  };

  interface ProxyMock<T extends (...args: unknown[]) => unknown> {
    returns(value: ReturnType<T>): ProxyMock<T>;
    returnsOnce(value: ReturnType<T>): ProxyMock<T>;
    throws(error: Error): ProxyMock<T>;
    throwsOnce(error: Error): ProxyMock<T>;
    resolves(value: Awaited<ReturnType<T>>): ProxyMock<T>;
    resolvesOnce(value: Awaited<ReturnType<T>>): ProxyMock<T>;
    withArgs(...args: Parameters<T>): ProxyMock<T>;
    defaultReturns(value: ReturnType<T>): ProxyMock<T>;
    firstInvocation: CallRecord<T>;
    nthCall(n: number): CallRecord<T>;
    lastCall: CallRecord<T>;
    allCalls: CallRecord<T>[];
  }

  interface SandboxedSpy<T extends (...args: unknown[]) => unknown> extends Spy<T> {
    (this: ThisParameterType<T>, ...args: Parameters<T>): ReturnType<T>;
    callThrough(): SandboxedSpy<T>;
    throwThrough(): SandboxedSpy<T>;
    callfake(fn: (...args: Parameters<T>) => ReturnType<T>): SandboxedSpy<T>;
    defaultCallThrough(): () => Spy<T>;
    ignoreGetters(): Spy<T>;
    get(): unknown;
    set(value: unknown): Spy<T>;
    key(str: string): Spy<T>;
  }

  interface StubResolving<T extends (...args: unknown[]) => unknown> extends Spy<T> {
    returns(obj: Record<keyof T, unknown>): StubResolving<T>;
    resolves(obj: unknown): StubResolving<T>;
  }

  interface ProxyConstructor {
    new <T extends (...args: unknown[]) => unknown>(
      target: T,
      behavior?: ProxyBehavior<T>
    ): Spy<T>;
    <T extends (...args: unknown[]) => unknown>(
      target: T,
      behavior: ProxyBehavior<T>
    ): Spy<T>;
  }

  interface ProxyBehavior<T extends (...args: unknown[]) => unknown> {
    apply?(target: T, thisArg: ThisParameterType<T>, args: Parameters<T>): ReturnType<T>;
    construct?(
      target: T,
      args: Arguments<Parameters<T>>,
      newTarget: new (...args: Parameters<T>) => ReturnType<T>
    ): ReturnType<T>;
    defineProperty?(
      target: T,
      property: string | symbol,
      descriptor: PropertyDescriptor
    ): boolean;
    deleteProperty?(target: T, property: string | symbol): boolean;
    get?(target: T, property: string | symbol, receiver: unknown): unknown;
    getOwnPropertyDescriptor?(
      target: T,
      property: string | symbol
    ): PropertyDescriptor | undefined;
    getPrototypeOf?(target: T): object | null;
    has?(target: T, property: string | symbol): boolean;
    isExtensible?(target: T): boolean;
    preventExtensions?(target: T): boolean;
    set?(target: T, property: string | symbol, value: unknown, receiver: unknown): boolean;
    setPrototypeOf?(target: T, prototype: object | null): boolean;
  }

  interface MatcherState {
    actual: unknown;
    expected: unknown;
    name: string;
    message: () => string;
    pass: boolean;
  }

  interface CustomMockable {
    (): unknown;
    mockName(name: string): CustomMockable;
  }

  type Matcher = {
    (state: MatcherState): void | boolean | string;
    (state: MatcherState): void | boolean;
  };

  type ExpectExtend = {
    <T>(actual: T): {
      calls: Matcher;
      called: Matcher;
      calledOnce: Matcher;
      calledOnceWith: Matcher;
      calledWith: Matcher;
      returnValues: Matcher;
      threw: Matcher;
    };
  };
}

function createSpy<T extends (...args: unknown[]) => unknown>(
  fn?: T | string
): SpyMockTypes.Spy<T>;
function createStub<T extends Record<string, unknown>>(
  obj?: T
): SpyMockTypes.Mock<T>;
function createMock<T extends (...args: unknown[]) => unknown>(
  fn?: T
): SpyMockTypes.Spy<T>;
function verify<T extends (...args: unknown[]) => unknown>(
  spy: SpyMockTypes.Spy<T>
): SpyMockTypes.ProxyMock<T>;

import { spy, stub, createSpy, createStub, mock, verify } from 'sinon';

describe('Spy Mock Types', () => {
  describe('Basic Spy', () => {
    it('should create spy', () => {
      const fn = spy();
      fn(1, 2, 3);
      expect(fn.calledOnce).toBe(true);
      expect(fn.calledWith(1, 2, 3)).toBe(true);
      expect(fn.firstCall.args).toEqual([1, 2, 3]);
    });

    it('should track calls', () => {
      const fn = spy();
      fn(1);
      fn(2);
      fn(3);
      expect(fn.callCount).toBe(3);
      expect(fn.getCalls().length).toBe(3);
    });
  });

  describe('Stub', () => {
    it('should create stub with return value', () => {
      const myStub = stub().returns(42);
      expect(myStub()).toBe(42);
    });

    it('should throw error', () => {
      const myStub = stub().throws(new Error('Failure'));
      expect(() => myStub()).toThrow('Failure');
    });

    it('should call fake', () => {
      const myStub = stub().callsFake((x: number) => x * 10);
      expect(myStub(5)).toBe(50);
    });
  });

  describe('Mock', () => {
    const obj = {
      method: () => 'original',
    };

    it('should create mock', () => {
      const myMock = mock(obj);
      myMock.expects('method').once().returns('mocked');
      expect(obj.method()).toBe('mocked');
      myMock.verify();
      myMock.restore();
    });
  });

  describe('Spy On Object', () => {
    it('should spy on object method', () => {
      const obj = {
        greet: (name: string) => `Hello, ${name}!`,
      };

      const greetSpy = spy(obj, 'greet');
      obj.greet('World');
      expect(greetSpy.calledOnce).toBe(true);
      greetSpy.restore();
    });
  });

  describe('With Args', () => {
    it('should handle different args', () => {
      const stubFn = stub().withArgs(1).returns('one').withArgs(2).returns('two');
      expect(stubFn(1)).toBe('one');
      expect(stubFn(2)).toBe('two');
    });
  });

  describe('Verify', () => {
    it('should verify expectations', () => {
      const mySpy = spy();
      mySpy(1, 2);
      verify(mySpy).calledOnce();
      verify(mySpy).calledWith(1, 2);
    });

    it('should verify call order', () => {
      const mySpy1 = spy();
      const mySpy2 = spy();
      
      mySpy1();
      mySpy2();
      
      verify(mySpy1).calledBefore(mySpy2);
    });
  });

  describe('Sinon.match', () => {
    it('should use matchers', () => {
      const object = { a: 1, b: 2 };
      const matcher = sinon.match({ a: 1 });
      expect(matcher.test(object)).toBe(true);
    });
  });
});

console.log('\n=== Spy Mock Types Complete ===');
console.log('Next: 07_Snapshot_Testing.ts');