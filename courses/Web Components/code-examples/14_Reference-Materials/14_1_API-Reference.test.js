/**
 * @group unit
 * @group reference-materials
 */
import { expect, fixture, html } from '@open-wc/testing';
import './14_1_API-Reference.js';

describe('APIReference', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<api-reference></api-reference>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should have default title', () => {
    expect(element.title).to.equal('API Reference');
  });

  it('should have default version', () => {
    expect(element.version).to.equal('1.0.0');
  });

  it('should have default theme', () => {
    expect(element.theme).to.equal('light');
  });

  it('should observe title attribute', () => {
    const observed = APIReference.observedAttributes;
    expect(observed).to.include('title');
  });

  describe('Property Changes', () => {
    it('should set title', () => {
      element.title = 'New Title';
      expect(element.title).to.equal('New Title');
    });

    it('should set version', () => {
      element.version = '2.0.0';
      expect(element.version).to.equal('2.0.0');
    });

    it('should set theme', () => {
      element.theme = 'dark';
      expect(element.theme).to.equal('dark');
    });

    it('should set data', () => {
      element.data = [{ id: 1, name: 'test' }];
      expect(element.data.length).to.equal(1);
    });

    it('should check compact', () => {
      expect(element.compact).to.be.false;
    });
  });

  describe('Events', () => {
    it('should have lifecycle hooks map', () => {
      expect(element._lifecycleHooks).to.be.a('Map');
    });
  });
});

describe('APIDocumentation', () => {
  let docs;

  beforeEach(() => {
    docs = new APIDocumentation();
  });

  it('should create docs', () => {
    expect(docs).to.exist;
  });

  it('should add section', () => {
    docs.addSection('Overview', 'Content');
    expect(docs.sections.size).to.equal(1);
  });
});