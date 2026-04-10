/**
 * Category: 02_ADVANCED
 * Subcategory: 01_PATTERNS
 * Concept: 06_Performance_Patterns
 * Topic: 05_Connection_Pooling
 * Purpose: Deep dive into Connection Pooling Patterns with TypeScript examples
 * Difficulty: intermediate
 * UseCase: web, backend, enterprise
 * Version: TS 5.0+
 * Compatibility: Browsers, Node.js
 * Performance: Significant for database operations
 * Security: Consider connection security
 */

/**
 * Connection Pooling - Comprehensive Guide
 * =========================================
 * 
 * WHAT: A technique to maintain a pool of database connections that can be
 * reused, avoiding the overhead of creating new connections for each request.
 * 
 * WHY:
 * - Reduce connection overhead
 * - Handle more concurrent requests
 * - Prevent connection exhaustion
 * - Improve response times
 * 
 * HOW:
 * - Create pool with min/max connections
 * - Acquire connection from pool
 * - Release connection back to pool
 * - Handle pool exhaustion
 */

// ============================================================================
// SECTION 1: CONNECTION POOL
// ============================================================================

interface PooledConnection {
  id: string;
  inUse: boolean;
  createdAt: Date;
  lastUsed: Date;
}

interface PoolOptions {
  min: number;
  max: number;
  acquireTimeout: number;
}

class ConnectionPool<T extends PooledConnection> {
  private pool: T[] = [];
  private waiting: Array<(connection: T) => void> = [];
  
  constructor(
    private options: PoolOptions,
    private factory: () => Promise<T>
  ) {}
  
  async initialize(): Promise<void> {
    for (let i = 0; i < this.options.min; i++) {
      const connection = await this.factory();
      this.pool.push(connection);
    }
  }
  
  async acquire(): Promise<T> {
    const available = this.pool.find(c => !c.inUse);
    
    if (available) {
      available.inUse = true;
      available.lastUsed = new Date();
      return available;
    }
    
    if (this.pool.length < this.options.max) {
      const connection = await this.factory();
      connection.inUse = true;
      this.pool.push(connection);
      return connection;
    }
    
    return new Promise((resolve) => {
      const timeout = setTimeout(() => {
        throw new Error("Acquire timeout");
      }, this.options.acquireTimeout);
      
      this.waiting.push((connection) => {
        clearTimeout(timeout);
        resolve(connection);
      });
    });
  }
  
  release(connection: T): void {
    connection.inUse = false;
    connection.lastUsed = new Date();
    
    if (this.waiting.length > 0) {
      const waiter = this.waiting.shift()!;
      this.acquire().then(waiter);
    }
  }
  
  async close(): Promise<void> {
    this.pool = [];
    this.waiting = [];
  }
}

// ============================================================================
// SECTION 2: DATABASE POOL EXAMPLE
// ============================================================================

interface DatabaseConnection extends PooledConnection {
  query(sql: string): Promise<any[]>;
}

class DatabasePool {
  private pool: ConnectionPool<DatabaseConnection>;
  
  constructor() {
    this.pool = new ConnectionPool<DatabaseConnection>(
      { min: 2, max: 10, acquireTimeout: 5000 },
      async () => ({
        id: crypto.randomUUID(),
        inUse: false,
        createdAt: new Date(),
        lastUsed: new Date(),
        query: async (sql: string) => {
          console.log(`Executing: ${sql}`);
          return [];
        }
      })
    );
  }
  
  async initialize(): Promise<void> {
    await this.pool.initialize();
  }
  
  async query<T>(fn: (conn: DatabaseConnection) => Promise<T>): Promise<T> {
    const connection = await this.pool.acquire();
    try {
      return await fn(connection);
    } finally {
      this.pool.release(connection);
    }
  }
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

// Connection Pooling Performance:
// - Reduces connection overhead
// - Prevents connection exhaustion
// - Consider pool size tuning

// ============================================================================
// COMPATIBILITY
// ============================================================================

// Compatible with:
// - TypeScript 1.6+
// - All ES targets
// - Node.js (databases)

// ============================================================================
// SECURITY CONSIDERATIONS
// ============================================================================

// Security considerations:
// - Secure connection credentials
// - Connection timeout
// - Limit pool size

// ============================================================================
// TESTING STRATEGY
// ============================================================================

// Testing connection pooling:
// 1. Test acquire/release
// 2. Test pool exhaustion
// 3. Test timeout

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related patterns:
// - Cache Patterns (04_Cache_Patterns.ts)
// - Batch Processing (06_Batch_Processing.ts)
// - Singleton (02_Creational_Patterns/02_Singleton_Pattern.ts)

// Next: 06_Batch_Processing.ts
