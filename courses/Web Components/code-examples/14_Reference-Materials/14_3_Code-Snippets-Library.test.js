/**
 * @group unit
 * @group reference-materials
 */
import { expect, fixture, html } from '@open-wc/testing';
import './14_3_Code-Snippets-Library.css';

describe('Code Snippets Library', () => {
  it('should have CSS styles', () => {
    const style = document.querySelector('style');
    expect(style).to.exist;
  });
});

describe('SnippetManager', () => {
  let manager;

  beforeEach(() => {
    manager = new SnippetManager();
  });

  it('should create manager', () => {
    expect(manager).to.exist;
  });

  it('should add snippet', () => {
    manager.addSnippet('button', '<button>Click</button>');
    expect(manager.getSnippet('button')).to.exist;
  });
});