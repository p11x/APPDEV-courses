/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 06 Testing Frameworks Topic: 05_Testing_Library Purpose: Testing Library (React Testing Library) integration with TypeScript Difficulty: intermediate UseCase: web, frontend Version: TS 5.0+ Compatibility: Node.js 16+, Browsers Performance: fast Security: N/A */

declare namespace TestingLibrary {
  export interface BoundElement {
    <E extends Element = Element>(selector: string): SingleElementResult<E>;
    <E extends Element = Element>(matcher: MatcherFunction): ArrayElementResult<E>;
  }
  export interface Query {
    <E extends Element = Element>(selector: string): SingleElementResult<E>;
  }

  export interface SingleElementResult<E extends Element = Element> extends ElementResult<E> {
    (debug: DebugInfo): HTMLElement;
  }

  export interface ArrayElementResult<E extends Element = Element> extends ElementsResult<E> {
    (debug: DebugInfo): HTMLElement[];
  }

  export interface DebugInfo {
    hidden?: boolean;
    visible?: boolean;
    baseElement?: Element;
  }

  export type MatcherFunction = (content: unknown) => boolean;

  interface BoundWaitForElement {
    <E extends Element = Element>(callback: () => E): Promise<E>;
    <E extends Element = Element>(callback: () => E, options: WaitForOptions): Promise<E>;
  }

  interface WaitOptions extends WaitForOptions {
    container?: Element;
  }

  interface WaitForOptions {
    timeout?: number;
    interval?: number;
    onTimeout?: (error: Error) => Error;
  }

  interface BoundFindBy {
    <E extends Element = Element>(selector: string): Promise<E>;
    <E extends Element = Element>(
      selector: string,
      options?: FindAllByTextOptions
    ): Promise<E[]>;
    <E extends Element = Element>(
      selector: string,
      getByText: QueryByText,
      options?: FindAllByTextOptions
    ): Promise<E>;
  }

  interface FindAllByTextOptions extends getByTextOptions {
    exact?: boolean;
  }

  interface getByTextOptions {
    exact?: boolean;
    ignoreCase?: boolean;
    trim?: boolean;
    normalizer?: (text: string) => string;
  }

  interface QueryByText extends Query {
    <E extends Element = Element>(text: string): SingleElementResult<E>;
    <E extends Element = Element>(text: string, options: getByTextOptions): SingleElementResult<E>;
  }

  type Config = {
    testIdAttribute?: string;
    getByText?: typeof getByText;
    getByLabelText?: typeof getByLabelText;
   queriall?: typeof queriall;
    timeout?: number;
    defaultHidden?: boolean;
    principal?: boolean;
    collapseInlineSnapshot?: boolean;
    computedStyleSupportsFlakeyElements?: boolean;
    hidden: boolean;
    label: string;
    name: string | RegExp;
    normalizer?: () => string;
     queryFallbacks: boolean;
    raiseMultipleCaughtError?: boolean;
    showOriginalStackTrace?: boolean;
    timeout?: number;
    verbosity?: number;
    selectedfe: string;
    value: string | RegExp;
  };

  function getByText(
    container: Element,
    text: string | RegExp,
    options?: Config
  ): HTMLElement;

  function getByRole(
    container: Element,
    role: Role
  ): HTMLElement;

  function getByLabelText(
    container: Element,
    text: string | RegExp,
    options?: Config
  ): HTMLElement;

  function getByPlaceholderText(
    container: Element,
    text: string | RegExp,
    options?: Config
  ): HTMLElement;

  function getByAltText(
    container: Element,
    text: string | RegExp,
    options?: Config
  ): HTMLElement;

  function getByTestId(
    container: Element,
    testId: string
  ): HTMLElement;

  function queriall(
    container: Element,
    text: string | RegExp,
    options?: Config
  ): HTMLElement[];

  type Queries = typeof import('@testing-library/dom').queries;

  interface BoundQueries {
    getAllBy: BoundRole & BoundText;
    getBy: BoundRole;
    findAllBy: BoundRole;
    findBy: BoundRole & BoundText;
    queryAllBy: BoundRole;
    queryBy: BoundRole;
  }

