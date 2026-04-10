/**
 * @group unit
 * @group html-standards
 */
import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import './01_2_HTML-Standards-and-History.js';

describe('ArticleElement', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<semantic-article author="Test Author" category="Tech"></semantic-article>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should handle author attribute', async () => {
    expect(element.author).to.equal('Test Author');
  });

  it('should handle category attribute', async () => {
    expect(element.category).to.equal('Tech');
  });

  it('should handle publish-date attribute', async () => {
    element.setAttribute('publish-date', '2024-01-15');
    await element.updateComplete;
    expect(element.publishDate).to.equal('2024-01-15');
  });

  it('should have default author', async () => {
    const el = await fixture(html`<semantic-article></semantic-article>`);
    expect(el.author).to.equal('Anonymous');
  });

  it('should update on attribute change', async () => {
    element.setAttribute('author', 'New Author');
    await element.updateComplete;
    expect(element.author).to.equal('New Author');
  });
});

describe('SemanticFormElement', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<semantic-form></semantic-form>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have form element', () => {
    const form = element.shadowRoot.querySelector('form');
    expect(form).to.exist;
  });

  it('should have proper fields', () => {
    const inputs = element.shadowRoot.querySelectorAll('input');
    expect(inputs.length).to.be.greaterThan(0);
  });

  it('should dispatch form-submit event', async () => {
    const input = element.shadowRoot.querySelector('input#name');
    input.value = 'Test User';
    input.dispatchEvent(new Event('input', { bubbles: true }));
    
    const form = element.shadowRoot.querySelector('form');
    form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
  });
});

describe('SemanticNavElement', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<semantic-nav></semantic-nav>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have navigation element', () => {
    const nav = element.shadowRoot.querySelector('nav');
    expect(nav).to.exist;
  });

  it('should have nav links', () => {
    const links = element.shadowRoot.querySelectorAll('a');
    expect(links.length).to.be.greaterThan(0);
  });

  it('should have proper accessibility', () => {
    const nav = element.shadowRoot.querySelector('nav');
    expect(nav.getAttribute('role')).to.equal('navigation');
    expect(nav.getAttribute('aria-label')).to.exist;
  });
});
