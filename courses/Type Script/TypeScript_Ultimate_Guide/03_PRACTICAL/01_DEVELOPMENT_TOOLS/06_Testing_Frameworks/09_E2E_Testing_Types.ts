/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 06 Testing Frameworks Topic: 09_E2E_Testing_Types Purpose: End-to-end testing type definitions and configurations Difficulty: advanced UseCase: web, mobile Version: TS 5.0+ Compatibility: Node.js 16+, Browsers Performance: varies Security: N/A */

declare namespace E2ETesting {
  interface Browser {
    pages(): Promise<Page[]>;
    version(): Promise<string>;
    contexts(): Promise<BrowserContext[]>;
    newContext(options?: BrowserContextOptions): Promise<BrowserContext>;
    close(): Promise<void>;
  }

  interface Page {
    opened: boolean;
    url(): Promise<string>;
    title(): Promise<string>;
    mainFrame(): Promise<Frame>;
    frames(): Promise<Frame[]>;
    context(): BrowserContext;
    viewport: Viewport | null;
    isClosed(): boolean;
    click(selector: string, options?: ClickOptions): Promise<void>;
    dblclick(selector: string, options?: ClickOptions): Promise<void>;
    tripleclick(selector: string): Promise<void>;
    check(selector: string, options?: CheckOptions): Promise<void>;
    uncheck(selector: string, options?: CheckOptions): Promise<void>;
    type(selector: string, text: string, options?: TypeOptions): Promise<void>;
    fill(selector: string, value: string, options?: FillOptions): Promise<void>;
    press(selector: string, key: string, options?: PressOptions): Promise<void>;
    locate(selector: string): Promise<Locator>;
    getByText(text: string, options?: GetByTextOptions): Promise<Locator>;
    getByRole(role: string, options?: GetByRoleOptions): Promise<Locator>;
    getByTestId(testId: string): Promise<Locator>;
    getByLabel(text: string, options?: GetByLabelOptions): Promise<Locator>;
    getByPlaceholder(text: string, options?: GetByPlaceholderOptions): Promise<Locator>;
    getByAltText(text: string, options?: GetByAltTextOptions): Promise<Locator>;
    waitForURL(url: string | RegExp, options?: WaitForOptions): Promise<void>;
    waitForLoadState(state: LoadState, options?: WaitForOptions): Promise<void>;
    waitForNavigation(options?: WaitForNavigationOptions): Promise<void>;
    screenshot(options?: ScreenshotOptions): Promise<Buffer>;
    pdf(options?: PDFOptions): Promise<Buffer>;
    evaluate<R>(fn: (global: Window) => R, arg?: unknown): Promise<R>;
    evaluateHandle<R>(fn: (global: Window) => R, arg?: unknown): Promise<JSHandle<R>>;
    addInitScript(script: Function | string): Promise<void>;
    exposeFunction(name: string, fn: Function): Promise<void>;
    route(url: string | RegExp, handler: RouteHandler): Promise<void>;
    unroute(url: string | RegEm): Promise<void>;
  }

  interface BrowserContext {
    browser(): Promise<Browser>;
    pages(): Promise<Page[]>;
    mainPage(): Promise<Page>;
    newPage(): Promise<Page>;
    closed: boolean;
    close(): Promise<void>;
    cookies(url?: string): Promise<Cookie[]>;
    setCookies(cookies: Cookie[]): Promise<void>;
    clearCookies(): Promise<void>;
    grantPermissions(
      url: string,
      permissions: Permission[]
    ): Promise<void>;
    clearPermissions(): Promise<void>;
    setDefaultNavigationTimeout(timeout: number): void;
    setDefaultTimeout(timeout: number): void;
    setExtraHTTPHeaders(headers: Record<string, string>): Promise<void>;
  }

  interface Viewport {
    width: number;
    height: number;
    deviceScaleFactor?: number;
    isLandscape?: boolean;
    isMobile?: boolean;
  }

