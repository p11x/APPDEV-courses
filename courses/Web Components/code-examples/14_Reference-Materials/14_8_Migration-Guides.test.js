/**
 * @group unit
 * @group reference-materials
 */
import { expect, fixture, html } from '@open-wc/testing';
import './14_8_Migration-Guides.css';

describe('Migration Guides', () => {
  it('should have CSS styles', () => {
    const style = document.querySelector('style');
    expect(style).to.exist;
  });
});

describe('MigrationPlanner', () => {
  let planner;

  beforeEach(() => {
    planner = new MigrationPlanner();
  });

  it('should create planner', () => {
    expect(planner).to.exist;
  });

  it('should add migration step', () => {
    planner.addStep('v1', 'v2', {});
    expect(planner.steps.length).to.equal(1);
  });
});