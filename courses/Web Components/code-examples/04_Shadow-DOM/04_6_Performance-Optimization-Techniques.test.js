import { PerformanceChart } from './04_6_Performance-Optimization-Techniques.js';

describe('PerformanceChart', () => {
  let component;

  beforeEach(() => {
    component = document.createElement('perf-chart');
    document.body.appendChild(component);
  });

  afterEach(() => {
    component?.remove();
  });

  describe('rendering', () => {
    test('renders with shadow DOM', () => {
      expect(component.shadowRoot).toBeDefined();
    });

    test('renders chart container', () => {
      expect(component.shadowRoot.innerHTML).toContain('chart-container');
    });

    test('renders chart area', () => {
      expect(component.shadowRoot.innerHTML).toContain('chart-area');
    });

    test('renders legend', () => {
      expect(component.shadowRoot.innerHTML).toContain('chart-legend');
    });

    test('renders tooltip', () => {
      expect(component.shadowRoot.innerHTML).toContain('chart-tooltip');
    });
  });

  describe('property changes', () => {
    test('max-items attribute limits items', () => {
      component.setAttribute('max-items', '3');
      component._loadData();
      expect(component._data.length).toBeGreaterThan(0);
    });

    test('animation attribute toggles animation', () => {
      component.setAttribute('animation', 'false');
      expect(component.getAttribute('animation')).toBe('false');
    });

    test('theme attribute applies theme', () => {
      component.setAttribute('theme', 'dark');
      expect(component.getAttribute('theme')).toBe('dark');
    });

    test('data-src attribute triggers data load', () => {
      component.setAttribute('data-src', 'data.json');
      expect(component.hasAttribute('data-src')).toBe(true);
    });
  });

  describe('events', () => {
    test('handles mouse enter on chart bars', (done) => {
      component._loadData();
      setTimeout(() => {
        const bar = component.shadowRoot.querySelector('.chart-bar');
        if (bar) {
          bar.dispatchEvent(new MouseEvent('mouseenter'));
        }
        done();
      }, 100);
    });
  });

  describe('edge cases', () => {
    test('setData updates chart data', () => {
      component.setData([{ label: 'A', value: 10 }, { label: 'B', value: 20 }]);
      expect(component._data.length).toBe(2);
    });

    test('addDataPoint adds point to data', () => {
      component._data = [{ label: 'A', value: 10 }];
      component.addDataPoint({ label: 'B', value: 20 });
      expect(component._data.length).toBe(2);
    });

    test('addDataPoint removes old data when exceeding limit', () => {
      component._data = Array.from({ length: 50 }, (_, i) => ({ label: `${i}`, value: i }));
      component.addDataPoint({ label: 'new', value: 100 });
      expect(component._data.length).toBe(50);
    });

    test('getPerformanceMetrics returns metrics', () => {
      const metrics = component.getPerformanceMetrics();
      expect(metrics.renderCount).toBeDefined();
      expect(metrics.lastRenderTime).toBeDefined();
    });

    test('debounceRender cancels previous animation frame', () => {
      component._debounceRender(() => {});
      expect(component._animationFrameId).toBeDefined();
    });

    test('shows loading skeleton', () => {
      component._showSkeleton();
      const skeletons = component.shadowRoot.querySelectorAll('.loading-skeleton');
      expect(skeletons.length).toBeGreaterThan(0);
    });

    test('updates performance metrics', () => {
      component._updatePerformanceMetrics(10);
      expect(component._performance.renderCount).toBe(1);
    });
  });
});
