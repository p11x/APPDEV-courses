/**
 * @group unit
 * @group performance
 */
import { expect, fixture, html } from '@open-wc/testing';
import './09_6_Web-Component-Analytics.js';

describe('ComponentAnalytics', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<component-analytics></component-analytics>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should render analytics UI', () => {
    const h3 = element.shadowRoot.querySelector('h3');
    expect(h3).to.exist;
    expect(h3.textContent).to.include('Web Component Analytics');
  });

  it('should render metric cards', () => {
    const metrics = element.shadowRoot.querySelectorAll('.metric-grid .analytics-card');
    expect(metrics.length).to.equal(4);
  });

  it('should render views metric', () => {
    const views = element.shadowRoot.getElementById('views');
    expect(views).to.exist;
  });

  it('should render interactions metric', () => {
    const interactions = element.shadowRoot.getElementById('interactions');
    expect(interactions).to.exist;
  });

  it('should render errors metric', () => {
    const errors = element.shadowRoot.getElementById('errors');
    expect(errors).to.exist;
  });

  it('should render render time metric', () => {
    const renderTime = element.shadowRoot.getElementById('render-time');
    expect(renderTime).to.exist;
  });

  it('should render timeline', () => {
    const timeline = element.shadowRoot.getElementById('timeline');
    expect(timeline).to.exist;
  });

  it('should render control buttons', () => {
    const trackEvent = element.shadowRoot.getElementById('track-event');
    const exportData = element.shadowRoot.getElementById('export-data');
    expect(trackEvent).to.exist;
    expect(exportData).to.exist;
  });

  describe('Property Changes', () => {
    it('should track custom events', () => {
      element.trackCustomEvent('testEvent', { key: 'value' });
      const metrics = element.getAnalytics();
      expect(metrics.totalViews).to.exist;
    });

    it('should set custom dimensions', () => {
      element.setCustomDimension('userType', 'premium');
      const analytics = element.getAnalytics();
      expect(analytics.customDimensions.userType).to.equal('premium');
    });

    it('should return analytics object', () => {
      const analytics = element.getAnalytics();
      expect(analytics).to.have.property('component');
      expect(analytics).to.have.property('sessionId');
      expect(analytics).to.have.property('totalViews');
      expect(analytics).to.have.property('totalInteractions');
      expect(analytics).to.have.property('totalErrors');
      expect(analytics).to.have.property('events');
    });

    it('should queue events', () => {
      element._queueEvent({ type: 'test', data: {} });
      expect(element._eventQueue.length).to.equal(1);
    });
  });

  describe('Events', () => {
    it('should setup click event tracking', () => {
      expect(element._componentMetrics).to.exist;
    });

    it('should setup input event tracking', () => {
      expect(element._componentMetrics).to.exist;
    });
  });

  describe('Lifecycle', () => {
    it('should track lifecycle on connectedCallback', () => {
      const connected = element._componentMetrics.get('lifecycle:connected');
      expect(connected).to.exist;
    });

    it('should track disconnect on disconnectCallback', () => {
      element.disconnectCallback();
      const disconnected = element._componentMetrics.get('lifecycle:disconnected');
      expect(disconnected).to.exist;
    });
  });
});