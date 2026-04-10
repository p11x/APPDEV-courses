/**
 * Category: 02_ADVANCED
 * Subcategory: 01_PATTERNS
 * Concept: 08_Scalability_Patterns
 * Topic: 02_Circuit_Breaker
 * Purpose: Deep dive into Circuit Breaker Patterns with TypeScript examples
 * Difficulty: advanced
 * UseCase: web, backend, enterprise
 * Version: TS 5.0+
 * Compatibility: Browsers, Node.js
 * Performance: Prevents cascading failures
 * Security: N/A
 */

/**
 * Circuit Breaker Patterns - Comprehensive Guide
 * ==============================================
 * 
 * WHAT: A design pattern that prevents an application from repeatedly trying
 * to execute an operation that's likely to fail, allowing it to fail fast
 * and recover gracefully.
 * 
 * WHY:
 * - Prevent cascading failures
 * - Enable graceful degradation
 * - Allow time for recovery
 * - Improve system resilience
 * 
 * HOW:
 * - Monitor failures
 * - Open circuit after threshold
 * - Half-open for testing recovery
 * - Close after success
 */

type CircuitState = "closed" | "open" | "half-open";

// ============================================================================
// SECTION 1: CIRCUIT BREAKER
// ============================================================================

interface CircuitBreakerOptions {
  failureThreshold: number;
  successThreshold: number;
  timeout: number;
}

class CircuitBreaker {
  private state: CircuitState = "closed";
  private failures = 0;
  private successes = 0;
  private lastFailureTime: number = 0;
  
  constructor(private options: CircuitBreakerOptions) {}
  
  async execute<T>(fn: () => Promise<T>): Promise<T> {
    if (this.state === "open") {
      if (Date.now() - this.lastFailureTime >= this.options.timeout) {
        this.state = "half-open";
        this.successes = 0;
      } else {
        throw new Error("Circuit breaker is open");
      }
    }
    
    try {
      const result = await fn();
      this.onSuccess();
      return result;
    } catch (error) {
      this.onFailure();
      throw error;
    }
  }
  
  private onSuccess(): void {
    this.failures = 0;
    
    if (this.state === "half-open") {
      this.successes++;
      
      if (this.successes >= this.options.successThreshold) {
        this.state = "closed";
      }
    }
  }
  
  private onFailure(): void {
    this.failures++;
    this.lastFailureTime = Date.now();
    
    if (this.failures >= this.options.failureThreshold) {
      this.state = "open";
    }
  }
  
  getState(): CircuitState {
    return this.state;
  }
  
  reset(): void {
    this.state = "closed";
    this.failures = 0;
    this.successes = 0;
  }
}

// ============================================================================
// SECTION 2: CIRCUIT BREAKER REGISTRY
// ============================================================================

class CircuitBreakerRegistry {
  private breakers: Map<string, CircuitBreaker> = new Map();
  
  getBreaker(name: string, options?: CircuitBreakerOptions): CircuitBreaker {
    if (!this.breakers.has(name)) {
      this.breakers.set(name, new CircuitBreaker(options || {
        failureThreshold: 5,
        successThreshold: 3,
        timeout: 30000
      }));
    }
    
    return this.breakers.get(name)!;
  }
  
  getAllBreakers(): Array<{ name: string; state: CircuitState }> {
    return Array.from(this.breakers.entries()).map(([name, breaker]) => ({
      name,
      state: breaker.getState()
    }));
  }
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

// Circuit Breaker Performance:
// - Minimal overhead
// - Prevents resource exhaustion
// - Allows recovery time

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
// - Don't expose internal state
// - Validate options

// ============================================================================
// TESTING STRATEGY
// ============================================================================

// Testing circuit breaker:
// 1. Test state transitions
// 2. Test failure handling
// 3. Test recovery

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related patterns:
// - Load Balancing (01_Load_Balancing.ts)
// - Retry Patterns
// - Fallback Patterns

// Next: 03_Sharding_Patterns.ts
