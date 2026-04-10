/**
 * @group unit
 * @group reference-materials
 */
import { expect, fixture, html } from '@open-wc/testing';
import './14_4_Performance-Metrics-Guide.css';

describe('Performance Metrics Guide', () => {
  it('should have CSS styles', () => {
    const style = document.querySelector('style');
    expect(style).to.exist;
  });
});

describe('MetricsCollector', () => {
  let collector;

  beforeEach(() => {
    collector = new MetricsCollector();
  });

  it('should create collector', () => {
    expect(collector).to.exist;
  });

  it('should collect metrics', () => {
    collector.collect('performance', {});
    expect(collector.metrics.size).to.be.greaterThan(0);
  });
});