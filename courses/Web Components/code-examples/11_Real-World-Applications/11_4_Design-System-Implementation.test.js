/**
 * @group unit
 * @group real-world
 */
import { expect, fixture, html } from '@open-wc/testing';
import './11_4_Design-System-Implementation.js';

describe('DesignTokens', () => {
  it('should have colors', () => {
    expect(DesignTokens.colors).to.exist;
    expect(DesignTokens.colors.primary).to.exist;
    expect(DesignTokens.colors.primary[500]).to.equal('#667eea');
  });

  it('should have secondary colors', () => {
    expect(DesignTokens.colors.secondary).to.exist;
  });

  it('should have success colors', () => {
    expect(DesignTokens.colors.success).to.exist;
    expect(DesignTokens.colors.success[500]).to.equal('#22c55e');
  });

  it('should have warning colors', () => {
    expect(DesignTokens.colors.warning).to.exist;
    expect(DesignTokens.colors.warning[500]).to.equal('#eab308');
  });

  it('should have error colors', () => {
    expect(DesignTokens.colors.error).to.exist;
    expect(DesignTokens.colors.error[500]).to.equal('#ef4444');
  });

  it('should have spacing', () => {
    expect(DesignTokens.spacing).to.exist;
  });

  it('should have typography', () => {
    expect(DesignTokens.typography).to.exist;
  });

  it('should have breakpoints', () => {
    expect(DesignTokens.breakpoints).to.exist;
  });

  it('should have border radius', () => {
    expect(DesignTokens.borderRadius).to.exist;
  });

  it('should have shadows', () => {
    expect(DesignTokens.shadows).to.exist;
  });
});

describe('ComponentRegistry', () => {
  let registry;

  beforeEach(() => {
    registry = new ComponentRegistry();
  });

  it('should register components', () => {
    registry.register('button', { tagName: 'design-button' });
    const component = registry.get('button');
    expect(component.tagName).to.equal('design-button');
  });

  it('should get registered component', () => {
    registry.register('button', { tagName: 'design-button' });
    expect(registry.get('button')).to.exist;
  });

  it('should get all components', () => {
    registry.register('button', {});
    registry.register('input', {});
    const all = registry.getAll();
    expect(all.length).to.equal(2);
  });
});