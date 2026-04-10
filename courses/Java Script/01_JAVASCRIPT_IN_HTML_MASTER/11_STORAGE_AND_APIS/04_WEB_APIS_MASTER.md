# 🌐 Web APIs Master Complete Guide

## Fetch API, Axios, and HTTP Best Practices

---

## Table of Contents

1. [Introduction to Web APIs](#introduction-to-web-apis)
2. [Fetch API Basics](#fetch-api-basics)
3. [Request and Response Objects](#request-and-response-objects)
4. [Error Handling](#error-handling)
5. [Headers and Configuration](#headers-and-configuration)
6. [Axios Comparison](#axios-comparison)
7. [Retry Logic](#retry-logic)
8. [Interceptors](#interceptors)
9. [Advanced Patterns](#advanced-patterns)
10. [Professional Use Cases](#professional-use-cases)
11. [Common Pitfalls](#common-pitfalls)
12. [Key Takeaways](#key-takeaways)

---

## Introduction to Web APIs

Modern web applications communicate with servers through HTTP APIs. The main options are:

- **Fetch API**: Native browser API, Promise-based
- **Axios**: Popular library with additional features
- **XMLHttpRequest**: Legacy API (avoid)

### When to Use Each

- **Fetch API**: Native, no dependencies, modern apps
- **Axios**: Need interceptors, automatic transforms, better error handling

---

## Fetch API Basics

### Simple GET Request

```javascript
// ===== File: fetch-basics.js =====
// Fetch API Basic Operations

// Simple GET request
async function fetchData(url) {
    const response = await fetch(url);
    const data = await response.json();
    return data;
}

// With error handling
async function fetchWithErrorHandling(url) {
    try {
        const response = await fetch(url);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        return await response.json();
    } catch (error) {
        console.error('Fetch failed:', error);
        throw error;
    }
}

// POST request
async function postData(url, data) {
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });
    
    return await response.json();
}

// PUT request
async function updateData(url, data) {
    const response = await fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    });
    
    return await response.json();
}

// DELETE request
async function deleteData(url) {
    const response = await fetch(url, {
        method: 'DELETE'
    });
    
    return response.ok;
}
```

### POST with Form Data

```javascript
// ===== File: fetch-form-data.js =====
// Fetch with Form Data

// Submit form data
async function submitForm(url, formElement) {
    const formData = new FormData(formElement);
    
    const response = await fetch(url, {
        method: 'POST',
        body: formData
    });
    
    return await response.json();
}

// Upload file
async function uploadFile(url, file) {
    const formData = new FormData();
    formData.append('file', file);
    
    const response = await fetch(url, {
        method: 'POST',
        body: formData
    });
    
    return await response.json();
}

// Multiple files
async function uploadFiles(url, files) {
    const formData = new FormData();
    
    Array.from(files).forEach((file, index) => {
        formData.append(`file_${index}`, file);
    });
    
    const response = await fetch(url, {
        method: 'POST',
        body: formData
    });
    
    return await response.json();
}
```

---

## Request and Response Objects

### Request Configuration

```javascript
// ===== File: request-config.js =====
// Request Configuration

// Complete Request configuration
const requestOptions = {
    method: 'GET',
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer token',
        'X-Custom-Header': 'value'
    },
    mode: 'cors', // 'cors', 'no-cors', 'same-origin'
    credentials: 'same-origin', // 'omit', 'same-origin', 'include'
    cache: 'no-cache', // 'default', 'no-store', 'reload', 'no-cache', 'force-cache', 'only-if-cached'
    redirect: 'follow', // 'follow', 'error', 'manual'
    referrer: 'no-referrer',
    referrerPolicy: 'no-referrer-when-downgrade',
    integrity: 'sha256-...',
    keepalive: false,
    signal: null // AbortController signal
};

// Create Request object
const request = new Request('/api/data', requestOptions);

// Use Request with fetch
async function useRequest(request) {
    const response = await fetch(request);
    return await response.json();
}
```

### Response Handling

```javascript
// ===== File: response-handling.js =====
// Response Object Methods

async function handleResponse(response) {
    // Check content type
    const contentType = response.headers.get('content-type');
    
    if (contentType?.includes('application/json')) {
        return await response.json();
    }
    
    if (contentType?.includes('text/html')) {
        return await response.text();
    }
    
    if (contentType?.includes('image/')) {
        return response.blob();
    }
    
    if (contentType?.includes('application/octet-stream')) {
        return response.arrayBuffer();
    }
    
    // Default to text
    return await response.text();
}

// Get response metadata
async function getResponseMetadata(response) {
    return {
        ok: response.ok,
        status: response.status,
        statusText: response.statusText,
        type: response.type,
        url: response.url,
        headers: Object.fromEntries(response.headers.entries()),
        redirected: response.redirected
    };
}

// Clone response
async function handleResponseWithClone(response) {
    const clone = response.clone();
    
    const data = await response.json();
    const clonedData = await clone.json();
    
    return { data, clonedData };
}
```

---

## Error Handling

### Robust Error Handling

```javascript
// ===== File: error-handling.js =====
// Comprehensive Error Handling

class APIError extends Error {
    constructor(message, status, code) {
        super(message);
        this.name = 'APIError';
        this.status = status;
        this.code = code;
    }
}

async function safeFetch(url, options = {}) {
    try {
        const response = await fetch(url, options);
        
        if (!response.ok) {
            let errorMessage;
            
            try {
                const errorData = await response.json();
                errorMessage = errorData.message || errorData.error;
            } catch {
                errorMessage = response.statusText;
            }
            
            throw new APIError(
                errorMessage,
                response.status,
                `HTTP_${response.status}`
            );
        }
        
        return response;
    } catch (error) {
        if (error instanceof APIError) {
            throw error;
        }
        
        if (error.name === 'AbortError') {
            throw new APIError('Request cancelled', 0, 'ABORTED');
        }
        
        if (error.name === 'TypeError' && error.message.includes('fetch')) {
            throw new APIError('Network error', 0, 'NETWORK_ERROR');
        }
        
        throw new APIError(error.message, 0, 'UNKNOWN');
    }
}

// Handle different error types
async function fetchWithErrorTypes(url) {
    try {
        const response = await safeFetch(url);
        return await response.json();
    } catch (error) {
        switch (error.code) {
            case 'HTTP_401':
                window.location.href = '/login';
                break;
            case 'HTTP_403':
                throw new Error('Access denied');
            case 'HTTP_404':
                return null;
            case 'HTTP_500':
                throw new Error('Server error. Please try again later.');
            case 'NETWORK_ERROR':
                throw new Error('No internet connection');
            default:
                throw error;
        }
    }
}
```

### Validation Error Handling

```javascript
// ===== File: validation-errors.js =====
// Validation Error Handling

class ValidationError extends Error {
    constructor(message, errors) {
        super(message);
        this.name = 'ValidationError';
        this.errors = errors;
    }
}

async function handleValidationErrors(response) {
    if (response.status === 400) {
        const errorData = await response.json();
        
        if (errorData.errors) {
            const formattedErrors = errorData.errors.reduce((acc, err) => {
                acc[err.field] = err.message;
                return acc;
            }, {});
            
            throw new ValidationError(
                'Validation failed',
                formattedErrors
            );
        }
    }
    
    return response;
}

// Display validation errors
function displayValidationErrors(error) {
    if (error instanceof ValidationError) {
        Object.entries(error.errors).forEach(([field, message]) => {
            const input = document.querySelector(`[name="${field}"]`);
            
            if (input) {
                const errorElement = input.parentElement.querySelector('.error');
                
                if (errorElement) {
                    errorElement.textContent = message;
                }
            }
        });
    }
}
```

---

## Headers and Configuration

### Working with Headers

```javascript
// ===== File: headers-manipulation.js =====
// Headers Manipulation

// Create custom headers
const headers = new Headers({
    'Content-Type': 'application/json',
    'X-API-Version': '2'
});

// Append headers
headers.append('X-Custom-Header', 'value');

// Check header exists
function hasHeader(headers, name) {
    return headers.has(name);
}

// Get header value
function getHeader(headers, name) {
    return headers.get(name);
}

// Delete header
headers.delete('X-Old-Header');

// Request with headers
async function fetchWithHeaders(url, customHeaders) {
    const headers = new Headers(customHeaders);
    
    // Add auth token if available
    const token = localStorage.getItem('authToken');
    
    if (token) {
        headers.set('Authorization', `Bearer ${token}`);
    }
    
    const response = await fetch(url, { headers });
    
    return await response.json();
}
```

### Request Abort Controller

```javascript
// ===== File: abort-controller.js =====
// Abort Controller

// Create abortable fetch
function createAbortableFetch() {
    const controller = new AbortController();
    const signal = controller.signal;
    
    return {
        signal,
        abort: () => controller.abort(),
        fetch: (url, options = {}) => {
            return fetch(url, { ...options, signal });
        }
    };
}

// Cancel request after timeout
async function fetchWithTimeout(url, timeout = 5000) {
    const controller = new AbortController();
    const id = setTimeout(() => controller.abort(), timeout);
    
    try {
        const response = await fetch(url, {
            signal: controller.signal
        });
        
        clearTimeout(id);
        return response;
    } catch (error) {
        clearTimeout(id);
        
        if (error.name === 'AbortError') {
            throw new Error('Request timeout');
        }
        
        throw error;
    }
}

// Cancel button UI
function setupCancelButton(url, button) {
    const { signal, abort, fetch } = createAbortableFetch();
    
    button.addEventListener('click', abort);
    
    return fetch(url);
}
```

---

## Axios Comparison

### Axios Basic Usage

```javascript
// ===== File: axios-basics.js =====
// Axios Library Usage

import axios from 'axios';

// Simple GET
async function getData(url) {
    const response = await axios.get(url);
    return response.data;
}

// POST
async function postData(url, data) {
    const response = await axios.post(url, data);
    return response.data;
}

// With config
async function getWithConfig(url) {
    const response = await axios.get(url, {
        params: { page: 1, limit: 10 },
        headers: { 'X-API-Key': 'key' },
        timeout: 5000
    });
    
    return response.data;
}

// All config options
const axiosConfig = {
    url: '/api/data',
    method: 'get',
    baseURL: 'https://api.example.com',
    headers: { 'X-Custom': 'value' },
    params: { id: 1 },
    data: { name: 'test' },
    timeout: 5000,
    responseType: 'json', // json, arraybuffer, blob, document, text
    validateStatus: (status) => status < 500 // default
};
```

### Fetch vs Axios Comparison

```javascript
// ===== File: fetch-vs-axios.js =====
// Fetch vs Axios Comparison

// ====== FETCH API ======
// Pros:
// - Native, no dependency
// - Smaller bundle size
// - Stream support
// - Request/Response objects

// Cons:
// - No automatic JSON transform
// - Error handling not automatic (no reject on 4xx/5xx)
// - No interceptors built-in
// - No request cancellation (older browsers)

async function fetchExample() {
    const response = await fetch('/api/data');
    const data = await response.json();
    return data;
}

// ====== AXIOS ======
// Pros:
// - Automatic JSON transform
// - Automatic error rejection
// - Built-in interceptors
// - Request/response transformation
// - Better browser support
// - Cancelled request support
// - HTTP interceptors

async function axiosExample() {
    const response = await axios.get('/api/data');
    return response.data;
}

// Feature Comparison Table
const comparison = {
    'Automatic JSON': { fetch: false, axios: true },
    'Interceptors': { fetch: false, axios: true },
    'Error Handling': { fetch: 'manual', axios: 'automatic' },
    'Request Cancel': { fetch: 'AbortController', axios: 'CancelToken' },
    'Upload Progress': { fetch: false, axios: true },
    'Response Transformation': { fetch: false, axios: true },
    'Bundle Size': { fetch: '0kb (native)', axios: '~15kb' }
};
```

---

## Retry Logic

### Automatic Retry

```javascript
// ===== File: retry-logic.js =====
// Retry Logic

async function fetchWithRetry(url, options = {}) {
    const {
        maxRetries = 3,
        retryDelay = 1000,
        retryOn = [408, 429, 500, 502, 503, 504]
    } = options;
    
    let lastError;
    
    for (let attempt = 0; attempt < maxRetries; attempt++) {
        try {
            const response = await fetch(url, options);
            
            if (!response.ok && retryOn.includes(response.status)) {
                throw new Error(`HTTP ${response.status}`);
            }
            
            return response;
        } catch (error) {
            lastError = error;
            
            const shouldRetry = attempt < maxRetries - 1 &&
                (retryOn.includes(lastError.status) ||
                 lastError.name === 'TypeError');
            
            if (!shouldRetry) {
                throw lastError;
            }
            
            // Exponential backoff
            const delay = retryDelay * Math.pow(2, attempt);
            console.log(`Retry ${attempt + 1} after ${delay}ms`);
            
            await new Promise(resolve => setTimeout(resolve, delay));
        }
    }
    
    throw lastError;
}
```

### Advanced Retry with Jitter

```javascript
// ===== File: retry-jitter.js =====
// Retry with Exponential Backoff and Jitter

async function fetchWithRetryJitter(url, options = {}) {
    const {
        maxRetries = 5,
        baseDelay = 1000,
        maxDelay = 30000,
        retryOn = [408, 429, 500, 502, 503, 504]
    } = options;
    
    for (let attempt = 0; attempt < maxRetries; attempt++) {
        try {
            const response = await fetch(url, options);
            
            if (response.ok) {
                return response;
            }
            
            if (!retryOn.includes(response.status)) {
                return response;
            }
        } catch (error) {
            if (attempt === maxRetries - 1) {
                throw error;
            }
        }
        
        // Calculate delay with jitter
        const exponentialDelay = baseDelay * Math.pow(2, attempt);
        const jitterRange = exponentialDelay - baseDelay;
        const jitter = Math.random() * jitterRange;
        const delay = Math.min(exponentialDelay + jitter, maxDelay);
        
        console.log(`Retry ${attempt + 1}/${maxRetries} after ${Math.round(delay)}ms`);
        
        await new Promise(resolve => setTimeout(resolve, delay));
    }
}
```

---

## Interceptors

### Fetch with Interceptors (Custom Implementation)

```javascript
// ===== File: fetch-interceptors.js =====
// Fetch with Interceptors

class FetchClient {
    constructor(baseURL = '') {
        this.baseURL = baseURL;
        this.requestInterceptors = [];
        this.responseInterceptors = [];
    }
    
    addRequestInterceptor(fn) {
        this.requestInterceptors.push(fn);
    }
    
    addResponseInterceptor(fn) {
        this.responseInterceptors.push(fn);
    }
    
    async request(url, options = {}) {
        let fullURL = this.baseURL + url;
        let config = { ...options };
        
        // Apply request interceptors
        for (const interceptor of this.requestInterceptors) {
            const result = interceptor(fullURL, config);
            fullURL = result.url || fullURL;
            config = { ...config, ...result.config };
        }
        
        let response;
        
        try {
            response = await fetch(fullURL, config);
        } catch (error) {
            throw error;
        }
        
        // Apply response interceptors
        for (const interceptor of this.responseInterceptors) {
            response = interceptor(response);
        }
        
        return response;
    }
    
    get(url, config) {
        return this.request(url, { ...config, method: 'GET' });
    }
    
    post(url, data, config) {
        return this.request(url, {
            ...config,
            method: 'POST',
            headers: { 'Content-Type': 'application/json', ...config?.headers },
            body: JSON.stringify(data)
        });
    }
}

// Create client with interceptors
const api = new FetchClient('https://api.example.com');

// Add auth interceptor
api.addRequestInterceptor((url, config) => {
    const token = localStorage.getItem('token');
    
    if (token) {
        config.headers = {
            ...config.headers,
            'Authorization': `Bearer ${token}`
        };
    }
    
    return { url, config };
});

// Add logging interceptor
api.addResponseInterceptor((response) => {
    console.log('Response:', response.status);
    return response;
});
```

### Axios Interceptors

```javascript
// ===== File: axios-interceptors.js =====
// Axios Interceptors

import axios from 'axios';

// Request interceptor
axios.interceptors.request.use(
    (config) => {
        // Add auth token
        const token = localStorage.getItem('token');
        
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        
        // Add timestamp for cache busting
        config.params = {
            ...config.params,
            _t: Date.now()
        };
        
        return config;
    },
    (error) => Promise.reject(error)
);

// Response interceptor
axios.interceptors.response.use(
    (response) => {
        console.log('Success:', response.config.url);
        return response;
    },
    (error) => {
        if (error.response?.status === 401) {
            // Handle unauthorized
            localStorage.removeItem('token');
            window.location.href = '/login';
        }
        
        return Promise.reject(error);
    }
);
```

---

## Advanced Patterns

### Parallel Requests

```javascript
// ===== File: parallel-requests.js =====
// Parallel Request Patterns

// Multiple parallel requests
async function fetchMultiple(urls) {
    const promises = urls.map(url => fetch(url));
    const responses = await Promise.all(promises);
    
    return Promise.all(
        responses.map(response => response.json())
    );
}

// Named parallel requests
async function fetchParallel(requests) {
    const promises = Object.entries(requests).map(
        ([name, url]) => fetch(url).then(r => r.json()).then(data => [name, data])
    );
    
    const results = await Promise.all(promises);
    
    return Object.fromEntries(results);
}

// Race condition handling
async function fetchWithFallback(urls) {
    const promises = urls.map(url => 
        fetch(url).then(r => r.json())
    );
    
    return await Promise.any(promises);
}

// Allsettled (doesn't reject)
async function fetchAllSettled(urls) {
    const promises = urls.map(url => 
        fetch(url).then(r => r.json())
    );
    
    const results = await Promise.allSettled(promises);
    
    return results.map((result, index) => ({
        url: urls[index],
        success: result.status === 'fulfilled',
        data: result.value,
        error: result.reason
    }));
}
```

### Sequential Requests

```javascript
// ===== File: sequential-requests.js =====
// Sequential Request Patterns

// Chain requests based on previous response
async function fetchChain(urls, transform = (data) => data) {
    let result;
    
    for (const url of urls) {
        const response = await fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(result || {})
        });
        
        result = await response.json();
        result = transform(result);
    }
    
    return result;
}

// Batch sequential for rate limiting
async function fetchSequentially(urls, delay = 100) {
    const results = [];
    
    for (const url of urls) {
        const response = await fetch(url);
        results.push(await response.json());
        
        if (delay > 0) {
            await new Promise(resolve => setTimeout(resolve, delay));
        }
    }
    
    return results;
}
```

---

## Professional Use Cases

### Use Case 1: REST API Client

```javascript
// ===== File: use-case-api-client.js =====
// REST API Client

class APIClient {
    constructor(baseURL, defaultHeaders = {}) {
        this.baseURL = baseURL;
        this.headers = defaultHeaders;
    }
    
    async request(endpoint, options = {}) {
        const url = `${this.baseURL}${endpoint}`;
        
        const response = await fetch(url, {
            ...options,
            headers: {
                ...this.headers,
                ...options.headers
            }
        });
        
        if (!response.ok) {
            throw new Error(`API Error: ${response.status}`);
        }
        
        return await response.json();
    }
    
    get(endpoint, params) {
        const query = new URLSearchParams(params).toString();
        const url = query ? `${endpoint}?${query}` : endpoint;
        return this.request(url);
    }
    
    post(endpoint, data) {
        return this.request(endpoint, {
            method: 'POST',
            body: JSON.stringify(data),
            headers: { 'Content-Type': 'application/json' }
        });
    }
    
    put(endpoint, data) {
        return this.request(endpoint, {
            method: 'PUT',
            body: JSON.stringify(data),
            headers: { 'Content-Type': 'application/json' }
        });
    }
    
    delete(endpoint) {
        return this.request(endpoint, { method: 'DELETE' });
    }
}

// Usage
const api = new APIClient('/api');
const users = await api.get('/users');
const newUser = await api.post('/users', { name: 'John' });
```

### Use Case 2: File Upload with Progress

```javascript
// ===== File: use-case-upload.js =====
// File Upload with Progress

async function uploadFile(url, file, onProgress) {
    return new Promise((resolve, reject) => {
        const xhr = new XMLHttpRequest();
        
        xhr.upload.addEventListener('progress', (event) => {
            if (event.lengthComputable && onProgress) {
                const percentage = Math.round((event.loaded / event.total) * 100);
                onProgress(percentage);
            }
        });
        
        xhr.addEventListener('load', () => {
            if (xhr.status >= 200 && xhr.status < 300) {
                resolve(JSON.parse(xhr.response));
            } else {
                reject(new Error(`Upload failed: ${xhr.status}`));
            }
        });
        
        xhr.addEventListener('error', () => reject(new Error('Upload failed')));
        
        xhr.open('POST', url);
        
        const formData = new FormData();
        formData.append('file', file);
        
        xhr.send(formData);
    });
}

// Usage
await uploadFile('/api/upload', fileInput.files[0], (percent) => {
    progressBar.style.width = `${percent}%`;
});
```

### Use Case 3: GraphQL Client

```javascript
// ===== File: use-case-graphql.js =====
// Simple GraphQL Client

class GraphQLClient {
    constructor(url, headers = {}) {
        this.url = url;
        this.headers = headers;
    }
    
    async query(query, variables = {}) {
        return this.request({ query, variables });
    }
    
    async mutation(mutation, variables = {}) {
        return this.request({ query: mutation, variables });
    }
    
    async request(body) {
        const response = await fetch(this.url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                ...this.headers
            },
            body: JSON.stringify(body)
        });
        
        const result = await response.json();
        
        if (result.errors) {
            throw new Error(result.errors[0].message);
        }
        
        return result.data;
    }
}

// Usage
const client = new GraphQLClient('/graphql');

const data = await client.query(`
    query GetUser($id: ID!) {
        user(id: $id) {
            name
            email
        }
    }
`, { id: '1' });
```

---

## Common Pitfalls

1. **Not checking response.ok**: Fetch doesn't reject on HTTP errors
2. **Not calling response.json()**: Promise chain breaks
3. **Reading response twice**: Must clone or store result
4. **No timeout**: Requests can hang forever
5. **Missing Content-Type**: API returns error
6. **CORS issues**: Not handling cross-origin requests
7. **Not handling abort**: Memory leaks
8. **Sequential awaits**: Should use Promise.all
9. **No error feedback**: Silent failures
10. **Ignoring network errors**: Only check HTTP status

---

## Key Takeaways

- Fetch API is native; Axios offers more features
- Always check response.ok or response.status
- Use clone() if you need to read response multiple times
- Implement retry logic with exponential backoff
- Use AbortController for request cancellation
- Interceptors enable cross-cutting concerns
- Promise.all for parallel requests
- Content-Type header is essential for POST/PUT
- Handle both network and HTTP errors
- Consider Axios for complex API interactions

---

## Related Files

- [03_CACHING_STRATEGIES.md](./03_CACHING_STRATEGIES.md) - For caching API responses
- [02_INDEXEDDB_ADVANCED.md](./02_INDEXEDDB_ADVANCED.md) - For offline storage
- [06_STORAGE_SECURITY_BEST_PRACTICES.md](./06_STORAGE_SECURITY_BEST_PRACTICES.md) - For API security