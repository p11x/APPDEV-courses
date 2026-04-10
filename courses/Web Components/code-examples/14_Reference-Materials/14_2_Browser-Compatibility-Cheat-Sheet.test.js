/**
 * @group unit
 * @group reference-materials
 */
import { expect, fixture, html } from '@open-wc/testing';
import './14_2_Browser-Compatibility-Cheat-Sheet.css';

describe('Browser Compatibility Cheat Sheet', () => {
  it('should have CSS styles', () => {
    const style = document.querySelector('style');
    expect(style).to.exist;
  });
});

describe('CompatibilityMatrix', () => {
  let matrix;

  beforeEach(() => {
    matrix = new CompatibilityMatrix();
  });

  it('should create matrix', () => {
    expect(matrix).to.exist;
  });

  it('should check browser support', () => {
    const result = matrix.checkSupport('Chrome', '80');
    expect(result).to.exist;
  });
});

describe('FeatureDetector', () => {
  let detector;

  beforeEach(() => {
    detector = new FeatureDetector();
  });

  it('should create detector', () => {
    expect(detector).to.exist;
  });

  it('should detect features', () => {
    expect(detector.detect).to.be.a('function');
  });
});