/**
 * Category: 02_ADVANCED
 * Subcategory: 01_PATTERNS
 * Concept: 06_Performance_Patterns
 * Topic: 03_Throttling_Debouncing
 * Purpose: Deep dive into Throttling and Debouncing Patterns with TypeScript examples
 * Difficulty: intermediate
 * UseCase: web, backend, enterprise
 * Version: TS 5.0+
 * Compatibility: Browsers, Node.js
 * Performance: Significant for frequent events
 * Security: N/A
 */

/**
 * Throttling/Debouncing - Comprehensive Guide
 * ===========================================
 * 
 * WHAT: Techniques to limit the rate at which a function can fire. Throttling
 * ensures the function is called at most once per time interval, while debouncing
 * ensures the function is called after a delay since the last call.
 * 
 * WHY:
 * - Prevent excessive function calls
 * - Optimize performance
 * - Reduce server load
 * - Improve user experience
 * 
 * HOW:
 * - Track last call time (throttle)
 * - Set timeout for delayed call (debounce)
 * - Cancel pending calls
 * - Handle leading/trailing edge
 */

// ============================================================================
// SECTION 1: THROTTLING
// ============================================================================

function throttle<T extends (...args: any[]) => any>(
  fn: T,
  limit: number
): T & { cancel: () => void } {
  let lastCall = 0;
  let timeoutId: NodeJS.Timeout | null = null;
  
  const throttled = (...args: Parameters<T>) => {
    const now = Date.now();
    
    if (now - lastCall >= limit) {
      lastCall = now;
      fn(...args);
    } else if (!timeoutId) {
      timeoutId = setTimeout(() => {
        lastCall = Date.now();
        timeoutId = null;
        fn(...args);
      }, limit - (now - lastCall));
    }
  };
  
  throttled.cancel = () => {
    if (timeoutId) {
      clearTimeout(timeoutId);
      timeoutId = null;
    }
  };
  
  return throttled as T & { cancel: () => void };
}

// ============================================================================
// SECTION 2: DEBOUNCING
// ============================================================================

function debounce<T extends (...args: any[]) => any>(
  fn: T,
  delay: number,
  options?: { leading?: boolean }
): T & { cancel: () => void } {
  let timeoutId: NodeJS.Timeout | null = null;
  
  const debounced = (...args: Parameters<T>) => {
    const isLeading = options?.leading && !timeoutId;
    
    if (timeoutId) {
      clearTimeout(timeoutId);
    }
    
    timeoutId = setTimeout(() => {
      timeoutId = null;
      if (!options?.leading) {
        fn(...args);
      }
    }, delay);
    
    if (isLeading) {
      fn(...args);
    }
  };
  
  debounced.cancel = () => {
    if (timeoutId) {
      clearTimeout(timeoutId);
      timeoutId = null;
    }
  };
  
  return debounced as T & { cancel: () => void };
}

// ============================================================================
// SECTION 3: PRACTICAL EXAMPLES
// ============================================================================

// Search input debounce
function createSearchHandler(
  onSearch: (query: string) => void
): (query: string) => void {
  return debounce(onSearch, 300);
}

// Window resize throttling
function createResizeHandler(
  onResize: () => void
): () => void {
  return throttle(onResize, 100);
}

// Button click throttle (prevent double-submit)
function createClickHandler(
  onClick: () => void
): () => void {
  return throttle(onClick, 1000);
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

// Throttling/Debouncing Performance:
// - Reduces function calls significantly
// - Memory for timers
// - Trade-off between responsiveness and performance

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
// - Don't expose sensitive data in handlers
// - Consider timer-based attacks

// ============================================================================
// TESTING STRATEGY
// ============================================================================

// Testing throttling/debouncing:
// 1. Test rate limiting
// 2. Test timer behavior
// 3. Test cancel
// 4. Test leading edge

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related patterns:
// - Memoization (02_Memoization_Patterns.ts)
// - Cache Patterns (04_Cache_Patterns.ts)
// - Lazy Loading (01_Lazy_Loading_Patterns.ts)

// Next: 04_Cache_Patterns.ts
