/**
 * Category: 02_ADVANCED
 * Subcategory: 01_PATTERNS
 * Concept: 06_Performance_Patterns
 * Topic: 02_Memoization_Patterns
 * Purpose: Deep dive into Memoization Patterns with TypeScript examples
 * Difficulty: intermediate
 * UseCase: web, backend, enterprise
 * Version: TS 5.0+
 * Compatibility: Browsers, Node.js
 * Performance: Significant for expensive computations
 * Security: N/A
 */

/**
 * Memoization Patterns - Comprehensive Guide
 * =========================================
 * 
 * WHAT: A caching technique that stores the results of expensive function calls
 * and returns cached results when the same inputs occur again.
 * 
 * WHY:
 * - Avoid redundant calculations
 * - Improve performance
 * - Reduce network calls
 * - Trade memory for speed
 * 
 * HOW:
 * - Create cache storage
 * - Check cache before computation
 * - Store results after computation
 * - Handle cache invalidation
 */

// ============================================================================
// SECTION 1: BASIC MEMOIZATION
// ============================================================================

function memoize<T extends (...args: any[]) => any>(fn: T): T {
  const cache = new Map<string, ReturnType<T>>();
  
  return ((...args: Parameters<T>): ReturnType<T> => {
    const key = JSON.stringify(args);
    
    if (cache.has(key)) {
      return cache.get(key)!;
    }
    
    const result = fn(...args);
    cache.set(key, result);
    return result;
  }) as T;
}

const expensiveCalculation = (n: number): number => {
  console.log(`Computing for ${n}...`);
  return n * n;
};

const memoizedCalculation = memoize(expensiveCalculation);

memoizedCalculation(5); // Computes
memoizedCalculation(5); // Uses cache

// ============================================================================
// SECTION 2: MEMOIZATION WITH TTL
// ============================================================================

interface CacheEntry<T> {
  value: T;
  expiresAt: number;
}

function memoizeWithTTL<T extends (...args: any[]) => any>(
  fn: T,
  ttl: number
): T {
  const cache = new Map<string, CacheEntry<ReturnType<T>>>();
  
  return ((...args: Parameters<T>): ReturnType<T> => {
    const key = JSON.stringify(args);
    const now = Date.now();
    
    const entry = cache.get(key);
    if (entry && entry.expiresAt > now) {
      return entry.value;
    }
    
    const result = fn(...args);
    cache.set(key, { value: result, expiresAt: now + ttl });
    return result;
  }) as T;
}

// ============================================================================
// SECTION 3: ASYNC MEMOIZATION
// ============================================================================

async function memoizeAsync<T extends (...args: any[]) => Promise<any>>(
  fn: T
): Promise<T> {
  const cache = new Map<string, Promise<ReturnType<T>>>();
  
  return (async (...args: Parameters<T>): Promise<ReturnType<T>> => {
    const key = JSON.stringify(args);
    
    if (cache.has(key)) {
      return cache.get(key)!;
    }
    
    const promise = fn(...args);
    cache.set(key, promise);
    
    try {
      return await promise;
    } catch (error) {
      cache.delete(key);
      throw error;
    }
  }) as T;
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

// Memoization Performance:
// - Memory growth over time
// - Cache key computation cost
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
// - Consider cache size

// ============================================================================
// TESTING STRATEGY
// ============================================================================

// Testing memoization:
// 1. Test cache hit
// 2. Test cache miss
// 3. Test TTL expiration
// 4. Test cache invalidation

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related patterns:
// - Lazy Loading (01_Lazy_Loading_Patterns.ts)
// - Cache Patterns (04_Cache_Patterns.ts)
// - Proxy (03_Structural_Patterns/08_Proxy_Pattern.ts)

// Next: 03_Throttling_Debouncing.ts
