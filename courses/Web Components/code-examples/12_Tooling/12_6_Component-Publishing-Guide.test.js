/**
 * @group unit
 * @group tooling
 */
import { expect, fixture, html } from '@open-wc/testing';
import './12_6_Component-Publishing-Guide.js';

describe('ComponentPublishingGuide', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<component-publishing-guide></component-publishing-guide>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should have default package name', () => {
    expect(element._packageInfo.name).to.equal('my-web-components');
  });

  it('should have default version', () => {
    expect(element._packageInfo.version).to.equal('1.0.0');
  });

  it('should observe name attribute', () => {
    const observed = ComponentPublishingGuide.observedAttributes;
    expect(observed).to.include('name');
  });

  describe('Property Changes', () => {
    it('should set package name', () => {
      element._packageInfo.name = 'new-components';
      expect(element._packageInfo.name).to.equal('new-components');
    });

    it('should set version', () => {
      element._packageInfo.version = '2.0.0';
      expect(element._packageInfo.version).to.equal('2.0.0');
    });

    it('should set license', () => {
      element._packageInfo.license = 'Apache-2.0';
      expect(element._packageInfo.license).to.equal('Apache-2.0');
    });
  });

  describe('Events', () => {
    it('should track is published', () => {
      expect(element._isPublished).to.be.false;
    });

    it('should have versions array', () => {
      expect(element._versions).to.be.an('array');
    });
  });
});

describe('PackageBuilder', () => {
  let builder;

  beforeEach(() => {
    builder = new PackageBuilder();
  });

  it('should create package config', () => {
    const config = builder.createConfig();
    expect(config).to.exist;
  });

  it('should set entry point', () => {
    builder.setEntryPoint('./dist/index.js');
    expect(builder.entryPoint).to.equal('./dist/index.js');
  });

  it('should add export', () => {
    builder.addExport('./button', './dist/button.js');
    expect(builder.exports['./button']).to.equal('./dist/button.js');
  });
});

describe('NPMPublisher', () => {
  let publisher;

  beforeEach(() => {
    publisher = new NPMPublisher();
  });

  it('should create publisher config', () => {
    const config = publisher.createConfig();
    expect(config).to.exist;
  });

  it('should set registry', () => {
    publisher.setRegistry('https://registry.npmjs.org');
    expect(publisher.registry).to.equal('https://registry.npmjs.org');
  });
});