  interface ClickOptions {
    button?: 'left' | 'right' | 'middle';
    modifiers?: Array<'Alt' | 'Control' | 'Meta' | 'Shift'>;
    position?: { x: number; y: number };
    force?: boolean;
    noWaitAfter?: boolean;
    timeout?: number;
    trial?: boolean;
  }

  interface CheckOptions {
    force?: boolean;
    noWaitAfter?: boolean;
    timeout?: number;
  }

  interface TypeOptions {
    delay?: number;
    noWaitAfter?: boolean;
    timeout?: number;
  }

  interface FillOptions {
    force?: boolean;
    noWaitAfter?: boolean;
    timeout?: number;
  }

  interface PressOptions {
    delay?: number;
    noWaitAfter?: boolean;
    timeout?: number;
  }

  interface Locator {
    first(): Locator;
    last(): Locator;
    nth(index: number): Locator;
    filter(options: FilterOptions): Locator;
    getByText(text: string | RegExp, options?: { exact?: boolean }): Locator;
    getByRole(role: string, options?: { checked?: boolean; disabled?: boolean; selected?: boolean }): Locator;
    getByTestId(testId: string): Locator;
    getByLabel(text: string, options?: { exact?: boolean }): Locator;
    getByPlaceholder(text: string, options?: { exact?: boolean }): Locator;
    getByAltText(text: string, options?: { exact?: boolean }): Locator;
    count(): Promise<number>;
    all(): Promise<ElementHandle[]>;
    allInnerTexts(): Promise<string[]>;
    allTextContents(): Promise<string[]>;
    boundingBox(): Promise<DOMRect | null>;
    check(options?: CheckOptions): Promise<void>;
    click(options?: ClickOptions): Promise<void>;
    contentFrame(): Promise<Frame | null>;
    count(): Promise<number>;
    dblClick(options?: ClickOptions): Promise<void>;
    dispatchEvent(type: string, eventInit?: Record<string, unknown>): Promise<void>;
    evaluate(fn: (element: HTMLElement) => unknown, arg?: unknown): Promise<unknown>;
    fill(value: string): Promise<void>;
    focus(): Promise<void>;
    getAttribute(name: string): Promise<string | null>;
    hover(options?: HoverOptions): Promise<void>;
    innerHTML(): Promise<string>;
    innerText(): Promise<string>;
    inputValue(): Promise<string>;
    isChecked(): Promise<boolean>;
    isDisabled(): Promise<boolean>;
    isEditable(): Promise<boolean>;
    isEnabled(): Promise<boolean>;
    isHidden(): Promise<boolean>;
    isVisible(): Promise<boolean>;
    press(key: string, options?: PressOptions): Promise<void>;
    screenshot(options?: ScreenshotOptions): Promise<Buffer>;
    scrollIntoViewIfNeeded(options?: ScrollOptions): Promise<void>;
    selectOption(values: string | string[]): Promise<string[]>;
    selectText(): Promise<void>;
    setInputFiles(files: string | string[] | FilePayload): Promise<void>;
    tap(options?: TapOptions): Promise<void>;
    textContent(): Promise<string | null>;
    type(text: string, options?: TypeOptions): Promise<void>;
    uncheck(options?: CheckOptions): Promise<void>;
    waitFor(options?: WaitForOptions): Promise<void>;
    waitForElementState(state: 'visible' | 'hidden' | 'stable' | 'enabled' | 'disabled', options?: WaitForOptions): Promise<void>;
  }

  interface FilterOptions {
    has?: Locator;
    hasNot?: Locator;
    hasText?: string | RegExp;
    hasNotText?: string | RegExp;
  }

  interface HoverOptions extends ClickOptions {
    position?: { x: number; y: number };
  }

  interface ScrollOptions {
    timeout?: number;
  }

  interface TapOptions extends ClickOptions {}

  interface GetByTextOptions {
    exact?: boolean;
    ignoreCase?: boolean;
  }

