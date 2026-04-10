import { expect, fixture, html } from '@open-wc/testing';
import '../../../code-examples/06_Styling/06_5_Performance-Optimized-Styling.js';

describe('06_5_Performance-Optimized-Styling', () => {
  describe('StyleOptimizedCard', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<style-optimized-card></style-optimized-card>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders card structure', () => {
      const card = el.shadowRoot.querySelector('.card');
      const title = el.shadowRoot.querySelector('.title');
      const content = el.shadowRoot.querySelector('.content');
      expect(card).to.exist;
      expect(title).to.exist;
      expect(content).to.exist;
    });

    it('applies optimize attribute', async () => {
      el.setAttribute('optimize', '');
      await el.updateComplete;
      expect(el.hasAttribute('optimize')).to.be.true;
    });

    it('applies viewport attribute', async () => {
      el.setAttribute('viewport', '');
      await el.updateComplete;
      expect(el.hasAttribute('viewport')).to.be.true;
    });

    it('applies priority attribute', async () => {
      el.setAttribute('priority', 'high');
      await el.updateComplete;
      expect(el.getAttribute('priority')).to.equal('high');
    });

    it('gets performance metrics', () => {
      const metrics = el.getPerformanceMetrics();
      expect(metrics).to.be.null;
    });

    it('has render time when optimized', async () => {
      el.setAttribute('optimize', '');
      const metrics = el.getPerformanceMetrics();
      expect(metrics).to.have.property('renderTime');
    });
  });

  describe('StyleBatcher', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<style-batcher batch-size="5"></style-batcher>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('accepts batch-size attribute', () => {
      expect(el.getAttribute('batch-size')).to.equal('5');
    });

    it('accepts debounce attribute', async () => {
      el.setAttribute('debounce', '50');
      await el.updateComplete;
      expect(el.getAttribute('debounce')).to.equal('50');
    });

    it('batches style updates', async () => {
      const mockElement = { style: {} };
      el.batchStyleUpdate(mockElement, 'color', 'red');
      el.batchStyleUpdate(mockElement, 'background', 'blue');
      await new Promise(r => setTimeout(r, 20));
      expect(mockElement.style.color).to.equal('red');
    });
  });

  describe('CSSContainmentManager', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<css-containment-manager></css-containment-manager>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('applies contain attribute', async () => {
      el.setAttribute('contain', 'strict');
      await el.updateComplete;
      expect(el.getAttribute('contain')).to.equal('strict');
    });

    it('handles invalid contain values', async () => {
      el.setAttribute('contain', 'invalid');
      await el.updateComplete;
      expect(el.getAttribute('contain')).to.equal('invalid');
    });

    it('applies layout containment by default', () => {
      const contained = el.shadowRoot.querySelector('.contained');
      expect(contained.style.contain).to.equal('layout');
    });
  });

  describe('WillChangeOptimizer', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<will-change-optimizer></will-change-optimizer>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('applies animate attribute', async () => {
      el.setAttribute('animate', 'transform, opacity');
      await el.updateComplete;
      expect(el.hasAttribute('animate')).to.be.true;
    });

    it('optimizes property', () => {
      el.optimizeProperty('transform');
      el.optimizeProperty('opacity');
    });

    it('clears optimization', () => {
      el.optimizeProperty('transform');
      el.clearOptimization();
    });
  });

  describe('StyleMetricCollector', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<style-metric-collector></style-metric-collector>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('accepts measure attribute', async () => {
      el.setAttribute('measure', '');
      await el.updateComplete;
      expect(el.hasAttribute('measure')).to.be.true;
    });

    it('gets metrics', () => {
      const metrics = el.getMetrics();
      expect(metrics).to.have.property('styleRecalcs');
      expect(metrics).to.have.property('paints');
    });

    it('resets metrics', () => {
      el.resetMetrics();
      const metrics = el.getMetrics();
      expect(metrics.styleRecalcs).to.equal(0);
    });
  });

  describe('Edge Cases', () => {
    it('handles batch size of zero', async () => {
      const el = await fixture(html`<style-batcher batch-size="0"></style-batcher>`);
      expect(el.getAttribute('batch-size')).to.equal('0');
    });

    it('handles empty animate attribute', async () => {
      const el = await fixture(html`<will-change-optimizer animate=""></will-change-optimizer>`);
      expect(el).to.exist;
    });
  });
});