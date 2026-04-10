/**
 * Category: 02_ADVANCED
 * Subcategory: 01_PATTERNS
 * Concept: 06_Performance_Patterns
 * Topic: 04_Cache_Patterns
 * Purpose: Deep dive into Cache Patterns with TypeScript examples
 * Difficulty: intermediate
 * UseCase: web, backend, enterprise
 * Version: TS 5.0+
 * Compatibility: Browsers, Node.js
 * Performance: Significant for expensive operations
 * Security: Consider sensitive data
 */

/**
 * Cache Patterns - Comprehensive Guide
 * ====================================
 * 
 * WHAT: Patterns for storing frequently accessed data in fast storage layers
 * to reduce latency and improve performance.
 * 
 * WHY:
 * - Reduce database load
 * - Improve response times
 * - Handle traffic spikes
 * - Enable offline access
 * 
 * HOW:
 * - Choose cache strategy
 * - Implement cache layer
 * - Handle invalidation
 * - Manage cache size
 */

// ============================================================================
// SECTION 1: CACHE INTERFACE
// ============================================================================

interface Cache<T> {
  get(key: string): Promise<T | null>;
  set(key: string, value: T, ttl?: number): Promise<void>;
  delete(key: string): Promise<void>;
  clear(): Promise<void>;
}

// ============================================================================
// SECTION 2: IN-MEMORY CACHE
// ============================================================================

class InMemoryCache<T> implements Cache<T> {
  private store: Map<string, { value: T; expiresAt?: number }> = new Map();
  
  async get(key: string): Promise<T | null> {
    const entry = this.store.get(key);
    
    if (!entry) return null;
    
    if (entry.expiresAt && entry.expiresAt < Date.now()) {
      this.store.delete(key);
      return null;
    }
    
    return entry.value;
  }
  
  async set(key: string, value: T, ttl?: number): Promise<void> {
    this.store.set(key, {
      value,
      expiresAt: ttl ? Date.now() + ttl : undefined
    });
  }
  
  async delete(key: string): Promise<void> {
    this.store.delete(key);
  }
  
  async clear(): Promise<void> {
    this.store.clear();
  }
}

// ============================================================================
// SECTION 3: CACHE-THROUGH PATTERN
// ============================================================================

class CacheAside<T> {
  constructor(
    private cache: Cache<T>,
    private fetcher: (key: string) => Promise<T>
  ) {}
  
  async get(key: string): Promise<T> {
    const cached = await this.cache.get(key);
    if (cached !== null) {
      return cached;
    }
    
    const value = await this.fetcher(key);
    await this.cache.set(key, value);
    return value;
  }
  
  async invalidate(key: string): Promise<void> {
    await this.cache.delete(key);
  }
}

// ============================================================================
// SECTION 4: CACHE-WRITE PATTERN
// ============================================================================

class WriteThrough<T> {
  constructor(
    private cache: Cache<T>,
    private storage: { write: (key: string, value: T) => Promise<void> }
  ) {}
  
  async write(key: string, value: T): Promise<void> {
    await Promise.all([
      this.cache.set(key, value),
      this.storage.write(key, value)
    ]);
  }
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

// Cache Performance:
// - Memory usage for in-memory cache
// - Cache hit ratio important
// - Consider cache size limits

// ============================================================================
// COMPATIBILITY
// ============================================================================

// Compatible with:
// - TypeScript 1.6+
// - All ES targets
// - Node.js and browsers

// ============================================================================
// SECURITY CONSIDERATIONS
// ============================================================================

// Security considerations:
// - Don't cache sensitive data
// - Encrypt if necessary
// - Consider cache poisoning

// ============================================================================
// TESTING STRATEGY
// ============================================================================

// Testing caches:
// 1. Test get/set
// 2. Test TTL expiration
// 3. Test invalidation
// 4. Test race conditions

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related patterns:
// - Memoization (02_Memoization_Patterns.ts)
// - Connection Pooling (05_Connection_Pooling.ts)
// - Proxy (03_Structural_Patterns/08_Proxy_Pattern.ts)

// Next: 05_Connection_Pooling.ts
