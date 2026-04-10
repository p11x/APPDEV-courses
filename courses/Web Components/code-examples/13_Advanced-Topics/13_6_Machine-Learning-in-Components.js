/**
 * Machine Learning in Components - ML Integration for Web Components
 * @description Integrate TensorFlow.js, ONNX, and ML models with Web Components for intelligent features
 * @module advanced/ml-components
 * @version 1.0.0
 */

(function() {
  'use strict';

  const ML_FEATURES = {
    tensorFlow: 'tf' in window,
    faceApi: 'faceapi' in window,
    handPose: 'handPoseDetection' in window,
    poseDetection: 'poseDetection' in window,
    cocoSsd: 'cocoSsd' in window,
    mobilenet: 'mobilenet' in window,
    toxicity: 'toxicity' in window
  };

  class TensorFlowModel {
    constructor(config = {}) {
      this.model = null;
      this.modelUrl = config.modelUrl || '';
      this.inputShape = config.inputShape || [224, 224, 3];
      this.outputClasses = config.outputClasses || [];
      this.isLoaded = false;
      this.loadingPromise = null;
    }

    async load() {
      if (this.isLoaded) return this.model;
      if (this.loadingPromise) return this.loadingPromise;

      this.loadingPromise = this._loadModel();
      return this.loadingPromise;
    }

    async _loadModel() {
      if (typeof tf === 'undefined') {
        console.warn('TensorFlow.js not loaded');
        return null;
      }

      try {
        this.model = await tf.loadLayers(this.modelUrl);
        this.isLoaded = true;
        return this.model;
      } catch (error) {
        console.error('Failed to load TensorFlow model:', error);
        throw error;
      }
    }

    async preprocess(input, options = {}) {
      const tensor = tf.browser.fromPixels(input);
      const resized = tf.image.resizeBilinear(tensor, [this.inputShape[0], this.inputShape[1]]);
      const normalized = resized.div(255.0);
      const expanded = normalized.expandDims(0);

      return expanded;
    }

    async predict(input) {
      if (!this.model) await this.load();

      const preprocessed = await this.preprocess(input);
      const prediction = await this.model.predict(preprocessed);
      const result = await prediction.data();

      preprocessed.dispose();
      prediction.dispose();

      return this.postprocess(result);
    }

    postprocess(output) {
      if (this.outputClasses.length === 0) {
        return Array.from(output);
      }

      const results = Array.from(output).map((score, index) => ({
        class: this.outputClasses[index],
        score
      }));

      return results.sort((a, b) => b.score - a.score);
    }

    dispose() {
      if (this.model) {
        this.model.dispose();
        this.model = null;
      }
      this.isLoaded = false;
    }
  }

  class ImageClassifier extends TensorFlowModel {
    constructor(config = {}) {
      super({
        ...config,
        modelUrl: config.modelUrl || 'https://storage.googleapis.com/tfjs-models/savedmodel/mobilenet_v2_1_0/default.json',
        outputClasses: config.outputClasses || ['airplane', 'automobile', 'bird', 'cat', 'deer', 'dog', 'frog', 'horse', 'ship', 'truck']
      });
    }
  }

  class ObjectDetector extends TensorFlowModel {
    constructor(config = {}) {
      super({
        ...config,
        modelUrl: config.modelUrl || 'https://storage.googleapis.com/tfjs-models/savedmodel/ssd_mobilenet_v2_2/default.json'
      });
      this.classNames = config.classNames || [
        'person', 'bicycle', 'car', 'motorcycle', 'airplane', 'bus', 'train', 'truck', 'boat',
        'traffic light', 'fire hydrant', 'stop sign', 'parking meter', 'bench', 'bird', 'cat',
        'dog', 'horse', 'sheep', 'cow', 'elephant', 'bear', 'zebra', 'giraffe', 'backpack',
        'umbrella', 'handbag', 'tie', 'suitcase', 'frisbee', 'skis', 'snowboard', 'sports ball',
        'kite', 'baseball bat', 'baseball glove', 'skateboard', 'surfboard', 'tennis racket',
        'bottle', 'wine glass', 'cup', 'fork', 'knife', 'spoon', 'bowl', 'banana', 'apple',
        'sandwich', 'orange', 'broccoli', 'carrot', 'hot dog', 'pizza', 'donut', 'cake',
        'chair', 'couch', 'potted plant', 'bed', 'dining table', 'toilet', 'tv', 'laptop',
        'mouse', 'remote', 'keyboard', 'cell phone', 'microwave', 'oven', 'toaster', 'sink',
        'refrigerator', 'book', 'clock', 'vase', 'scissors', 'teddy bear', 'hair drier',
        'toothbrush'
      ];
    }

    async detect(imageElement) {
      if (!this.model) await this.load();

      const inputTensor = tf.browser.fromPixels(imageElement);
      const resized = tf.image.resizeBilinear(inputTensor, [this.inputShape[0], this.inputShape[1]]);
      const normalized = resized.div(255.0).expandDims(0);

      const prediction = await this.model.predict(normalized);
      const boxes = await prediction[0].array();
      const scores = await prediction[1].array();
      const classIndices = await prediction[2].array();

      inputTensor.dispose();
      resized.dispose();
      normalized.dispose();
      prediction.forEach(t => t.dispose());

      return this.formatResults(boxes[0], scores[0], classIndices[0]);
    }

    formatResults(boxes, scores, classIndices) {
      const minScore = 0.5;
      const results = [];

      for (let i = 0; i < scores.length; i++) {
        if (scores[i] > minScore) {
          results.push({
            bbox: boxes[i],
            score: scores[i],
            class: this.classNames[classIndices[i]] || `class_${classIndices[i]}`
          });
        }
      }

      return results;
    }
  }

  class SentimentAnalyzer {
    constructor(config = {}) {
      this.model = null;
      this.vocab = new Map();
      this.maxLength = config.maxLength || 128;
      this.isLoaded = false;
    }

    async load(config = {}) {
      if (this.isLoaded) return this.model;

      this.vocab = new Map([
        ['good', 1], ['great', 2], ['excellent', 3], ['amazing', 4], ['wonderful', 5],
        ['bad', -1], ['terrible', -2], ['awful', -3], ['horrible', -4], ['poor', -5],
        ['ok', 0], ['okay', 0], ['fine', 0], ['nice', 1], ['love', 3], ['hate', -3]
      ]);

      this.isLoaded = true;
      return this;
    }

    tokenize(text) {
      return text.toLowerCase()
        .replace(/[^\w\s]/g, '')
        .split(/\s+/)
        .slice(0, this.maxLength);
    }

    async analyze(text) {
      const tokens = this.tokenize(text);
      let score = 0;
      let count = 0;
      const words = [];

      for (const token of tokens) {
        if (this.vocab.has(token)) {
          const value = this.vocab.get(token);
          score += value;
          count++;
          words.push({ word: token, score: value });
        }
      }

      const normalizedScore = count > 0 ? score / count : 0;
      const sentiment = normalizedScore > 0.2 ? 'positive' : normalizedScore < -0.2 ? 'negative' : 'neutral';

      return {
        text,
        sentiment,
        score: normalizedScore,
        confidence: Math.min(Math.abs(normalizedScore) * 2, 1),
        tokens: words,
        wordCount: tokens.length,
        analyzedWordCount: count
      };
    }

    async analyzeBatch(texts) {
      return Promise.all(texts.map(text => this.analyze(text)));
    }
  }

  class RecommendationEngine {
    constructor(config = {}) {
      this.items = config.items || [];
      this.features = config.features || [];
      this.userProfile = null;
      this.similarity = config.similarity || 'cosine';
    }

    setItems(items) {
      this.items = items;
    }

    setUserProfile(profile) {
      this.userProfile = profile;
    }

    calculateSimilarity(itemA, itemB) {
      const vectorA = this.extractFeatures(itemA);
      const vectorB = this.extractFeatures(itemB);

      if (this.similarity === 'cosine') {
        return this.cosineSimilarity(vectorA, vectorB);
      }

      return this.euclideanDistance(vectorA, vectorB);
    }

    extractFeatures(item) {
      return this.features.map(f => item[f] || 0);
    }

    cosineSimilarity(vecA, vecB) {
      const dotProduct = vecA.reduce((sum, a, i) => sum + a * (vecB[i] || 0), 0);
      const magnitudeA = Math.sqrt(vecA.reduce((sum, a) => sum + a * a, 0));
      const magnitudeB = Math.sqrt(vecB.reduce((sum, b) => sum + b * b, 0));

      if (magnitudeA === 0 || magnitudeB === 0) return 0;
      return dotProduct / (magnitudeA * magnitudeB);
    }

    euclideanDistance(vecA, vecB) {
      const sum = vecA.reduce((total, a, i) => {
        const b = vecB[i] || 0;
        return total + Math.pow(a - b, 2);
      }, 0);
      return Math.sqrt(sum);
    }

    recommend(userId, options = {}) {
      const limit = options.limit || 10;
      const minScore = options.minScore || 0;

      if (!this.userProfile) {
        return this.items.slice(0, limit);
      }

      const scores = this.items.map(item => ({
        item,
        score: this.calculateSimilarity(this.userProfile, item)
      }));

      return scores
        .filter(s => s.score >= minScore)
        .sort((a, b) => b.score - a.score)
        .slice(0, limit)
        .map(s => ({ ...s.item, recommendationScore: s.score }));
    }
  }

  class MLWebComponent extends HTMLElement {
    static get observedAttributes() {
      return ['model', 'mode', 'threshold'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._model = null;
      this._mode = 'classify';
      this._threshold = 0.5;
    }

    static get observedAttributes() {
      return ['model', 'mode', 'threshold', 'model-url'];
    }

    connectedCallback() {
      this._mode = this.getAttribute('mode') || 'classify';
      this._threshold = parseFloat(this.getAttribute('threshold') || '0.5');
      this.initializeModel();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'mode') {
          this._mode = newValue;
        } else if (name === 'threshold') {
          this._threshold = parseFloat(newValue);
        } else if (name === 'model-url') {
          this.initializeModel();
        }
      }
    }

    async initializeModel() {
      const modelType = this.getAttribute('model') || 'image';
      const modelUrl = this.getAttribute('model-url');

      if (modelType === 'image') {
        this._model = new ImageClassifier({ modelUrl });
      } else if (modelType === 'object') {
        this._model = new ObjectDetector({ modelUrl });
      } else if (modelType === 'sentiment') {
        this._model = new SentimentAnalyzer();
      }

      if (this._model) {
        await this._model.load();
      }
    }

    async predict(input) {
      if (!this._model) {
        throw new Error('Model not loaded');
      }

      if (this._model instanceof ImageClassifier || this._model instanceof ObjectDetector) {
        return this._model.predict(input);
      } else if (this._model instanceof SentimentAnalyzer) {
        return this._model.analyze(input);
      }

      return this._model.predict(input);
    }

    async detectObjects(imageElement) {
      if (!this._model || !(this._model instanceof ObjectDetector)) {
        throw new Error('Object detection model not loaded');
      }

      return this._model.detect(imageElement);
    }

    async analyzeSentiment(text) {
      if (!this._model || !(this._model instanceof SentimentAnalyzer)) {
        throw new Error('Sentiment model not loaded');
      }

      return this._model.analyze(text);
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
          .ml-container {
            padding: 16px;
            font-family: system-ui, -apple-system, sans-serif;
          }
          .status {
            display: flex;
            align-items: center;
            gap: 8px;
          }
          .status-indicator {
            width: 8px;
            height: 8px;
            border-radius: 50%;
            background: #10b981;
          }
          .status-indicator.loading {
            background: #f59e0b;
            animation: pulse 1s infinite;
          }
          .status-indicator.error {
            background: #ef4444;
          }
          @keyframes pulse {
            0%, 100% { opacity: 1; }
            50% { opacity: 0.5; }
          }
        </style>
        <div class="ml-container">
          <slot></slot>
        </div>
      `;
    }
  }

  class ImageClassificationResult extends HTMLElement {
    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
    }

    static get observedAttributes() {
      return ['accuracy', 'label', 'show-percentage'];
    }

    connectedCallback() {
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        this.render();
      }
    }

    setResult(predictions) {
      this._predictions = predictions;
      this.render();
    }

    render() {
      const prediction = this._predictions?.[0];

      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
          .result {
            padding: 8px 12px;
            background: var(--color-surface, #f3f4f6);
            border-radius: 6px;
          }
          .label {
            font-weight: 500;
          }
          .confidence {
            color: var(--color-text-secondary, #6b7280);
            font-size: 14px;
          }
        </style>
        <div class="result">
          ${prediction ? `
            <div class="label">${prediction.class || prediction}</div>
            <div class="confidence">
              ${(prediction.score * 100).toFixed(1)}% confidence
            </div>
          ` : '<slot></slot>'}
        </div>
      `;
    }
  }

  customElements.define('ml-component', MLWebComponent);
  customElements.define('image-classification-result', ImageClassificationResult);

  if (typeof window !== 'undefined') {
    window.MLComponents = {
      ML_FEATURES,
      TensorFlowModel,
      ImageClassifier,
      ObjectDetector,
      SentimentAnalyzer,
      RecommendationEngine,
      MLWebComponent,
      ImageClassificationResult
    };
  }

  export {
    ML_FEATURES,
    TensorFlowModel,
    ImageClassifier,
    ObjectDetector,
    SentimentAnalyzer,
    RecommendationEngine,
    MLWebComponent,
    ImageClassificationResult
  };
})();