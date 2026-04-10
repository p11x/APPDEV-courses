/**
 * @group unit
 * @group browser-compatibility
 */
import { expect, fixture, html } from '@open-wc/testing';
import './01_3_Browser-Compatibility-Matrix.js';

describe('WebComponentFeatures', () => {
  it('should detect custom elements support', () => {
    const hasSupport = WebComponentFeatures.customElements;
    expect(hasSupport).to.be.a('boolean');
  });

  it('should detect shadow DOM support', () => {
    const hasSupport = WebComponentFeatures.shadowDOM;
    expect(hasSupport).to.be.a('boolean');
  });

  it('should detect templates support', () => {
    const hasSupport = WebComponentFeatures.templates;
    expect(hasSupport).to.be.a('boolean');
  });

  it('should detect constructable stylesheets support', () => {
    const hasSupport = WebComponentFeatures.constructableSheets;
    expect(hasSupport).to.be.a('boolean');
  });

  it('should detect adoptedStyleSheets support', () => {
    const hasSupport = WebComponentFeatures.adoptedStyleSheets;
    expect(hasSupport).to.be.a('boolean');
  });

  it('should detect formAssociated support', () => {
    const hasSupport = WebComponentFeatures.formAssociated;
    expect(hasSupport).to.be.a('boolean');
  });

  it('should get all features', () => {
    const features = WebComponentFeatures.getAll();
    expect(features).to.have.property('customElements');
    expect(features).to.have.property('shadowDOM');
    expect(features).to.have.property('templates');
  });

  it('should determine if polyfills are needed', () => {
    const needs = WebComponentFeatures.needsPolyfills();
    expect(needs).to.be.a('boolean');
  });
});

describe('PolyfillLoader', () => {
  let loader;

  beforeEach(() => {
    loader = new PolyfillLoader();
  });

  it('should create loader instance', () => {
    expect(loader).to.exist;
    expect(loader.loaded).to.be.false;
    expect(loader.loading).to.be.false;
  });

  it('should have static ensurePolyfills method', () => {
    expect(PolyfillLoader.ensurePolyfills).to.be.a('function');
  });
});

describe('FeatureAwareComponent', () => {
  it('should be a function decorator', () => {
    expect(FeatureAwareComponent).to.be.a('function');
  });
});
