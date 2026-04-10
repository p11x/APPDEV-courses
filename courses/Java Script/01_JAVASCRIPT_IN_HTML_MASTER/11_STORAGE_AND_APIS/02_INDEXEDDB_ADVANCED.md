# 💾 IndexedDB Advanced Complete Guide

## Enterprise-Grade Browser Database

---

## Table of Contents

1. [Introduction to IndexedDB](#introduction-to-indexeddb)
2. [Database Basics](#database-basics)
3. [Object Stores](#object-stores)
4. [Transactions](#transactions)
5. [Indexes and Queries](#indexes-and-queries)
6. [Cursors](#cursors)
7. [Complex Data Structures](#complex-data-structures)
8. [Performance Optimization](#performance-optimization)
9. [Professional Use Cases](#professional-use-cases)
10. [Common Pitfalls](#common-pitfalls)
11. [Key Takeaways](#key-takeaways)

---

## Introduction to IndexedDB

IndexedDB is a transactional, synchronous-looking (but actually asynchronous) database system built into browsers. Unlike localStorage, IndexedDB can:

- Store large amounts of structured data (hundreds of MB or more)
- Support complex queries using indexes
- Provide ACID transaction guarantees
- Handle complex data types (blobs, dates, arrays)
- Offer cursor-based iteration
- Support efficient range queries

### When to Use IndexedDB

Use IndexedDB when you need:

- Large data storage (>5MB)
- Complex querying and filtering
- Transaction support (atomic operations)
- Indexes for fast lookups
- Structured data (not just key-value)
- Cursor-based iteration
- Offline-first applications

---

## Database Basics

### Opening a Database

```javascript
// ===== File: indexeddb-basics.js =====
// IndexedDB Basic Operations

class IndexedDBManager {
    constructor(dbName, version = 1) {
        this.dbName = dbName;
        this.version = version;
        this.db = null;
    }
    
    open() {
        return new Promise((resolve, reject) => {
            const request = indexedDB.open(this.dbName, this.version);
            
            request.onerror = () => {
                console.error('Database open error:', request.error);
                reject(request.error);
            };
            
            request.onsuccess = () => {
                this.db = request.result;
                console.log('Database opened:', this.db.name);
                resolve(this.db);
            };
            
            request.onupgradeneeded = (event) => {
                const db = event.target.result;
                console.log('Database upgrade needed, version:', db.version);
                
                // Create object stores here
                this.createObjectStores(db);
            };
        });
    }
    
    createObjectStores(db) {
        // Override in subclass
    }
    
    close() {
        if (this.db) {
            this.db.close();
            this.db = null;
        }
    }
}

// Simple open example
function openDatabase(name, version) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(name, version);
        
        request.onerror = () => reject(request.error);
        request.onsuccess = () => resolve(request.result);
        
        request.onupgradeneeded = (event) => {
            const db = event.target.result;
            
            // Create users object store with autoIncrement key
            if (!db.objectStoreNames.contains('users')) {
                const userStore = db.createObjectStore('users', { 
                    keyPath: 'id',
                    autoIncrement: true 
                });
            }
        };
    });
}
```

---

## Object Stores

### Creating Object Stores

```javascript
// ===== File: object-stores.js =====
// Object Store Creation and Configuration

class DatabaseSetup extends IndexedDBManager {
    constructor() {
        super('MyAppDB', 1);
    }
    
    createObjectStores(db) {
        // Users store with auto-incrementing ID
        if (!db.objectStoreNames.contains('users')) {
            db.createObjectStore('users', {
                keyPath: 'id',
                autoIncrement: true
            });
        }
        
        // Products store with custom key
        if (!db.objectStoreNames.contains('products')) {
            db.createObjectStore('products', { keyPath: 'sku' });
        }
        
        // Orders store with compound key
        if (!db.objectStoreNames.contains('orders')) {
            const orderStore = db.createObjectStore('orders', {
                keyPath: ['userId', 'orderId']
            });
        }
        
        // Files store for large blobs
        if (!db.objectStoreNames.contains('files')) {
            db.createObjectStore('files', { keyPath: 'id' });
        }
    }
}

// Advanced object store with indexes
class AdvancedDatabaseSetup extends IndexedDBManager {
    constructor() {
        super('AdvancedDB', 1);
    }
    
    createObjectStores(db) {
        // Create users store
        if (!db.objectStoreNames.contains('users')) {
            const userStore = db.createObjectStore('users', {
                keyPath: 'id',
                autoIncrement: true
            });
            
            // Add indexes
            userStore.createIndex('email', 'email', { unique: true });
            userStore.createIndex('username', 'username', { unique: false });
            userStore.createIndex('createdAt', 'createdAt', { unique: false });
            userStore.createIndex('status', 'status', { unique: false });
            userStore.createIndex('email_verified', 'emailVerified', { unique: false });
        }
        
        // Create products store with multiple indexes
        if (!db.objectStoreNames.contains('products')) {
            const productStore = db.createObjectStore('products', {
                keyPath: 'id',
                autoIncrement: true
            });
            
            productStore.createIndex('sku', 'sku', { unique: true });
            productStore.createIndex('category', 'category', { unique: false });
            productStore.createIndex('price', 'price', { unique: false });
            productStore.createIndex('name', 'name', { unique: false });
            productStore.createIndex('category_price', ['category', 'price'], { unique: false });
        }
    }
}
```

---

## Transactions

### Transaction Modes

```javascript
// ===== File: transactions.js =====
// Transaction Management

class TransactionManager {
    constructor(db) {
        this.db = db;
    }
    
    // Readonly transaction
    getObjectStore(storeName) {
        const transaction = this.db.transaction(storeName, 'readonly');
        return transaction.objectStore(storeName);
    }
    
    // Readwrite transaction
    getWriteableStore(storeName) {
        const transaction = this.db.transaction(storeName, 'readwrite');
        return transaction.objectStore(storeName);
    }
    
    // Get all items from store
    async getAll(storeName) {
        return new Promise((resolve, reject) => {
            const store = this.getObjectStore(storeName);
            const request = store.getAll();
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Get single item
    async get(storeName, key) {
        return new Promise((resolve, reject) => {
            const store = this.getObjectStore(storeName);
            const request = store.get(key);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Add item
    async add(storeName, value, key = null) {
        return new Promise((resolve, reject) => {
            const store = this.getWriteableStore(storeName);
            const request = key ? store.add(value, key) : store.add(value);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Put (add or update)
    async put(storeName, value, key = null) {
        return new Promise((resolve, reject) => {
            const store = this.getWriteableStore(storeName);
            const request = key ? store.put(value, key) : store.put(value);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Delete item
    async delete(storeName, key) {
        return new Promise((resolve, reject) => {
            const store = this.getWriteableStore(storeName);
            const request = store.delete(key);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Clear store
    async clear(storeName) {
        return new Promise((resolve, reject) => {
            const store = this.getWriteableStore(storeName);
            const request = store.clear();
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Multi-store transaction
    async multiStoreOperation(operations) {
        return new Promise((resolve, reject) => {
            const storeNames = [...new Set(operations.map(op => op.store))];
            const transaction = this.db.transaction(storeNames, 'readwrite');
            
            const results = [];
            transaction.oncomplete = () => resolve(results);
            transaction.onerror = () => reject(transaction.error);
            
            operations.forEach((op, index) => {
                const store = transaction.objectStore(op.store);
                let request;
                
                switch (op.type) {
                    case 'add':
                        request = store.add(op.value);
                        break;
                    case 'put':
                        request = store.put(op.value);
                        break;
                    case 'delete':
                        request = store.delete(op.key);
                        break;
                }
                
                results[index] = request.result;
            });
        });
    }
}
```

---

## Indexes and Queries

### Using Indexes for Queries

```javascript
// ===== File: indexes-queries.js =====
// Index-Based Queries

class QueryManager {
    constructor(db) {
        this.db = db;
    }
    
    // Get all items with an index
    async getByIndex(storeName, indexName, value) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readonly');
            const store = transaction.objectStore(storeName);
            const index = store.index(indexName);
            const request = index.get(value);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Get all items with matching index (including duplicates)
    async getAllByIndex(storeName, indexName, value) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readonly');
            const store = transaction.objectStore(storeName);
            const index = store.index(indexName);
            const request = index.getAll(value);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Range queries using IDBKeyRange
    async getByRange(storeName, lower, upper, openBounds = false) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readonly');
            const store = transaction.objectStore(storeName);
            
            const range = IDBKeyRange.bound(lower, upper, openBounds, openBounds);
            const request = store.getAll(range);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Get items greater than or equal to value
    async getGreaterThan(storeName, value) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readonly');
            const store = transaction.objectStore(storeName);
            
            const range = IDBKeyRange.lowerBound(value, true);
            const request = store.getAll(range);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Get items less than value
    async getLessThan(storeName, value) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readonly');
            const store = transaction.objectStore(storeName);
            
            const range = IDBKeyRange.upperBound(value, true);
            const request = store.getAll(range);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Advanced compound query with multiple conditions
    async advancedQuery(storeName, conditions) {
        const results = await this.getAll(storeName);
        
        return results.filter(item => {
            return conditions.every((condition) => {
                const { field, operator, value } = condition;
                
                switch (operator) {
                    case 'eq': return item[field] === value;
                    case 'ne': return item[field] !== value;
                    case 'gt': return item[field] > value;
                    case 'gte': return item[field] >= value;
                    case 'lt': return item[field] < value;
                    case 'lte': return item[field] <= value;
                    case 'in': return value.includes(item[field]);
                    case 'contains': return String(item[field]).includes(value);
                    default: return true;
                }
            });
        });
    }
}
```

---

## Cursors

### Cursor-Based Iteration

```javascript
// ===== File: cursors.js =====
// Cursor Operations

class CursorManager {
    constructor(db) {
        this.db = db;
    }
    
    // Iterate over all items
    async iterateAll(storeName, callback) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readonly');
            const store = transaction.objectStore(storeName);
            const request = store.openCursor();
            const processed = [];
            
            request.onsuccess = (event) => {
                const cursor = event.target.result;
                
                if (cursor) {
                    processed.push(callback(cursor.value, cursor));
                    cursor.continue();
                } else {
                    resolve(processed);
                }
            };
            
            request.onerror = () => reject(request.error);
        });
    }
    
    // Iterate with key range
    async iterateRange(storeName, lower, upper, callback) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readonly');
            const store = transaction.objectStore(storeName);
            const range = IDBKeyRange.bound(lower, upper);
            const request = store.openCursor(range);
            const processed = [];
            
            request.onsuccess = (event) => {
                const cursor = event.target.result;
                
                if (cursor) {
                    processed.push(callback(cursor.value, cursor));
                    cursor.continue();
                } else {
                    resolve(processed);
                }
            };
            
            request.onerror = () => reject(request.error);
        });
    }
    
    // Iterate using an index
    async iterateIndex(storeName, indexName, callback, direction = 'next') {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readonly');
            const store = transaction.objectStore(storeName);
            const index = store.index(indexName);
            const request = index.openCursor(null, direction);
            const processed = [];
            
            request.onsuccess = (event) => {
                const cursor = event.target.result;
                
                if (cursor) {
                    processed.push(callback(cursor.value, cursor));
                    cursor.continue();
                } else {
                    resolve(processed);
                }
            };
            
            request.onerror = () => reject(request.error);
        });
    }
    
    // Batch processing with cursor
    async batchProcess(storeName, batchSize, processBatch) {
        let offset = 0;
        let batchNumber = 0;
        
        while (true) {
            const batch = await this.getBatch(storeName, offset, batchSize);
            
            if (batch.length === 0) break;
            
            await processBatch(batch, batchNumber);
            offset += batchSize;
            batchNumber++;
        }
    }
    
    async getBatch(storeName, offset, limit) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readonly');
            const store = transaction.objectStore(storeName);
            const results = [];
            
            let count = 0;
            const request = store.openCursor(IDBKeyRange.lowerBound(offset), 'next');
            
            request.onsuccess = (event) => {
                const cursor = event.target.result;
                
                if (cursor && count < limit) {
                    results.push(cursor.value);
                    count++;
                    cursor.continue();
                } else {
                    resolve(results);
                }
            };
            
            request.onerror = () => reject(request.error);
        });
    }
}
```

---

## Complex Data Structures

### Storing Complex Data Types

```javascript
// ===== File: complex-data.js =====
// Complex Data Structures

class ComplexDataManager {
    constructor(db) {
        this.db = db;
    }
    
    // Store nested objects
    async storeUserWithProfile(user) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction('users', 'readwrite');
            const store = transaction.objectStore('users');
            
            const userData = {
                id: user.id,
                profile: {
                    basic: {
                        name: user.name,
                        email: user.email,
                        avatar: user.avatar
                    },
                    social: {
                        friends: user.friends || [],
                        followers: user.followers || []
                    },
                    preferences: {
                        theme: user.theme || 'light',
                        notifications: user.notifications || {}
                    }
                },
                metadata: {
                    createdAt: new Date(),
                    updatedAt: new Date(),
                    version: 1
                }
            };
            
            const request = store.put(userData);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Store files as blobs
    async storeFile(file) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction('files', 'readwrite');
            const store = transaction.objectStore('files');
            
            const fileData = {
                id: crypto.randomUUID(),
                name: file.name,
                type: file.type,
                size: file.size,
                blob: file,
                uploadedAt: new Date()
            };
            
            const request = store.put(fileData);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Store arrays of objects
    async storeOrder(order) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction('orders', 'readwrite');
            const store = transaction.objectStore('orders');
            
            const orderData = {
                id: order.id,
                userId: order.userId,
                items: order.items.map(item => ({
                    productId: item.productId,
                    name: item.name,
                    quantity: item.quantity,
                    price: item.price,
                    options: item.options || {}
                })),
                totals: {
                    subtotal: order.subtotal,
                    tax: order.tax,
                    shipping: order.shipping,
                    total: order.total
                },
                status: 'pending',
                timeline: [
                    { status: 'created', timestamp: new Date() }
                ]
            };
            
            const request = store.put(orderData);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Add to order timeline
    async addOrderTimelineEntry(orderId, status) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction('orders', 'readwrite');
            const store = transaction.objectStore('orders');
            
            const getRequest = store.get(orderId);
            
            getRequest.onsuccess = () => {
                const order = getRequest.result;
                
                if (!order) {
                    reject(new Error('Order not found'));
                    return;
                }
                
                order.timeline.push({
                    status,
                    timestamp: new Date()
                });
                
                const updateRequest = store.put(order);
                updateRequest.onsuccess = () => resolve(order);
                updateRequest.onerror = () => reject(updateRequest.error);
            };
            
            getRequest.onerror = () => reject(getRequest.error);
        });
    }
}
```

---

## Performance Optimization

### Performance Best Practices

```javascript
// ===== File: performance.js =====
// Performance Optimization

class OptimizedManager {
    constructor(db) {
        this.db = db;
    }
    
    // Use keyPath for direct access
    async optimizedGet(storeName, key) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readonly');
            const store = transaction.objectStore(storeName);
            const request = store.get(key);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Batch writes in single transaction
    async batchWrite(storeName, items) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readwrite');
            const store = transaction.objectStore(storeName);
            
            const results = [];
            transaction.oncomplete = () => resolve(results);
            transaction.onerror = () => reject(transaction.error);
            
            items.forEach((item, index) => {
                const request = store.put(item);
                results[index] = request.result;
            });
        });
    }
    
    // Use indexes for queries (much faster)
    async indexedQuery(storeName, indexName, value) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readonly');
            const store = transaction.objectStore(storeName);
            const index = store.index(indexName);
            const request = index.get(value);
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Get count without loading all data
    async getCount(storeName) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readonly');
            const store = transaction.objectStore(storeName);
            const request = store.count();
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Pagination with key-only cursor
    async paginate(storeName, pageSize, lastKey = null) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(storeName, 'readonly');
            const store = transaction.objectStore(storeName);
            
            const results = [];
            const range = lastKey 
                ? IDBKeyRange.upperBound(lastKey, true)
                : null;
            
            const request = store.openCursor(range, 'next');
            
            request.onsuccess = (event) => {
                const cursor = event.target.result;
                
                if (cursor && results.length < pageSize) {
                    if (!lastKey || cursor.key !== lastKey) {
                        results.push({
                            key: cursor.key,
                            value: cursor.value
                        });
                    }
                    cursor.continue();
                } else {
                    resolve({
                        items: results,
                        nextKey: results.length === pageSize 
                            ? results[results.length - 1]?.key 
                            : null
                    });
                }
            };
            
            request.onerror = () => reject(request.error);
        });
    }
}
```

---

## Professional Use Cases

### Use Case 1: E-commerce Cart

```javascript
// ===== File: use-case-cart.js =====
// E-commerce Shopping Cart

class ShoppingCart {
    constructor(db) {
        this.db = db;
        this.storeName = 'cart';
    }
    
    addItem(product, quantity = 1) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(this.storeName, 'readwrite');
            const store = transaction.objectStore(this.storeName);
            
            const getRequest = store.get(product.id);
            
            getRequest.onsuccess = () => {
                const existing = getRequest.result;
                
                if (existing) {
                    existing.quantity += quantity;
                    existing.updatedAt = new Date();
                    store.put(existing);
                } else {
                    store.add({
                        id: product.id,
                        product,
                        quantity,
                        addedAt: new Date(),
                        updatedAt: new Date()
                    });
                }
            };
            
            transaction.oncomplete = () => resolve();
            transaction.onerror = () => reject(transaction.error);
        });
    }
    
    updateQuantity(productId, quantity) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(this.storeName, 'readwrite');
            const store = transaction.objectStore(this.storeName);
            
            const request = store.get(productId);
            
            request.onsuccess = () => {
                const item = request.result;
                
                if (item) {
                    item.quantity = quantity;
                    item.updatedAt = new Date();
                    store.put(item);
                }
            };
            
            transaction.oncomplete = () => resolve();
            transaction.onerror = () => reject(transaction.error);
        });
    }
    
    removeItem(productId) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(this.storeName, 'readwrite');
            const store = transaction.objectStore(this.storeName);
            
            store.delete(productId);
            
            transaction.oncomplete = () => resolve();
            transaction.onerror = () => reject(transaction.error);
        });
    }
    
    getItems() {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(this.storeName, 'readonly');
            const store = transaction.objectStore(this.storeName);
            const request = store.getAll();
            
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    clear() {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction(this.storeName, 'readwrite');
            const store = transaction.objectStore(this.storeName);
            
            store.clear();
            
            transaction.oncomplete = () => resolve();
            transaction.onerror = () => reject(transaction.error);
        });
    }
}
```

### Use Case 2: Offline-First Database

```javascript
// ===== File: use-case-offline.js =====
// Offline-First Data Management

class OfflineDatabase {
    constructor(dbName) {
        this.dbName = dbName;
        this.db = null;
    }
    
    async initialize() {
        const request = indexedDB.open(this.dbName, 1);
        
        request.onupgradeneeded = (event) => {
            const db = event.target.result;
            
            if (!db.objectStoreNames.contains('pending')) {
                db.createObjectStore('pending', { keyPath: 'id', autoIncrement: true });
            }
            
            if (!db.objectStoreNames.contains('syncQueue')) {
                db.createObjectStore('syncQueue', { keyPath: 'id', autoIncrement: true });
            }
        };
        
        return new Promise((resolve, reject) => {
            request.onsuccess = () => {
                this.db = request.result;
                resolve(this.db);
            };
            
            request.onerror = () => reject(request.error);
        });
    }
    
    async queueForSync(operation) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction('syncQueue', 'readwrite');
            const store = transaction.objectStore('syncQueue');
            
            store.add({
                operation,
                createdAt: new Date(),
                status: 'pending'
            });
            
            transaction.oncomplete = () => resolve();
            transaction.onerror = () => reject(transaction.error);
        });
    }
    
    async processSyncQueue() {
        return new Promise(async (resolve, reject) => {
            const transaction = this.db.transaction('syncQueue', 'readwrite');
            const store = transaction.objectStore('syncQueue');
            const request = store.getAll();
            
            request.onsuccess = async () => {
                const queue = request.result;
                
                for (const item of queue) {
                    try {
                        await this.executeOperation(item.operation);
                        store.delete(item.id);
                    } catch (error) {
                        console.error('Sync failed:', error);
                    }
                }
            };
            
            transaction.oncomplete = () => resolve();
            transaction.onerror = () => reject(transaction.error);
        });
    }
    
    async executeOperation(operation) {
        // Simulate API call
        await new Promise(resolve => setTimeout(resolve, 100));
        console.log('Executed:', operation);
    }
}
```

### Use Case 3: Search with Indexes

```javascript
// ===== File: use-case-search.js =====
// Product Search with Indexes

class ProductSearch {
    constructor(db) {
        this.db = db;
    }
    
    // Search by category using index
    async searchByCategory(category) {
        const transaction = this.db.transaction('products', 'readonly');
        const store = transaction.objectStore('products');
        const index = store.index('category');
        const request = index.getAll(category);
        
        return new Promise((resolve, reject) => {
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Search by price range
    async searchByPriceRange(minPrice, maxPrice) {
        const transaction = this.db.transaction('products', 'readonly');
        const store = transaction.objectStore('products');
        const range = IDBKeyBound.bound(minPrice, maxPrice);
        const index = store.index('price');
        const request = index.getAll(range);
        
        return new Promise((resolve, reject) => {
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }
    
    // Full-text search simulation
    async search(query) {
        return new Promise((resolve, reject) => {
            const transaction = this.db.transaction('products', 'readonly');
            const store = transaction.objectStore('products');
            const request = store.getAll();
            
            request.onsuccess = () => {
                const products = request.result;
                const results = products.filter(product => {
                    const searchText = `${product.name} ${product.description}`.toLowerCase();
                    return searchText.includes(query.toLowerCase());
                });
                
                resolve(results);
            };
            
            request.onerror = () => reject(request.error);
        });
    }
    
    // Sort results
    async searchAndSort(query, sortBy = 'name', direction = 'asc') {
        const results = await this.search(query);
        
        return results.sort((a, b) => {
            const aVal = a[sortBy];
            const bVal = b[sortBy];
            
            if (direction === 'asc') {
                return aVal > bVal ? 1 : -1;
            } else {
                return aVal < bVal ? 1 : -1;
            }
        });
    }
}
```

---

## Common Pitfalls

1. **Not handling onupgradeneeded**: Database schema changes only happen in this event
2. **Using synchronous patterns**: IndexedDB is asynchronous; use Promises or callbacks
3. **Not wrapping in transactions**: Always use transactions for data integrity
4. **Creating too many indexes**: Indexes slow down writes; create only necessary ones
5. **Not handling errors**: Always add error handlers to requests
6. **Storing large blobs**: Can impact performance; consider alternatives
7. **Not using keyPath**: Makes direct access more efficient
8. **Opening database on every operation**: Keep reference open
9. **Ignoring quota warnings**: Browser storage is limited
10. **Not using cursor for large datasets**: Loading all data at once is slow

---

## Key Takeaways

- IndexedDB stores structured data with full query support
- Use onupgradeneeded to create/upgrade schema
- Indexes provide fast lookups on specific fields
- Transactions ensure data integrity
- Cursors enable efficient large dataset iteration
- KeyPath provides direct access by primary key
- Ranges enable efficient filtering
- Batch operations improve performance
- Complex data structures can be nested objects
- Offline-first applications benefit greatly

---

## Related Files

- [01_LOCAL_STORAGE_SESSION_STORAGE.md](./01_LOCAL_STORAGE_SESSION_STORAGE.md) - For simple key-value storage
- [03_CACHING_STRATEGIES.md](./03_CACHING_STRATEGIES.md) - For network caching
- [06_STORAGE_SECURITY_BEST_PRACTICES.md](./06_STORAGE_SECURITY_BEST_PRACTICES.md) - For security