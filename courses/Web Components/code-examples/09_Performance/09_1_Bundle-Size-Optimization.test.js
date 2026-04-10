/**
 * @group unit
 * @group performance
 */
import { expect, fixture, html } from '@open-wc/testing';
import './09_1_Bundle-Size-Optimization.js';

describe('BundleOptimizer', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<bundle-optimizer></bundle-optimizer>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should render bundle optimizer UI', () => {
    const h3 = element.shadowRoot.querySelector('h3');
    expect(h3).to.exist;
    expect(h3.textContent).to.include('Bundle Size Optimizer');
  });

  it('should render load buttons', () => {
    const buttons = element.shadowRoot.querySelectorAll('button');
    expect(buttons.length).to.equal(3);
  });

  it('should render chunk list', () => {
    const chunkList = element.shadowRoot.getElementById('chunk-list');
    expect(chunkList).to.exist;
  });

  it('should have proper container styling', () => {
    const container = element.shadowRoot.querySelector('.container');
    expect(container).to.exist;
  });

  it('should have controls section', () => {
    const controls = element.shadowRoot.querySelector('.controls');
    expect(controls).to.exist;
  });

  it('should have initial empty loaded chunks', () => {
    expect(element.getLoadedChunks()).to.be.an('array').that.is.empty;
  });

  describe('Property Changes', () => {
    it('should track loaded chunks after loading', async () => {
      await element._loadChunk('1');
      expect(element.getLoadedChunks()).to.include('1');
    });

    it('should load multiple chunks', async () => {
      await element._loadChunk('1');
      await element._loadChunk('2');
      const loaded = element.getLoadedChunks();
      expect(loaded).to.include('1');
      expect(loaded).to.include('2');
    });

    it('should return module after loading', async () => {
      await element._loadChunk('1');
      const module = element.getModule('1');
      expect(module).to.exist;
    });
  });

  describe('Events', () => {
    it('should dispatch click event on load button', async () => {
      const button = element.shadowRoot.querySelector('#load-chunk-1');
      let clicked = false;
      button.addEventListener('click', () => { clicked = true; });
      button.click();
      expect(clicked).to.be.true;
    });

    it('should handle button clicks for loading chunks', async () => {
      const button = element.shadowRoot.querySelector('#load-chunk-1');
      button.click();
      await new Promise(resolve => setTimeout(resolve, 200));
      expect(element.getLoadedChunks()).to.include('1');
    });

    it('should update chunk list after loading', async () => {
      await element._loadChunk('1');
      element._updateChunkList();
      const chunkItems = element.shadowRoot.querySelectorAll('.chunk-item.loaded');
      expect(chunkItems.length).to.be.greaterThan(0);
    });
  });

  describe('Lifecycle', () => {
    it('should setup lazy loading on connectedCallback', () => {
      const buttons = element.shadowRoot.querySelectorAll('button');
      expect(buttons.length).to.equal(3);
    });

    it('should clear state on disconnectCallback', async () => {
      await element._loadChunk('1');
      element.disconnectCallback();
      expect(element.getLoadedChunks()).to.be.an('array').that.is.empty;
    });
  });
});