  interface GetByRoleOptions {
    checked?: boolean;
    disabled?: boolean;
    expanded?: boolean;
    level?: number;
    pressed?: boolean;
    selected?: boolean;
    name?: string | RegExp;
    description?: string;
  }

  interface GetByLabelOptions extends GetByTextOptions {}

  interface GetByPlaceholderOptions extends GetByTextOptions {}

  interface GetByAltTextOptions extends GetByTextOptions {}

  interface ScreenshotOptions {
    type?: 'png' | 'jpeg';
    fullPage?: boolean;
    clip?: DOMRect;
    omitBackground?: boolean;
  }

  interface PDFOptions {
    scale?: number;
    displayHeaderFooter?: boolean;
    headerTemplate?: string;
    footerTemplate?: string;
    printBackground?: boolean;
    landscape?: boolean;
    pageRanges?: string;
    format?: string;
    width?: string;
    height?: string;
    margin?: {
      top?: string;
      right?: string;
      bottom?: string;
      left?: string;
    };
  }

  interface WaitForOptions {
    state?: 'attached' | 'detached' | 'visible' | 'hidden';
    timeout?: number;
  }

  interface WaitForNavigationOptions extends WaitForOptions {
    url?: string | RegExp;
  }

  interface LoadState {
    domcontentloaded?: boolean;
    load?: boolean;
    networkidle?: boolean;
  }

  interface Cookie {
    name: string;
    value: string;
    domain?: string;
    path?: string;
    expires?: number;
    httpOnly?: boolean;
    secure?: boolean;
    sameSite?: 'Strict' | 'Lax' | 'None';
  }

  interface BrowserContextOptions {
    viewport?: Viewport;
    userAgent?: string;
    deviceScaleFactor?: number;
    isMobile?: boolean;
    hasTouch?: boolean;
    javaScriptEnabled?: boolean;
    timezoneId?: string;
    geolocation?: { longitude: number; latitude: number };
    permissions?: string[];
    extraHTTPHeaders?: Record<string, string>;
    proxy?: ProxySettings;
    ignoreHTTPSErrors?: boolean;
    acceptDownloads?: boolean;
    acceptDownloads?: boolean;
    tracesDir?: string;
    recordHarPath?: string;
    recordVideoDir?: string;
  }

  interface ProxySettings {
    server: string;
    username?: string;
    password?: string;
    bypass: string[];
  }

  type Permission =
    | 'geolocation'
    | 'midi'
    | 'notifications'
    | 'camera'
    | 'microphone'
    | 'background-sync'
    | 'ambient-light-sensor'
    | 'accelerometer'
    | 'gyroscope'
    | 'magnetometer'
    | 'accessibility-events'
    | 'clipboard-read'
    | 'clipboard-write'
    | 'payment-handler';

  interface Frame {
    page(): Promise<Page>;
    name(): Promise<string>;
    url(): Promise<string>;
    parentFrame(): Promise<Frame | null>;
    childFrames(): Promise<Frame[]>;
  }

  interface ElementHandle {
    asElement(): ElementHandle;
    evaluate(fn: (element: HTMLElement) => unknown, arg?: unknown): Promise<unknown>;
    boundingBox(): Promise<DOMRect | null>;
    click(options?: ClickOptions): Promise<void>;
    contentFrame(): Promise<Frame | null>;
  }

  interface JSHandle<T> {
    evaluate(fn: (value: T) => unknown, arg?: unknown): Promise<unknown>;
    jsonValue(): Promise<T>;
  }

  interface RouteHandler {
    request: RouteHandler;
    response: () => Promise<Response>;
    abort(errorCode?: string): Promise<void>;
    fulfill(response: ResponseOptions): Promise<void>;
  }

  interface Response {
    ok(): Promise<boolean>;
    url(): Promise<string>;
    status(): Promise<number>;
    statusText(): Promise<string>;
    headers(): Promise<Record<string, string>>;
    body(): Promise<Buffer>;
    text(): Promise<string>;
    json(): Promise<unknown>;
    arrayBuffer(): Promise<ArrayBuffer>;
  }

