import { DebugPanel } from './04_7_Shadow-DOM-Debugging-Guide.js';

describe('DebugPanel', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('debug-panel');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders with shadow DOM', () => {
      expect(component.shadowRoot).toBeDefined();
    });

    test('renders debug panel', () => {
      expect(component.shadowRoot.innerHTML).toContain('debug-panel');
    });

    test('renders header', () => {
      expect(component.shadowRoot.innerHTML).toContain('debug-header');
    });

    test('renders log list', () => {
      expect(component.shadowRoot.innerHTML).toContain('log-list');
    });
  });

  describe('property changes', () => {
    test('log-level attribute updates log level', () => {
      component.setAttribute('log-level', 'error');
      expect(component._logLevel).toBe('error');
    });

    test('expanded attribute toggles expansion', () => {
      component.setAttribute('expanded', 'true');
      expect(component._isExpanded).toBe(true);
    });

    test('debug attribute toggles debug mode', () => {
      component.setAttribute('debug', 'true');
      expect(component._debugMode).toBe(true);
    });
  });

  describe('events', () => {
    test('debug API is available', () => {
      expect(window.__DEBUG_PANEL__).toBeDefined();
    });
  });

  describe('edge cases', () => {
    test('log method adds log entry', () => {
      component._log('test message');
      expect(component._logs.length).toBe(1);
    });

    test('warn method adds warn entry', () => {
      component._warn('warning');
      expect(component._logs[0].type).toBe('warn');
    });

    test('error method adds error entry', () => {
      component._error('error');
      expect(component._logs[0].type).toBe('error');
    });

    test('clear removes all logs', () => {
      component._log('test');
      component.clear();
      expect(component._logs.length).toBe(0);
    });

    test('clearAll removes logs and history', () => {
      component._log('test');
      component.clearAll();
      expect(component._logs.length).toBe(0);
      expect(component._stateHistory.length).toBe(0);
    });

    test('assert fails when condition is false', () => {
      component._assert(false, 'Test assertion');
      expect(component._logs.length).toBe(1);
    });

    test('assert passes when condition is true', () => {
      component._assert(true, 'Test assertion');
      expect(component._logs.length).toBe(0);
    });

    test('exportLogs creates downloadable JSON', () => {
      component._log('test');
      const logBtn = component.shadowRoot.getElementById('export-btn');
      logBtn.click();
    });
  });
});
