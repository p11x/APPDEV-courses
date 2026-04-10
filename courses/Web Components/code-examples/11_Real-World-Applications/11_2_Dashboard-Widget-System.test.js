/**
 * @group unit
 * @group real-world
 */
import { expect, fixture, html } from '@open-wc/testing';
import './11_2_Dashboard-Widget-System.js';

describe('MetricCard', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<metric-card></metric-card>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should have default value', () => {
    expect(element.value).to.equal(0);
  });

  it('should have default label', () => {
    expect(element.label).to.equal('Metric');
  });

  it('should have default change', () => {
    expect(element.change).to.equal(0);
  });

  it('should observe value attribute', () => {
    const observed = MetricCard.observedAttributes;
    expect(observed).to.include('value');
  });

  it('should observe label attribute', () => {
    const observed = MetricCard.observedAttributes;
    expect(observed).to.include('label');
  });

  describe('Property Changes', () => {
    it('should set value', () => {
      element.value = 100;
      expect(element.value).to.equal(100);
    });

    it('should set label', () => {
      element.label = 'Sales';
      expect(element.label).to.equal('Sales');
    });

    it('should set change', () => {
      element.change = 15;
      expect(element.change).to.equal(15);
    });

    it('should set prefix and suffix', () => {
      element.prefix = '$';
      element.suffix = 'k';
      expect(element.prefix).to.equal('$');
      expect(element.suffix).to.equal('k');
    });
  });

  describe('Lifecycle', () => {
    it('should initialize on connectedCallback', () => {
      expect(element.value).to.equal(0);
    });
  });
});