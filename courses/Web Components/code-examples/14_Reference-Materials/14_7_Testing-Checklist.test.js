/**
 * @group unit
 * @group reference-materials
 */
import { expect, fixture, html } from '@open-wc/testing';
import './14_7_Testing-Checklist.css';

describe('Testing Checklist', () => {
  it('should have CSS styles', () => {
    const style = document.querySelector('style');
    expect(style).to.exist;
  });
});

describe('TestCoverageChecker', () => {
  let checker;

  beforeEach(() => {
    checker = new TestCoverageChecker();
  });

  it('should create checker', () => {
    expect(checker).to.exist;
  });

  it('should check coverage', () => {
    const result = checker.checkCoverage({});
    expect(result).to.exist;
  });
});