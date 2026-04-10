/**
 * @group unit
 * @group reference-materials
 */
import { expect, fixture, html } from '@open-wc/testing';
import './14_6_Security-Checklist.css';

describe('Security Checklist', () => {
  it('should have CSS styles', () => {
    const style = document.querySelector('style');
    expect(style).to.exist;
  });
});

describe('SecurityAuditor', () => {
  let auditor;

  beforeEach(() => {
    auditor = new SecurityAuditor();
  });

  it('should create auditor', () => {
    expect(auditor).to.exist;
  });

  it('should audit', () => {
    const result = auditor.audit({});
    expect(result).to.exist;
  });
});