/**
 * Progressive Web Apps Integration - PWA Features for Web Components
 * @description Add Service Worker, offline support, push notifications, and installability to Web Components
 * @module advanced/pwa-integration
 * @version 1.0.0
 */

(function() {
  'use strict';

  const PWA_FEATURES = {
    serviceWorker: 'serviceWorker' in navigator,
    pushManager: 'PushManager' in window,
    notification: 'Notification' in window,
    periodicSync: 'PeriodicBackgroundSync' in window,
    backgroundFetch: 'BackgroundFetchManager' in window,
    share: 'share' in navigator,
    paymentRequest: 'PaymentRequest' in window,
    contacts: 'ContactsManager' in window
  };

  const DEFAULT_CONFIG = {
    scope: '/',
    cacheName: 'web-component-pwa-cache',
    version: '1.0.0',
    assetsToCache: [],
    networkTimeoutSeconds: 3,
    maxEntries: 100,
    maxAgeSeconds: 60 * 60 * 24 * 7,
    allowedMethods: ['GET'],
    allowedStatusCodes: [200, 304],
    notificationIcon: '/icon-192.png',
    notificationBadge: '/badge-72.png',
    themeColor: '#667eea',
    backgroundColor: '#ffffff',
    display: 'standalone',
    orientation: 'any',
    startUrl: '/'
  };

  class PWAInstaller {
    constructor(config = {}) {
      this.config = { ...DEFAULT_CONFIG, ...config };
      this.deferredPrompt = null;
      this.setupInstallListener();
    }

    setupInstallListener() {
      window.addEventListener('beforeinstallprompt', (e) => {
        e.preventDefault();
        this.deferredPrompt = e;
        this.dispatchEvent(new CustomEvent('pwa-install-available', {
          detail: { prompt: e },
          bubbles: true,
          composed: true
        }));
      });

      window.addEventListener('appinstalled', (e) => {
        this.deferredPrompt = null;
        this.dispatchEvent(new CustomEvent('pwa-installed', {
          bubbles: true,
          composed: true
        }));
      });
    }

    async showInstallPrompt() {
      if (!this.deferredPrompt) {
        return { outcome: 'dismissed' };
      }

      this.deferredPrompt.prompt();
      const { outcome } = await this.deferredPrompt.userChoice;
      return { outcome };
    }

    get canInstall() {
      return this.deferredPrompt !== null;
    }
  }

  class ServiceWorkerManager {
    constructor(registration, config = {}) {
      this.registration = registration;
      this.config = config;
      this.updateFound = false;
      this.controllerChange = false;
    }

    async register(scope = '/', options = {}) {
      const scriptURL = options.scriptURL || '/sw.js';

      try {
        const registration = await navigator.serviceWorker.register(scriptURL, {
          scope: scope || this.config.scope,
          type: options.type || 'classic'
        });

        this.registration = registration;
        this.setupListeners();

        return registration;
      } catch (error) {
        console.error('Service Worker registration failed:', error);
        throw error;
      }
    }

    setupListeners() {
      if (!this.registration) return;

      this.registration.addEventListener('updatefound', () => {
        const newWorker = this.registration.installing;
        if (newWorker) {
          newWorker.addEventListener('statechange', () => {
            if (newWorker.state === 'installed' && navigator.serviceWorker.controller) {
              this.updateFound = true;
              this.dispatchEvent(new CustomEvent('pwa-update-available', {
                bubbles: true,
                composed: true
              }));
            }
          });
        }
      });

      navigator.serviceWorker.addEventListener('controllerchange', () => {
        this.controllerChange = true;
        this.dispatchEvent(new CustomEvent('pwa-controller-change', {
          bubbles: true,
          composed: true
        }));
      });
    }

    async update() {
      if (!this.registration) return;
      await this.registration.update();
    }

    async unregister() {
      if (!this.registration) return false;
      return await this.registration.unregister();
    }

    async skipWaiting() {
      if (!this.registration || !this.registration.active) return;
      this.registration.active.postMessage({ type: 'SKIP_WAITING' });
    }

    async claim() {
      if (!navigator.serviceWorker.controller) return;
      await navigator.serviceWorker.controller.postMessage({ type: 'CLAIM_CLIENTS' });
    }

    get active() {
      return this.registration?.active;
    }

    get installing() {
      return this.registration?.installing;
    }

    get waiting() {
      return this.registration?.waiting;
    }
  }

  class PushNotificationManager {
    constructor(registration) {
      this.registration = registration;
      this.subscription = null;
    }

    async subscribe(options = {}) {
      if (!this.registration) {
        throw new Error('No service worker registration');
      }

      const vapidPublicKey = options.vapidPublicKey || '';
      if (!vapidPublicKey) {
        throw new Error('VAPID public key is required');
      }

      try {
        const subscription = await this.registration.pushManager.subscribe({
          userVisibleOnly: options.userVisibleOnly !== false,
          applicationServerKey: this.urlBase64ToUint8Array(vapidPublicKey)
        });

        this.subscription = subscription;
        return subscription;
      } catch (error) {
        console.error('Push subscription failed:', error);
        throw error;
      }
    }

    async unsubscribe() {
      if (!this.subscription) return true;
      const success = await this.subscription.unsubscribe();
      this.subscription = null;
      return success;
    }

    async getSubscription() {
      if (!this.registration) return null;
      this.subscription = await this.registration.pushManager.getSubscription();
      return this.subscription;
    }

    async isSupported() {
      if (!this.registration) return false;
      const permission = await this.registration.pushManager.permissionState();
      return permission === 'granted';
    }

    urlBase64ToUint8Array(base64String) {
      const padding = '='.repeat((4 - base64String.length % 4) % 4);
      const base64 = (base64String + padding)
        .replace(/-/g, '+')
        .replace(/_/g, '/');

      const rawData = window.atob(base64);
      const outputArray = new Uint8Array(rawData.length);

      for (let i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
      }

      return outputArray;
    }
  }

  class NotificationController {
    constructor(options = {}) {
      this.icon = options.icon || DEFAULT_CONFIG.notificationIcon;
      this.badge = options.badge || DEFAULT_CONFIG.notificationBadge;
      this.themeColor = options.themeColor || DEFAULT_CONFIG.themeColor;
    }

    async requestPermission() {
      if (!('Notification' in window)) {
        return 'denied';
      }

      if (Notification.permission === 'granted') {
        return 'granted';
      }

      if (Notification.permission !== 'denied') {
        const permission = await Notification.requestPermission();
        return permission;
      }

      return Notification.permission;
    }

    async show(title, options = {}) {
      const permission = await this.requestPermission();
      if (permission !== 'granted') {
        throw new Error('Notification permission not granted');
      }

      const notification = new Notification(title, {
        icon: options.icon || this.icon,
        badge: options.badge || this.badge,
        body: options.body || '',
        data: options.data || {},
        tag: options.tag || '',
        renotify: options.renotify || false,
        requireInteraction: options.requireInteraction || false,
        vibrate: options.vibrate || [],
        actions: options.actions || [],
        ...options
      });

      notification.addEventListener('click', () => {
        this.dispatchEvent(new CustomEvent('notification-click', {
          detail: { notification },
          bubbles: true,
          composed: true
        }));
        notification.close();
      });

      return notification;
    }

    async showFromData(data) {
      return this.show(data.title || 'Web Component', {
        body: data.body || '',
        icon: data.icon || this.icon,
        data: data
      });
    }

    closeAll() {
      self.Notification?.getNotifications?.().then(notifications => {
        notifications.forEach(n => n.close());
      });
    }
  }

  class OfflineManager {
    constructor(config = {}) {
      this.config = { ...DEFAULT_CONFIG, ...config };
      this.isOnline = navigator.onLine;
      this.setupListeners();
    }

    setupListeners() {
      window.addEventListener('online', () => {
        this.isOnline = true;
        this.dispatchEvent(new CustomEvent('pwa-online', {
          bubbles: true,
          composed: true
        }));
      });

      window.addEventListener('offline', () => {
        this.isOnline = false;
        this.dispatchEvent(new CustomEvent('pwa-offline', {
          bubbles: true,
          composed: true
        }));
      });
    }

    get online() {
      return this.isOnline;
    }

    get offline() {
      return !this.isOnline;
    }

    async cacheRequest(url, options = {}) {
      const cacheName = options.cacheName || this.config.cacheName;

      try {
        const cache = await caches.open(cacheName);
        const response = await fetch(url, { mode: 'cors' });

        if (response.ok) {
          await cache.put(url, response.clone());
        }

        return response;
      } catch (error) {
        const cache = await caches.open(cacheName);
        const cachedResponse = await cache.match(url);

        if (cachedResponse) {
          return cachedResponse;
        }

        throw error;
      }
    }

    async getCached(url, options = {}) {
      const cacheName = options.cacheName || this.config.cacheName;

      try {
        const cache = await caches.open(cacheName);
        return await cache.match(url);
      } catch (error) {
        return null;
      }
    }

    async cacheAssets(assets) {
      const cacheName = this.config.cacheName + '-assets-' + this.config.version;

      try {
        const cache = await caches.open(cacheName);
        return await cache.addAll(assets);
      } catch (error) {
        console.error('Failed to cache assets:', error);
      }
    }

    async clearCache(cacheName) {
      try {
        await caches.delete(cacheName);
      } catch (error) {
        console.error('Failed to clear cache:', error);
      }
    }
  }

  class PWAWebComponent extends HTMLElement {
    static get observedAttributes() {
      return ['pwa-enabled', 'installable', 'offline'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._pwaEnabled = true;
      this._installable = false;
      this._offlineSupport = false;
      this._installer = null;
      this._swManager = null;
      this._offlineManager = null;
      this._notificationController = null;
    }

    static get observedAttributes() {
      return ['pwa-enabled', 'installable', 'offline', 'notification-enabled', 'scope', 'sw-path'];
    }

    connectedCallback() {
      this._pwaEnabled = this.hasAttribute('pwa-enabled');
      this._installable = this.hasAttribute('installable');
      this._offlineSupport = this.hasAttribute('offline');

      if (this._installable) {
        this._installer = new PWAInstaller();
      }

      if (this._pwaEnabled) {
        this.initializeServiceWorker();
      }

      if (this._offlineSupport) {
        this._offlineManager = new OfflineManager();
      }

      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'installable') {
          this._installable = this.hasAttribute('installable');
          if (this._installable && !this._installer) {
            this._installer = new PWAInstaller();
          }
        } else if (name === 'offline') {
          this._offlineSupport = this.hasAttribute('offline');
        }
      }
    }

    async initializeServiceWorker() {
      const swPath = this.getAttribute('sw-path') || '/sw.js';
      const scope = this.getAttribute('scope') || '/';

      try {
        const registration = await navigator.serviceWorker.register(swPath, { scope });
        this._swManager = new ServiceWorkerManager(registration);
        this._notificationController = new NotificationController();

        this.dispatchEvent(new CustomEvent('pwa-ready', {
          detail: { registration },
          bubbles: true,
          composed: true
        }));
      } catch (error) {
        console.error('PWA initialization failed:', error);
      }
    }

    async showInstallPrompt() {
      if (!this._installer) return { outcome: 'unavailable' };
      return this._installer.showInstallPrompt();
    }

    get canInstall() {
      return this._installer?.canInstall || false;
    }

    async subscribeToPush(vapidPublicKey) {
      if (!this._swManager?.registration) return null;

      const pushManager = new PushNotificationManager(this._swManager.registration);
      return pushManager.subscribe({ vapidPublicKey };
    }

    async showNotification(title, options = {}) {
      if (!this._notificationController) {
        this._notificationController = new NotificationController();
      }
      return this._notificationController.show(title, options);
    }

    async requestNotificationPermission() {
      if (!this._notificationController) {
        this._notificationController = new NotificationController();
      }
      return this._notificationController.requestPermission();
    }

    get isOnline() {
      return this._offlineManager?.online !== false;
    }

    get isOffline() {
      return this._offlineManager?.offline || false;
    }

    async cacheAssets(assets) {
      if (!this._offlineManager) return;
      return this._offlineManager.cacheAssets(assets);
    }

    async update() {
      if (this._swManager) {
        return this._swManager.update();
      }
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
          .pwa-offline-indicator {
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            background: #ff9800;
            color: white;
            padding: 8px 16px;
            text-align: center;
            font-family: system-ui, -apple-system, sans-serif;
            font-size: 14px;
            z-index: 9999;
          }
          .pwa-offline-indicator[hidden] {
            display: none;
          }
          .install-button {
            position: fixed;
            bottom: 16px;
            right: 16px;
            padding: 12px 24px;
            background: #667eea;
            color: white;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            font-size: 16px;
            font-weight: 500;
            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
            z-index: 9999;
          }
          .install-button:hover {
            background: #5a6fd6;
          }
        </style>
        ${this._offlineSupport ? `<div class="pwa-offline-indicator" hidden>You are offline</div>` : ''}
        ${this._installable && this.canInstall ? `<button class="install-button">Install App</button>` : ''}
        <slot></slot>
      `;
    }
  }

  class AddToHomeScreen extends HTMLElement {
    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._hidden = true;
    }

    connectedCallback() {
      this._installer = new PWAInstaller();
      this.addEventListener('click', this.handleClick.bind(this));
      this.render();
    }

    handleClick() {
      this._installer.showInstallPrompt();
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: none;
          }
          :host([visible]) {
            display: block;
          }
        </style>
        <slot></slot>
      `;
    }
  }

  customElements.define('pwa-component', PWAWebComponent);
  customElements.define('add-to-homescreen', AddToHomeScreen);

  if (typeof window !== 'undefined') {
    window.PWAIntegration = {
      PWA_FEATURES,
      PWAInstaller,
      ServiceWorkerManager,
      PushNotificationManager,
      NotificationController,
      OfflineManager,
      PWAWebComponent,
      AddToHomeScreen
    };
  }

  export {
    PWA_FEATURES,
    PWAInstaller,
    ServiceWorkerManager,
    PushNotificationManager,
    NotificationController,
    OfflineManager,
    PWAWebComponent,
    AddToHomeScreen
  };
})();