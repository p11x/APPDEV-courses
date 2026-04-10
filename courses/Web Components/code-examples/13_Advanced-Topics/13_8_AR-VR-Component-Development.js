/**
 * AR VR Component Development - WebXR Components
 * @description Build Augmented Reality and Virtual Reality experiences using WebXR API
 * @module advanced/ar-vr-components
 * @version 1.0.0
 */

(function() {
  'use strict';

  const XR_FEATURES = {
    immersiveVR: 'immersive-vr',
    immersiveAR: 'immersive-ar',
    localFloor: 'local-floor',
    boundedFloor: 'bounded-floor',
    handTracking: 'hand-tracking',
    eyeTracking: 'eye-tracking',
    layers: 'layers'
  };

  const DEFAULT_CONFIG = {
    referenceSpace: 'local-floor',
    requiredFeatures: ['local-floor'],
    optionalFeatures: []
  };

  class XRManager {
    constructor(config = {}) {
      this.config = { ...DEFAULT_CONFIG, ...config };
      this.xrSession = null;
      this.xrReferenceSpace = null;
      this.viewerSpace = null;
      this.refSpace = null;
      this.isSupported = { ar: false, vr: false };
      this.layers = [];
      this.checkSupport();
    }

    async checkSupport() {
      if (!navigator.xr) {
        this.isSupported = { ar: false, vr: false };
        return this.isSupported;
      }

      this.isSupported.vr = await navigator.xr.isSessionSupported('immersive-vr');
      this.isSupported.ar = await navigator.xr.isSessionSupported('immersive-ar');

      return this.isSupported;
    }

    async startSession(mode = 'immersive-vr', options = {}) {
      if (!navigator.xr) {
        throw new Error('WebXR not supported');
      }

      const sessionInit = {
        requiredFeatures: options.requiredFeatures || this.config.requiredFeatures,
        optionalFeatures: options.optionalFeatures || this.config.optionalFeatures,
        outputContext: options.outputContext
      };

      try {
        this.xrSession = await navigator.xr.requestSession(mode, sessionInit);
        
        this.xrSession.addEventListener('end', () => {
          this.onSessionEnd();
        });

        this.xrSession.addEventListener('inputsourceschange', (event) => {
          this.onInputSourcesChange(event);
        });

        this.viewerSpace = await this.xrSession.requestReferenceSpace('viewer');
        this.refSpace = await this.xrSession.requestReferenceSpace(
          options.referenceSpace || this.config.referenceSpace
        );

        return this.xrSession;
      } catch (error) {
        console.error('Failed to start XR session:', error);
        throw error;
      }
    }

    async endSession() {
      if (this.xrSession) {
        await this.xrSession.end();
        this.xrSession = null;
      }
    }

    onSessionEnd() {
      this.xrSession = null;
      this.dispatchEvent(new CustomEvent('xr-session-end', { bubbles: true, composed: true }));
    }

    onInputSourcesChange(event) {
      this.dispatchEvent(new CustomEvent('xr-input-change', {
        detail: { added: event.added, removed: event.removed },
        bubbles: true,
        composed: true
      }));
    }

    getInputSources() {
      return this.xrSession?.inputSources || [];
    }

    getSupportedModes() {
      return this.isSupported;
    }

    isSessionActive() {
      return this.xrSession !== null;
    }
  }

  class XRScene {
    constructor(manger) {
      this.manager = manager;
      this.objects = [];
      this.controllers = [];
    }

    addObject(object) {
      this.objects.push(object);
    }

    removeObject(object) {
      const index = this.objects.indexOf(object);
      if (index > -1) {
        this.objects.splice(index, 1);
      }
    }

    update(timestamp, frame) {
      this.objects.forEach(obj => {
        if (obj.update) {
          obj.update(timestamp, frame);
        }
      });
    }

    render(renderer, frame) {
      const session = renderer.xrSession;
      if (!session) return;

      const baseLayer = new XRWebGLLayer(session, renderer);
      session.requestAnimationFrame(this.renderFrame.bind(this, renderer));
    }

    renderFrame(renderer, frame) {
      this.objects.forEach(obj => {
        if (obj.render) {
          obj.render(renderer, frame);
        }
      });
    }
  }

  class XRController {
    constructor(inputSource) {
      this.inputSource = inputSource;
      this.gamepad = inputSource.gamepad;
      this.targetRayMode = inputSource.targetRayMode;
      this.handedness = inputSource.handedness;
      this.position = { x: 0, y: 0, z: 0 };
      this.rotation = { x: 0, y: 0, z: 0 };
      this.trigger = 0;
      this.squeeze = 0;
      this.touchpad = { x: 0, y: 0, pressed: false };
      this.thumbstick = { x: 0, y: 0, pressed: false };
      this.buttons = [];
      this.isActive = true;
    }

    update(frame) {
      if (!this.inputSource) return;

      const pose = frame.getPose(this.inputSource.targetRaySpace, this.manager.refSpace);
      if (pose) {
        this.position = pose.transform.position;
        this.rotation = pose.transform.orientation;
      }

      if (this.gamepad) {
        if (this.gamepad.buttons.length > 0) {
          this.trigger = this.gamepad.buttons[0].value;
          this.buttons = this.gamepad.buttons;
        }
        if (this.gamepad.axes.length >= 4) {
          this.thumbstick.x = this.gamepad.axes[2];
          this.thumbstick.y = this.gamepad.axes[3];
        }
      }
    }

    isPressed(buttonIndex = 0) {
      return this.gamepad?.buttons[buttonIndex]?.pressed || false;
    }

    getAxis(axisIndex = 0) {
      return this.gamepad?.axes[axisIndex] || 0;
    }
  }

  class XRHandController extends XRController {
    constructor(inputSource, handedness) {
      super(inputSource);
      this.handedness = handedness;
      this.joints = [];
      this.isPinching = false;
      this.isGrabbing = false;
      this.pointerPose = null;
    }

    update(frame) {
      super.update(frame);

      if (this.inputSource.hand) {
        this.updateHandJoints(frame);
      }
    }

    updateHandJoints(frame) {
      const hand = this.inputSource.hand;
      const session = this.manager.xrSession;

      for (const jointName of hand.keys()) {
        const jointSpace = hand.get(jointName);
        const pose = frame.getPose(jointSpace, this.manager.refSpace);

        if (pose) {
          this.joints[jointName] = pose.transform;
        }
      }

      if (hand.get('index-finger-tip') && hand.get('thumb-tip')) {
        this.isPinching = this.checkPinch(hand);
      }
    }

    checkPinch(hand) {
      return false;
    }

    getJointPosition(jointName) {
      const joint = this.joints[jointName];
      return joint ? { x: joint.position.x, y: joint.position.y, z: joint.position.z } : null;
    }
  }

  class AROverlay extends HTMLElement {
    static get observedAttributes() {
      return ['enabled'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._xrManager = null;
      this._enabled = false;
      this._canvas = null;
    }

    static get observedAttributes() {
      return ['enabled', 'hit-test'];
    }

    connectedCallback() {
      this._xrManager = new XRManager();
      this.initializeAR();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'enabled') {
          this._enabled = newValue !== null;
          this.toggleAR();
        }
      }
    }

    async checkSupport() {
      return this._xrManager.getSupportedModes();
    }

    async initializeAR() {
      const supported = await this.checkSupport();
      if (!supported.ar) {
        console.warn('AR not supported');
        return;
      }

      this.render();
    }

    async toggleAR() {
      if (this._enabled) {
        await this.startAR();
      } else {
        await this.stopAR();
      }
    }

    async startAR() {
      try {
        await this._xrManager.startSession('immersive-ar');
        this.dispatchEvent(new CustomEvent('ar-started', { bubbles: true, composed: true }));
      } catch (error) {
        console.error('Failed to start AR:', error);
      }
    }

    async stopAR() {
      await this._xrManager.endSession();
      this.dispatchEvent(new CustomEvent('ar-stopped', { bubbles: true, composed: true }));
    }

    onHitTestResult(results) {
      this.dispatchEvent(new CustomEvent('hit-result', {
        detail: { results },
        bubbles: true,
        composed: true
      }));
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
        </style>
        <slot></slot>
      `;
    }
  }

  class VRScene extends HTMLElement {
    static get observedAttributes() {
      return ['environment'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._xrManager = new XRManager();
      this._environment = 'basic';
      this._scene = null;
      this._renderLoop = null;
    }

    static get observedAttributes() {
      return ['environment', 'stereo', 'foveated'];
    }

    connectedCallback() {
      this._environment = this.getAttribute('environment') || 'basic';
      this._scene = new XRScene(this._xrManager);
      this.initializeVR();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'environment') {
          this._environment = newValue;
        }
      }
    }

    async checkSupport() {
      return this._xrManager.getSupportedModes();
    }

    async enter() {
      try {
        await this._xrManager.startSession('immersive-vr');
        this.startRenderLoop();
        
        this.dispatchEvent(new CustomEvent('vr-enter', { bubbles: true, composed: true }));
      } catch (error) {
        console.error('Failed to enter VR:', error);
      }
    }

    async exit() {
      await this._xrManager.endSession();
      this.stopRenderLoop();
      
      this.dispatchEvent(new CustomEvent('vr-exit', { bubbles: true, composed: true }));
    }

    startRenderLoop() {
      const render = (timestamp, frame) => {
        this._scene?.update(timestamp, frame);
        this._scene?.render(frame);
        this._renderLoop = frame.requestAnimationFrame(render);
      };
      
      this._renderLoop = this._xrManager.xrSession.requestAnimationFrame(render);
    }

    stopRenderLoop() {
      if (this._renderLoop) {
        cancelAnimationFrame(this._renderLoop);
        this._renderLoop = null;
      }
    }

    add3DObject(object) {
      this._scene?.addObject(object);
    }

    remove3DObject(object) {
      this._scene?.removeObject(object);
    }

    renderEnvironment() {
      return {
        skybox: this._environment !== 'basic',
        grid: this._environment === 'grid',
        studio: this._environment === 'studio'
      };
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
          .vr-button {
            padding: 16px 32px;
            background: #667eea;
            color: white;
            border: none;
            border-radius: 8px;
            font-size: 16px;
            cursor: pointer;
          }
          .vr-button:hover {
            background: #5a6fd6;
          }
        </style>
        <button class="vr-button">Enter VR</button>
      `;

      const button = this.shadowRoot.querySelector('.vr-button');
      button?.addEventListener('click', () => this.enter());
    }
  }

  class XRInputController extends HTMLElement {
    static get observedAttributes() {
      return ['tracked'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._controllers = [];
      this._tracked = false;
    }

    static get observedAttributes() {
      return ['tracked', 'handedness'];
    }

    connectedCallback() {
      this._tracked = this.hasAttribute('tracked');
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'tracked') {
          this._tracked = newValue !== null;
        }
      }
    }

    updateControllers(inputSources) {
      this._controllers = Array.from(inputSources).map(source => {
        if (source.handedness) {
          return new XRHandController(source, source.handedness);
        }
        return new XRController(source);
      });

      this.dispatchEvent(new CustomEvent('controllers-updated', {
        detail: { controllers: this._controllers },
        bubbles: true,
        composed: true
      }));
    }

    get controllers() {
      return this._controllers;
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: none;
          }
        </style>
      `;
    }
  }

  customElements.define('ar-overlay', AROverlay);
  customElements.define('vr-scene', VRScene);
  customElements.define('xr-input-controller', XRInputController);

  if (typeof window !== 'undefined') {
    window.ARVRComponents = {
      XR_FEATURES,
      XRManager,
      XRScene,
      XRController,
      XRHandController,
      AROverlay,
      VRScene,
      XRInputController
    };
  }

  export {
    XR_FEATURES,
    XRManager,
    XRScene,
    XRController,
    XRHandController,
    AROverlay,
    VRScene,
    XRInputController
  };
})();