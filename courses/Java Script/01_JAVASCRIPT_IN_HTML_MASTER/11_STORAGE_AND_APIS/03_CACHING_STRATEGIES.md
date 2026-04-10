# 💾 Caching Strategies Complete Guide

## Service Worker and Cache API Mastery

---

## Table of Contents

1. [Introduction to Caching](#introduction-to-caching)
2. [Cache API](#cache-api)
3. [Service Worker Basics](#service-worker-basics)
4. [Cache-First Strategy](#cache-first-strategy)
5. [Network-First Strategy](#network-first-strategy)
6. [Stale-While-Revalidate](#stale-while-revalidate)
7. [Network-Only Strategy](#network-only-strategy)
8. [Cache-Only Strategy](#cache-only-strategy)
9. [Advanced Strategies](#advanced-strategies)
10. [Offline Support](#offline-support)
11. [Professional Use Cases](#professional-use-cases)
12. [Common Pitfalls](#common-pitfalls)
13. [Key Takeaways](#key-takeaways)

---

## Introduction to Caching

Caching in web applications improves performance by storing resources locally. The Cache API combined with Service Workers provides:

- **Offline access**: App works without internet
- **Faster loading**: No network requests for cached content
- **Reduced bandwidth**: Less data transferred
- **Better UX**: Instant loading for repeat visits
- **API responses**: Cache API responses

### When to Use Caching

- Static assets (HTML, CSS, JS, images)
- API responses that rarely change
- User-generated content (with sync)
- Progressive Web Apps
- Offline-first applications

---

## Cache API

### Basic Cache Operations

```javascript
// ===== File: cache-api-basics.js =====
// Cache API Basic Operations

// Open or create a cache
async function openCache(name) {
    return await caches.open(name);
}

// Add a single request to cache
async function addToCache(cacheName, url) {
    const cache = await caches.open(cacheName);
    return await cache.add(url);
}

// Add multiple requests to cache
async function addAllToCache(cacheName, urls) {
    const cache = await caches.open(cacheName);
    return await cache.addAll(urls);
}

// Check if request is in cache
async function isCached(cacheName, url) {
    const cache = await caches.open(cacheName);
    const response = await cache.match(url);
    return response !== undefined;
}

// Get response from cache
async function getFromCache(cacheName, url) {
    const cache = await caches.open(cacheName);
    return await cache.match(url);
}

// Delete from cache
async function deleteFromCache(cacheName, url) {
    const cache = await caches.open(cacheName);
    return await cache.delete(url);
}

// Get all cache keys
async function getCacheKeys(cacheName) {
    const cache = await caches.open(cacheName);
    return await cache.keys();
}

// Clear all caches
async function clearAllCaches() {
    const cacheNames = await caches.keys();
    return Promise.all(
        cacheNames.map(cacheName => caches.delete(cacheName))
    );
}
```

### Advanced Cache Operations

```javascript
// ===== File: cache-advanced.js =====
// Advanced Cache Operations

// Cache API with custom Request/Response
async function cacheCustomResponse(cacheName, url, responseData) {
    const cache = await caches.open(cacheName);
    
    const response = new Response(JSON.stringify(responseData), {
        headers: {
            'Content-Type': 'application/json',
            'X-Cached-At': new Date().toISOString()
        }
    });
    
    const request = new Request(url);
    return await cache.put(request, response);
}

// Cache with custom options
async function cacheWithOptions(cacheName, request, response) {
    const cache = await caches.open(cacheName);
    
    const clonedResponse = response.clone();
    const requestToCache = new Request(request, {
        mode: 'cors',
        credentials: 'same-origin'
    });
    
    return await cache.put(requestToCache, clonedResponse);
}

// Cache blob data
async function cacheBlob(cacheName, url, blob) {
    const cache = await caches.open(cacheName);
    
    const response = new Response(blob, {
        headers: {
            'Content-Type': blob.type || 'application/octet-stream'
        }
    });
    
    return await cache.put(url, response);
}

// Get cache storage estimate
async function getCacheStorageEstimate() {
    if ('storage' in navigator && 'estimate' in navigator.storage) {
        const estimate = await navigator.storage.estimate();
        return {
            usage: estimate.usage,
            usagePercentage: (estimate.usage / estimate.quota * 100).toFixed(2),
            quota: estimate.quota
        };
    }
    return null;
}
```

---

## Service Worker Basics

### Service Worker Registration

```javascript
// ===== File: service-worker-basics.js =====
// Service Worker Registration

// Register service worker
async function registerServiceWorker() {
    if ('serviceWorker' in navigator) {
        try {
            const registration = await navigator.serviceWorker.register('/sw.js', {
                scope: '/'
            });
            
            console.log('Service Worker registered:', registration.scope);
            
            registration.addEventListener('updatefound', () => {
                const newWorker = registration.installing;
                console.log('Service Worker update found');
            });
            
            return registration;
        } catch (error) {
            console.error('Service Worker registration failed:', error);
            throw error;
        }
    }
    throw new Error('Service Workers not supported');
}

// Check for updates
async function checkForUpdates() {
    const registration = await navigator.serviceWorker.getRegistration();
    
    if (registration) {
        await registration.update();
    }
}

// Unregister service worker
async function unregisterServiceWorker() {
    const registration = await navigator.serviceWorker.getRegistration();
    
    if (registration) {
        await registration.unregister();
    }
}
```

### Basic Service Worker Setup

```javascript
// ===== File: sw-basic.js =====
// Basic Service Worker (sw.js)

const CACHE_NAME = 'my-app-cache-v1';
const ASSETS_TO_CACHE = [
    '/',
    '/index.html',
    '/styles.css',
    '/app.js',
    '/images/logo.png'
];

// Install event - cache static assets
self.addEventListener('install', (event) => {
    console.log('Service Worker installing...');
    
    event.waitUntil(
        caches.open(CACHE_NAME).then((cache) => {
            console.log('Caching static assets');
            return cache.addAll(ASSETS_TO_CACHE);
        }).then(() => {
            return self.skipWaiting();
        })
    );
});

// Activate event - clean up old caches
self.addEventListener('activate', (event) => {
    console.log('Service Worker activating...');
    
    event.waitUntil(
        caches.keys().then((cacheNames) => {
            return Promise.all(
                cacheNames
                    .filter((name) => name !== CACHE_NAME)
                    .map((name) => caches.delete(name))
            );
        }).then(() => {
            return self.clients.claim();
        })
    );
});

// Fetch event - serve from cache, fallback to network
self.addEventListener('fetch', (event) => {
    event.respondWith(
        caches.match(event.request).then((cachedResponse) => {
            return cachedResponse || fetch(event.request);
        })
    );
});
```

---

## Cache-First Strategy

### Implementing Cache-First

```javascript
// ===== File: strategy-cache-first.js =====
// Cache-First Strategy

// Cache-first implementation
async function cacheFirstStrategy(request, cacheName) {
    const cache = await caches.open(cacheName);
    const cachedResponse = await cache.match(request);
    
    if (cachedResponse) {
        console.log('Serving from cache:', request.url);
        return cachedResponse;
    }
    
    console.log('Fetching from network:', request.url);
    const networkResponse = await fetch(request);
    
    if (networkResponse.ok) {
        cache.put(request, networkResponse.clone());
    }
    
    return networkResponse;
}

// Service Worker implementation
function cacheFirstSW(request, caches, fetch) {
    return caches.match(request).then((cachedResponse) => {
        if (cachedResponse) {
            return cachedResponse;
        }
        
        return fetch(request).then((networkResponse) => {
            if (networkResponse.ok) {
                const cacheCopy = networkResponse.clone();
                caches.open(CACHE_NAME).then((cache) => {
                    cache.put(request, cacheCopy);
                });
            }
            
            return networkResponse;
        });
    });
}

// Modern async/await version
async function cacheFirstModern(request, cacheName) {
    const cached = await caches.match(request);
    
    if (cached) {
        return cached;
    }
    
    try {
        const response = await fetch(request);
        
        if (response.ok) {
            const cache = await caches.open(cacheName);
            cache.put(request, response.clone());
        }
        
        return response;
    } catch (error) {
        // Return offline fallback if available
        return caches.match('/offline.html');
    }
}
```

### Cache-First with Timeout

```javascript
// ===== File: cache-first-timeout.js =====
// Cache-First with Timeout

async function cacheFirstWithTimeout(request, cacheName, timeout = 3000) {
    const cache = await caches.open(cacheName);
    const cachedResponse = await cache.match(request);
    
    if (cachedResponse) {
        // Update cache in background
        updateCacheInBackground(request, cacheName);
        return cachedResponse;
    }
    
    const networkPromise = fetch(request).then((response) => {
        if (response.ok) {
            cache.put(request, response.clone());
        }
        return response;
    });
    
    return Promise.race([
        networkPromise,
        new Promise((_, reject) => 
            setTimeout(() => reject(new Error('Timeout')), timeout)
        )
    ]);
}

async function updateCacheInBackground(request, cacheName) {
    setTimeout(async () => {
        try {
            const response = await fetch(request);
            
            if (response.ok) {
                const cache = await caches.open(cacheName);
                await cache.put(request, response);
                console.log('Cache updated:', request.url);
            }
        } catch (error) {
            // Silent fail for background updates
        }
    }, 1000);
}
```

---

## Network-First Strategy

### Implementing Network-First

```javascript
// ===== File: strategy-network-first.js =====
// Network-First Strategy

async function networkFirstStrategy(request, cacheName) {
    try {
        console.log('Fetching from network:', request.url);
        const networkResponse = await fetch(request);
        
        if (networkResponse.ok) {
            const cache = await caches.open(cacheName);
            cache.put(request, networkResponse.clone());
        }
        
        return networkResponse;
    } catch (error) {
        console.log('Network failed, trying cache:', request.url);
        const cachedResponse = await caches.match(request);
        
        if (cachedResponse) {
            return cachedResponse;
        }
        
        throw error;
    }
}

// Service Worker implementation
function networkFirstSW(request, caches, fetch) {
    return fetch(request).then((networkResponse) => {
        if (networkResponse.ok) {
            const cacheCopy = networkResponse.clone();
            caches.open(CACHE_NAME).then((cache) => {
                cache.put(request, cacheCopy);
            });
        }
        
        return networkResponse;
    }).catch(() => {
        return caches.match(request);
    });
}

// Network-first with fallback
async function networkFirstWithFallback(request, cacheName, fallbackPage) {
    try {
        const response = await fetch(request);
        return response;
    } catch (error) {
        const cached = await caches.match(request);
        
        if (cached) {
            return cached;
        }
        
        return await caches.match(fallbackPage);
    }
}

// Network-first for API calls
async function networkFirstAPI(request, cacheName) {
    const cache = await caches.open(cacheName);
    
    try {
        const response = await fetch(request);
        
        if (response.ok) {
            const body = await response.clone().json();
            
            await cache.put(request, new Response(JSON.stringify(body), {
                headers: { 'Content-Type': 'application/json' }
            }));
        }
        
        return response;
    } catch (error) {
        const cached = await cache.match(request);
        
        if (cached) {
            // Mark response as stale
            const body = await cached.json();
            return new Response(JSON.stringify(body), {
                headers: { 'X-Stale-While-Revalidate': 'true' }
            });
        }
        
        throw error;
    }
}
```

---

## Stale-While-Revalidate

### Implementing Stale-While-Revalidate

```javascript
// ===== File: strategy-swr.js =====
// Stale-While-Revalidate Strategy

async function staleWhileRevalidate(request, cacheName) {
    const cache = await caches.open(cacheName);
    const cachedResponse = await cache.match(request);
    
    const fetchPromise = fetch(request).then((networkResponse) => {
        if (networkResponse.ok) {
            cache.put(request, networkResponse.clone());
        }
        return networkResponse;
    }).catch(() => null);
    
    return cachedResponse || fetchPromise;
}

// With update callback
async function swrWithUpdate(request, cacheName, onUpdate) {
    const cache = await caches.open(cacheName);
    const cached = await cache.match(request);
    
    fetch(request).then((response) => {
        if (response.ok) {
            cache.put(request, response.clone());
            
            if (onUpdate) {
                onUpdate(response);
            }
        }
    }).catch(() => {});
    
    return cached;
}

// Service Worker implementation
self.addEventListener('fetch', (event) => {
    if (event.request.url.includes('/api/')) {
        event.respondWith(staleWhileRevalidateSW(event.request));
    }
});

function staleWhileRevalidateSW(request) {
    return caches.match(request).then((cachedResponse) => {
        const fetchPromise = fetch(request).then((networkResponse) => {
            if (networkResponse.ok) {
                const cacheCopy = networkResponse.clone();
                caches.open(CACHE_NAME).then((cache) => {
                    cache.put(request, cacheCopy);
                });
            }
            return networkResponse;
        }).catch(() => null);
        
        return cachedResponse || fetchPromise;
    });
}
```

### SWR with Background Update

```javascript
// ===== File: swr-background.js =====
// SWR with Background Update

class StaleWhileRevalidate {
    constructor(cacheName) {
        this.cacheName = cacheName;
    }
    
    async fetch(request) {
        const cache = await caches.open(this.cacheName);
        
        // Try to get cached response
        const cached = await cache.match(request);
        
        // Always fetch from network
        fetch(request).then((response) => {
            if (response.ok) {
                cache.put(request, response.clone());
            }
        }).catch(() => {});
        
        // Return cached response immediately if available
        return cached || fetch(request);
    }
}

// With stale detection
async function swrWithStaleDetection(request, cacheName) {
    const cache = await caches.open(cacheName);
    const cached = await cache.match(request);
    
    fetch(request).then((response) => {
        if (response.ok) {
            cache.put(request, response.clone());
        }
    });
    
    return {
        data: cached ? await cached.json() : null,
        isStale: cached !== null,
        isLoading: !cached
    };
}
```

---

## Network-Only Strategy

### Network-Only Implementation

```javascript
// ===== File: strategy-network-only.js =====
// Network-Only Strategy

// Network-only: skip cache entirely
async function networkOnlyStrategy(request) {
    return fetch(request);
}

// Service Worker
self.addEventListener('fetch', (event) => {
    const url = new URL(event.request.url);
    
    // API calls should always go to network
    if (url.pathname.startsWith('/api/')) {
        event.respondWith(fetch(event.request));
    }
});
```

---

## Cache-Only Strategy

### Cache-Only Implementation

```javascript
// ===== File: strategy-cache-only.js =====
// Cache-Only Strategy

// Cache-only: never go to network
async function cacheOnlyStrategy(request, cacheName) {
    const cache = await caches.open(cacheName);
    const cached = await cache.match(request);
    
    if (cached) {
        return cached;
    }
    
    throw new Error('Not in cache');
}

// Service Worker
self.addEventListener('fetch', (event) => {
    const url = new URL(event.request.url);
    
    // Static assets can use cache-only (already pre-cached)
    if (url.pathname.endsWith('.js') || url.pathname.endsWith('.css')) {
        event.respondWith(
            caches.match(event.request).then((response) => {
                return response || fetch(event.request);
            })
        );
    }
});
```

---

## Advanced Strategies

### Custom Strategy Router

```javascript
// ===== File: strategy-router.js =====
// Strategy Router

const STRATEGIES = {
    cacheFirst,
    networkFirst,
    staleWhileRevalidate,
    networkOnly,
    cacheOnly
};

const CACHE_CONFIG = {
    '/static/': { strategy: 'cacheFirst', cache: 'static-v1' },
    '/api/': { strategy: 'networkFirst', cache: 'api-v1', maxAge: 300 },
    '/images/': { strategy: 'cacheFirst', cache: 'images-v1' },
    '/fonts/': { strategy: 'cacheFirst', cache: 'fonts-v1' }
};

async function routeStrategy(request) {
    const url = new URL(request.url);
    
    // Match by path prefix
    for (const [pattern, config] of Object.entries(CACHE_CONFIG)) {
        if (url.pathname.startsWith(pattern)) {
            const strategy = STRATEGIES[config.strategy];
            return strategy(request, config.cache);
        }
    }
    
    // Default strategy
    return fetch(request);
}

// Service Worker fetch handler
self.addEventListener('fetch', (event) => {
    event.respondWith(routeStrategy(event.request));
});
```

### Versioned Cache Management

```javascript
// ===== File: cache-versions.js =====
// Versioned Cache Management

const CACHE_VERSIONS = {
    static: 'static-v1',
    api: 'api-v1',
    images: 'images-v1'
};

const CURRENT_VERSION = 'v2';

function getCacheName(type) {
    return `${CACHE_VERSIONS[type]}-${CURRENT_VERSION}`;
}

async function cleanOldCaches() {
    const allCaches = await caches.keys();
    const currentCaches = Object.values(CACHE_VERSIONS).map(
        name => `${name}-${CURRENT_VERSION}`
    );
    
    const oldCaches = allCaches.filter(
        name => !currentCaches.includes(name)
    );
    
    await Promise.all(oldCaches.map(name => caches.delete(name)));
}

// Precache new version assets
async function precacheVersionAssets(assets) {
    const cacheName = getCacheName('static');
    const cache = await caches.open(cacheName);
    
    await cache.addAll(assets);
}
```

---

## Offline Support

### Offline Fallback Page

```javascript
// ===== File: offline-support.js =====
// Offline Support

const OFFLINE_PAGE = '/offline.html';

const CACHE_NAME = 'app-cache-v1';

const ASSETS_TO_CACHE = [
    '/',
    '/index.html',
    '/offline.html',
    '/styles.css',
    '/app.js'
];

// Precache offline assets
self.addEventListener('install', (event) => {
    event.waitUntil(
        caches.open(CACHE_NAME).then((cache) => {
            return cache.addAll(ASSETS_TO_CACHE);
        })
    );
});

// Handle offline requests
self.addEventListener('fetch', (event) => {
    event.respondWith(
        fetch(event.request).catch(() => {
            return caches.match(OFFLINE_PAGE);
        })
    );
});

// Client-side offline detection
function checkOnlineStatus() {
    return new Promise((resolve) => {
        if (!navigator.onLine) {
            resolve(false);
            return;
        }
        
        fetch('/api/health', { method: 'HEAD' })
            .then(() => resolve(true))
            .catch(() => resolve(false));
    });
}

window.addEventListener('offline', () => {
    document.body.classList.add('offline');
});

window.addEventListener('online', () => {
    document.body.classList.remove('offline');
});
```

---

## Professional Use Cases

### Use Case 1: API Response Caching

```javascript
// ===== File: use-case-api-cache.js =====
// API Response Caching

class APICache {
    constructor(cacheName, maxAge = 300000) {
        this.cacheName = cacheName;
        this.maxAge = maxAge;
    }
    
    async get(url) {
        const cache = await caches.open(this.cacheName);
        const cached = await cache.match(url);
        
        if (!cached) return null;
        
        const date = cached.headers.get('date');
        const age = Date.now() - new Date(date).getTime();
        
        if (age > this.maxAge) {
            return { data: cached, stale: true };
        }
        
        return { data: cached, stale: false };
    }
    
    async set(url, response) {
        const cache = await caches.open(this.cacheName);
        
        const responseToCache = response.clone();
        await cache.put(url, responseToCache);
    }
    
    async fetch(url, options = {}) {
        const cached = await this.get(url);
        
        if (cached && !options.noCache) {
            return cached.data;
        }
        
        const response = await fetch(url);
        
        if (response.ok) {
            await this.set(url, response);
        }
        
        return response;
    }
}
```

### Use Case 2: Image Caching

```javascript
// ===== File: use-case-images.js =====
// Image Caching

class ImageCache {
    constructor() {
        this.cacheName = 'images-v1';
    }
    
    async cacheImage(url) {
        const cache = await caches.open(this.cacheName);
        
        try {
            const response = await fetch(url);
            
            if (response.ok) {
                await cache.put(url, response);
            }
        } catch (error) {
            console.error('Failed to cache image:', error);
        }
    }
    
    async getImage(url) {
        const cache = await caches.open(this.cacheName);
        return await cache.match(url);
    }
    
    async preloadImages(urls) {
        const cache = await caches.open(this.cacheName);
        
        const promises = urls.map(url => {
            return fetch(url, { mode: 'cors' }).then(response => {
                if (response.ok) {
                    cache.put(url, response);
                }
            });
        });
        
        await Promise.allSettled(promises);
    }
}
```

### Use Case 3: Dynamic Content Caching

```javascript
// ===== File: use-case-dynamic.js =====
// Dynamic Content Caching

class DynamicCache {
    constructor() {
        this.cacheName = 'dynamic-v1';
    }
    
    async cacheHTML(url, response) {
        const cache = await caches.open(this.cacheName);
        const html = await response.text();
        
        const modified = html.replace(
            '<body>',
            `<body><div data-cached="${new Date().toISOString()}">`
        );
        
        const modifiedResponse = new Response(modified, {
            headers: { 'Content-Type': 'text/html' }
        });
        
        await cache.put(url, modifiedResponse);
    }
}
```

---

## Common Pitfalls

1. **Not handling fetch errors**: Always wrap in try-catch
2. **Caching POST requests**: Cannot cache POST without custom handling
3. **Not versioning caches**: Old caches accumulate
4. **Caching too aggressively**: stale data causes issues
5. **Ignoring storage limits**: Quota errors happen
6. **Not testing offline**: Must test in DevTools
7. **Blocking main thread**: Use async/await
8. **Forgetting to clone responses**: Can only read once
9. **Service worker update issues**: Must skipWaiting and claim clients
10. **Not cleaning old caches**: Leads to storage bloat

---

## Key Takeaways

- **Cache-first**: Best for static assets (HTML, CSS, JS, images)
- **Network-first**: Best for API calls and dynamic content
- **Stale-While-Revalidate**: Balance of speed and freshness
- **Network-only**: For data that must be fresh
- **Cache-only**: For offline-only content
- Service Workers must be registered separately
- Always version cache names
- Clean old caches on activate
- Use proper response cloning
- Test offline functionality thoroughly

---

## Related Files

- [01_LOCAL_STORAGE_SESSION_STORAGE.md](./01_LOCAL_STORAGE_SESSION_STORAGE.md) - For simple storage
- [02_INDEXEDDB_ADVANCED.md](./02_INDEXEDDB_ADVANCED.md) - For structured data
- [ADVANCED_BROWSER_APIS/05_PWA_MASTER.md](../ADVANCED_BROWSER_APIS/05_PWA_MASTER.md) - For PWA context