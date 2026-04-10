/**
 * @group unit
 * @group reference-materials
 */
import { expect, fixture, html } from '@open-wc/testing';
import './14_5_Accessibility-Checklist.css';

describe('Accessibility Checklist', () => {
  it('should have CSS styles', () => {
    const style = document.querySelector('style');
    expect(style).to.exist;
  });
});

describe('AccessibilityChecker', () => {
  let checker;

  beforeEach(() => {
    checker = new AccessibilityChecker();
  });

  it('should create checker', () => {
    expect(checker).to.exist;
  });

  it('should check accessibility', () => {
    const result = checker.check({});
    expect(result).to.exist;
  });
});