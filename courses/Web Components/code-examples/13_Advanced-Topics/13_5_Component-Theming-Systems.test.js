/**
 * @group unit
 * @group advanced-topics
 */
import { expect, fixture, html } from '@open-wc/testing';
import './13_5_Component-Theming-Systems.js';

describe('Component Theming Systems', () => {
  it('should have DesignTokens', () => {
    expect(DesignTokens).to.exist;
    expect(DesignTokens.colors).to.exist;
    expect(DesignTokens.spacing).to.exist;
    expect(DesignTokens.typography).to.exist;
  });

  it('should have color tokens', () => {
    expect(DesignTokens.colors.primary.value).to.equal('#667eea');
    expect(DesignTokens.colors.primary.cssVar).to.equal('--color-primary');
  });

  it('should have spacing tokens', () => {
    expect(DesignTokens.spacing.xs.value).to.equal('4px');
    expect(DesignTokens.spacing.md.value).to.equal('16px');
    expect(DesignTokens.spacing.xl.value).to.equal('32px');
  });

  it('should have typography tokens', () => {
    expect(DesignTokens.typography['font-family'].value).to.include('system-ui');
    expect(DesignTokens.typography['font-size-base'].value).to.equal('16px');
  });

  it('should have border radius tokens', () => {
    expect(DesignTokens.borderRadius).to.exist;
    expect(DesignTokens.borderRadius.sm.value).to.equal('4px');
  });

  it('should have shadows', () => {
    expect(DesignTokens.shadows).to.exist;
    expect(DesignTokens.shadows.sm.value).to.exist;
  });
});

describe('ThemeManager', () => {
  let manager;

  beforeEach(() => {
    manager = new ThemeManager();
  });

  it('should create manager', () => {
    expect(manager).to.exist;
  });

  it('should get current theme', () => {
    expect(manager.currentTheme).to.exist;
  });

  it('should set theme', () => {
    manager.setTheme('dark');
    expect(manager.currentTheme).to.equal('dark');
  });

  it('should have themes', () => {
    expect(manager.themes).to.be.a('Map');
  });
});

describe('ThemeProvider', () => {
  let provider;

  beforeEach(() => {
    provider = new ThemeProvider();
  });

  it('should create provider', () => {
    expect(provider).to.exist;
  });

  it('should provide theme', () => {
    expect(provider.provide).to.be.a('function');
  });
});