  interface BoundRole extends BoundQuery {
    <E extends Element = Element>(
      role: Role,
      options?: { name?: string | RegExp; level?: number }
    ): E;
  }

  interface BoundText extends BoundQuery {
    <E extends Element = Element>(
      text: string | RegExp,
      options?: { exact?: boolean }
    ): E;
  }

  interface BoundQuery {
    <E extends Element = Element>(selector: string): E;
  }

  type Role =
    | 'alert'
    | 'alertdialog'
    | 'application'
    | 'article'
    | 'banner'
    | 'button'
    | 'cell'
    | 'checkbox'
    | 'columnheader'
    | 'combobox'
    | 'complementary'
    | 'contentinfo'
    | 'definition'
    | 'dialog'
    | 'directory'
    | 'document'
    | 'feed'
    | 'figure'
    | 'form'
    | 'grid'
    | 'gridcell'
    | 'group'
    | 'heading'
    | 'img'
    | 'link'
    | 'list'
    | 'listbox'
    | 'listitem'
    | 'log'
    | 'main'
    | 'marquee'
    | 'math'
    | 'menu'
    | 'menubar'
    | 'menuitem'
    | 'menuitemcheckbox'
    | 'menuitemradio'
    | 'navigation'
    | 'none'
    | 'note'
    | 'option'
    | 'presentation'
    | 'progressbar'
    | 'radio'
    | 'radiogroup'
    | 'region'
    | 'row'
    | 'rowgroup'
    | 'rowheader'
    | 'scrollbar'
    | 'search'
    | 'searchbox'
    | 'separator'
    | 'slider'
    | 'spinbutton'
    | 'status'
    | 'switch'
    | 'tab'
    | 'table'
    | 'tablist'
    | 'tabpanel'
    | 'term'
    | 'textbox'
    | 'timer'
    | 'toolbar'
    | 'tooltip'
    | 'tree'
    | 'treegrid'
    | 'treeitem';

  function render(
    component: React.ComponentType,
    options?: RenderOptions
  ): RenderResult;

  interface RenderOptions {
    wrapper?: React.ComponentType;
    hydrate?: boolean;
    baseElement?: Element;
    queries?: Queries;
  }

  interface RenderResult extends Un_boundRenderResult {
    asFragment: () => DocumentFragment;
    baseElement: Element;
    container: Element;
    debug: (element?: HTMLElement | DocumentFragment) => void;
  }

  interface UnboundRenderResult {
    rerender: (ui: React.ReactElement) => void;
    unmount: () => void;
    output: string;
    clear: () => void;
  }

  function createEvent(name: string, element: Element, eventInitDict?: EventInit): Event;

  // User Event API
  const userEvent: {
    click: (element: Element, init?: MouseEventInit) => Promise<void>;
    dblClick: (element: Element, init?: MouseEventInit) => Promise<void>;
    tripleClick: (element: Element, init?: MouseEventInit) => Promise<void>;
    hover: (element: Element, init?: MouseEventInit) => Promise<void>;
    unhover: (element: Element, init?: MouseEventInit) => Promise<void>;
    type: (
      element: Element,
      text: string,
      options?: { delay?: number }
    ) => Promise<void>;
    clear: (element: Element) => Promise<void>;
    selectOptions: (
      element: Element,
      values: string | string[]
    ) => Promise<void>;
    deselectOptions: (
      element: Element,
      values: string | string[]
    ) => Promise<void>;
    tab: (options?: { shift?: boolean }) => Promise<void>;
    paste: (element: Element, text: string, options?: { pasteInit?: ClipboardEventInit }) => Promise<void>;
    copy: (element: Element) => Promise<void>;
    cut: (element: Element) => Promise<void>;
  };
}

import * as React from 'react';
import {
  render,
  screen,
  fireEvent,
  waitFor,
  waitForElementToBeRemoved,
  within,
  getByText,
  getByRole,
  getByLabelText,
  getByPlaceholderText,
  getByAltText,
  getByTestId,
  queryByText,
  queryByRole,
  queryByLabelText,
  queryByPlaceholderText,
  queryByAltText,
  queryByTestId,
  findByText,
  findByRole,
  findByLabelText,
  findByPlaceholderText,
  findByAltText,
  findByTestId,
} from '@testing-library/react';