  interface ResponseOptions {
    status?: number;
    headers?: Record<string, string>;
    body?: string | Buffer;
    contentType?: string;
  }

  type RouteHandler = (route: Route) => Promise<void>;
}

interface PlaywrightConfig {
  testDir?: string;
  testMatch?: string | string[];
  testIgnore?: string | string[];
  timeout?: number;
  expect?: {
    timeout?: number;
  };
  use?: PlaywrightTestOptions;
  projects?: Project[];
  ignoreSnapshots?: boolean;
  updateSnapshots?: 'all' | 'none' | 'missing';
  workers?: number;
  forbidOnly?: boolean;
  retries?: number;
  reporter?: string | (string | Reporter)[]
  globalTimeout?: number;
  sharding?: ShardingOptions;
}

interface Project {
  name?: string;
  use?: PlaywrightTestOptions;
  match?: string | string[];
}

interface PlaywrightTestOptions {
  baseURL?: string;
  browserName?: 'chromium' | 'firefox' | 'webkit';
  channel?: string;
  viewport?: { width: number; height: number };
  deviceScaleFactor?: number;
  hasTouch?: boolean;
  colorScheme?: 'light' | 'dark' | 'no-preference';
  storageState?: string;
  trace?: 'on-first-retry' | 'on' | 'off' | 'retain-on-failure';
  screenshot?: 'only-on-failure' | 'on' | 'off';
  video?: 'on-first-retry' | 'on' | 'off' | 'retain-on-failure';
  fullPage?: boolean;
}

interface Reporter {
  toString(): string;
  onBegin(config: FullConfig, suite: Suite): void;
  onTestBegin(test: Test, result: TestResult): void;
  onStdOut(chunk: string): void;
  onStdErr(chunk: string): void;
  onTestEnd(test: Test, result: TestResult): void;
}

interface FullConfig {
  projects: FullProject[];
  timeout: number;
}

interface FullProject {
  name: string;
  testDir: string;
  use: PlaywrightTestOptions;
}

import { test, expect, defineConfig } from '@playwright/test';

defineConfig<PlaywrightConfig>({
  testDir: './tests',
  timeout: 30000,
  use: {
    baseURL: 'http://localhost:3000',
    trace: 'on-first-retry',
  },
  projects: [
    {
      name: 'chromium',
      use: { browserName: 'chromium' },
    },
  ],
});

describe('E2E Testing', () => {
  describe('Page Interactions', () => {
    test('should navigate to page', async ({ page }) => {
      await page.goto('http://localhost:3000');
      await expect(page).toHaveTitle(/Test App/);
    });

    test('should click button', async ({ page }) => {
      await page.goto('http://localhost:3000');
      await page.click('#submit-button');
      await page.waitForURL(/.*success/);
    });

    test('should fill form', async ({ page }) => {
      await page.goto('http://localhost:3000');
      await page.fill('#username', 'testuser');
      await page.fill('#password', 'password123');
      await page.click('#login');
    });
  });

  describe('Form Testing', () => {
    test('should validate form', async ({ page }) => {
      await page.goto('http://localhost:3000');
      
      await page.fill('#email', 'invalid-email');
      await page.click('#submit');
      
      const error = await page.locator('.error-message');
      await expect(error).toBeVisible();
    });
  });

  describe('Assertions', () => {
    test('should assert page content', async ({ page }) => {
      await page.goto('http://localhost:3000');
      
      await expect(page.locator('h1')).toHaveText('Welcome');
      await expect(page.locator('.content')).toContainText('Hello World');
    });

    test('should assert visibility', async ({ page }) => {
      await page.goto('http://localhost:3000');
      
      await expect(page.locator('#modal')).toBeHidden();
      await page.click('#open-modal');
      await expect(page.locator('#modal')).toBeVisible();
    });
  });
});

console.log('\n=== E2E Testing Complete ===');
console.log('Next: 10_Integration_Testing.ts');