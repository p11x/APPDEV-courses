/**
 * @group unit
 * @group real-world
 */
import { expect, fixture, html } from '@open-wc/testing';
import './11_7_Social-Media-Components.js';

describe('SocialShare', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<social-share></social-share>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should have default empty url', () => {
    expect(element.url).to.equal('');
  });

  it('should have default platforms', () => {
    expect(element.platforms).to.include('whatsapp');
    expect(element.platforms).to.include('facebook');
    expect(element.platforms).to.include('twitter');
    expect(element.platforms).to.include('linkedin');
  });

  it('should observe url attribute', () => {
    const observed = SocialShare.observedAttributes;
    expect(observed).to.include('url');
  });

  describe('Property Changes', () => {
    it('should set url', () => {
      element.url = 'https://example.com';
      expect(element.url).to.equal('https://example.com');
    });

    it('should set title', () => {
      element.title = 'Share Title';
      expect(element.title).to.equal('Share Title');
    });

    it('should set description', () => {
      element.description = 'Description';
      expect(element.description).to.equal('Description');
    });

    it('should set platforms', () => {
      element.platforms = ['instagram', 'telegram'];
      expect(element.platforms).to.include('instagram');
    });
  });

  describe('Lifecycle', () => {
    it('should initialize on connectedCallback', () => {
      expect(element.platforms).to.be.an('array');
    });
  });
});

describe('SocialLogin', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<social-login></social-login>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have default providers', () => {
    expect(element.providers).to.include('google');
    expect(element.providers).to.include('facebook');
  });
});

describe('FollowButton', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<follow-button></follow-button>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have default state', () => {
    expect(element.following).to.be.false;
  });
});