describe('Testing Library', () => {
  describe('Render', () => {
    const MyComponent = () => <button>Click me</button>;

    it('should render component', () => {
      const { container, getByText } = render(<MyComponent />);
      expect(getByText('Click me')).toBeInTheDocument();
    });
  });

  describe('Queries', () => {
    it('should get by text', () => {
      const container = document.createElement('div');
      const button = document.createElement('button');
      button.textContent = 'Submit';
      container.appendChild(button);

      const element = TestingLibrary.getByText(container, 'Submit');
      expect(element).toBeTruthy();
    });

    it('should get by role', () => {
      const container = document.createElement('button');
      container.setAttribute('role', 'button');

      const element = TestingLibrary.getByRole(container, 'button');
      expect(element).toBeTruthy();
    });

    it('should get by label text', () => {
      const container = document.createElement('div');
      container.innerHTML = '<label for="username">Username</label><input id="username" />';

      const element = TestingLibrary.getByLabelText(container, 'Username');
      expect(element).toBeTruthy();
    });

    it('should get by test id', () => {
      const container = document.createElement('div');
      const element = document.createElement('div');
      element.setAttribute('data-testid', 'my-element');
      container.appendChild(element);

      const result = TestingLibrary.getByTestId(container, 'my-element');
      expect(result).toBeTruthy();
    });
  });

  describe('Queries Array Suffix', () => {
    it('should use queryAllBy', () => {
      const container = document.createElement('div');
      container.innerHTML = '<li>Item 1</li><li>Item 2</li><li>Item 3</li>';

      const elements = TestingLibrary.queriall(container, /Item/);
      expect(elements).toHaveLength(3);
    });
  });

  describe('Fire Event', () => {
    it('should fire click event', () => {
      const button = document.createElement('button');
      let clicked = false;
      button.addEventListener('click', () => {
        clicked = true;
      });

      fireEvent.click(button);
      expect(clicked).toBe(true);
    });

    it('should fire change event', () => {
      const input = document.createElement('input');
      let value = '';
      input.addEventListener('change', (e) => {
        value = (e.target as HTMLInputElement).value;
      });

      fireEvent.change(input, { target: { value: 'test' } });
      expect(value).toBe('test');
    });

    it('should fire keyboard event', () => {
      const input = document.createElement('input');
      fireEvent.keyDown(input, { key: 'Enter', code: 'Enter' });
    });
  });

  describe('Wait For', () => {
    it('should wait for element', async () => {
      const container = document.createElement('div');
      const button = document.createElement('button');
      button.textContent = 'Load More';
      button.style.display = 'none';
      container.appendChild(button);

      setTimeout(() => {
        button.style.display = 'block';
      }, 100);

      await waitFor(() => {
        expect(button).toBeVisible();
      });
    });

    it('should wait for element to be removed', async () => {
      const container = document.createElement('div');
      const button = document.createElement('button');
      button.textContent = 'Loading...';
      container.appendChild(button);

      setTimeout(() => {
        container.removeChild(button);
      }, 100);

      await waitForElementToBeRemoved(() => container.querySelector('button')!);
    });
  });

  describe('Within', () => {
    it('should use within', () => {
      const container = document.createElement('div');
      container.innerHTML = '<span>Inside</span>';

      const element = TestingLibrary.getByText(container, 'Inside');
      expect(element).toBeTruthy();
    });
  });

  describe('User Event', () => {
    it('should use userEvent.click', async () => {
      const button = document.createElement('button');
      let clicked = false;
      button.addEventListener('click', () => {
        clicked = true;
      });

      await TestingLibrary.userEvent.click(button);
      expect(clicked).toBe(true);
    });

    it('should use userEvent.type', async () => {
      const input = document.createElement('input');
      document.body.appendChild(input);

      await TestingLibrary.userEvent.type(input, 'Hello World');
      expect((input as HTMLInputElement).value).toBe('Hello World');
    });
  });
});

console.log('\n=== Testing Library Complete ===');
console.log('Next: 06_Spy_Mock_Types.ts');