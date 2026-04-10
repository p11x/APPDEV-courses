/**
 * @group unit
 * @group advanced-topics
 */
import { expect, fixture, html } from '@open-wc/testing';
import './13_8_AR-VR-Component-Development.js';

describe('AR VR Component Development', () => {
  it('should have XR_FEATURES', () => {
    expect(XR_FEATURES).to.exist;
    expect(XR_FEATURES.immersiveVR).to.equal('immersive-vr');
    expect(XR_FEATURES.immersiveAR).to.equal('immersive-ar');
    expect(XR_FEATURES.localFloor).to.equal('local-floor');
    expect(XR_FEATURES.handTracking).to.equal('hand-tracking');
  });

  it('should have DEFAULT_CONFIG', () => {
    expect(DEFAULT_CONFIG.referenceSpace).to.equal('local-floor');
    expect(DEFAULT_CONFIG.requiredFeatures).to.include('local-floor');
  });

  it('should have boundedFloor feature', () => {
    expect(XR_FEATURES.boundedFloor).to.equal('bounded-floor');
  });

  it('should have eyeTracking feature', () => {
    expect(XR_FEATURES.eyeTracking).to.equal('eye-tracking');
  });
});

describe('XRManager', () => {
  let manager;

  beforeEach(() => {
    manager = new XRManager();
  });

  it('should create manager', () => {
    expect(manager).to.exist;
    expect(manager.isSupported).to.be.an('object');
  });

  it('should have config', () => {
    expect(manager.config.referenceSpace).to.equal('local-floor');
  });

  it('should have layers', () => {
    expect(manager.layers).to.be.an('array');
  });

  it('should check support', () => {
    manager.checkSupport();
    expect(manager.isSupported).to.exist;
  });
});

describe('ARScene', () => {
  let scene;

  beforeEach(() => {
    scene = new ARScene();
  });

  it('should create scene', () => {
    expect(scene).to.exist;
  });

  it('should add object', () => {
    scene.addObject({ id: 'test' });
    expect(scene.objects.length).to.equal(1);
  });
});