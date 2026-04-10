/**
 * @group unit
 * @group performance
 */
import { expect, fixture, html } from '@open-wc/testing';
import './09_2_Runtime-Performance-Techniques.js';

describe('RuntimePerformanceOptimizer', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<performance-optimizer></performance-optimizer>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should render optimizer UI with title', () => {
    const h3 = element.shadowRoot.querySelector('h3');
    expect(h3).to.exist;
    expect(h3.textContent).to.include('Runtime Performance Optimizer');
  });

  it('should render canvas element', () => {
    const canvas = element.shadowRoot.querySelector('canvas');
    expect(canvas).to.exist;
  });

  it('should render FPS stat', () => {
    const fps = element.shadowRoot.getElementById('fps');
    expect(fps).to.exist;
  });

  it('should render frame time stat', () => {
    const frameTime = element.shadowRoot.getElementById('frame-time');
    expect(frameTime).to.exist;
  });

  it('should render dropped frames stat', () => {
    const dropped = element.shadowRoot.getElementById('dropped');
    expect(dropped).to.exist;
  });

  it('should have container with proper styling', () => {
    const container = element.shadowRoot.querySelector('.container');
    expect(container).to.exist;
  });

  describe('Property Changes', () => {
    it('should return metrics object', () => {
      const metrics = element.getMetrics();
      expect(metrics).to.have.property('fps');
      expect(metrics).to.have.property('avgFrameTime');
      expect(metrics).to.have.property('droppedFrames');
      expect(metrics).to.have.property('samples');
    });

    it('should pause monitoring', () => {
      element.pause();
      const metrics = element.getMetrics();
      expect(metrics).to.exist;
    });

    it('should resume monitoring', () => {
      element.pause();
      element.resume();
      const metrics = element.getMetrics();
      expect(metrics).to.exist;
    });
  });

  describe('Events', () => {
    it('should handle animation frame events', async () => {
      await new Promise(resolve => setTimeout(resolve, 50));
      const metrics = element.getMetrics();
      expect(metrics.samples).to.be.greaterThan(0);
    });

    it('should update stats display', async () => {
      await new Promise(resolve => setTimeout(resolve, 100));
      const fps = element.shadowRoot.getElementById('fps');
      expect(fps.textContent).to.not.equal('60');
    });
  });

  describe('Lifecycle', () => {
    it('should start monitoring on connectedCallback', () => {
      const canvas = element.shadowRoot.getElementById('fps-chart');
      expect(canvas).to.exist;
    });

    it('should cancel animation frame on disconnect', () => {
      element.disconnectCallback();
      expect(element._frameRequestId).to.be.null;
    });
  });
});