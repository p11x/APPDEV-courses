/**
 * @group unit
 * @group performance
 */
import { expect, fixture, html } from '@open-wc/testing';
import './09_5_Performance-Monitoring-Tools.js';

describe('PerformanceMonitor', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<performance-monitor></performance-monitor>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should render monitor UI', () => {
    const h3 = element.shadowRoot.querySelector('h3');
    expect(h3).to.exist;
    expect(h3.textContent).to.include('Performance Monitor');
  });

  it('should render control buttons', () => {
    const buttons = element.shadowRoot.querySelectorAll('button');
    expect(buttons.length).to.equal(3);
  });

  it('should render metrics list', () => {
    const metricsList = element.shadowRoot.getElementById('metrics-list');
    expect(metricsList).to.exist;
  });

  it('should render status indicator', () => {
    const status = element.shadowRoot.getElementById('status');
    expect(status).to.exist;
  });

  describe('Property Changes', () => {
    it('should start recording', () => {
      element.startRecording();
      expect(element._isRecording).to.be.true;
    });

    it('should stop recording', () => {
      element.startRecording();
      element.stopRecording();
      expect(element._isRecording).to.be.false;
    });

    it('should create markers', () => {
      element.mark('testMark');
      expect(element._markers.length).to.equal(1);
      expect(element._markers[0].label).to.equal('testMark');
    });

    it('should measure between marks', () => {
      element.mark('start');
      element.mark('end');
      element.measure('testMeasure', 'start', 'end');
      expect(element._measures.length).to.equal(1);
    });

    it('should return metrics object', () => {
      const metrics = element.getMetrics();
      expect(metrics).to.have.property('sessionId');
      expect(metrics).to.have.property('metrics');
      expect(metrics).to.have.property('markers');
      expect(metrics).to.have.property('measures');
    });
  });

  describe('Events', () => {
    it('should setup start recording button handler', () => {
      const startBtn = element.shadowRoot.getElementById('start-record');
      expect(startBtn).to.exist;
    });

    it('should setup stop recording button handler', () => {
      const stopBtn = element.shadowRoot.getElementById('stop-record');
      expect(stopBtn).to.exist;
    });

    it('should setup get metrics button handler', () => {
      const getBtn = element.shadowRoot.getElementById('get-metrics');
      expect(getBtn).to.exist;
    });
  });

  describe('Lifecycle', () => {
    it('should setup performance observers on connectedCallback', () => {
      expect(element._observers).to.be.an('array');
    });

    it('should cleanup on disconnectCallback', () => {
      element.disconnectCallback();
      expect(element._updateInterval).to.be.undefined;
    });
  });
});