/**
 * @group unit
 * @group advanced-topics
 */
import { expect, fixture, html } from '@open-wc/testing';
import './13_2_Advanced-Animation-Frameworks.js';

describe('Advanced Animation Frameworks', () => {
  it('should have AnimationPresets', () => {
    expect(AnimationPresets).to.exist;
    expect(AnimationPresets.fadeIn).to.exist;
    expect(AnimationPresets.slideInUp).to.exist;
    expect(AnimationPresets.bounce).to.exist;
  });

  it('should have fadeIn preset', () => {
    expect(AnimationPresets.fadeIn.opacity.from).to.equal(0);
    expect(AnimationPresets.fadeIn.opacity.to).to.equal(1);
    expect(AnimationPresets.fadeIn.duration).to.equal(0.3);
  });

  it('should have slideInUp preset', () => {
    expect(AnimationPresets.slideInUp.transform.from).to.include('translateY');
    expect(AnimationPresets.slideInUp.duration).to.equal(0.4);
  });

  it('should have bounce preset', () => {
    expect(AnimationPresets.bounce.keyframes).to.be.an('array');
    expect(AnimationPresets.bounce.keyframes.length).to.be.greaterThan(0);
  });

  it('should have shake preset', () => {
    expect(AnimationPresets.shake.transform.from).to.include('translateX');
  });

  it('should have pulse preset', () => {
    expect(AnimationPresets.pulse.repeat).to.equal(1);
    expect(AnimationPresets.pulse.yoyo).to.be.true;
  });
});

describe('AnimationController', () => {
  let controller;

  beforeEach(() => {
    const element = document.createElement('div');
    controller = new AnimationController(element);
  });

  it('should create controller', () => {
    expect(controller).to.exist;
    expect(controller.isAnimating).to.be.false;
  });

  it('should have animation queue', () => {
    expect(controller.animationQueue).to.be.an('array');
  });

  it('should set duration', () => {
    controller.setDuration(500);
    expect(controller._duration).to.equal(500);
  });

  it('should set easing', () => {
    controller.setEasing('linear');
    expect(controller._easing).to.equal('linear');
  });

  it('should check if paused', () => {
    expect(controller.isPaused).to.be.false;
    controller.pause();
    expect(controller.isPaused).to.be.true;
  });

  it('should have easeOutCubic easing', () => {
    expect(controller.easeOutCubic).to.be.a('function');
  });
});

describe('GSAPIntegration', () => {
  let integration;

  beforeEach(() => {
    integration = new GSAPIntegration();
  });

  it('should create integration', () => {
    expect(integration).to.exist;
  });

  it('should initialize GSAP', () => {
    integration.initGSAP({});
    expect(integration.gsap).to.exist;
  });

  it('should set timeline', () => {
    integration.setTimeline({});
    expect(integration.timeline).to.exist;
  });
});