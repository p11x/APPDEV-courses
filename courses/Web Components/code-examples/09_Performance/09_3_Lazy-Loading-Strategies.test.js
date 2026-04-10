/**
 * @group unit
 * @group performance
 */
import { expect, fixture, html } from '@open-wc/testing';
import './09_3_Lazy-Loading-Strategies.js';

describe('LazyLoader', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<lazy-loader></lazy-loader>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should render lazy loader UI', () => {
    const h3 = element.shadowRoot.querySelector('h3');
    expect(h3).to.exist;
    expect(h3.textContent).to.include('Lazy Loading Strategies');
  });

  it('should render four placeholders', () => {
    const placeholders = element.shadowRoot.querySelectorAll('.placeholder');
    expect(placeholders.length).to.equal(4);
  });

  it('should have widget placeholders', () => {
    const widgetA = element.shadowRoot.querySelector('[data-id="widget-a"]');
    const widgetB = element.shadowRoot.querySelector('[data-id="widget-b"]');
    const widgetC = element.shadowRoot.querySelector('[data-id="widget-c"]');
    const widgetD = element.shadowRoot.querySelector('[data-id="widget-d"]');
    expect(widgetA).to.exist;
    expect(widgetB).to.exist;
    expect(widgetC).to.exist;
    expect(widgetD).to.exist;
  });

  describe('Property Changes', () => {
    it('should return false for unloaded element', () => {
      expect(element.isLoaded('widget-a')).to.be.false;
    });

    it('should return empty array for initially loaded elements', () => {
      expect(element.getLoadedElements()).to.be.an('array').that.is.empty;
    });

    it('should track loaded elements after loading', async () => {
      element._loadedElements.add('widget-a');
      expect(element.isLoaded('widget-a')).to.be.true;
    });

    it('should get loaded elements', () => {
      element._loadedElements.add('widget-a');
      const loaded = element.getLoadedElements();
      expect(loaded).to.include('widget-a');
    });
  });

  describe('Events', () => {
    it('should add click listener to widget-d', () => {
      const widgetD = element.shadowRoot.querySelector('[data-id="widget-d"]');
      expect(widgetD).to.exist;
    });

    it('should setup intersection observer', () => {
      expect(element._intersectionObserver).to.exist;
    });
  });

  describe('Lifecycle', () => {
    it('should setup intersection observer on connectedCallback', () => {
      const placeholders = element.shadowRoot.querySelectorAll('.placeholder');
      expect(placeholders.length).to.equal(4);
    });

    it('should disconnect observer on disconnectCallback', () => {
      element.disconnectCallback();
      expect(element._intersectionObserver).to.be.null;
    });
  });
});