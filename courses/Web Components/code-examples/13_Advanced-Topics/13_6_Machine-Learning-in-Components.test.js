/**
 * @group unit
 * @group advanced-topics
 */
import { expect, fixture, html } from '@open-wc/testing';
import './13_6_Machine-Learning-in-Components.js';

describe('Machine Learning in Components', () => {
  it('should have ML_FEATURES', () => {
    expect(ML_FEATURES).to.exist;
    expect(ML_FEATURES.tensorFlow).to.be.a('boolean');
  });

  it('should have faceApi feature', () => {
    expect(ML_FEATURES).to.have.property('faceApi');
  });

  it('should have poseDetection feature', () => {
    expect(ML_FEATURES).to.have.property('poseDetection');
  });

  it('should have cocoSsd feature', () => {
    expect(ML_FEATURES).to.have.property('cocoSsd');
  });
});

describe('TensorFlowModel', () => {
  let model;

  beforeEach(() => {
    model = new TensorFlowModel({ modelUrl: '/model/model.json' });
  });

  it('should create model', () => {
    expect(model).to.exist;
    expect(model.modelUrl).to.equal('/model/model.json');
    expect(model.isLoaded).to.be.false;
  });

  it('should have input shape', () => {
    expect(model.inputShape).to.deep.equal([224, 224, 3]);
  });

  it('should have output classes', () => {
    expect(model.outputClasses).to.be.an('array');
  });

  it('should predict', async () => {
    model.isLoaded = true;
    model.model = { predict: () => [[0.1, 0.9]] };
    const result = await model.predict([1, 2, 3]);
    expect(result).to.exist;
  });
});

describe('ModelLoader', () => {
  let loader;

  beforeEach(() => {
    loader = new ModelLoader();
  });

  it('should create loader', () => {
    expect(loader).to.exist;
  });

  it('should load model', () => {
    loader.load('mobilenet', {});
    expect(loader.loadedModels.has('mobilenet')).to.be.true;
  });
});