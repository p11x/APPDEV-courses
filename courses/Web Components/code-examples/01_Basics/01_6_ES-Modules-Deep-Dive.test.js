/**
 * @group unit
 * @group es-modules
 */
import { expect, fixture, html } from '@open-wc/testing';
import './01_6_ES-Modules-Deep-Dive.js';

describe('ComponentLoader', () => {
  let loader;

  beforeEach(() => {
    loader = new ComponentLoader();
  });

  it('should create loader instance', () => {
    expect(loader).to.exist;
  });

  it('should have cache', () => {
    expect(loader._cache).to.be.instanceOf(Map);
  });

  it('should check if component is cached', () => {
    expect(loader.has('test')).to.be.false;
  });

  it('should clear cache', () => {
    loader.clear();
    expect(loader._cache.size).to.equal(0);
  });
});

describe('LazyComponent', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<lazy-component></lazy-component>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should show loading state initially', () => {
    const spinner = element.shadowRoot.querySelector('.spinner');
    expect(spinner).to.exist;
  });

  it('should have loaded flag', () => {
    expect(element._loaded).to.be.a('boolean');
  });
});
